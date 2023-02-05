using DispelTools.Common;
using System.IO.Abstractions;
using System.Windows;

namespace FileDialogs
{
    public class SaveFileDialog
    {
        private readonly Microsoft.Win32.SaveFileDialog dialog;
        private readonly Window owner;
        private readonly IFileSystem fs;

        public class Configuration
        {
            private string subFolder = string.Empty;

            public bool HaveSubFolder { get; private set; }
            public string SubFolder { get => subFolder; init { HaveSubFolder = true; subFolder = value; } }
            public string Filter { get; set; } = string.Empty;
        }
        public string[] FileNames => dialog.FileNames;
        public string FileName => dialog.FileName;
        public string Filter => dialog.Filter;
        public int FilterIndex => dialog.FilterIndex;
        public SaveFileDialog(IFileSystem fs, Window owner, Configuration configuration)
        {
            this.fs = fs;
            this.owner = owner;
            dialog = new()
            {
                Filter = configuration.Filter,
                InitialDirectory = configuration.HaveSubFolder ? Settings.GameRootDir + '\\' + configuration.SubFolder : Settings.GameRootDir
            };
        }

        public void ShowDialog(Action fileSelected)
        {
            if (dialog.ShowDialog(owner) == true)
            {
                fileSelected.Invoke();
            }
        }
    }
}
