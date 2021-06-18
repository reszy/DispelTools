using System;
using System.IO;
using System.Windows.Forms;

namespace DispelTools.Common
{
    public static class OpenDialogExtension
    {
        public static void ShowDialog(this OpenFileDialog dialog, Action fileSelected)
        {
            if (string.IsNullOrEmpty(dialog.InitialDirectory))
            {
                dialog.InitialDirectory = Settings.GameRootDir;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Multiselect && dialog.FileNames.Length > 0)
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileNames[0]);
                    fileSelected.Invoke();
                }
                else if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileName);
                    fileSelected.Invoke();
                }
            }
        }
        public static void ShowDialog(this FolderBrowserDialog dialog, Action fileSelected)
        {
            if (string.IsNullOrEmpty(dialog.SelectedPath))
            {
                dialog.SelectedPath = Settings.GameRootDir;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                fileSelected.Invoke();
            }
        }
    }
}
