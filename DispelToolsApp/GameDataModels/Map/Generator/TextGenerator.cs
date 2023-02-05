using DispelTools.Common;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DispelTools.GameDataModels.Map.Generator
{
    public class TextGenerator
    {
        public static readonly int DigitWidth = 7;
        public static readonly int DigitHeight = 10;

        //private readonly Font font;
        public RawRgb[] DigitCache;

        public TextGenerator(/*Font font*/)
        {
            DigitCache = new RawRgb[10];
            //this.font = font;
            for (int i = 0; i < 10; i++)
            {
                DigitCache[i] = CreateDigit(i);
            }
        }

        private RawRgb CreateDigit(int digit)
        {
            if (digit < 0 || digit > 9) { throw new ArgumentException("number must be single digit"); }

            RawRgb image = new RawRgb(DigitWidth, DigitHeight);

            RectangleF rectf = new RectangleF(0, 0, DigitWidth, DigitHeight);

            //Graphics g = Graphics.FromImage(image.Bitmap);

            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            //g.DrawString(digit.ToString(), font, Brushes.White, rectf);

            //g.Flush();

            return image;
        }

        public void PlotIdOnMap(RawRgb image, short id, int destX, int destY)
        {
            if (destX + DigitWidth <= image.Width && destX >= 0 && destY >= 0 && destY + DigitHeight <= image.Height)
            {
                string strId = id.ToString();
                int xStart = destX - (DigitWidth * strId.Length / 2);
                for (int i = 0; i < strId.Length; i++)
                {
                    char character = strId[i];
                    if (char.IsDigit(character))
                    {
                        PlotDigit(image, DigitCache[character - '0'], xStart + i * DigitWidth, destY);
                    }
                }
            }
        }

        private void PlotDigit(RawRgb image, RawRgb digit, int destX, int destY)
        {
            for (int y = 0; y < DigitHeight; y++)
            {
                for (int x = 0; x < DigitWidth; x++)
                {
                    image.SetPixel(destX + x, destY + y, digit.GetPixel(x, y));
                }
            }
        }
    }
}
