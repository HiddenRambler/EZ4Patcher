namespace EZ4Patcher
{
    partial class EZPatcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EZPatcher));
            this.btnDestinationSelect = new System.Windows.Forms.Button();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.cmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmOutputName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmProcessed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnPatch = new System.Windows.Forms.Button();
            this.cbOverwrite = new System.Windows.Forms.CheckBox();
            this.btnClearSuccessful = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.cbGenSaveFiles = new System.Windows.Forms.CheckBox();
            this.prgProgress = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDestinationSelect
            // 
            this.btnDestinationSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDestinationSelect.Location = new System.Drawing.Point(582, 381);
            this.btnDestinationSelect.Name = "btnDestinationSelect";
            this.btnDestinationSelect.Size = new System.Drawing.Size(75, 23);
            this.btnDestinationSelect.TabIndex = 0;
            this.btnDestinationSelect.Text = "Select";
            this.btnDestinationSelect.UseVisualStyleBackColor = true;
            this.btnDestinationSelect.Click += new System.EventHandler(this.btnDestinationSelect_Click);
            // 
            // txtDestination
            // 
            this.txtDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDestination.Location = new System.Drawing.Point(66, 383);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(510, 20);
            this.txtDestination.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Output Dir";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select output path";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.gba";
            this.openFileDialog1.Filter = "GBA Roms|*.gba|All Files|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select Roms";
            // 
            // lvFileList
            // 
            this.lvFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cmName,
            this.cmPath,
            this.cmSize,
            this.cmOutputName,
            this.cmProcessed});
            this.lvFileList.Location = new System.Drawing.Point(3, 42);
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.Size = new System.Drawing.Size(654, 333);
            this.lvFileList.TabIndex = 3;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            // 
            // cmName
            // 
            this.cmName.Text = "Name";
            this.cmName.Width = 213;
            // 
            // cmPath
            // 
            this.cmPath.Text = "Path";
            this.cmPath.Width = 36;
            // 
            // cmSize
            // 
            this.cmSize.Text = "Size";
            this.cmSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmOutputName
            // 
            this.cmOutputName.Text = "OutputName";
            this.cmOutputName.Width = 191;
            // 
            // cmProcessed
            // 
            this.cmProcessed.Text = "Processed";
            this.cmProcessed.Width = 112;
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFiles.Location = new System.Drawing.Point(344, 12);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(75, 23);
            this.btnAddFiles.TabIndex = 4;
            this.btnAddFiles.Text = "Add Files";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // btnPatch
            // 
            this.btnPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPatch.Location = new System.Drawing.Point(582, 410);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(75, 23);
            this.btnPatch.TabIndex = 5;
            this.btnPatch.Text = "Patch";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // cbOverwrite
            // 
            this.cbOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbOverwrite.AutoSize = true;
            this.cbOverwrite.Location = new System.Drawing.Point(131, 414);
            this.cbOverwrite.Name = "cbOverwrite";
            this.cbOverwrite.Size = new System.Drawing.Size(71, 17);
            this.cbOverwrite.TabIndex = 6;
            this.cbOverwrite.Text = "Overwrite";
            this.cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnClearSuccessful
            // 
            this.btnClearSuccessful.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSuccessful.Location = new System.Drawing.Point(454, 12);
            this.btnClearSuccessful.Name = "btnClearSuccessful";
            this.btnClearSuccessful.Size = new System.Drawing.Size(98, 23);
            this.btnClearSuccessful.TabIndex = 7;
            this.btnClearSuccessful.Text = "Clear Successful";
            this.btnClearSuccessful.UseVisualStyleBackColor = true;
            this.btnClearSuccessful.Click += new System.EventHandler(this.btnClearSuccessful_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Location = new System.Drawing.Point(558, 12);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(98, 23);
            this.btnClearAll.TabIndex = 8;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // cbGenSaveFiles
            // 
            this.cbGenSaveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbGenSaveFiles.AutoSize = true;
            this.cbGenSaveFiles.Checked = true;
            this.cbGenSaveFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenSaveFiles.Location = new System.Drawing.Point(3, 414);
            this.cbGenSaveFiles.Name = "cbGenSaveFiles";
            this.cbGenSaveFiles.Size = new System.Drawing.Size(122, 17);
            this.cbGenSaveFiles.TabIndex = 9;
            this.cbGenSaveFiles.Text = "Generate Save Files";
            this.cbGenSaveFiles.UseVisualStyleBackColor = true;
            // 
            // prgProgress
            // 
            this.prgProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgProgress.Location = new System.Drawing.Point(209, 414);
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(367, 19);
            this.prgProgress.Step = 1;
            this.prgProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgProgress.TabIndex = 10;
            this.prgProgress.Value = 50;
            this.prgProgress.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(12, 17);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(27, 13);
            this.lblProgress.TabIndex = 11;
            this.lblProgress.Text = "0/0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(582, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EZPatcher
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 445);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.prgProgress);
            this.Controls.Add(this.cbGenSaveFiles);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnClearSuccessful);
            this.Controls.Add(this.cbOverwrite);
            this.Controls.Add(this.btnPatch);
            this.Controls.Add(this.btnAddFiles);
            this.Controls.Add(this.lvFileList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.btnDestinationSelect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EZPatcher";
            this.Text = "EZPatcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EZPatcher_FormClosing);
            this.Load += new System.EventHandler(this.EZPatcher_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.EZPatcher_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.EZPatcher_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDestinationSelect;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.ColumnHeader cmName;
        private System.Windows.Forms.ColumnHeader cmPath;
        private System.Windows.Forms.ColumnHeader cmSize;
        private System.Windows.Forms.ColumnHeader cmOutputName;
        private System.Windows.Forms.ColumnHeader cmProcessed;
        private System.Windows.Forms.CheckBox cbOverwrite;
        private System.Windows.Forms.Button btnClearSuccessful;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.CheckBox cbGenSaveFiles;
        private System.Windows.Forms.ProgressBar prgProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnCancel;
    }
}

