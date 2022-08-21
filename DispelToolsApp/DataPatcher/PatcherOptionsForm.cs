using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            keepBackupsCheckBox.Checked = patcherParams.KeepBackupFiles;
            keepImageSizeCheckBox.Checked = patcherParams.KeepImageSize;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            patcherParams.KeepBackupFiles = keepBackupsCheckBox.Checked;
            patcherParams.KeepImageSize = keepImageSizeCheckBox.Checked;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
