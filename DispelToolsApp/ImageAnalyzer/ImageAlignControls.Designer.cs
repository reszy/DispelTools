namespace DispelTools.ImageAnalyzer
{
    partial class ImageAlignControls
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
            this.skipForwButton = new System.Windows.Forms.Button();
            this.skipBackButton = new System.Windows.Forms.Button();
            this.inImageOffset = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.inImageNumber = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.autoAdjust = new System.Windows.Forms.CheckBox();
            this.cbTransparency = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbColorMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.inLineLen = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.inHeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.inWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.inOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inImageOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inImageNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inLineLen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // skipForwButton
            // 
            this.skipForwButton.Location = new System.Drawing.Point(253, 10);
            this.skipForwButton.Name = "skipForwButton";
            this.skipForwButton.Size = new System.Drawing.Size(32, 20);
            this.skipForwButton.TabIndex = 46;
            this.skipForwButton.Text = ">>";
            this.skipForwButton.UseVisualStyleBackColor = true;
            this.skipForwButton.Click += new System.EventHandler(this.skipForwButton_Click);
            // 
            // skipBackButton
            // 
            this.skipBackButton.Location = new System.Drawing.Point(215, 10);
            this.skipBackButton.Name = "skipBackButton";
            this.skipBackButton.Size = new System.Drawing.Size(32, 20);
            this.skipBackButton.TabIndex = 45;
            this.skipBackButton.Text = "<<";
            this.skipBackButton.UseVisualStyleBackColor = true;
            this.skipBackButton.Click += new System.EventHandler(this.skipBackButton_Click);
            // 
            // inImageOffset
            // 
            this.inImageOffset.Location = new System.Drawing.Point(293, 89);
            this.inImageOffset.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inImageOffset.Name = "inImageOffset";
            this.inImageOffset.Size = new System.Drawing.Size(100, 20);
            this.inImageOffset.TabIndex = 43;
            this.inImageOffset.ValueChanged += new System.EventHandler(this.inImageOffset_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(212, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 42;
            this.label8.Text = "Image offset";
            // 
            // inImageNumber
            // 
            this.inImageNumber.Location = new System.Drawing.Point(95, 88);
            this.inImageNumber.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inImageNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inImageNumber.Name = "inImageNumber";
            this.inImageNumber.Size = new System.Drawing.Size(100, 20);
            this.inImageNumber.TabIndex = 41;
            this.inImageNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inImageNumber.ValueChanged += new System.EventHandler(this.inImageNumber_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Image Number";
            // 
            // autoAdjust
            // 
            this.autoAdjust.AutoSize = true;
            this.autoAdjust.Location = new System.Drawing.Point(17, 231);
            this.autoAdjust.Name = "autoAdjust";
            this.autoAdjust.Size = new System.Drawing.Size(79, 17);
            this.autoAdjust.TabIndex = 39;
            this.autoAdjust.Text = "Auto adjust";
            this.autoAdjust.UseVisualStyleBackColor = true;
            // 
            // cbTransparency
            // 
            this.cbTransparency.FormattingEnabled = true;
            this.cbTransparency.Location = new System.Drawing.Point(95, 161);
            this.cbTransparency.Name = "cbTransparency";
            this.cbTransparency.Size = new System.Drawing.Size(100, 21);
            this.cbTransparency.TabIndex = 38;
            this.cbTransparency.SelectedIndexChanged += new System.EventHandler(this.cbTransparency_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "Transparency";
            // 
            // cbColorMode
            // 
            this.cbColorMode.FormattingEnabled = true;
            this.cbColorMode.Location = new System.Drawing.Point(95, 128);
            this.cbColorMode.Name = "cbColorMode";
            this.cbColorMode.Size = new System.Drawing.Size(100, 21);
            this.cbColorMode.TabIndex = 36;
            this.cbColorMode.SelectedIndexChanged += new System.EventHandler(this.cbColorMode_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "ColorMode";
            // 
            // inLineLen
            // 
            this.inLineLen.Location = new System.Drawing.Point(293, 36);
            this.inLineLen.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inLineLen.Name = "inLineLen";
            this.inLineLen.Size = new System.Drawing.Size(100, 20);
            this.inLineLen.TabIndex = 34;
            this.inLineLen.ValueChanged += new System.EventHandler(this.inLineLen_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Line length";
            // 
            // inHeight
            // 
            this.inHeight.Location = new System.Drawing.Point(95, 62);
            this.inHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inHeight.Name = "inHeight";
            this.inHeight.Size = new System.Drawing.Size(100, 20);
            this.inHeight.TabIndex = 32;
            this.inHeight.ValueChanged += new System.EventHandler(this.inHeight_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Height";
            // 
            // inWidth
            // 
            this.inWidth.Location = new System.Drawing.Point(95, 36);
            this.inWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.inWidth.Name = "inWidth";
            this.inWidth.Size = new System.Drawing.Size(100, 20);
            this.inWidth.TabIndex = 30;
            this.inWidth.ValueChanged += new System.EventHandler(this.inWidth_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Width";
            // 
            // inOffset
            // 
            this.inOffset.Location = new System.Drawing.Point(95, 10);
            this.inOffset.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.inOffset.Name = "inOffset";
            this.inOffset.Size = new System.Drawing.Size(100, 20);
            this.inOffset.TabIndex = 28;
            this.inOffset.ValueChanged += new System.EventHandler(this.inOffset_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Offset";
            // 
            // ImageAlignControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skipForwButton);
            this.Controls.Add(this.skipBackButton);
            this.Controls.Add(this.inImageOffset);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.inImageNumber);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.autoAdjust);
            this.Controls.Add(this.cbTransparency);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbColorMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.inLineLen);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inOffset);
            this.Controls.Add(this.label1);
            this.Name = "ImageAlignControls";
            this.Size = new System.Drawing.Size(424, 289);
            ((System.ComponentModel.ISupportInitialize)(this.inImageOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inImageNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inLineLen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button skipForwButton;
        private System.Windows.Forms.Button skipBackButton;
        private System.Windows.Forms.NumericUpDown inImageOffset;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown inImageNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox autoAdjust;
        private System.Windows.Forms.ComboBox cbTransparency;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbColorMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown inLineLen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown inHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown inWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown inOffset;
        private System.Windows.Forms.Label label1;
    }
}
