
namespace DispelTools.DataEditor
{
    partial class SimpleEditorForm
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
            this.openButton = new System.Windows.Forms.Button();
            this.openedFileLabel = new System.Windows.Forms.Label();
            this.inElementNumber = new System.Windows.Forms.NumericUpDown();
            this.maxElementsLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.customPropertyGrid1 = new DispelTools.Components.CustomPropertyGridControl.CustomPropertyGrid();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.hideUnnamedCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.inElementNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(13, 13);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // openedFileLabel
            // 
            this.openedFileLabel.AutoEllipsis = true;
            this.openedFileLabel.Location = new System.Drawing.Point(95, 22);
            this.openedFileLabel.Name = "openedFileLabel";
            this.openedFileLabel.Size = new System.Drawing.Size(263, 13);
            this.openedFileLabel.TabIndex = 2;
            // 
            // inElementNumber
            // 
            this.inElementNumber.Location = new System.Drawing.Point(13, 43);
            this.inElementNumber.Name = "inElementNumber";
            this.inElementNumber.Size = new System.Drawing.Size(75, 20);
            this.inElementNumber.TabIndex = 3;
            this.inElementNumber.ValueChanged += new System.EventHandler(this.elementNumber_ValueChanged);
            // 
            // maxElementsLabel
            // 
            this.maxElementsLabel.AutoSize = true;
            this.maxElementsLabel.Location = new System.Drawing.Point(95, 45);
            this.maxElementsLabel.Name = "maxElementsLabel";
            this.maxElementsLabel.Size = new System.Drawing.Size(18, 13);
            this.maxElementsLabel.TabIndex = 4;
            this.maxElementsLabel.Text = "/0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(364, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(26, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(314, 43);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // customPropertyGrid1
            // 
            this.customPropertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customPropertyGrid1.AutoScroll = true;
            this.customPropertyGrid1.AutoScrollMinSize = new System.Drawing.Size(20, 20);
            this.customPropertyGrid1.BackColor = System.Drawing.SystemColors.Window;
            this.customPropertyGrid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.customPropertyGrid1.Location = new System.Drawing.Point(12, 69);
            this.customPropertyGrid1.Name = "customPropertyGrid1";
            this.customPropertyGrid1.SelectedItem = null;
            this.customPropertyGrid1.Size = new System.Drawing.Size(377, 432);
            this.customPropertyGrid1.TabIndex = 8;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All handled|*.ref;*.REF;*.db;*.DB|Reference files|*.ref;*.REF|Database file|*.db;" +
    "*.DB";
            // 
            // hideUnnamedCheckBox
            // 
            this.hideUnnamedCheckBox.AutoSize = true;
            this.hideUnnamedCheckBox.Location = new System.Drawing.Point(186, 45);
            this.hideUnnamedCheckBox.Name = "hideUnnamedCheckBox";
            this.hideUnnamedCheckBox.Size = new System.Drawing.Size(122, 17);
            this.hideUnnamedCheckBox.TabIndex = 5;
            this.hideUnnamedCheckBox.Text = "Hide unnamed fields";
            this.hideUnnamedCheckBox.UseVisualStyleBackColor = true;
            this.hideUnnamedCheckBox.CheckedChanged += new System.EventHandler(this.hideUnnamedCheckBox_CheckedChanged);
            // 
            // SimpleEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 513);
            this.Controls.Add(this.hideUnnamedCheckBox);
            this.Controls.Add(this.customPropertyGrid1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.maxElementsLabel);
            this.Controls.Add(this.inElementNumber);
            this.Controls.Add(this.openedFileLabel);
            this.Controls.Add(this.openButton);
            this.Name = "SimpleEditorForm";
            this.Text = "SimpleReaderForm";
            ((System.ComponentModel.ISupportInitialize)(this.inElementNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label openedFileLabel;
        private System.Windows.Forms.NumericUpDown inElementNumber;
        private System.Windows.Forms.Label maxElementsLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button saveButton;
        private Components.CustomPropertyGridControl.CustomPropertyGrid customPropertyGrid1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox hideUnnamedCheckBox;
    }
}