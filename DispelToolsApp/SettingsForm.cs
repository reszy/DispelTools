using DispelTools.Common;
using System;
using System.IO;
using System.Windows.Forms;

namespace DispelTools
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            gameDirTextBox.Text = Settings.GameRootDir;
            outDirTextBox.Text = Settings.OutRootDir;
        }

        private void gameDirButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = gameDirTextBox.Text;
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.ShowDialog();

            SetGameDir(folderBrowserDialog1.SelectedPath);
        }

        private void outDirButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = outDirTextBox.Text;
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.ShowDialog();

            SetOutDir(folderBrowserDialog1.SelectedPath);
        }

        private void SetGameDir(string gameDir)
        {
            if (Directory.Exists(gameDir))
            {
                try
                {
                    Settings.GameRootDir = gameDir;
                    gameDirTextBox.Text = gameDir;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Cannot set this path. path is not absolute or incorrect.");
                }
            }
        }

        private void SetOutDir(string outDir)
        {
            if (Directory.Exists(outDir))
            {
                try
                {
                    Settings.OutRootDir = outDir;
                    outDirTextBox.Text = outDir;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Cannot set this path. path is not absolute or incorrect.");
                }
            }
        }

        private void gameDirTextBox_TextChanged(object sender, EventArgs e) => SetGameDir(gameDirTextBox.Text);

        private void outDirTextBox_TextChanged(object sender, EventArgs e) => SetOutDir(outDirTextBox.Text);

        private void SettingsForm_Load(object sender, EventArgs e) => ActiveControl = label1;
    }
}
