using System;
using System.IO;
using System.Windows.Forms;

namespace DispelTools.ImageAnalyzer
{
    public partial class ImageAnalyzerForm : Form
    {
        private ImageAnalyzerCore imageAnalyzerCore;

        private string filename = null;
        private bool ready = false;
        private bool loaded = false;
        private bool readyToSave = false;

        public ImageAnalyzerForm()
        {
            InitializeComponent();
            imageAlignControls.OptionsChangedEvent += AutoAlign;
            imageAnalyzeControls1.AnalyzerChangedEvent += RefreshImage;

            imageAnalyzerCore = new ImageAnalyzerCore();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog()
            {
                Multiselect = false
            };
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openDialog.FileName;
                ready = true;
                filenameLabel.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if (ready)
            {
                loaded = true;
                createImage(imageAlignControls.ImageOptions);
            }
        }

        private void AutoAlign(object sender, ImageAlignControls.Options options)
        {
            if (ready)
            {
                createImage(options);
            }
        }

        private void RefreshImage(object sender, EventArgs e)
        {
            if (ready)
            {
                imageAnalyzerCore.ApplyFilter(imageAnalyzeControls1.AnalyzePixel);
                pictureDisplayer.SetImage(imageAnalyzerCore.FilteredImage, false, imageAnalyzerCore.RawImageAnalyzed);
            }
        }

        private void createImage(ImageAlignControls.Options options)
        {
            imageAnalyzerCore.LoadImage(filename, options);
            pictureDisplayer.SetImage(imageAnalyzerCore.RawImage, true, imageAnalyzerCore.RawImageAnalyzed);
            RefreshImage(null, EventArgs.Empty);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (readyToSave)
            {
                var saveDialog = new SaveFileDialog()
                {
                    FileName = Path.GetFileNameWithoutExtension(filename),
                    Filter = "PNG|*.png"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureDisplayer.Image.Save(saveDialog.FileName);
                }
            }
        }
    }
}
