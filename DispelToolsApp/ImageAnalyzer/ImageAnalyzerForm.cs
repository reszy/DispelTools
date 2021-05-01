using DispelTools.Components;
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
            imageAnalyzeControls.AnalyzerChangedEvent += RefreshImage;
            imageEditControls.EditToolChangedEvent += SetSelection;
            pictureDisplayer.PixelSelectedEvent += imageEditControls.PixelSelected;

            imageAnalyzerCore = new ImageAnalyzerCore();
            imageEditControls.SetImages(ref imageAnalyzerCore);
            imageEditControls.ChangesMadeEvent += RefreshImage;
            imageAnalyzerCore.CreatedNewLayerEvent += RefreshImage;
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
                imageAnalyzerCore.ClearAll();
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
                if (imageAnalyzeControls.CurrentAnalyzer != ImageAnalyzeControls.Analyzer.NONE)
                {
                    imageAnalyzerCore.ApplyFilter(imageAnalyzeControls.ActiveFilter);
                    pictureDisplayer.SetImage(imageAnalyzerCore.FilteredImage.Bitmap, false, imageAnalyzerCore.RawImageAnalyzed);
                }
                else if (imageAnalyzerCore.EditedImage != null)
                {
                    pictureDisplayer.SetImage(imageAnalyzerCore.EditedImage.Bitmap, false, imageAnalyzerCore.RawImageAnalyzed);
                }
                else
                {
                    pictureDisplayer.SetImage(imageAnalyzerCore.RawImage.Bitmap, false, imageAnalyzerCore.RawImageAnalyzed);
                }
            }
        }

        private void SetSelection(object sender, EventArgs e)
        {
            switch (imageEditControls.CurrentEditTool)
            {
                case ImageEditControls.EditTool.ROW_SELECTOR:
                    if (pictureDisplayer.CurrentMouseMode != PictureDiplayer.MouseMode.RectSelector)
                    {
                        pictureDisplayer.CurrentMouseMode = PictureDiplayer.MouseMode.RectSelector;
                    }
                    break;
                case ImageEditControls.EditTool.RECT_SELECTOR:
                    if (pictureDisplayer.CurrentMouseMode != PictureDiplayer.MouseMode.RowSelector)
                    {
                        pictureDisplayer.CurrentMouseMode = PictureDiplayer.MouseMode.RowSelector;
                    }
                    break;
                case ImageEditControls.EditTool.NONE:
                case ImageEditControls.EditTool.COLOR_PICKER:
                case ImageEditControls.EditTool.PENCIL:
                default:
                    if (pictureDisplayer.CurrentMouseMode != PictureDiplayer.MouseMode.Pointer)
                    {
                        pictureDisplayer.CurrentMouseMode = PictureDiplayer.MouseMode.Pointer;
                    }
                    break;
            }
        }

        private void createImage(ImageAlignControls.Options options)
        {
            imageEditControls.ColorManagmentChanged(options.colorMode);
            imageAnalyzerCore.LoadImage(filename, options);
            pictureDisplayer.SetImage(imageAnalyzerCore.RawImage.Bitmap, true, imageAnalyzerCore.RawImageAnalyzed);
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

        private void reloadButton_Click(object sender, EventArgs e)
        {
            imageAnalyzerCore.ClearAll();
            createImage(imageAlignControls.ImageOptions);
        }

        private void overwriteButton_Click(object sender, EventArgs e)
        {
            if (imageAnalyzerCore.EditedImage == null)
            {
                return;
            }

            string orginalBackup = filename + "orginalbackup.bak";
            try
            {
                if (!File.Exists(orginalBackup))
                {
                    File.Copy(filename, orginalBackup);
                }
                imageAnalyzerCore.SaveEditedImage(filename);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show($"Cannot write in directory {Path.GetDirectoryName(orginalBackup)}");
            }
        }
    }
}
