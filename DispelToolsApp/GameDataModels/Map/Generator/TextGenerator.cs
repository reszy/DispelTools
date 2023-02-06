using DispelTools.Common;
using ImageMagick;
using System.Reflection;

namespace DispelTools.GameDataModels.Map.Generator
{
    public class TextGenerator
    {
        public static readonly int DigitWidth = 7;
        public static readonly int DigitHeight = 10;

        public RawRgb[] DigitCache;

        public TextGenerator()
        {
            DigitCache = new RawRgb[10];
            var image = LoadDigitsImage();
            for (int i = 0; i < 10; i++)
            {
                DigitCache[i] = CreateDigit(i, image);
            }
        }

        private static RawRgb LoadDigitsImage()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DispelTools.Resources.numbers.png");
            if (stream is null) throw new Exception("Cannot open embeded resource numbers.png");

            var image = new MagickImage(stream);
            var rawRgbImage = new RawRgb(image.Width, image.Height);
            using (var memory = new MemoryStream(rawRgbImage.Bytes, true))
                image.Write(memory, MagickFormat.Rgb);
            return rawRgbImage;
        }

        private static RawRgb CreateDigit(int digit, RawBitmap digitsImage)
        {
            if (digit < 0 || digit > 9) { throw new ArgumentException("number must be single digit"); }

            RawRgb image = new RawRgb(DigitWidth, DigitHeight);

            for (int x = digit * DigitWidth, xDst = 0; xDst < DigitWidth; x++, xDst++)
            {
                for (int y = 0; y < DigitHeight; y++)
                {
                    image.SetPixel(xDst, y, digitsImage.GetPixel(x, y));
                }
            }

            return image;
        }

        public void PlotIdOnMap(RawBitmap image, short id, int destX, int destY)
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

        private static void PlotDigit(RawBitmap image, RawRgb digit, int destX, int destY)
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
