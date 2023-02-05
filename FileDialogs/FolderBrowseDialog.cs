using DispelTools.Common;
using System.IO.Abstractions;
using System.Windows;

namespace FileDialogs
{
    public class FolderBrowseDialog
    {
        private readonly System.Windows.Forms.FolderBrowserDialog dialog;
        private readonly Window owner;
        private readonly IFileSystem fs;

        public class Configuration
        {
            private string subFolder = string.Empty;

            public bool HaveSubFolder { get; private set; }
            public string SubFolder { get => subFolder; init { HaveSubFolder = true; subFolder = value; } }
            public bool ShowNewFolderButton { get; init; } = false;
        }
        public string DirectoryPath { get => dialog.SelectedPath; set => dialog.SelectedPath = value; }
        public FolderBrowseDialog(IFileSystem fs, Window owner, Configuration configuration)
        {
            this.fs = fs;
            this.owner = owner;
            dialog = new()
            {
                ShowNewFolderButton = configuration.ShowNewFolderButton,
                InitialDirectory = configuration.HaveSubFolder ? Settings.GameRootDir + '\\' + configuration.SubFolder : Settings.GameRootDir
            };
        }

        public void ShowDialog(Action fileSelected)
        {
            if (dialog.ShowDialog((System.Windows.Forms.IWin32Window)owner) == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    dialog.InitialDirectory = fs.Path.GetDirectoryName(dialog.SelectedPath) ?? string.Empty;
                    fileSelected.Invoke();
                }
            }
        }
    }
}
