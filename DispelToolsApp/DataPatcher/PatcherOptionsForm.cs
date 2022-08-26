using System;
using System.Windows.Forms;
using static DispelTools.DataPatcher.PatcherParams;

namespace DispelTools.DataPatcher
{
    public partial class PatcherOptionsForm : Form
    {
        private PatcherParams patcherParams;
        public PatcherOptionsForm(OptionNames acceptedOptions, PatcherParams patcherParams)
        {
            InitializeComponent();
            //Enable options
            keepBackupsCheckBox.Enabled = acceptedOptions.HasFlag(OptionNames.KeepBackupFiles);
            keepImageSizeCheckBox.Enabled = acceptedOptions.HasFlag(OptionNames.KeepImageSize);

            //Set values
            this.patcherParams = patcherParams;
            keepBackupsCheckBox.Checked = patcherParams.options.KeepBackupFiles;
            keepImageSizeCheckBox.Checked = patcherParams.options.KeepImageSize;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            patcherParams.options.KeepBackupFiles = keepBackupsCheckBox.Checked;
            patcherParams.options.KeepImageSize = keepImageSizeCheckBox.Checked;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
