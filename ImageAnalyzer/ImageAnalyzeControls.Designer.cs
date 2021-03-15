namespace DispelTools.ImageAnalyzer
{
    partial class ImageAnalyzeControls
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colorLevelResetButton = new System.Windows.Forms.Button();
            this.inColorLevelHigh = new System.Windows.Forms.NumericUpDown();
            this.inColorLevelLow = new System.Windows.Forms.NumericUpDown();
            this.colorLevelRadioButton = new System.Windows.Forms.RadioButton();
            this.valueHighlightRadioButton = new System.Windows.Forms.RadioButton();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.valueHighlightChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inValueHighlightValue = new System.Windows.Forms.NumericUpDown();
            this.channelViewRadioButton = new System.Windows.Forms.RadioButton();
            this.channelsGroupBox = new System.Windows.Forms.GroupBox();
            this.ch4CheckBox = new System.Windows.Forms.CheckBox();
            this.ch3CheckBox = new System.Windows.Forms.CheckBox();
            this.ch2CheckBox = new System.Windows.Forms.CheckBox();
            this.ch1CheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inColorLevelHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inColorLevelLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inValueHighlightValue)).BeginInit();
            this.channelsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.colorLevelResetButton);
            this.groupBox1.Controls.Add(this.inColorLevelHigh);
            this.groupBox1.Controls.Add(this.inColorLevelLow);
            this.groupBox1.Location = new System.Drawing.Point(2, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 77);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color levels";
            // 
            // colorLevelResetButton
            // 
            this.colorLevelResetButton.Location = new System.Drawing.Point(7, 47);
            this.colorLevelResetButton.Name = "colorLevelResetButton";
            this.colorLevelResetButton.Size = new System.Drawing.Size(75, 23);
            this.colorLevelResetButton.TabIndex = 2;
            this.colorLevelResetButton.Text = "Reset";
            this.colorLevelResetButton.UseVisualStyleBackColor = true;
            this.colorLevelResetButton.Click += new System.EventHandler(this.colorLevelResetButton_Click);
            // 
            // inColorLevelHigh
            // 
            this.inColorLevelHigh.Location = new System.Drawing.Point(119, 20);
            this.inColorLevelHigh.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inColorLevelHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inColorLevelHigh.Name = "inColorLevelHigh";
            this.inColorLevelHigh.Size = new System.Drawing.Size(84, 20);
            this.inColorLevelHigh.TabIndex = 1;
            this.inColorLevelHigh.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inColorLevelHigh.ValueChanged += new System.EventHandler(this.inColorLevelHigh_ValueChanged);
            // 
            // inColorLevelLow
            // 
            this.inColorLevelLow.Location = new System.Drawing.Point(7, 20);
            this.inColorLevelLow.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.inColorLevelLow.Name = "inColorLevelLow";
            this.inColorLevelLow.Size = new System.Drawing.Size(84, 20);
            this.inColorLevelLow.TabIndex = 0;
            this.inColorLevelLow.ValueChanged += new System.EventHandler(this.inColorLevelLow_ValueChanged);
            // 
            // colorLevelRadioButton
            // 
            this.colorLevelRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.colorLevelRadioButton.AutoSize = true;
            this.colorLevelRadioButton.Location = new System.Drawing.Point(3, 31);
            this.colorLevelRadioButton.Name = "colorLevelRadioButton";
            this.colorLevelRadioButton.Size = new System.Drawing.Size(74, 23);
            this.colorLevelRadioButton.TabIndex = 26;
            this.colorLevelRadioButton.Text = "Level colors";
            this.colorLevelRadioButton.UseVisualStyleBackColor = true;
            this.colorLevelRadioButton.Click += new System.EventHandler(this.checkedAnalyzerRadioButton);
            // 
            // valueHighlightRadioButton
            // 
            this.valueHighlightRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.valueHighlightRadioButton.AutoSize = true;
            this.valueHighlightRadioButton.Location = new System.Drawing.Point(3, 143);
            this.valueHighlightRadioButton.Name = "valueHighlightRadioButton";
            this.valueHighlightRadioButton.Size = new System.Drawing.Size(87, 23);
            this.valueHighlightRadioButton.TabIndex = 27;
            this.valueHighlightRadioButton.Text = "Highlight value";
            this.valueHighlightRadioButton.UseVisualStyleBackColor = true;
            this.valueHighlightRadioButton.Click += new System.EventHandler(this.checkedAnalyzerRadioButton);
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.noneRadioButton.AutoSize = true;
            this.noneRadioButton.Checked = true;
            this.noneRadioButton.Location = new System.Drawing.Point(3, 3);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(43, 23);
            this.noneRadioButton.TabIndex = 28;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "None";
            this.noneRadioButton.UseVisualStyleBackColor = true;
            this.noneRadioButton.Click += new System.EventHandler(this.checkedAnalyzerRadioButton);
            // 
            // valueHighlightChannelComboBox
            // 
            this.valueHighlightChannelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueHighlightChannelComboBox.FormattingEnabled = true;
            this.valueHighlightChannelComboBox.Location = new System.Drawing.Point(56, 172);
            this.valueHighlightChannelComboBox.Name = "valueHighlightChannelComboBox";
            this.valueHighlightChannelComboBox.Size = new System.Drawing.Size(68, 21);
            this.valueHighlightChannelComboBox.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Value";
            // 
            // inValueHighlightValue
            // 
            this.inValueHighlightValue.Location = new System.Drawing.Point(172, 172);
            this.inValueHighlightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inValueHighlightValue.Name = "inValueHighlightValue";
            this.inValueHighlightValue.Size = new System.Drawing.Size(77, 20);
            this.inValueHighlightValue.TabIndex = 32;
            // 
            // channelViewRadioButton
            // 
            this.channelViewRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.channelViewRadioButton.AutoSize = true;
            this.channelViewRadioButton.Location = new System.Drawing.Point(3, 199);
            this.channelViewRadioButton.Name = "channelViewRadioButton";
            this.channelViewRadioButton.Size = new System.Drawing.Size(81, 23);
            this.channelViewRadioButton.TabIndex = 33;
            this.channelViewRadioButton.Text = "Channel view";
            this.channelViewRadioButton.UseVisualStyleBackColor = true;
            this.channelViewRadioButton.Click += new System.EventHandler(this.checkedAnalyzerRadioButton);
            // 
            // channelsGroupBox
            // 
            this.channelsGroupBox.Controls.Add(this.ch4CheckBox);
            this.channelsGroupBox.Controls.Add(this.ch3CheckBox);
            this.channelsGroupBox.Controls.Add(this.ch2CheckBox);
            this.channelsGroupBox.Controls.Add(this.ch1CheckBox);
            this.channelsGroupBox.Location = new System.Drawing.Point(2, 229);
            this.channelsGroupBox.Name = "channelsGroupBox";
            this.channelsGroupBox.Size = new System.Drawing.Size(182, 43);
            this.channelsGroupBox.TabIndex = 34;
            this.channelsGroupBox.TabStop = false;
            // 
            // ch4CheckBox
            // 
            this.ch4CheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ch4CheckBox.AutoSize = true;
            this.ch4CheckBox.Checked = true;
            this.ch4CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ch4CheckBox.Location = new System.Drawing.Point(133, 9);
            this.ch4CheckBox.Name = "ch4CheckBox";
            this.ch4CheckBox.Size = new System.Drawing.Size(36, 23);
            this.ch4CheckBox.TabIndex = 3;
            this.ch4CheckBox.Text = "Ch4";
            this.ch4CheckBox.UseVisualStyleBackColor = true;
            this.ch4CheckBox.Click += new System.EventHandler(this.channelsCheckBox_Click);
            // 
            // ch3CheckBox
            // 
            this.ch3CheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ch3CheckBox.AutoSize = true;
            this.ch3CheckBox.Checked = true;
            this.ch3CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ch3CheckBox.Location = new System.Drawing.Point(91, 9);
            this.ch3CheckBox.Name = "ch3CheckBox";
            this.ch3CheckBox.Size = new System.Drawing.Size(36, 23);
            this.ch3CheckBox.TabIndex = 2;
            this.ch3CheckBox.Text = "Ch3";
            this.ch3CheckBox.UseVisualStyleBackColor = true;
            this.ch3CheckBox.Click += new System.EventHandler(this.channelsCheckBox_Click);
            // 
            // ch2CheckBox
            // 
            this.ch2CheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ch2CheckBox.AutoSize = true;
            this.ch2CheckBox.Checked = true;
            this.ch2CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ch2CheckBox.Location = new System.Drawing.Point(49, 9);
            this.ch2CheckBox.Name = "ch2CheckBox";
            this.ch2CheckBox.Size = new System.Drawing.Size(36, 23);
            this.ch2CheckBox.TabIndex = 1;
            this.ch2CheckBox.Text = "Ch2";
            this.ch2CheckBox.UseVisualStyleBackColor = true;
            this.ch2CheckBox.Click += new System.EventHandler(this.channelsCheckBox_Click);
            // 
            // ch1CheckBox
            // 
            this.ch1CheckBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.ch1CheckBox.AutoSize = true;
            this.ch1CheckBox.Checked = true;
            this.ch1CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ch1CheckBox.Location = new System.Drawing.Point(7, 9);
            this.ch1CheckBox.Name = "ch1CheckBox";
            this.ch1CheckBox.Size = new System.Drawing.Size(36, 23);
            this.ch1CheckBox.TabIndex = 0;
            this.ch1CheckBox.Text = "Ch1";
            this.ch1CheckBox.UseVisualStyleBackColor = true;
            this.ch1CheckBox.Click += new System.EventHandler(this.channelsCheckBox_Click);
            // 
            // ImageAnalyzeControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.channelsGroupBox);
            this.Controls.Add(this.channelViewRadioButton);
            this.Controls.Add(this.inValueHighlightValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.valueHighlightChannelComboBox);
            this.Controls.Add(this.noneRadioButton);
            this.Controls.Add(this.valueHighlightRadioButton);
            this.Controls.Add(this.colorLevelRadioButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImageAnalyzeControls";
            this.Size = new System.Drawing.Size(412, 374);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inColorLevelHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inColorLevelLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inValueHighlightValue)).EndInit();
            this.channelsGroupBox.ResumeLayout(false);
            this.channelsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button colorLevelResetButton;
        private System.Windows.Forms.NumericUpDown inColorLevelHigh;
        private System.Windows.Forms.NumericUpDown inColorLevelLow;
        private System.Windows.Forms.RadioButton colorLevelRadioButton;
        private System.Windows.Forms.RadioButton valueHighlightRadioButton;
        private System.Windows.Forms.RadioButton noneRadioButton;
        private System.Windows.Forms.ComboBox valueHighlightChannelComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown inValueHighlightValue;
        private System.Windows.Forms.RadioButton channelViewRadioButton;
        private System.Windows.Forms.GroupBox channelsGroupBox;
        private System.Windows.Forms.CheckBox ch4CheckBox;
        private System.Windows.Forms.CheckBox ch3CheckBox;
        private System.Windows.Forms.CheckBox ch2CheckBox;
        private System.Windows.Forms.CheckBox ch1CheckBox;
    }
}
