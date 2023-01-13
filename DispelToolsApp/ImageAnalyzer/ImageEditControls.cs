using DispelTools.Components.PictureDisplay;
using DispelTools.ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.ImageAnalyzer
{
    public partial class ImageEditControls : UserControl
    {
        private ColorManagement colorManagement;
        private Color selectedColor = Color.FromArgb(160, 83, 187, 38);
        private DataAnalyzedBitmap.DataPixel selectedValue;
        private bool valuesAreUpdated = false;
        private bool editingActive = false;
        private ImageAnalyzerCore images;
        public ImageEditControls()
        {
            InitializeComponent();

            colorBackground.Paint += drawSelectedColor;
            inValue.Maximum = uint.MaxValue;
            inValue.Minimum = 0;
        }

        public enum EditTool { NONE, COLOR_PICKER, ROW_SELECTOR, RECT_SELECTOR, PENCIL };

        public EditTool CurrentEditTool { get; private set; }

        public event EventHandler EditToolChangedEvent;
        public event EventHandler ChangesMadeEvent;

        internal void SetImages(ImageAnalyzerCore imageAnalyzerCore) => images = imageAnalyzerCore;

        public void ColorManagmentChanged(ColorManagement.ColorMode colorMode) => colorManagement = ColorManagement.From(colorMode);

        private void ColorInputsChanged(object sender, EventArgs e)
        {
            if (valuesAreUpdated || colorManagement == null) { return; }
            valuesAreUpdated = true;
            var choosedColor = Color.FromArgb((int)inA.Value, (int)inR.Value, (int)inG.Value, (int)inB.Value);
            byte[] newValue = colorManagement.ProduceBytes(choosedColor);
            selectedValue = new DataAnalyzedBitmap.DataPixel(newValue);
            selectedColor = colorManagement.ProduceColor(newValue);
            if (selectedColor != choosedColor)
            {
                UpdateColorInputs();
            }
            UpdateValueInputs();
            Invalidate();
            valuesAreUpdated = false;
        }

        private void ValueInputsChanged(object sender, EventArgs e)
        {
            if (valuesAreUpdated || colorManagement == null) { return; }
            valuesAreUpdated = true;
            byte[] bytes = BitConverter.GetBytes((long)inValue.Value);
            selectedColor = colorManagement.ProduceColor(bytes);
            UpdateColorInputs();
            Invalidate();
            valuesAreUpdated = false;
        }

        private void UpdateColorInputs()
        {
            inR.Value = selectedColor.R;
            inG.Value = selectedColor.G;
            inB.Value = selectedColor.B;
            inA.Value = selectedColor.A;
        }
        private void UpdateValueInputs()
        {
            inBytes.Maximum = colorManagement.BytesConsumed;
            inBytes.Value = colorManagement.BytesConsumed;
            inValue.Value = (uint)selectedValue.QWord;
        }

        private void ProbeColor(Color color)
        {
            selectedValue = new DataAnalyzedBitmap.DataPixel(colorManagement.ProduceBytes(color));
            selectedColor = color;
            valuesAreUpdated = true;
            UpdateColorInputs();
            UpdateValueInputs();
            Invalidate();
            valuesAreUpdated = false;
        }

        private void drawSelectedColor(object sender, PaintEventArgs e)
        {
            using (var brush = new SolidBrush(Color.FromArgb(selectedColor.R, selectedColor.G, selectedColor.B)))
            using (var transparentBrush = new SolidBrush(selectedColor))
            {
                var size = (sender as Control).Size;

                e.Graphics.FillRectangle(brush, 0, 0, size.Width / 2, size.Height);
                e.Graphics.FillRectangle(transparentBrush, size.Width / 2, 0, size.Width / 2, size.Height);
            }
        }

        private void selectButton_CheckedChanged(object sender, EventArgs e)
        {
            CurrentEditTool = EditTool.RECT_SELECTOR;
            EditToolChangedEvent?.Invoke(sender, EventArgs.Empty);
        }

        private void pointerButton_CheckedChanged(object sender, EventArgs e)
        {
            CurrentEditTool = EditTool.NONE;
            EditToolChangedEvent?.Invoke(sender, EventArgs.Empty);
        }

        private void pencilButton_CheckedChanged(object sender, EventArgs e)
        {
            CurrentEditTool = EditTool.PENCIL;
            EditToolChangedEvent?.Invoke(sender, EventArgs.Empty);
        }

        private void colorPickerButton_CheckedChanged(object sender, EventArgs e)
        {
            CurrentEditTool = EditTool.COLOR_PICKER;
            EditToolChangedEvent?.Invoke(sender, EventArgs.Empty);
        }

        public void PixelSelected(object sender, PixelSelectedArgs pixelSelectedArgs)
        {
            if (CurrentEditTool == EditTool.COLOR_PICKER)
            {
                ProbeColor(pixelSelectedArgs.PixelColor);
            }
            if (CurrentEditTool == EditTool.PENCIL)
            {
                images.EditPixel(pixelSelectedArgs.Position, selectedColor);
                ChangesMadeEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
