using DispelTools.ImageProcessing;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DispelTools.ImageAnalyzer
{
    internal class ImageAnalyzerCore
    {

        public Bitmap RawImage { get; private set; } = null;
        public DataAnalyzedBitmap RawImageAnalyzed { get; private set; } = null;
        public Bitmap FilteredImage { get; private set; } = null;
        public Bitmap EditedImage { get; private set; } = null;

        private ImageAlignControls.Options optionsUsed = null;

        public bool IsReadyToSave => RawImage != null;

        internal void LoadImage(string filename, ImageAlignControls.Options options)
        {
            optionsUsed = options;
            if (options.width > 0 && options.height * options.imageNumber > 0)
            {
                using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    file.BaseStream.Seek(options.offset, SeekOrigin.Begin);
                    var colorManager = ColorManagement.From(options.colorMode);

                    var bitmap = new Bitmap(options.width, options.height * options.imageNumber, PixelFormat.Format32bppArgb);
                    var dataAnalyzedBitmap = new DataAnalyzedBitmap(options.width, options.height * options.imageNumber);
                    bool eof = false;

                    for (int i = 0; i < options.imageNumber; i++)
                    {
                        for (int y = 0; y < options.height; y++)
                        {
                            for (int x = 0; x < options.width; x++)
                            {
                                byte[] colorBytes = file.ReadBytes(colorManager.BytesConsumed);
                                dataAnalyzedBitmap.SetPixel(x, (options.height * i) + y, colorBytes);
                                if (colorBytes.Length != colorManager.BytesConsumed)
                                {
                                    eof = true;
                                }
                                if (colorBytes.Length == 4 && colorBytes[3] == 2)
                                {
                                    colorBytes[3] = 2;
                                }
                                var color = eof ? Color.Transparent : colorManager.ProduceColor(colorBytes);

                                if (options.transparency == ImageAlignControls.Options.Transparency.COLOR_KEY_BLACK && color.GetBrightness() == 0)
                                {
                                    color = Color.FromArgb(0, color);
                                }
                                bitmap.SetPixel(x, (options.height * i) + y, color);
                            }
                            file.ReadBytes((options.lineLen - options.width) * colorManager.BytesConsumed);
                        }
                        if (options.imageOffset > 0)
                        {
                            file.ReadBytes(options.imageOffset);
                        }
                    }
                    RawImage = bitmap;
                    RawImageAnalyzed = dataAnalyzedBitmap;
                }
            }
        }
        internal void ApplyFilter(Func<Color, Color> function)
        {
            if (FilteredImage == null)
            {
                FilteredImage = new Bitmap(RawImage.Width, RawImage.Height, PixelFormat.Format32bppArgb);
            }
            for (int y = 0; y < RawImage.Height; y++)
            {
                for (int x = 0; x < RawImage.Width; x++)
                {
                    FilteredImage.SetPixel(x, y, function.Invoke(RawImage.GetPixel(x, y)));
                }
            }
        }
        internal void ModifyImage()
        {

        }
    }
}
