using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DispelTools.GifCreator
{
    public partial class GifCreatorForm : Form
    {
        private string checkedRadioButton;
        public GifCreatorForm()
        {
            InitializeComponent();
            checkedRadioButton = radioButton1.Text;
        }

        private void framesListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (framesListBox.SelectedItem == null)
            {
                return;
            }

            framesListBox.DoDragDrop(framesListBox.SelectedItem, DragDropEffects.Move);
        }

        private void framesListBox_DragOver(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Move;

        private void framesListBox_DragDrop(object sender, DragEventArgs e)
        {
            var point = framesListBox.PointToClient(new Point(e.X, e.Y));
            int index = framesListBox.IndexFromPoint(point);
            if (index < 0)
            {
                index = framesListBox.Items.Count - 1;
            }

            object data = e.Data.GetData(typeof(string));
            framesListBox.Items.Remove(data);
            framesListBox.Items.Insert(index, data);
        }

        private void framesListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (framesListBox.SelectedItem != null)
                {
                    int index = framesListBox.SelectedIndex;
                    framesListBox.Items.RemoveAt(index);
                    if (framesListBox.Items.Count > 0)
                    {
                        framesListBox.SelectedIndex = framesListBox.Items.Count > index ? index : framesListBox.Items.Count - 1;
                    }
                }
                e.Handled = true;
            }
        }

        private void addImagesButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "PNG|*.png;*.PNG",
                Multiselect = true
            };
            dialog.ShowDialog();
            if (dialog.FileNames != null)
            {
                framesListBox.Items.AddRange(dialog.FileNames);
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (framesListBox.Items.Count <= 0) { return; }
            string s1 = Path.GetFileNameWithoutExtension(framesListBox.Items[0] as string);
            string s2 = framesListBox.Items.Count > 1 ? Path.GetFileNameWithoutExtension(framesListBox.Items[1] as string) : null;
            string guessName = s2 == null ? s1 : GenerateName(s1, s2);

            var dialog = new SaveFileDialog()
            {
                Filter = "GIF|*.gif",
                FileName = (guessName + ".gif").Replace("..", ".")
            };
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName.Length > 0)
            {
                createButton.Enabled = false;

                backgroundWorker.RunWorkerAsync(
                    new ToGifConverter.Options()
                    {
                        Aligment = (ToGifConverter.Options.Align)Enum.Parse(typeof(ToGifConverter.Options.Align), checkedRadioButton.ToUpper()),
                        FPS = (int)numericUpDown1.Value,
                        OutputFilename = dialog.FileName
                    });
            }
        }

        private string GenerateName(string s1, string s2)
        {
            string s1l = s1.ToLower();
            string s2l = s2.ToLower();
            string sSmaller = s1.Length < s2.Length ? s1 : s2;
            string result = "";
            for (int i = 0; i < sSmaller.Length; i++)
            {
                if (s1l[i] == s2l[i])
                {
                    result += sSmaller[i];
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) => ToGifConverter.ConvertToGifAndSave(framesListBox.Items.Cast<string>().ToList(), (ToGifConverter.Options)e.Argument);

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            framesListBox.Items.Clear();
            createButton.Enabled = true;
        }

        private void aligmentRadioButton_CheckedChanged(object sender, EventArgs e) => checkedRadioButton = (sender as RadioButton).Text;
    }
}
