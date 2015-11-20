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
        public EZPatcher()
        {
            InitializeComponent();

            //list view doesnt retain design time column sizes
            lvFileList.Columns[0].Width = 213;
            lvFileList.Columns[1].Width = 36;
            lvFileList.Columns[2].Width = 60;
            lvFileList.Columns[3].Width = 191;
            lvFileList.Columns[4].Width = 60;

            LoadSettings();
        }

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
            foreach(var f in filenames){
                var item = new string[6];
                item[0] = System.IO.Path.GetFileName(f);
                item[1] = System.IO.Path.GetDirectoryName(f);
                item[2] = GetBytesReadable(new System.IO.FileInfo(f).Length);
                item[3] = item[0]; //TODO: some cleaned up name
                item[4] = "";
                item[5] = f;

                var lv = new ListViewItem(item);
                lvFileList.Items.Add(lv);
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

        private void btnPatch_Click(object sender, EventArgs e)
        {
            var outputdir = txtDestination.Text;
            var overwrite = cbOverwrite.Checked;
            var gensavefile = cbGenSaveFiles.Checked;

            if (!System.IO.Directory.Exists(outputdir))
            {
                MessageBox.Show("Invalid destination", "Error");
            }

            foreach (ListViewItem item in lvFileList.Items)
            {
                if (item.SubItems[4].Text.StartsWith("OK "))
                    continue;

                var source = item.SubItems[5].Text;
                var destination = System.IO.Path.Combine(outputdir, item.SubItems[3].Text);
                item.SubItems[4].Text = PatchFile(source, destination, overwrite, gensavefile);
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
            lvFileList.Items.Clear();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.Items)
            {
                if (item.SubItems[4].Text.StartsWith("OK "))
                    lvFileList.Items.Remove(item);
            }
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

    }
}
