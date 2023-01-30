using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace View.Dialog
{
    internal class SaveFileDialog
    {
        private readonly Microsoft.Win32.SaveFileDialog dialog;
        private readonly Window owner;
        private readonly IFileSystem fs;

        public class Configuration
        {
            private string subFolder = string.Empty;

            public bool HaveSubFolder { get; private set; }
            public string SubFolder { get => subFolder; init { HaveSubFolder = true; subFolder = value; } }
        }
        public string[] FileNames => dialog.FileNames;
        public string FileName => dialog.FileName;
        public SaveFileDialog(IFileSystem fs, Window owner, Configuration configuration)
        {
            this.fs = fs;
            this.owner = owner;
            dialog = new()
            {
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
