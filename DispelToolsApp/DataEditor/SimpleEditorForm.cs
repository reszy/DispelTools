using DispelTools.Common;
using System;
using System.Windows.Forms;

namespace DispelTools.DataEditor
{
    public partial class SimpleEditorForm : Form
    {

        private SimpleEditor editor;
        public SimpleEditorForm()
        {
            InitializeComponent();
            customPropertyGrid1.SelectedItem = Components.CustomPropertyGridControl.PropertyItem.Sample();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog(() =>
            {
                openFileDialog.InitialDirectory = openFileDialog.FileName;
                editor = new SimpleEditor(openFileDialog.FileName);
                if (editor.CanOpen())
                {
                    inElementNumber.Value = 0;
                    customPropertyGrid1.SelectedItem = editor.ReadValue(0);

                    SetMaxElementsLabel(inElementNumber.Maximum = editor.GetElementCount());
                    openedFileLabel.Text = openFileDialog.FileName;
                }
                else
                {
                    MessageBox.Show(editor.ValidationMessage, "File unsupported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void elementNumber_ValueChanged(object sender, EventArgs e) => Read();
        private void SetMaxElementsLabel(decimal number) => maxElementsLabel.Text = "/ " + number;

        private void debugButton_Click(object sender, EventArgs e)
        {
        }

        private void saveButton_Click(object sender, EventArgs e) => editor?.Save(customPropertyGrid1.SelectedItem, (int)inElementNumber.Value);

        private void Read()
        {
            if (editor != null)
            {
                customPropertyGrid1.SelectedItem = editor.ReadValue((int)inElementNumber.Value);
            }
        }

        private void hideUnnamedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            customPropertyGrid1.HideUnnamedFields = hideUnnamedCheckBox.Checked;
            Read();
        }
    }
}
