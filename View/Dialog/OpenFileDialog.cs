﻿using DispelTools.Common;
using System;
using System.IO.Abstractions;
using System.Windows;

namespace View.Dialog
{
    internal class OpenFileDialog
    {
        private readonly Microsoft.Win32.OpenFileDialog dialog;
        private readonly Window owner;
        private readonly IFileSystem fs;

        public class Configuration
        {
            private string subFolder = string.Empty;

            public bool HaveSubFolder { get; private set; }
            public string SubFolder { get => subFolder; init { HaveSubFolder = true; subFolder = value; } }
            public bool Multiselect { get; init; } = false;
        }
        public string[] FileNames => dialog.FileNames;
        public string FileName => dialog.FileName;
        public OpenFileDialog(IFileSystem fs, Window owner, Configuration configuration)
        {
            this.fs = fs;
            this.owner = owner;
            dialog = new()
            {
                Multiselect = configuration.Multiselect,
                InitialDirectory = configuration.HaveSubFolder ? Settings.GameRootDir + '\\' + configuration.SubFolder : Settings.GameRootDir
            };
        }

        public void ShowDialog(Action fileSelected)
        {
            if (dialog.ShowDialog(owner) == true)
            {
                if (dialog.Multiselect && dialog.FileNames.Length > 0)
                {
                    dialog.InitialDirectory = fs.Path.GetDirectoryName(dialog.FileNames[0]);
                    fileSelected.Invoke();
                }
                else if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    dialog.InitialDirectory = fs.Path.GetDirectoryName(dialog.FileName);
                    fileSelected.Invoke();
                }
            }
        }
    }
}
