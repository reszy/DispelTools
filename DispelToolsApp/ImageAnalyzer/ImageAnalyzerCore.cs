using DispelTools.Common;
using DispelTools.ImageProcessing;
using System;
using System.Drawing;
using System.IO;

namespace DispelTools.ImageAnalyzer
{
    internal class ImageAnalyzerCore
    {

        public DirectBitmap RawImage { get; private set; } = null;
        public DataAnalyzedBitmap RawImageAnalyzed { get; private set; } = null;
        public DirectBitmap FilteredImage { get; private set; } = null;
        public DirectBitmap EditedImage { get; private set; } = null;

        private ImageAlignControls.Options optionsUsed = null;

        public event EventHandler CreatedNewLayerEvent;

        public void ClearAll()
        {
            EditedImage?.Dispose();
            FilteredImage?.Dispose();
            RawImage?.Dispose();

            RawImageAnalyzed = null;
            EditedImage = null;
            FilteredImage = null;
            RawImage = null;
        }

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

                    var bitmap = new DirectBitmap(options.width, options.height * options.imageNumber);
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
            CreatedNewLayerEvent?.Invoke(this, EventArgs.Empty);
        }

        internal void SaveEditedImage(string filename)
        {
            if (EditedImage == null) { return; }
            if (optionsUsed.width > 0 && optionsUsed.height * optionsUsed.imageNumber > 0)
            {
                using (var file = new BinaryWriter(new FileStream(filename, FileMode.Open, FileAccess.Write)))
                {
                    file.BaseStream.Seek(optionsUsed.offset, SeekOrigin.Begin);
                    var colorManager = ColorManagement.From(optionsUsed.colorMode);

                    for (int i = 0; i < optionsUsed.imageNumber; i++)
                    {
                        for (int y = 0; y < optionsUsed.height; y++)
                        {
                            for (int x = 0; x < optionsUsed.width; x++)
                            {
                                byte[] bytesOverwrite = colorManager.ProduceBytes(EditedImage.GetPixel(x, (optionsUsed.height * i) + y));
                                if (bytesOverwrite.Length + file.BaseStream.Position < file.BaseStream.Length)
                                {
                                    file.Write(bytesOverwrite);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            file.Skip((optionsUsed.lineLen - optionsUsed.width) * colorManager.BytesConsumed);
                        }
                        if (optionsUsed.imageOffset > 0)
                        {
                            file.Skip(optionsUsed.imageOffset);
                        }
                    }
                }
            }
        }
        internal void ApplyFilter(Func<Color, Color> function)
        {
            if (FilteredImage == null)
            {
                FilteredImage = new DirectBitmap(RawImage.Width, RawImage.Height);
                CreatedNewLayerEvent?.Invoke(this, EventArgs.Empty);
            }
            var sourceImage = EditedImage ?? RawImage;
            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    FilteredImage.SetPixel(x, y, function.Invoke(sourceImage.GetPixel(x, y)));
                }
            }
        }
        internal void ModifyImage()
        {

        }

        internal void EditPixel(Point position, Color color)
        {
            if (EditedImage == null)
            {
                EditedImage = DirectBitmap.From(RawImage);
                CreatedNewLayerEvent?.Invoke(this, EventArgs.Empty);
            }
            EditedImage.SetPixel(position.X, position.Y, color);
        }
    }
}
