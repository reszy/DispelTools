using ImageMagick;
using ImageMagick.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.Common
{
    public static class MagickImageSave
    {
        public static IMagickImageEncoder[] Encoders { get; } = CreateEncoders();
        public static string Filter { get; } = CreateFilter();

        private static string CreateFilter()
        {
            return string.Join("|", Encoders.Select(encoder => $"{encoder.Name} ({encoder.FileExtensions})|{encoder.FileExtensions}"));
        }
        private static IMagickImageEncoder[] CreateEncoders()
        {
            return new IMagickImageEncoder[]
            {
                new PngEncoder(),
                new JpegEncoder(),
                new BmpEncoder()
            };
        }

        public interface IMagickImageEncoder
        {
            string Name { get; }
            string FileExtensions { get; }
            MagickFormat Format { get; }
            void ApplySettings(MagickImage image);
        }
        public static void SaveAs(this RawRgb pixels, string output, IMagickImageEncoder encoder)
        {
            var settings = new MagickReadSettings()
            {
                Width = pixels.Width,
                Height = pixels.Height,
                Format = MagickFormat.Rgb,
                Depth = 8,
            };
            var image = new MagickImage(pixels.Bytes, settings);
            encoder.ApplySettings(image);
            image.Write(output, encoder.Format);
            image.Dispose();
        }
        public static void SaveAs(this DirectBitmap bitmap, string output, IMagickImageEncoder encoder)
        {
            var settings = new MagickReadSettings()
            {
                Width = bitmap.Width,
                Height = bitmap.Height,
                Format = MagickFormat.Bgra,
                Depth = 8
            };
            using (var stream = bitmap.Stream())
            {
                var image = new MagickImage(stream, settings);
                encoder.ApplySettings(image);
                image.Write(output, encoder.Format);
                image.Dispose();
            }
        }

        private class PngEncoder : IMagickImageEncoder
        {
            public string Name => "Portable Network Graphics File";
            public string FileExtensions => "*.png";
            public MagickFormat Format => MagickFormat.Png;
            public void ApplySettings(MagickImage image) { }
        }
        private class JpegEncoder : IMagickImageEncoder
        {
            public string Name => "JPEG File";
            public string FileExtensions => "*.jpg;*.jpeg";
            public MagickFormat Format => MagickFormat.Jpg;
            public void ApplySettings(MagickImage image)
            {
                image.Quality = 40;
            }
        }
        private class BmpEncoder : IMagickImageEncoder
        {
            public string Name => "Windows Bitmap File";
            public string FileExtensions => "*.bmp";
            public void ApplySettings(MagickImage image) { }
            public MagickFormat Format => MagickFormat.Bmp3;
        }
    }
}
