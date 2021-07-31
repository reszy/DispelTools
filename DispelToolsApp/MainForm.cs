using DispelTools.DebugTools.MetricTools;
using System;
using System.Windows.Forms;

namespace DispelTools
{
    public partial class MainForm : Form
    {
        private static string title = "Dispel Extractor Tools";
        private Form currentlyEmbedded;
        public MainForm()
        {
            InitializeComponent();
            Text = title;
            nestForm(new SettingsForm());
            FormClosed += Metrics.DumpMetrics;
        }

        private void nestForm(Form embeddedForm)
        {
            currentlyEmbedded?.Close();
            currentlyEmbedded = embeddedForm;
            contentPanel.Controls.Clear();

            embeddedForm.TopLevel = false;
            embeddedForm.FormBorderStyle = FormBorderStyle.None;

            Text = $"{title} - {embeddedForm.Text}";

            contentPanel.Controls.Add(embeddedForm);
            embeddedForm.Show();
        }

        private void selectMenuButton(object sender)
        {
            foreach (object subitem in menuStrip.Items)
            {
                if (subitem is ToolStripMenuItem)
                {
                    uncheckExcept(subitem as ToolStripMenuItem, sender);
                }
            }
        }

        private void uncheckExcept(ToolStripMenuItem menuItem, object itemException)
        {
            if (menuItem != itemException)
            {
                menuItem.Checked = false;
                foreach (object subitem in menuItem.DropDownItems)
                {
                    if (subitem is ToolStripMenuItem)
                    {
                        uncheckExcept(subitem as ToolStripMenuItem, itemException);
                    }
                }
            }
        }

        private void soundExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataExtractor.ExtractorForm(new DataExtractor.SoundExtractor.SnfSoundExtractorFactory()));
        }

        private void imageAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new ImageAnalyzer.ImageAnalyzerForm());
        }

        private void imageExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataExtractor.ExtractorForm(new DataExtractor.ImageExtractor.SprImageExtractorFactory()));
        }

        private void stringExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataExtractor.ExtractorForm(new DataExtractor.StringExtractor.StringExtractorFactory()));
        }

        private void mapImageExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataExtractor.ExtractorForm(new DataExtractor.MapExtractor.MapImageExtractorFactory()));
        }

        private void contentPanel_Resize(object sender, EventArgs e)
        {

        }

        private void allExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataExtractor.ExtractorForm(new DataExtractor.AllExtractor.AllFilesExtractorFactory()));
        }

        private void simpleEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new DataEditor.SimpleEditorForm());
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e) => Close();

        private void mapViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new Viewers.MapViewer.MapViewerForm());
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectMenuButton(sender);
            nestForm(new SettingsForm());
        }
    }
}
