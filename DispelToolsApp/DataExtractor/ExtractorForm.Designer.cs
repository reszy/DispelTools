namespace DispelTools.DataExtractor
{
    partial class ExtractorForm
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
            this.openButton = new System.Windows.Forms.Button();
            this.extractButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.directorySelectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.outputDirectoryInfo = new System.Windows.Forms.Label();
            this.selectedLabel = new System.Windows.Forms.Label();
            this.openOutputDirectoryButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new DispelTools.Components.ProgressBarWithText();
            this.details = new DispelTools.Components.DetailsComponent();
            this.optionsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 12);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 0;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // extractButton
            // 
            this.extractButton.Enabled = false;
            this.extractButton.Location = new System.Drawing.Point(12, 41);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 37);
            this.extractButton.TabIndex = 1;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(90, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Selected:";
            // 
            // directorySelectButton
            // 
            this.directorySelectButton.Location = new System.Drawing.Point(93, 12);
            this.directorySelectButton.Name = "directorySelectButton";
            this.directorySelectButton.Size = new System.Drawing.Size(141, 23);
            this.directorySelectButton.TabIndex = 2;
            this.directorySelectButton.Text = "Select output directory";
            this.directorySelectButton.UseVisualStyleBackColor = true;
            this.directorySelectButton.Click += new System.EventHandler(this.outputDirectoryButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(90, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Output directory:";
            // 
            // outputDirectoryInfo
            // 
            this.outputDirectoryInfo.AutoEllipsis = true;
            this.outputDirectoryInfo.Location = new System.Drawing.Point(181, 65);
            this.outputDirectoryInfo.Name = "outputDirectoryInfo";
            this.outputDirectoryInfo.Size = new System.Drawing.Size(416, 13);
            this.outputDirectoryInfo.TabIndex = 6;
            // 
            // selectedLabel
            // 
            this.selectedLabel.AutoEllipsis = true;
            this.selectedLabel.BackColor = System.Drawing.Color.Transparent;
            this.selectedLabel.Location = new System.Drawing.Point(148, 41);
            this.selectedLabel.Name = "selectedLabel";
            this.selectedLabel.Size = new System.Drawing.Size(449, 13);
            this.selectedLabel.TabIndex = 7;
            // 
            // openOutputDirectoryButton
            // 
            this.openOutputDirectoryButton.Enabled = false;
            this.openOutputDirectoryButton.Location = new System.Drawing.Point(447, 12);
            this.openOutputDirectoryButton.Name = "openOutputDirectoryButton";
            this.openOutputDirectoryButton.Size = new System.Drawing.Size(152, 23);
            this.openOutputDirectoryButton.TabIndex = 4;
            this.openOutputDirectoryButton.Text = "Open output directory";
            this.openOutputDirectoryButton.UseVisualStyleBackColor = true;
            this.openOutputDirectoryButton.Click += new System.EventHandler(this.openOutputDirectoryButton_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // progressBar
            // 
            this.progressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressBar.Location = new System.Drawing.Point(13, 84);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(586, 45);
            this.progressBar.TabIndex = 10;
            // 
            // details
            // 
            this.details.Header = "Details";
            this.details.Location = new System.Drawing.Point(13, 135);
            this.details.MinimumSize = new System.Drawing.Size(100, 25);
            this.details.Name = "details";
            this.details.Size = new System.Drawing.Size(586, 25);
            this.details.TabIndex = 5;
            this.details.UnfoldSize = 240;
            // 
            // optionsButton
            // 
            this.optionsButton.Enabled = false;
            this.optionsButton.Location = new System.Drawing.Point(295, 12);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(84, 22);
            this.optionsButton.TabIndex = 3;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = true;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // ExtractorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(612, 172);
            this.Controls.Add(this.optionsButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.details);
            this.Controls.Add(this.openOutputDirectoryButton);
            this.Controls.Add(this.selectedLabel);
            this.Controls.Add(this.outputDirectoryInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directorySelectButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.openButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ExtractorForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.Text = "ImageExtractor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button directorySelectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label outputDirectoryInfo;
        private System.Windows.Forms.Label selectedLabel;
        private System.Windows.Forms.Button openOutputDirectoryButton;
        private Components.DetailsComponent details;
        private Components.ProgressBarWithText progressBar;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button optionsButton;
    }
}