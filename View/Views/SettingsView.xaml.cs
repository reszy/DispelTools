using DispelTools.Common;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl, INestedView
    {
        private readonly System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new();
        private string gameDirText;
        private string outDirText;

        public SettingsView()
        {
            InitializeComponent();
            GameDirTextBox.Text = gameDirText = Settings.GameRootDir;
            OutDirTextBox.Text = outDirText = Settings.OutRootDir;
        }

        public string GameDirText { get => gameDirText; set => SetGameDir(value); }
        public string OutDirText { get => outDirText; set => SetOutDir(value); }

        public string ViewName => "Settings";

        private void GameDirButtonClick(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.SelectedPath = GameDirText;
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.ShowDialog();

            SetGameDir(folderBrowserDialog.SelectedPath);
        }
        private void OutDirButtonClick(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.SelectedPath = OutDirText;
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.ShowDialog();

            SetOutDir(folderBrowserDialog.SelectedPath);
        }

        private void SetGameDir(string gameDir)
        {
            if (Directory.Exists(gameDir))
            {
                try
                {
                    Settings.GameRootDir = gameDir;
                    GameDirTextBox.Text = gameDirText = gameDir;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Cannot set this path. Path is not absolute or incorrect.");
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
                    OutDirTextBox.Text = outDirText = outDir;
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Cannot set this path. Path is not absolute or incorrect.");
                }
            }
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            Settings.SaveSettings();
        }
    }
}
