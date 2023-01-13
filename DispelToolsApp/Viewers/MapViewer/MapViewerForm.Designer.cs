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

            mapImage?.Dispose();
            sidePreviewImage?.Dispose();
            pictureBox.Image?.Dispose();
            pictureBox.Image = null;

            tileDisplayer.Image?.Dispose();
            tileDisplayer.Image = null;
            backgroundWorker?.Dispose();
            mapContainer?.Dispose();
            mapContainer = null;
            mapImage = null;
            sidePreviewImage = null;

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openButton = new System.Windows.Forms.Button();
            this.tileShowNumber = new System.Windows.Forms.NumericUpDown();
            this.tileSetCombo = new System.Windows.Forms.ComboBox();
            this.collisionsCheckBox = new System.Windows.Forms.CheckBox();
            this.btlCheckBox = new System.Windows.Forms.CheckBox();
            this.roofsCheckBox = new System.Windows.Forms.CheckBox();
            this.gtlCheckBox = new System.Windows.Forms.CheckBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.debugButton = new System.Windows.Forms.Button();
            this.spritesCheckBox = new System.Windows.Forms.CheckBox();
            this.statsTextBox = new System.Windows.Forms.RichTextBox();
            this.occludeCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.eventsCheckBox = new System.Windows.Forms.CheckBox();
            this.saveAsImageButton = new System.Windows.Forms.Button();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.labeledSeparator1 = new DispelTools.Components.LabeledSeparator();
            this.tileDisplayer = new DispelTools.Components.PictureDisplay.PictureDisplayer();
            this.progressBar = new DispelTools.Components.ProgressBarWithText();
            this.pictureBox = new DispelTools.Components.PictureDisplay.PictureDisplayer();
            this.labeledSeparator2 = new DispelTools.Components.LabeledSeparator();
            this.labeledSeparator3 = new DispelTools.Components.LabeledSeparator();
            this.labeledSeparator4 = new DispelTools.Components.LabeledSeparator();
            this.npcCheckBox = new System.Windows.Forms.CheckBox();
            this.monsterCheckBox = new System.Windows.Forms.CheckBox();
            this.extraCheckBox = new System.Windows.Forms.CheckBox();
            this.debugDotsCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tileShowNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileDisplayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
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
            this.tileShowNumber.Location = new System.Drawing.Point(1037, 59);
            this.tileShowNumber.Name = "tileShowNumber";
            this.tileShowNumber.Size = new System.Drawing.Size(132, 20);
            this.tileShowNumber.TabIndex = 3;
            this.tileShowNumber.ValueChanged += new System.EventHandler(this.tileShowNumber_ValueChanged);
            // 
            // tileSetCombo
            // 
            this.tileSetCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tileSetCombo.FormattingEnabled = true;
            this.tileSetCombo.Items.AddRange(new object[] {
            "GTL",
            "BTL",
            "Sprite"});
            this.tileSetCombo.Location = new System.Drawing.Point(1037, 32);
            this.tileSetCombo.Name = "tileSetCombo";
            this.tileSetCombo.Size = new System.Drawing.Size(132, 21);
            this.tileSetCombo.TabIndex = 6;
            this.tileSetCombo.SelectedIndexChanged += new System.EventHandler(this.tileSetCombo_SelectedIndexChanged);
            // 
            // collisionsCheckBox
            // 
            this.collisionsCheckBox.AutoSize = true;
            this.collisionsCheckBox.Location = new System.Drawing.Point(1037, 267);
            this.collisionsCheckBox.Name = "collisionsCheckBox";
            this.collisionsCheckBox.Size = new System.Drawing.Size(69, 17);
            this.collisionsCheckBox.TabIndex = 7;
            this.collisionsCheckBox.Text = "Collisions";
            this.collisionsCheckBox.UseVisualStyleBackColor = true;
            // 
            // btlCheckBox
            // 
            this.btlCheckBox.AutoSize = true;
            this.btlCheckBox.Location = new System.Drawing.Point(1037, 332);
            this.btlCheckBox.Name = "btlCheckBox";
            this.btlCheckBox.Size = new System.Drawing.Size(46, 17);
            this.btlCheckBox.TabIndex = 8;
            this.btlCheckBox.Text = "BTL";
            this.btlCheckBox.UseVisualStyleBackColor = true;
            // 
            // roofsCheckBox
            // 
            this.roofsCheckBox.AutoSize = true;
            this.roofsCheckBox.Location = new System.Drawing.Point(1037, 486);
            this.roofsCheckBox.Name = "roofsCheckBox";
            this.roofsCheckBox.Size = new System.Drawing.Size(54, 17);
            this.roofsCheckBox.TabIndex = 9;
            this.roofsCheckBox.Text = "Roofs";
            this.roofsCheckBox.UseVisualStyleBackColor = true;
            // 
            // gtlCheckBox
            // 
            this.gtlCheckBox.AutoSize = true;
            this.gtlCheckBox.Checked = true;
            this.gtlCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gtlCheckBox.Location = new System.Drawing.Point(1037, 244);
            this.gtlCheckBox.Name = "gtlCheckBox";
            this.gtlCheckBox.Size = new System.Drawing.Size(47, 17);
            this.gtlCheckBox.TabIndex = 10;
            this.gtlCheckBox.Text = "GTL";
            this.gtlCheckBox.UseVisualStyleBackColor = true;
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
            // debugButton
            // 
            this.debugButton.Location = new System.Drawing.Point(465, 3);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(82, 23);
            this.debugButton.TabIndex = 12;
            this.debugButton.Text = "debugButton";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.debugButton_Click);
            // 
            // spritesCheckBox
            // 
            this.spritesCheckBox.AutoSize = true;
            this.spritesCheckBox.Location = new System.Drawing.Point(1037, 355);
            this.spritesCheckBox.Name = "spritesCheckBox";
            this.spritesCheckBox.Size = new System.Drawing.Size(58, 17);
            this.spritesCheckBox.TabIndex = 13;
            this.spritesCheckBox.Text = "Sprites";
            this.spritesCheckBox.UseVisualStyleBackColor = true;
            // 
            // statsTextBox
            // 
            this.statsTextBox.Location = new System.Drawing.Point(1037, 509);
            this.statsTextBox.Name = "statsTextBox";
            this.statsTextBox.ReadOnly = true;
            this.statsTextBox.Size = new System.Drawing.Size(132, 187);
            this.statsTextBox.TabIndex = 14;
            this.statsTextBox.Text = "";
            // 
            // occludeCheckBox
            // 
            this.occludeCheckBox.AutoSize = true;
            this.occludeCheckBox.Checked = true;
            this.occludeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.occludeCheckBox.Location = new System.Drawing.Point(1037, 202);
            this.occludeCheckBox.Name = "occludeCheckBox";
            this.occludeCheckBox.Size = new System.Drawing.Size(66, 17);
            this.occludeCheckBox.TabIndex = 15;
            this.occludeCheckBox.Text = "Occlude";
            this.occludeCheckBox.UseVisualStyleBackColor = true;
            // 
            // eventsCheckBox
            // 
            this.eventsCheckBox.AutoSize = true;
            this.eventsCheckBox.Location = new System.Drawing.Point(1037, 290);
            this.eventsCheckBox.Name = "eventsCheckBox";
            this.eventsCheckBox.Size = new System.Drawing.Size(59, 17);
            this.eventsCheckBox.TabIndex = 16;
            this.eventsCheckBox.Text = "Events";
            this.eventsCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveAsImageButton
            // 
            this.saveAsImageButton.Enabled = false;
            this.saveAsImageButton.Location = new System.Drawing.Point(1037, 702);
            this.saveAsImageButton.Name = "saveAsImageButton";
            this.saveAsImageButton.Size = new System.Drawing.Size(132, 40);
            this.saveAsImageButton.TabIndex = 17;
            this.saveAsImageButton.Text = "Save As Image";
            this.saveAsImageButton.UseVisualStyleBackColor = true;
            this.saveAsImageButton.Click += new System.EventHandler(this.saveAsImageButton_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "JPEG *.jpg|*.jpg;*.jpeg";
            // 
            // labeledSeparator1
            // 
            this.labeledSeparator1.Label = "Base Layer";
            this.labeledSeparator1.Location = new System.Drawing.Point(1037, 225);
            this.labeledSeparator1.Name = "labeledSeparator1";
            this.labeledSeparator1.Size = new System.Drawing.Size(132, 13);
            this.labeledSeparator1.TabIndex = 18;
            // 
            // tileDiplayer
            // 
            this.tileDisplayer.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tileDisplayer.CurrentMouseMode = DispelTools.Components.PictureDisplay.PictureDisplayer.MouseMode.Pointer;
            this.tileDisplayer.DebugText = "";
            this.tileDisplayer.Font = new System.Drawing.Font("Courier New", 10F);
            this.tileDisplayer.Image = null;
            this.tileDisplayer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.tileDisplayer.Location = new System.Drawing.Point(1037, 85);
            this.tileDisplayer.Name = "tileDiplayer";
            this.tileDisplayer.OffsetTileSelector = false;
            this.tileDisplayer.Size = new System.Drawing.Size(132, 110);
            this.tileDisplayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.tileDisplayer.TabIndex = 5;
            this.tileDisplayer.TabStop = false;
            this.tileDisplayer.ToolTip = false;
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
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox.CurrentMouseMode = DispelTools.Components.PictureDisplay.PictureDisplayer.MouseMode.Pointer;
            this.pictureBox.DebugText = "";
            this.pictureBox.Font = new System.Drawing.Font("Courier New", 10F);
            this.pictureBox.Image = null;
            this.pictureBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pictureBox.Location = new System.Drawing.Point(12, 32);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.OffsetTileSelector = false;
            this.pictureBox.Size = new System.Drawing.Size(1019, 664);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // labeledSeparator2
            // 
            this.labeledSeparator2.Label = "Internal objects";
            this.labeledSeparator2.Location = new System.Drawing.Point(1037, 313);
            this.labeledSeparator2.Name = "labeledSeparator2";
            this.labeledSeparator2.Size = new System.Drawing.Size(132, 13);
            this.labeledSeparator2.TabIndex = 19;
            // 
            // labeledSeparator3
            // 
            this.labeledSeparator3.Label = "External ebjects";
            this.labeledSeparator3.Location = new System.Drawing.Point(1038, 379);
            this.labeledSeparator3.Name = "labeledSeparator3";
            this.labeledSeparator3.Size = new System.Drawing.Size(132, 13);
            this.labeledSeparator3.TabIndex = 20;
            // 
            // labeledSeparator4
            // 
            this.labeledSeparator4.Label = "Last layer";
            this.labeledSeparator4.Location = new System.Drawing.Point(1037, 467);
            this.labeledSeparator4.Name = "labeledSeparator4";
            this.labeledSeparator4.Size = new System.Drawing.Size(132, 13);
            this.labeledSeparator4.TabIndex = 21;
            // 
            // npcCheckBox
            // 
            this.npcCheckBox.AutoSize = true;
            this.npcCheckBox.Location = new System.Drawing.Point(1037, 444);
            this.npcCheckBox.Name = "npcCheckBox";
            this.npcCheckBox.Size = new System.Drawing.Size(53, 17);
            this.npcCheckBox.TabIndex = 22;
            this.npcCheckBox.Text = "NPCs";
            this.npcCheckBox.UseVisualStyleBackColor = true;
            // 
            // monsterCheckBox
            // 
            this.monsterCheckBox.AutoSize = true;
            this.monsterCheckBox.Location = new System.Drawing.Point(1037, 421);
            this.monsterCheckBox.Name = "monsterCheckBox";
            this.monsterCheckBox.Size = new System.Drawing.Size(69, 17);
            this.monsterCheckBox.TabIndex = 23;
            this.monsterCheckBox.Text = "Monsters";
            this.monsterCheckBox.UseVisualStyleBackColor = true;
            // 
            // extraCheckBox
            // 
            this.extraCheckBox.AutoSize = true;
            this.extraCheckBox.Location = new System.Drawing.Point(1037, 398);
            this.extraCheckBox.Name = "extraCheckBox";
            this.extraCheckBox.Size = new System.Drawing.Size(55, 17);
            this.extraCheckBox.TabIndex = 24;
            this.extraCheckBox.Text = "Extras";
            this.extraCheckBox.UseVisualStyleBackColor = true;
            // 
            // debugDotsCheckBox
            // 
            this.debugDotsCheckBox.AutoSize = true;
            this.debugDotsCheckBox.Location = new System.Drawing.Point(653, 9);
            this.debugDotsCheckBox.Name = "debugDotsCheckBox";
            this.debugDotsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.debugDotsCheckBox.TabIndex = 25;
            this.debugDotsCheckBox.Text = "DebugDots";
            this.debugDotsCheckBox.UseVisualStyleBackColor = true;
            // 
            // MapViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 754);
            this.Controls.Add(this.debugDotsCheckBox);
            this.Controls.Add(this.extraCheckBox);
            this.Controls.Add(this.monsterCheckBox);
            this.Controls.Add(this.npcCheckBox);
            this.Controls.Add(this.labeledSeparator4);
            this.Controls.Add(this.labeledSeparator3);
            this.Controls.Add(this.labeledSeparator2);
            this.Controls.Add(this.labeledSeparator1);
            this.Controls.Add(this.saveAsImageButton);
            this.Controls.Add(this.eventsCheckBox);
            this.Controls.Add(this.occludeCheckBox);
            this.Controls.Add(this.statsTextBox);
            this.Controls.Add(this.spritesCheckBox);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.gtlCheckBox);
            this.Controls.Add(this.roofsCheckBox);
            this.Controls.Add(this.btlCheckBox);
            this.Controls.Add(this.collisionsCheckBox);
            this.Controls.Add(this.tileSetCombo);
            this.Controls.Add(this.tileDisplayer);
            this.Controls.Add(this.tileShowNumber);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.pictureBox);
            this.Name = "MapViewerForm";
            this.Text = "MapViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.tileShowNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileDisplayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.PictureDisplay.PictureDisplayer pictureBox;
        private System.Windows.Forms.Button openButton;
        private Components.ProgressBarWithText progressBar;
        private System.Windows.Forms.NumericUpDown tileShowNumber;
        private Components.PictureDisplay.PictureDisplayer tileDisplayer;
        private System.Windows.Forms.ComboBox tileSetCombo;
        private System.Windows.Forms.CheckBox collisionsCheckBox;
        private System.Windows.Forms.CheckBox btlCheckBox;
        private System.Windows.Forms.CheckBox roofsCheckBox;
        private System.Windows.Forms.CheckBox gtlCheckBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button debugButton;
        private System.Windows.Forms.CheckBox spritesCheckBox;
        private System.Windows.Forms.RichTextBox statsTextBox;
        private System.Windows.Forms.CheckBox occludeCheckBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox eventsCheckBox;
        private System.Windows.Forms.Button saveAsImageButton;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private Components.LabeledSeparator labeledSeparator1;
        private Components.LabeledSeparator labeledSeparator2;
        private Components.LabeledSeparator labeledSeparator3;
        private Components.LabeledSeparator labeledSeparator4;
        private System.Windows.Forms.CheckBox npcCheckBox;
        private System.Windows.Forms.CheckBox monsterCheckBox;
        private System.Windows.Forms.CheckBox extraCheckBox;
        private System.Windows.Forms.CheckBox debugDotsCheckBox;
    }
}