
namespace DispelTools.GifCreator
{
    partial class GifCreatorForm
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
            this.addImagesButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.framesListBox = new System.Windows.Forms.ListBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.centringGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.centringGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // addImagesButton
            // 
            this.addImagesButton.Location = new System.Drawing.Point(13, 13);
            this.addImagesButton.Name = "addImagesButton";
            this.addImagesButton.Size = new System.Drawing.Size(75, 23);
            this.addImagesButton.TabIndex = 0;
            this.addImagesButton.Text = "Add images";
            this.addImagesButton.UseVisualStyleBackColor = true;
            this.addImagesButton.Click += new System.EventHandler(this.addImagesButton_Click);
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(284, 183);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(122, 33);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "Create GIF";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // framesListBox
            // 
            this.framesListBox.AllowDrop = true;
            this.framesListBox.FormattingEnabled = true;
            this.framesListBox.Location = new System.Drawing.Point(13, 43);
            this.framesListBox.Name = "framesListBox";
            this.framesListBox.Size = new System.Drawing.Size(265, 251);
            this.framesListBox.TabIndex = 2;
            this.framesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.framesListBox_DragDrop);
            this.framesListBox.DragOver += new System.Windows.Forms.DragEventHandler(this.framesListBox_DragOver);
            this.framesListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.framesListBox_KeyUp);
            this.framesListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.framesListBox_MouseDown);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(284, 59);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(284, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "FPS";
            // 
            // centringGroupBox
            // 
            this.centringGroupBox.Controls.Add(this.radioButton3);
            this.centringGroupBox.Controls.Add(this.radioButton2);
            this.centringGroupBox.Controls.Add(this.radioButton1);
            this.centringGroupBox.Location = new System.Drawing.Point(285, 86);
            this.centringGroupBox.Name = "centringGroupBox";
            this.centringGroupBox.Size = new System.Drawing.Size(121, 91);
            this.centringGroupBox.TabIndex = 5;
            this.centringGroupBox.TabStop = false;
            this.centringGroupBox.Text = "Centring";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 65);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(62, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "TopLeft";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.aligmentRadioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(56, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Middle";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.aligmentRadioButton_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(58, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Bottom";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.aligmentRadioButton_CheckedChanged);
            // 
            // GifCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 307);
            this.Controls.Add(this.centringGroupBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.framesListBox);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.addImagesButton);
            this.Name = "GifCreatorForm";
            this.Text = "Gif creator";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.centringGroupBox.ResumeLayout(false);
            this.centringGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addImagesButton;
        private System.Windows.Forms.Button createButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ListBox framesListBox;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox centringGroupBox;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}