﻿using System;
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

        public EZPatcher()
        {
            InitializeComponent();

            //list view doesnt retain design time column sizes
            lvFileList.Columns[COL_FNAME].Width = 213;
            lvFileList.Columns[COL_DIR].Width = 36;
            lvFileList.Columns[COL_SIZE].Width = 60;
            lvFileList.Columns[COL_OUTNAME].Width = 191;
            lvFileList.Columns[COL_RESULT].Width = 112;

            LoadSettings();
        }


        private long _TotalSize;
        private long TotalSize { get { return _TotalSize; } set { _TotalSize = value; stlblSummary.Text = GetBytesReadable(value); } }
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
            long totsize = 0;
            foreach(var f in filenames) {
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

        private void btnPatch_Click(object sender, EventArgs e)
        {
            CancalPatching = false;

            var outputdir = txtDestination.Text;
            var overwrite = cbOverwrite.Checked;
            var gensavefile = cbGenSaveFiles.Checked;

            if (!System.IO.Directory.Exists(outputdir))
            {
                MessageBox.Show("Invalid destination", "Error");
            }

            ShowStatus(true);
            try
            {
                stprgProgress.Value = 0;
                stprgProgress.Maximum = lvFileList.Items.Count;

                int count = lvFileList.Items.Count;
                for(int i=0 ;i<count; i++)
                {
                    if (CancalPatching) break;

                    ListViewItem item = lvFileList.Items[i];
                    if (item.SubItems[COL_RESULT].Text.StartsWith("OK "))
                        continue;

                    var source = item.SubItems[COL_FULL_NAME].Text;
                    var destination = System.IO.Path.Combine(outputdir, item.SubItems[COL_OUTNAME].Text);
                    item.SubItems[COL_RESULT].Text = PatchFile(source, destination, overwrite, gensavefile);
                    Application.DoEvents();
                    UpdateProgress(i, count);
                }
            }
            finally
            {
                ShowStatus(false);
            }
        }

        void GenerateSaveFile(string filename, bool overwrite, int size)
        {
            if (size < 1) return;

            var str = new string('\xFF', size);
            System.IO.File.WriteAllText(filename, str);
        }

        string PatchFile(string source, string destination, bool overwrite, bool generateSaver)
        {
            if (source.ToLowerInvariant() == destination.ToLowerInvariant())
                return "Invalid Destination";

            if (!overwrite && System.IO.File.Exists(destination))
                return "Already Exists";

            try
            {
                var data = System.IO.File.ReadAllBytes(source);
                uint size = 0;
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
                    var dest = System.IO.Path.GetDirectoryName(destination);
                    dest = System.IO.Path.Combine(dest, "saver");
                    if (!System.IO.Directory.Exists(dest))
                        System.IO.Directory.CreateDirectory(dest);

                    var destname = System.IO.Path.GetFileName(destination);
                    var saverfile = System.IO.Path.Combine(dest, destname);
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
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.DestinationDir = txtDestination.Text;
            Properties.Settings.Default.SourceDir = openFileDialog1.InitialDirectory;
            Properties.Settings.Default.GenerateSaveFiles = cbGenSaveFiles.Checked;
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

    }
}
