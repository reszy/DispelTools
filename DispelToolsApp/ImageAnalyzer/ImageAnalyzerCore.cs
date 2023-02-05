using DispelTools.Common;
using DispelTools.ImageProcessing;
using DispelTools.ImageProcessing.Filters;
using System;
using System.Drawing;
using System.IO;

namespace DispelTools.ImageAnalyzer
{
    public class ImageAnalyzerCore
    {
        public class ImageAlignOptions
        {
            public int offset { get; internal set; }
            public int width { get; internal set; }
            public int height { get; internal set; }
            public int lineLen { get; internal set; }
            public int imageNumber { get; internal set; }
            public int imageOffset { get; internal set; }
            public ColorManagement.ColorMode colorMode { get; internal set; }
            public Transparency transparency { get; internal set; }

            public enum Transparency { NONE, COLOR_KEY_BLACK, ALPHA };
        }

        public RawRgba? RawImage { get; private set; } = null;
        public DataAnalyzedBitmap? RawImageAnalyzed { get; private set; } = null;
        public RawRgba? FilteredImage { get; private set; } = null;
        public RawRgba? EditedImage { get; private set; } = null;

        private ImageAlignOptions optionsUsed = null;

        public event EventHandler CreatedNewLayerEvent;

        public void ClearAll()
        {
            RawImageAnalyzed = null;
            EditedImage = null;
            FilteredImage = null;
            RawImage = null;
        }

        public bool IsReadyToSave => RawImage != null;

        public void LoadImage(string filename, ImageAlignOptions options)
        {
            optionsUsed = options;
            if (options.width > 0 && options.height * options.imageNumber > 0)
            {
                using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    file.BaseStream.Seek(options.offset, SeekOrigin.Begin);
                    var colorManager = ColorManagement.From(options.colorMode);

                    var bitmap = new RawRgba(options.width, options.height * options.imageNumber);
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

                                if (options.transparency == ImageAlignOptions.Transparency.COLOR_KEY_BLACK && color.GetBrightness() == 0)
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

        public void SaveEditedImage(string filename)
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
        public void ApplyFilter(IPerPixelFilter filter)
        {
            if (FilteredImage == null || (FilteredImage.Width != RawImage.Width || FilteredImage.Height != RawImage.Height))
            {
                FilteredImage = new RawRgba(RawImage.Width, RawImage.Height);
                CreatedNewLayerEvent?.Invoke(this, EventArgs.Empty);
            }
            if(filter == null)
            {
                return;
            }
            var sourceImage = EditedImage ?? RawImage;
            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    FilteredImage.SetPixel(x, y, filter.Apply(sourceImage.GetPixel(x, y)));
                }
            }
        }
        public void ModifyImage()
        {

        }

        public void EditPixel(Point position, Color color)
        {
            if (EditedImage == null)
            {
                //TODO EditedImage = RawRgba.From(RawImage);
                CreatedNewLayerEvent?.Invoke(this, EventArgs.Empty);
            }
            EditedImage.SetPixel(position.X, position.Y, color);
        }
    }
}
