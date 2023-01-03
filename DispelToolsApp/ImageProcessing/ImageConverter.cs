using DispelTools.Common;
using ImageMagick;
using System;
using System.Collections.Generic;

namespace DispelTools.ImageProcessing
{
    public static class ImageConverter
    {
        private static readonly MagickColor black = MagickColor.FromRgb(0, 0, 0);
        private static readonly MagickColor transparent = MagickColor.FromRgba(0, 0, 0 ,0);


        public static DirectBitmap ToDirectBitmap(this RawRgb pixels)
        {
            var bitmap = new DirectBitmap(pixels.Width, pixels.Height);
            for (int i = 0; i < bitmap.Bits.Length; i++)
            {
                var bi = i * 3;
                uint r = pixels.Bytes[bi + 0];
                uint g = pixels.Bytes[bi + 1];
                uint b = pixels.Bytes[bi + 2];
                uint a = ((r + g + b) > 0) ? 255u : 0;
                uint color = (a << 24) + (r << 16) + (g << 8) + b;
                bitmap.Bits[i] = (int)color;
            }
            return bitmap;
        }
        public static void SaveAsPng(this RawRgb pixels, string output, bool blackAsTransparent)
        {
            var settings = new MagickReadSettings()
            {
                Width = pixels.Width,
                Height = pixels.Height,
                Format = MagickFormat.Rgb,
                Depth = 8,
            };
            var image = new MagickImage(pixels.Bytes, settings);
            if(blackAsTransparent)
            {
                image.ColorAlpha(black);
            }
            image.Write(output, MagickFormat.Png);
            image.Dispose();
        }
        public static AnimationFrame ToFrame(this RawRgb pixels, int offsetX, int offsetY)
        {
            return new AnimationFrame(pixels, offsetX, offsetY);
        }

        public static void SaveAsGif(List<AnimationFrame> frames, int width, int height, string output, bool blackAsTransparent)
        {
            var collection = new MagickImageCollection();
            for (int i = 0; i < frames.Count; i++)
            {
                var frame = frames[i];
                var settings = new MagickReadSettings()
                {
                    Width = frame.Pixels.Width,
                    Height = frame.Pixels.Height,
                    Format = MagickFormat.Rgb,
                    Depth = 8,
                };
                var image = new MagickImage(frame.Pixels.Bytes, settings);
                if (blackAsTransparent)
                {
                    image.Transparent(black);
                }
                var resized = new MagickImage(blackAsTransparent ? transparent : black, width, height);
                resized.Composite(image, frame.OffsetX, frame.OffsetY, CompositeOperator.Src);
                resized.AnimationIterations = 0;
                resized.AnimationDelay = 17;
                resized.GifDisposeMethod = GifDisposeMethod.Background;
                collection.Add(resized);
                image.Dispose();
            }
            collection.Quantize();
            collection.Write(output, MagickFormat.Gif);
            collection.Dispose();
        }

        public class AnimationFrame
        {
            public AnimationFrame(RawRgb pixels, int offsetX, int offsetY)
            {
                Pixels = pixels;
                OffsetX = offsetX;
                OffsetY = offsetY;
            }

            public RawRgb Pixels { get; }
            public int OffsetX { get; }
            public int OffsetY { get; }
        }
    }
}
