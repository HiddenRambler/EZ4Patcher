using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EZ4Patcher
{
    public partial class EZPatcher : Form
    {
        const int COL_FNAME     = 0;
        const int COL_DIR       = 1;
        const int COL_SIZE      = 2;
        const int COL_OUTNAME   = 3;
        const int COL_RESULT    = 4;
        const int COL_FULL_NAME = 5;

        const int OUTPUT_FILE_GROUP_COUNT = 80;

        private ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public EZPatcher()
        {
            InitializeComponent();

            //list view doesnt retain design time column sizes
            lvFileList.Columns[COL_FNAME].Width = 213;
            lvFileList.Columns[COL_DIR].Width = 36;
            lvFileList.Columns[COL_SIZE].Width = 60;
            lvFileList.Columns[COL_OUTNAME].Width = 191;
            lvFileList.Columns[COL_RESULT].Width = 112;

            lvFileList.ListViewItemSorter = lvwColumnSorter;

            LoadSettings();
        }


        private long _TotalSize;
        private long TotalSize { get { return _TotalSize; } set { _TotalSize = value; stlblSummary.Text = String.Format("{0} in {1} File(s)", GetBytesReadable(value), filelist.Count); } }
        private HashSet<string> filelist = new HashSet<string>();

        private void EZPatcher_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void EZPatcher_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            AddFiles(files);
        }

        private void btnDestinationSelect_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDestination.Text = folderBrowserDialog1.SelectedPath;
                SaveSettings();
            }
        }

        void AddFiles(string[] filenames)
        {
            var oldc = Cursor;
            Cursor = Cursors.WaitCursor;
            lvFileList.BeginUpdate();
            try
            {
                long totsize = 0;
                foreach (var f in filenames)
                {
                    var fLower = f.ToLowerInvariant();
                    if (filelist.Contains(fLower)) continue;

                    var size = new System.IO.FileInfo(f).Length;

                    var item = new string[6];
                    item[COL_FNAME] = System.IO.Path.GetFileName(f);
                    item[COL_DIR] = System.IO.Path.GetDirectoryName(f);
                    item[COL_SIZE] = GetBytesReadable(size);
                    item[COL_OUTNAME] = item[0]; //TODO: some cleaned up name
                    item[COL_RESULT] = "";
                    item[COL_FULL_NAME] = f;

                    var lv = new ListViewItem(item);
                    lvFileList.Items.Add(lv);
                    lv.Tag = size;
                    totsize += size;

                    filelist.Add(fLower);
                }
                TotalSize += totsize;
            }
            finally
            {
                lvFileList.EndUpdate();
                Cursor = oldc;
            }
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
                var filenames = openFileDialog1.FileNames;
                AddFiles(filenames);
            }
        }

        public string GetBytesReadable(long i)
        {
            //from stackoverflow.com


            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }


        volatile bool CancalPatching = false;

        void PatchFiles(string outputRoot, bool overwrite, bool gensavefile, bool groupOutput, bool trimRoms)
        {
            CancalPatching = false;
            ShowStatus(true);
            try
            {
                string outputDir = outputRoot;
                string saverRoot = null;

                if (gensavefile)
                {
                    saverRoot = System.IO.Path.Combine(outputRoot, "saver");
                    if (!System.IO.Directory.Exists(saverRoot))
                        System.IO.Directory.CreateDirectory(saverRoot);
                }

                stprgProgress.Value = 0;
                stprgProgress.Maximum = lvFileList.Items.Count;

                int count = lvFileList.Items.Count;
                int groupcount = 0;
                for (int i = 0; i < count; i++)
                {
                    if (CancalPatching) break;

                    ListViewItem item = lvFileList.Items[i];
                    if (item.SubItems[COL_RESULT].Text.StartsWith("OK "))
                        continue;

                    if (groupcount == 0 && groupOutput)
                    {
                        var outname = System.IO.Path.GetFileNameWithoutExtension(item.SubItems[COL_OUTNAME].Text);
                        var prefix = outname.Substring(0, outname.Length > 4 ? 4 : outname.Length);

                        outputDir = System.IO.Path.Combine(outputRoot, prefix);
                        int counter = 0;
                        while (System.IO.Directory.Exists(prefix))
                        {
                            outputDir = System.IO.Path.Combine(outputRoot, prefix + counter.ToString(" X"));
                        }

                        System.IO.Directory.CreateDirectory(outputDir);
                    }

                    var source = item.SubItems[COL_FULL_NAME].Text;
                    var destination = System.IO.Path.Combine(outputDir, item.SubItems[COL_OUTNAME].Text);
                    item.SubItems[COL_RESULT].Text = PatchFile(source, destination, overwrite, gensavefile, saverRoot, trimRoms);
                    Application.DoEvents();
                    UpdateProgress(i, count);

                    if (++groupcount > OUTPUT_FILE_GROUP_COUNT) groupcount = 0;
                }
            }
            finally
            {
                ShowStatus(false);
            }
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            SaveSettings();
            var outputdir = txtDestination.Text;
            if (!System.IO.Directory.Exists(outputdir))
            {
                MessageBox.Show("Invalid destination", "Error");
            }
            PatchFiles(outputdir, cbOverwrite.Checked, cbGenSaveFiles.Checked, cbGroupOutput.Checked, cbAutoTrim.Checked);
        }

        void GenerateSaveFile(string filename, bool overwrite, int size)
        {
            if (size < 1) return;

            var str = new string('\xFF', size);
            System.IO.File.WriteAllText(filename, str);
        }

        string PatchFile(string source, string destination, bool overwrite, bool generateSaver, string saverRoot, bool trim)
        {
            if (source.ToLowerInvariant() == destination.ToLowerInvariant())
                return "Invalid Destination";

            if (!overwrite && System.IO.File.Exists(destination))
                return "Already Exists";

            try
            {
                var data = System.IO.File.ReadAllBytes(source);
                uint size = trim ? 0 : (uint)data.Length;
                ushort savetype = 0;
                uint savesize = 0;

                EZBridge.PatchRom(data, (uint)data.Length, ref size, ref savetype, ref savesize);

                if (size != data.Length)
                {
                    var dataout = new byte[size];
                    Array.Copy(data, dataout, size);
                    data = dataout;
                }

                System.IO.File.WriteAllBytes(destination, data);

                if (generateSaver && savesize > 0)
                {
                    var destname = System.IO.Path.GetFileName(destination);
                    var saverfile = System.IO.Path.Combine(saverRoot, destname);
                    saverfile = System.IO.Path.ChangeExtension(saverfile, "sav");

                    GenerateSaveFile(saverfile, overwrite, (int)savesize);
                }

                return string.Format("OK {0}/{1}", StypeToStr(savetype), savesize);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        string StypeToStr(int savetype)
        {
            switch (savetype)
            {
                case 0: return "None";
                case 1: return "SRAM";
                case 2: return "EEPROM";
                case 3: return "Flash";
                default: return "Unknown";
            }
        }

        private void btnClearSuccessful_Click(object sender, EventArgs e)
        {
            long size = 0;
            foreach (ListViewItem item in lvFileList.Items)
            {
                if (item.SubItems[4].Text.StartsWith("OK "))
                {
                    lvFileList.Items.Remove(item);
                    filelist.Remove(item.SubItems[COL_FULL_NAME].Text.ToLowerInvariant());
                }
                else
                    size += (long)item.Tag;
            }
            TotalSize = size;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            lvFileList.Items.Clear();
            filelist.Clear();
            TotalSize = 0;
        }

        private void LoadSettings()
        {
            var dir = Properties.Settings.Default.DestinationDir;
            if (System.IO.Directory.Exists(dir))
            {
                folderBrowserDialog1.SelectedPath = dir;
                txtDestination.Text = dir;
            }

            dir = Properties.Settings.Default.SourceDir;
            if (System.IO.Directory.Exists(dir))
            {
                openFileDialog1.InitialDirectory = dir;
            }

            cbGenSaveFiles.Checked = Properties.Settings.Default.GenerateSaveFiles;
            cbGroupOutput.Checked = Properties.Settings.Default.GroupOutput;
            cbAutoTrim.Checked = Properties.Settings.Default.AutoTrim;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.DestinationDir = txtDestination.Text;
            Properties.Settings.Default.SourceDir = openFileDialog1.InitialDirectory;
            Properties.Settings.Default.GenerateSaveFiles = cbGenSaveFiles.Checked;
            Properties.Settings.Default.GroupOutput = cbGroupOutput.Checked;
            Properties.Settings.Default.AutoTrim = cbAutoTrim.Checked;
            Properties.Settings.Default.Save();
        }

        private void EZPatcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        void ShowStatus(bool busy)
        {
            stlblProgress.Visible = busy;
            stprgProgress.Visible = busy;
            btnPatch.Enabled = !busy;
            btnCancel.Enabled = busy;
        }

        void UpdateProgress(int current, int max)
        {
            stlblProgress.Text = string.Format("{0}/{1}", current, max);
            stprgProgress.Value = current;
        }

        private void EZPatcher_Load(object sender, EventArgs e)
        {
            ShowStatus(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancalPatching = true;
        }

        private void lvFileList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            lvFileList.Sort();
        }

    }
}
