namespace DispelTools.ImageAnalyzer
{
    partial class ImageEditControls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditControls));
            this.colorGroupBox = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.inA = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inBytes = new System.Windows.Forms.NumericUpDown();
            this.inValue = new System.Windows.Forms.NumericUpDown();
            this.inB = new System.Windows.Forms.NumericUpDown();
            this.inG = new System.Windows.Forms.NumericUpDown();
            this.inR = new System.Windows.Forms.NumericUpDown();
            this.colorBackground = new System.Windows.Forms.PictureBox();
            this.toolsGroup = new System.Windows.Forms.GroupBox();
            this.colorPickerButton = new System.Windows.Forms.RadioButton();
            this.selectButton = new System.Windows.Forms.RadioButton();
            this.pencilButton = new System.Windows.Forms.RadioButton();
            this.pointerButton = new System.Windows.Forms.RadioButton();
            this.colorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inBytes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorBackground)).BeginInit();
            this.toolsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorGroupBox
            // 
            this.colorGroupBox.Controls.Add(this.label6);
            this.colorGroupBox.Controls.Add(this.label5);
            this.colorGroupBox.Controls.Add(this.label4);
            this.colorGroupBox.Controls.Add(this.inA);
            this.colorGroupBox.Controls.Add(this.label3);
            this.colorGroupBox.Controls.Add(this.label2);
            this.colorGroupBox.Controls.Add(this.label1);
            this.colorGroupBox.Controls.Add(this.inBytes);
            this.colorGroupBox.Controls.Add(this.inValue);
            this.colorGroupBox.Controls.Add(this.inB);
            this.colorGroupBox.Controls.Add(this.inG);
            this.colorGroupBox.Controls.Add(this.inR);
            this.colorGroupBox.Controls.Add(this.colorBackground);
            this.colorGroupBox.Location = new System.Drawing.Point(3, 154);
            this.colorGroupBox.Name = "colorGroupBox";
            this.colorGroupBox.Size = new System.Drawing.Size(419, 214);
            this.colorGroupBox.TabIndex = 0;
            this.colorGroupBox.TabStop = false;
            this.colorGroupBox.Text = "Color/Value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Bytes";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "A";
            // 
            // inA
            // 
            this.inA.Location = new System.Drawing.Point(133, 97);
            this.inA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inA.Name = "inA";
            this.inA.Size = new System.Drawing.Size(70, 20);
            this.inA.TabIndex = 9;
            this.inA.ValueChanged += new System.EventHandler(this.ColorInputsChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "B";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "G";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(112, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "R";
            // 
            // inBytes
            // 
            this.inBytes.Location = new System.Drawing.Point(64, 126);
            this.inBytes.Name = "inBytes";
            this.inBytes.Size = new System.Drawing.Size(70, 20);
            this.inBytes.TabIndex = 5;
            // 
            // inValue
            // 
            this.inValue.Hexadecimal = true;
            this.inValue.Location = new System.Drawing.Point(208, 126);
            this.inValue.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.inValue.Minimum = new decimal(new int[] {
            1215752192,
            23,
            0,
            -2147483648});
            this.inValue.Name = "inValue";
            this.inValue.Size = new System.Drawing.Size(122, 20);
            this.inValue.TabIndex = 4;
            this.inValue.ValueChanged += new System.EventHandler(this.ValueInputsChanged);
            // 
            // inB
            // 
            this.inB.Location = new System.Drawing.Point(133, 71);
            this.inB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inB.Name = "inB";
            this.inB.Size = new System.Drawing.Size(70, 20);
            this.inB.TabIndex = 3;
            this.inB.ValueChanged += new System.EventHandler(this.ColorInputsChanged);
            // 
            // inG
            // 
            this.inG.Location = new System.Drawing.Point(133, 45);
            this.inG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inG.Name = "inG";
            this.inG.Size = new System.Drawing.Size(70, 20);
            this.inG.TabIndex = 2;
            this.inG.ValueChanged += new System.EventHandler(this.ColorInputsChanged);
            // 
            // inR
            // 
            this.inR.Location = new System.Drawing.Point(133, 19);
            this.inR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.inR.Name = "inR";
            this.inR.Size = new System.Drawing.Size(70, 20);
            this.inR.TabIndex = 1;
            this.inR.ValueChanged += new System.EventHandler(this.ColorInputsChanged);
            // 
            // colorBackground
            // 
            this.colorBackground.BackgroundImage = global::DispelTools.Properties.Resources.transparentPattern;
            this.colorBackground.Location = new System.Drawing.Point(7, 20);
            this.colorBackground.Name = "colorBackground";
            this.colorBackground.Size = new System.Drawing.Size(100, 100);
            this.colorBackground.TabIndex = 0;
            this.colorBackground.TabStop = false;
            // 
            // toolsGroup
            // 
            this.toolsGroup.Controls.Add(this.colorPickerButton);
            this.toolsGroup.Controls.Add(this.selectButton);
            this.toolsGroup.Controls.Add(this.pencilButton);
            this.toolsGroup.Controls.Add(this.pointerButton);
            this.toolsGroup.Location = new System.Drawing.Point(3, 3);
            this.toolsGroup.Name = "toolsGroup";
            this.toolsGroup.Size = new System.Drawing.Size(63, 145);
            this.toolsGroup.TabIndex = 1;
            this.toolsGroup.TabStop = false;
            this.toolsGroup.Text = "Tools";
            // 
            // colorPickerButton
            // 
            this.colorPickerButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.colorPickerButton.Image = ((System.Drawing.Image)(resources.GetObject("colorPickerButton.Image")));
            this.colorPickerButton.Location = new System.Drawing.Point(6, 110);
            this.colorPickerButton.Name = "colorPickerButton";
            this.colorPickerButton.Size = new System.Drawing.Size(44, 24);
            this.colorPickerButton.TabIndex = 6;
            this.colorPickerButton.UseVisualStyleBackColor = true;
            this.colorPickerButton.CheckedChanged += new System.EventHandler(this.colorPickerButton_CheckedChanged);
            // 
            // selectButton
            // 
            this.selectButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.selectButton.Image = ((System.Drawing.Image)(resources.GetObject("selectButton.Image")));
            this.selectButton.Location = new System.Drawing.Point(7, 80);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(44, 24);
            this.selectButton.TabIndex = 5;
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.CheckedChanged += new System.EventHandler(this.selectButton_CheckedChanged);
            // 
            // pencilButton
            // 
            this.pencilButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.pencilButton.Image = ((System.Drawing.Image)(resources.GetObject("pencilButton.Image")));
            this.pencilButton.Location = new System.Drawing.Point(7, 50);
            this.pencilButton.Name = "pencilButton";
            this.pencilButton.Size = new System.Drawing.Size(44, 24);
            this.pencilButton.TabIndex = 3;
            this.pencilButton.UseVisualStyleBackColor = true;
            this.pencilButton.CheckedChanged += new System.EventHandler(this.pencilButton_CheckedChanged);
            // 
            // pointerButton
            // 
            this.pointerButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.pointerButton.Image = ((System.Drawing.Image)(resources.GetObject("pointerButton.Image")));
            this.pointerButton.Location = new System.Drawing.Point(7, 20);
            this.pointerButton.Name = "pointerButton";
            this.pointerButton.Size = new System.Drawing.Size(44, 24);
            this.pointerButton.TabIndex = 2;
            this.pointerButton.UseVisualStyleBackColor = true;
            this.pointerButton.CheckedChanged += new System.EventHandler(this.pointerButton_CheckedChanged);
            // 
            // ImageEditControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolsGroup);
            this.Controls.Add(this.colorGroupBox);
            this.Name = "ImageEditControls";
            this.Size = new System.Drawing.Size(425, 371);
            this.colorGroupBox.ResumeLayout(false);
            this.colorGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inBytes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorBackground)).EndInit();
            this.toolsGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox colorGroupBox;
        private System.Windows.Forms.GroupBox toolsGroup;
        private System.Windows.Forms.RadioButton pencilButton;
        private System.Windows.Forms.RadioButton pointerButton;
        private System.Windows.Forms.RadioButton selectButton;
        private System.Windows.Forms.RadioButton colorPickerButton;
        private System.Windows.Forms.PictureBox colorBackground;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown inA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown inBytes;
        private System.Windows.Forms.NumericUpDown inValue;
        private System.Windows.Forms.NumericUpDown inB;
        private System.Windows.Forms.NumericUpDown inG;
        private System.Windows.Forms.NumericUpDown inR;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}
