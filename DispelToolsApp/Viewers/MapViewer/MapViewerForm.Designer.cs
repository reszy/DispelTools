namespace DispelTools.Viewers.MapViewer
{
    partial class MapViewerForm
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

            backgroundWorker.DoWork -= LoadMap;
            backgroundWorker.ProgressChanged -= ProgressChanged;
            backgroundWorker.RunWorkerCompleted -= LoadingCompleted;

            image?.Dispose();
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;

            tileDiplayer.Image?.Dispose();
            tileDiplayer.Image = null;
            backgroundWorker?.Dispose();
            mapReader?.Dispose();
            mapReader = null;
            image = null;

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
            this.tileShowNumber = new System.Windows.Forms.NumericUpDown();
            this.tileSetCombo = new System.Windows.Forms.ComboBox();
            this.collisionsCheckBox = new System.Windows.Forms.CheckBox();
            this.btlCheckBox = new System.Windows.Forms.CheckBox();
            this.bldgCheckBox = new System.Windows.Forms.CheckBox();
            this.gtlCheckBox = new System.Windows.Forms.CheckBox();
            this.tileDiplayer = new DispelTools.Components.PictureDiplayer();
            this.progressBar = new DispelTools.Components.ProgressBarWithText();
            this.pictureBox1 = new DispelTools.Components.PictureDiplayer();
            this.generateButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tileShowNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileDiplayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 3);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // tileShowNumber
            // 
            this.tileShowNumber.Location = new System.Drawing.Point(1037, 98);
            this.tileShowNumber.Name = "tileShowNumber";
            this.tileShowNumber.Size = new System.Drawing.Size(124, 20);
            this.tileShowNumber.TabIndex = 3;
            this.tileShowNumber.ValueChanged += new System.EventHandler(this.tileShowNumber_ValueChanged);
            // 
            // tileSetCombo
            // 
            this.tileSetCombo.FormattingEnabled = true;
            this.tileSetCombo.Items.AddRange(new object[] {
            "GTL",
            "BTL"});
            this.tileSetCombo.Location = new System.Drawing.Point(1037, 71);
            this.tileSetCombo.Name = "tileSetCombo";
            this.tileSetCombo.Size = new System.Drawing.Size(124, 21);
            this.tileSetCombo.TabIndex = 6;
            this.tileSetCombo.SelectedIndexChanged += new System.EventHandler(this.tileSetCombo_SelectedIndexChanged);
            // 
            // collisionsCheckBox
            // 
            this.collisionsCheckBox.AutoSize = true;
            this.collisionsCheckBox.Location = new System.Drawing.Point(1038, 364);
            this.collisionsCheckBox.Name = "collisionsCheckBox";
            this.collisionsCheckBox.Size = new System.Drawing.Size(69, 17);
            this.collisionsCheckBox.TabIndex = 7;
            this.collisionsCheckBox.Text = "Collisions";
            this.collisionsCheckBox.UseVisualStyleBackColor = true;
            this.collisionsCheckBox.CheckedChanged += new System.EventHandler(this.collisionsCheckBox_CheckedChanged);
            // 
            // btlCheckBox
            // 
            this.btlCheckBox.AutoSize = true;
            this.btlCheckBox.Enabled = false;
            this.btlCheckBox.Location = new System.Drawing.Point(1038, 388);
            this.btlCheckBox.Name = "btlCheckBox";
            this.btlCheckBox.Size = new System.Drawing.Size(46, 17);
            this.btlCheckBox.TabIndex = 8;
            this.btlCheckBox.Text = "BTL";
            this.btlCheckBox.UseVisualStyleBackColor = true;
            this.btlCheckBox.CheckedChanged += new System.EventHandler(this.btlCheckBox_CheckedChanged);
            // 
            // bldgCheckBox
            // 
            this.bldgCheckBox.AutoSize = true;
            this.bldgCheckBox.Enabled = false;
            this.bldgCheckBox.Location = new System.Drawing.Point(1038, 412);
            this.bldgCheckBox.Name = "bldgCheckBox";
            this.bldgCheckBox.Size = new System.Drawing.Size(55, 17);
            this.bldgCheckBox.TabIndex = 9;
            this.bldgCheckBox.Text = "BLDG";
            this.bldgCheckBox.UseVisualStyleBackColor = true;
            this.bldgCheckBox.CheckedChanged += new System.EventHandler(this.bldgCheckBox_CheckedChanged);
            // 
            // gtlCheckBox
            // 
            this.gtlCheckBox.AutoSize = true;
            this.gtlCheckBox.Checked = true;
            this.gtlCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gtlCheckBox.Location = new System.Drawing.Point(1037, 341);
            this.gtlCheckBox.Name = "gtlCheckBox";
            this.gtlCheckBox.Size = new System.Drawing.Size(47, 17);
            this.gtlCheckBox.TabIndex = 10;
            this.gtlCheckBox.Text = "GTL";
            this.gtlCheckBox.UseVisualStyleBackColor = true;
            this.gtlCheckBox.CheckedChanged += new System.EventHandler(this.gtlCheckBox_CheckedChanged);
            // 
            // tileDiplayer
            // 
            this.tileDiplayer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tileDiplayer.CurrentMouseMode = DispelTools.Components.PictureDiplayer.MouseMode.Pointer;
            this.tileDiplayer.Font = new System.Drawing.Font("Courier New", 10F);
            this.tileDiplayer.Image = null;
            this.tileDiplayer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.tileDiplayer.Location = new System.Drawing.Point(1037, 124);
            this.tileDiplayer.Name = "tileDiplayer";
            this.tileDiplayer.Size = new System.Drawing.Size(124, 110);
            this.tileDiplayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tileDiplayer.TabIndex = 5;
            this.tileDiplayer.TabStop = false;
            this.tileDiplayer.ToolTip = false;
            // 
            // progressBar
            // 
            this.progressBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressBar.CausesValidation = false;
            this.progressBar.Location = new System.Drawing.Point(149, 702);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(882, 40);
            this.progressBar.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.CurrentMouseMode = DispelTools.Components.PictureDiplayer.MouseMode.Pointer;
            this.pictureBox1.Font = new System.Drawing.Font("Courier New", 10F);
            this.pictureBox1.Image = null;
            this.pictureBox1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pictureBox1.Location = new System.Drawing.Point(12, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1019, 664);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(12, 703);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(131, 39);
            this.generateButton.TabIndex = 11;
            this.generateButton.Text = "GENERATE";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Dispel map|*.map";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(486, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "debugButton";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MapViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 754);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.gtlCheckBox);
            this.Controls.Add(this.bldgCheckBox);
            this.Controls.Add(this.btlCheckBox);
            this.Controls.Add(this.collisionsCheckBox);
            this.Controls.Add(this.tileSetCombo);
            this.Controls.Add(this.tileDiplayer);
            this.Controls.Add(this.tileShowNumber);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MapViewerForm";
            this.Text = "MapViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.tileShowNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileDiplayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PictureDiplayer pictureBox1;
        private System.Windows.Forms.Button openButton;
        private Components.ProgressBarWithText progressBar;
        private System.Windows.Forms.NumericUpDown tileShowNumber;
        private Components.PictureDiplayer tileDiplayer;
        private System.Windows.Forms.ComboBox tileSetCombo;
        private System.Windows.Forms.CheckBox collisionsCheckBox;
        private System.Windows.Forms.CheckBox btlCheckBox;
        private System.Windows.Forms.CheckBox bldgCheckBox;
        private System.Windows.Forms.CheckBox gtlCheckBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button button1;
    }
}