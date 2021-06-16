
namespace DispelTools
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.programToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageAnalyzerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapImageExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rgbConverterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gifCreatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(484, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // programToolStripMenuItem
            // 
            this.programToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analyzersToolStripMenuItem,
            this.extractorsToolStripMenuItem,
            this.editorsToolStripMenuItem,
            this.mapViewerToolStripMenuItem,
            this.gifCreatorToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.programToolStripMenuItem.Name = "programToolStripMenuItem";
            this.programToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.programToolStripMenuItem.Text = "Program";
            // 
            // analyzersToolStripMenuItem
            // 
            this.analyzersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageAnalyzerToolStripMenuItem});
            this.analyzersToolStripMenuItem.Name = "analyzersToolStripMenuItem";
            this.analyzersToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.analyzersToolStripMenuItem.Text = "Analyzers";
            // 
            // imageAnalyzerToolStripMenuItem
            // 
            this.imageAnalyzerToolStripMenuItem.CheckOnClick = true;
            this.imageAnalyzerToolStripMenuItem.Name = "imageAnalyzerToolStripMenuItem";
            this.imageAnalyzerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.imageAnalyzerToolStripMenuItem.Text = "ImageAnalyzer";
            this.imageAnalyzerToolStripMenuItem.Click += new System.EventHandler(this.imageAnalyzerToolStripMenuItem_Click);
            // 
            // extractorsToolStripMenuItem
            // 
            this.extractorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageExtractorToolStripMenuItem,
            this.soundExtractorToolStripMenuItem,
            this.stringExtractorToolStripMenuItem,
            this.mapImageExtractorToolStripMenuItem,
            this.allExtractorToolStripMenuItem,
            this.rgbConverterToolStripMenuItem});
            this.extractorsToolStripMenuItem.Name = "extractorsToolStripMenuItem";
            this.extractorsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.extractorsToolStripMenuItem.Text = "Extractors";
            // 
            // imageExtractorToolStripMenuItem
            // 
            this.imageExtractorToolStripMenuItem.CheckOnClick = true;
            this.imageExtractorToolStripMenuItem.Name = "imageExtractorToolStripMenuItem";
            this.imageExtractorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.imageExtractorToolStripMenuItem.Text = "ImageExtractor";
            this.imageExtractorToolStripMenuItem.Click += new System.EventHandler(this.imageExtractorToolStripMenuItem_Click);
            // 
            // soundExtractorToolStripMenuItem
            // 
            this.soundExtractorToolStripMenuItem.CheckOnClick = true;
            this.soundExtractorToolStripMenuItem.Name = "soundExtractorToolStripMenuItem";
            this.soundExtractorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.soundExtractorToolStripMenuItem.Text = "SoundExtractor";
            this.soundExtractorToolStripMenuItem.Click += new System.EventHandler(this.soundExtractorToolStripMenuItem_Click);
            // 
            // stringExtractorToolStripMenuItem
            // 
            this.stringExtractorToolStripMenuItem.CheckOnClick = true;
            this.stringExtractorToolStripMenuItem.Name = "stringExtractorToolStripMenuItem";
            this.stringExtractorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.stringExtractorToolStripMenuItem.Text = "StringExtractor";
            this.stringExtractorToolStripMenuItem.Click += new System.EventHandler(this.stringExtractorToolStripMenuItem_Click);
            // 
            // mapImageExtractorToolStripMenuItem
            // 
            this.mapImageExtractorToolStripMenuItem.CheckOnClick = true;
            this.mapImageExtractorToolStripMenuItem.Name = "mapImageExtractorToolStripMenuItem";
            this.mapImageExtractorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.mapImageExtractorToolStripMenuItem.Text = "MapImageExtractor";
            this.mapImageExtractorToolStripMenuItem.Click += new System.EventHandler(this.mapImageExtractorToolStripMenuItem_Click);
            // 
            // allExtractorToolStripMenuItem
            // 
            this.allExtractorToolStripMenuItem.CheckOnClick = true;
            this.allExtractorToolStripMenuItem.Name = "allExtractorToolStripMenuItem";
            this.allExtractorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.allExtractorToolStripMenuItem.Text = "AllExtractor";
            this.allExtractorToolStripMenuItem.Click += new System.EventHandler(this.allExtractorToolStripMenuItem_Click);
            // 
            // rgbConverterToolStripMenuItem
            // 
            this.rgbConverterToolStripMenuItem.Name = "rgbConverterToolStripMenuItem";
            this.rgbConverterToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.rgbConverterToolStripMenuItem.Text = "RgbConverter";
            this.rgbConverterToolStripMenuItem.Click += new System.EventHandler(this.rgbConverterToolStripMenuItem_Click);
            // 
            // editorsToolStripMenuItem
            // 
            this.editorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.simpleEditorToolStripMenuItem});
            this.editorsToolStripMenuItem.Name = "editorsToolStripMenuItem";
            this.editorsToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.editorsToolStripMenuItem.Text = "Editors";
            // 
            // simpleEditorToolStripMenuItem
            // 
            this.simpleEditorToolStripMenuItem.CheckOnClick = true;
            this.simpleEditorToolStripMenuItem.Name = "simpleEditorToolStripMenuItem";
            this.simpleEditorToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.simpleEditorToolStripMenuItem.Text = "SimpleEditor";
            this.simpleEditorToolStripMenuItem.Click += new System.EventHandler(this.simpleEditorToolStripMenuItem_Click);
            // 
            // mapViewerToolStripMenuItem
            // 
            this.mapViewerToolStripMenuItem.CheckOnClick = true;
            this.mapViewerToolStripMenuItem.Name = "mapViewerToolStripMenuItem";
            this.mapViewerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mapViewerToolStripMenuItem.Text = "MapViewer";
            this.mapViewerToolStripMenuItem.Click += new System.EventHandler(this.mapViewerToolStripMenuItem_Click);
            // 
            // gifCreatorToolStripMenuItem
            // 
            this.gifCreatorToolStripMenuItem.CheckOnClick = true;
            this.gifCreatorToolStripMenuItem.Name = "gifCreatorToolStripMenuItem";
            this.gifCreatorToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.gifCreatorToolStripMenuItem.Text = "GifCreator";
            this.gifCreatorToolStripMenuItem.Click += new System.EventHandler(this.gifCreatorToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(130, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.AutoSize = true;
            this.contentPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.contentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 24);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(484, 137);
            this.contentPanel.TabIndex = 1;
            this.contentPanel.Resize += new System.EventHandler(this.contentPanel_Resize);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 161);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.Text = "DispelExtractorTools";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem programToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analyzersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageAnalyzerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.ToolStripMenuItem stringExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapImageExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allExtractorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simpleEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rgbConverterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gifCreatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

