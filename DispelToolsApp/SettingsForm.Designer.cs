
namespace DispelTools
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gameDirTextBox = new System.Windows.Forms.TextBox();
            this.gameDirButton = new System.Windows.Forms.Button();
            this.outDirButton = new System.Windows.Forms.Button();
            this.outDirTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game directory: ";
            // 
            // gameDirTextBox
            // 
            this.gameDirTextBox.Location = new System.Drawing.Point(104, 51);
            this.gameDirTextBox.Name = "gameDirTextBox";
            this.gameDirTextBox.Size = new System.Drawing.Size(287, 20);
            this.gameDirTextBox.TabIndex = 1;
            this.gameDirTextBox.TextChanged += new System.EventHandler(this.gameDirTextBox_TextChanged);
            // 
            // gameDirButton
            // 
            this.gameDirButton.Location = new System.Drawing.Point(398, 50);
            this.gameDirButton.Name = "gameDirButton";
            this.gameDirButton.Size = new System.Drawing.Size(29, 23);
            this.gameDirButton.TabIndex = 2;
            this.gameDirButton.Text = "...";
            this.gameDirButton.UseVisualStyleBackColor = true;
            this.gameDirButton.Click += new System.EventHandler(this.gameDirButton_Click);
            // 
            // outDirButton
            // 
            this.outDirButton.Location = new System.Drawing.Point(398, 91);
            this.outDirButton.Name = "outDirButton";
            this.outDirButton.Size = new System.Drawing.Size(29, 23);
            this.outDirButton.TabIndex = 5;
            this.outDirButton.Text = "...";
            this.outDirButton.UseVisualStyleBackColor = true;
            this.outDirButton.Click += new System.EventHandler(this.outDirButton_Click);
            // 
            // outDirTextBox
            // 
            this.outDirTextBox.Location = new System.Drawing.Point(104, 92);
            this.outDirTextBox.Name = "outDirTextBox";
            this.outDirTextBox.Size = new System.Drawing.Size(287, 20);
            this.outDirTextBox.TabIndex = 4;
            this.outDirTextBox.TextChanged += new System.EventHandler(this.outDirTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output directory: ";
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.Location = new System.Drawing.Point(188, 118);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.saveSettingsButton.TabIndex = 6;
            this.saveSettingsButton.Text = "Save";
            this.saveSettingsButton.UseVisualStyleBackColor = true;
            this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 189);
            this.Controls.Add(this.saveSettingsButton);
            this.Controls.Add(this.outDirButton);
            this.Controls.Add(this.outDirTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gameDirButton);
            this.Controls.Add(this.gameDirTextBox);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox gameDirTextBox;
        private System.Windows.Forms.Button gameDirButton;
        private System.Windows.Forms.Button outDirButton;
        private System.Windows.Forms.TextBox outDirTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveSettingsButton;
    }
}