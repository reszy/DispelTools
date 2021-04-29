namespace DispelTools.Components
{
    partial class DetailsComponent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.contentBox = new System.Windows.Forms.TextBox();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerArrowLabel = new System.Windows.Forms.Label();
            this.headerLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.contentBox);
            this.panel1.Controls.Add(this.headerPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 217);
            this.panel1.TabIndex = 0;
            // 
            // contentBox
            // 
            this.contentBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.contentBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentBox.Location = new System.Drawing.Point(0, 25);
            this.contentBox.Multiline = true;
            this.contentBox.Name = "contentBox";
            this.contentBox.ReadOnly = true;
            this.contentBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.contentBox.Size = new System.Drawing.Size(327, 192);
            this.contentBox.TabIndex = 2;
            this.contentBox.WordWrap = false;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.headerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headerPanel.Controls.Add(this.headerArrowLabel);
            this.headerPanel.Controls.Add(this.headerLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(327, 25);
            this.headerPanel.TabIndex = 0;
            this.headerPanel.Click += new System.EventHandler(this.headerPanel_Click);
            // 
            // headerArrowLabel
            // 
            this.headerArrowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerArrowLabel.AutoSize = true;
            this.headerArrowLabel.Location = new System.Drawing.Point(306, 4);
            this.headerArrowLabel.Name = "headerArrowLabel";
            this.headerArrowLabel.Size = new System.Drawing.Size(14, 13);
            this.headerArrowLabel.TabIndex = 1;
            this.headerArrowLabel.Text = "V";
            this.headerArrowLabel.Click += new System.EventHandler(this.headerPanel_Click);
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Location = new System.Drawing.Point(4, 4);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(42, 13);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Header";
            this.headerLabel.Click += new System.EventHandler(this.headerPanel_Click);
            // 
            // DetailsComponent
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(100, 25);
            this.Name = "DetailsComponent";
            this.Size = new System.Drawing.Size(327, 217);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label headerArrowLabel;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.TextBox contentBox;
    }
}
