namespace DispelTools.ImageAnalyzer
{
    partial class ImageAnalyzerForm
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
            this.previewButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.imageAlignControls = new DispelTools.ImageAnalyzer.ImageAlignControls();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imageEditControls = new DispelTools.ImageAnalyzer.ImageEditControls();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.imageAnalyzeControls = new DispelTools.ImageAnalyzer.ImageAnalyzeControls();
            this.overwriteButton = new System.Windows.Forms.Button();
            this.pictureDisplayer = new DispelTools.Components.PictureDiplayer();
            this.reloadButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // previewButton
            // 
            this.previewButton.Location = new System.Drawing.Point(12, 564);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(136, 56);
            this.previewButton.TabIndex = 1;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.previewButton_Click);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 19);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 14;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(104, 28);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(0, 13);
            this.filenameLabel.TabIndex = 22;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(362, 593);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(120, 23);
            this.saveButton.TabIndex = 23;
            this.saveButton.Text = "Save Preview";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Location = new System.Drawing.Point(12, 48);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(474, 416);
            this.tabControl.TabIndex = 27;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.imageAlignControls);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(466, 390);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Align";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // imageAlignControls
            // 
            this.imageAlignControls.Location = new System.Drawing.Point(3, 6);
            this.imageAlignControls.MinimumSize = new System.Drawing.Size(100, 25);
            this.imageAlignControls.Name = "imageAlignControls";
            this.imageAlignControls.Size = new System.Drawing.Size(424, 289);
            this.imageAlignControls.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.imageEditControls);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(466, 390);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edit";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imageEditControls
            // 
            this.imageEditControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageEditControls.Location = new System.Drawing.Point(3, 3);
            this.imageEditControls.Name = "imageEditControls";
            this.imageEditControls.Size = new System.Drawing.Size(460, 384);
            this.imageEditControls.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.imageAnalyzeControls);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(466, 390);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Analyze";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // imageAnalyzeControls
            // 
            this.imageAnalyzeControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageAnalyzeControls.Location = new System.Drawing.Point(3, 3);
            this.imageAnalyzeControls.Name = "imageAnalyzeControls";
            this.imageAnalyzeControls.Size = new System.Drawing.Size(460, 384);
            this.imageAnalyzeControls.TabIndex = 25;
            // 
            // overwriteButton
            // 
            this.overwriteButton.Location = new System.Drawing.Point(362, 564);
            this.overwriteButton.Name = "overwriteButton";
            this.overwriteButton.Size = new System.Drawing.Size(120, 23);
            this.overwriteButton.TabIndex = 28;
            this.overwriteButton.Text = "Overwrite File";
            this.overwriteButton.UseVisualStyleBackColor = true;
            this.overwriteButton.Click += new System.EventHandler(this.overwriteButton_Click);
            // 
            // pictureDisplayer
            // 
            this.pictureDisplayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureDisplayer.CurrentMouseMode = DispelTools.Components.PictureDiplayer.MouseMode.Pointer;
            this.pictureDisplayer.Font = new System.Drawing.Font("Courier New", 10F);
            this.pictureDisplayer.Image = null;
            this.pictureDisplayer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pictureDisplayer.Location = new System.Drawing.Point(489, 9);
            this.pictureDisplayer.Margin = new System.Windows.Forms.Padding(0);
            this.pictureDisplayer.Name = "pictureDisplayer";
            this.pictureDisplayer.Padding = new System.Windows.Forms.Padding(5);
            this.pictureDisplayer.Size = new System.Drawing.Size(586, 611);
            this.pictureDisplayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureDisplayer.TabIndex = 0;
            this.pictureDisplayer.TabStop = false;
            // 
            // reloadButton
            // 
            this.reloadButton.Location = new System.Drawing.Point(12, 535);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 29;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click);
            // 
            // ImageAnalyzerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 632);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.overwriteButton);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.filenameLabel);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.previewButton);
            this.Controls.Add(this.pictureDisplayer);
            this.Name = "ImageAnalyzerForm";
            this.Text = "ImageAnalyzer";
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureDisplayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PictureDiplayer pictureDisplayer;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private ImageAlignControls imageAlignControls;
        private System.Windows.Forms.Button overwriteButton;
        private ImageAnalyzeControls imageAnalyzeControls;
        private ImageEditControls imageEditControls;
        private System.Windows.Forms.Button reloadButton;
    }
}

