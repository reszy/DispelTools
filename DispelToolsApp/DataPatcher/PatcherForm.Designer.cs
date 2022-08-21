namespace DispelTools.DataPatcher
{
    partial class PatcherForm
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
            backgroundWorker.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputSelectButton = new System.Windows.Forms.Button();
            this.patchButton = new System.Windows.Forms.Button();
            this.outputSelectButton = new System.Windows.Forms.Button();
            this.outputDirectoryInfo = new System.Windows.Forms.Label();
            this.selectPatchesDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new DispelTools.Components.ProgressBarWithText();
            this.details = new DispelTools.Components.DetailsComponent();
            this.optionsButton = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.selectDestinationFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.selectionTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // inputSelectButton
            // 
            this.inputSelectButton.Location = new System.Drawing.Point(12, 12);
            this.inputSelectButton.Name = "inputSelectButton";
            this.inputSelectButton.Size = new System.Drawing.Size(141, 23);
            this.inputSelectButton.TabIndex = 0;
            this.inputSelectButton.Text = "Select patches";
            this.inputSelectButton.UseVisualStyleBackColor = true;
            this.inputSelectButton.Click += new System.EventHandler(this.selectPatchesButton_Click);
            // 
            // patchButton
            // 
            this.patchButton.Enabled = false;
            this.patchButton.Location = new System.Drawing.Point(12, 133);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new System.Drawing.Size(75, 37);
            this.patchButton.TabIndex = 1;
            this.patchButton.Text = "Patch";
            this.patchButton.UseVisualStyleBackColor = true;
            this.patchButton.Click += new System.EventHandler(this.patchButton_Click);
            // 
            // outputSelectButton
            // 
            this.outputSelectButton.Location = new System.Drawing.Point(158, 11);
            this.outputSelectButton.Name = "outputSelectButton";
            this.outputSelectButton.Size = new System.Drawing.Size(141, 23);
            this.outputSelectButton.TabIndex = 2;
            this.outputSelectButton.Text = "Select file to patch";
            this.outputSelectButton.UseVisualStyleBackColor = true;
            this.outputSelectButton.Click += new System.EventHandler(this.selectPatchedFileButton_Click);
            // 
            // outputDirectoryInfo
            // 
            this.outputDirectoryInfo.AutoEllipsis = true;
            this.outputDirectoryInfo.Location = new System.Drawing.Point(181, 65);
            this.outputDirectoryInfo.Name = "outputDirectoryInfo";
            this.outputDirectoryInfo.Size = new System.Drawing.Size(416, 13);
            this.outputDirectoryInfo.TabIndex = 6;
            // 
            // progressBar
            // 
            this.progressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressBar.Location = new System.Drawing.Point(12, 176);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(586, 45);
            this.progressBar.TabIndex = 10;
            // 
            // details
            // 
            this.details.Header = "Details";
            this.details.Location = new System.Drawing.Point(12, 227);
            this.details.MinimumSize = new System.Drawing.Size(100, 25);
            this.details.Name = "details";
            this.details.Size = new System.Drawing.Size(586, 25);
            this.details.TabIndex = 5;
            this.details.UnfoldSize = 240;
            // 
            // optionsButton
            // 
            this.optionsButton.Location = new System.Drawing.Point(513, 12);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(84, 22);
            this.optionsButton.TabIndex = 3;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ApplyPatchesToFile);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.PatchingProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.PatchingCompleted);
            // 
            // selectDestinationFileDialog
            // 
            this.selectDestinationFileDialog.FileName = "openFileDialog1";
            // 
            // selectionTextBox
            // 
            this.selectionTextBox.DetectUrls = false;
            this.selectionTextBox.Enabled = false;
            this.selectionTextBox.Location = new System.Drawing.Point(98, 42);
            this.selectionTextBox.Name = "selectionTextBox";
            this.selectionTextBox.Size = new System.Drawing.Size(409, 128);
            this.selectionTextBox.TabIndex = 11;
            this.selectionTextBox.Text = "";
            // 
            // PatcherForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(612, 267);
            this.Controls.Add(this.selectionTextBox);
            this.Controls.Add(this.optionsButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.details);
            this.Controls.Add(this.outputDirectoryInfo);
            this.Controls.Add(this.outputSelectButton);
            this.Controls.Add(this.patchButton);
            this.Controls.Add(this.inputSelectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PatcherForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Text = "ImageExtractor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button inputSelectButton;
        private System.Windows.Forms.Button patchButton;
        private System.Windows.Forms.Button outputSelectButton;
        private System.Windows.Forms.Label outputDirectoryInfo;
        private Components.DetailsComponent details;
        private Components.ProgressBarWithText progressBar;
        private System.Windows.Forms.OpenFileDialog selectPatchesDialog;
        private System.Windows.Forms.Button optionsButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.OpenFileDialog selectDestinationFileDialog;
        private System.Windows.Forms.RichTextBox selectionTextBox;
    }
}