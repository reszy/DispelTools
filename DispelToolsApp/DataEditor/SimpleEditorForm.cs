using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DispelTools.DataEditor
{
    public partial class SimpleEditorForm : Form
    {

        private SimpleEditor editor;
        private SortedSet<int> stat = new SortedSet<int>();
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

        private void elementNumber_ValueChanged(object sender, EventArgs e)
        {
            if (editor != null)
            {
                customPropertyGrid1.SelectedItem = editor.ReadValue((int)inElementNumber.Value);
            }
        }
        private void SetMaxElementsLabel(decimal number) => maxElementsLabel.Text = "/ " + number;

        private void button1_Click(object sender, EventArgs e)
        {
            //if (editor != null)
            //{
            //    var count = stat.Count;
            //    for (int i = 0; i <= editor.GetElementCount(); i++)
            //    {
            //        var npc = (NpcRefDto)editor.ReadValue(i);
            //        stat.Add(npc.NpcId);
            //    }
            //    count = stat.Count - count;
            //    var txt = string.Join(", ", stat);
            //    if (MessageBox.Show(txt, $"Found {count} new NPCs. (OK to copy)", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    { Clipboard.SetText(txt); }
            //}
        }

        private void saveButton_Click(object sender, EventArgs e) => editor?.Save(customPropertyGrid1.SelectedItem, (int)inElementNumber.Value);
    }
}
