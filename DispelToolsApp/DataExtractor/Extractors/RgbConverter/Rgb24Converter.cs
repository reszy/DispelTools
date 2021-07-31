using System.Drawing;
using System.Drawing.Imaging;

namespace DispelTools.DataExtractor.RgbConverter
{
    public class Rgb24Converter : Extractor
    {

        public override void ExtractFile(ExtractionFileProcess process)
        {
            var bitmap = new Bitmap(process.Stream);
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    newBitmap.SetPixel(x, y, ConvertFrom555to565(bitmap.GetPixel(x, y)));
                }
            }
            string newFilename = $"{ process.Filename }_Converted.png";
            newBitmap.Save($"{process.OutputDirectory}\\{newFilename}", ImageFormat.Png);
            process.WorkReporter.ReportFileCreated(process, newFilename);
        }

        private Color ConvertFrom555to565(Color color)
        {
            byte newRed = (byte)(color.R << 1);
            if ((color.G & 0b1000_0000) > 0)
            {
                newRed += 0b0000_1000;
            }
            byte newGreen = (byte)(color.G << 1);
            return Color.FromArgb(color.A, newRed, newGreen, color.B);
        }
    }
}
