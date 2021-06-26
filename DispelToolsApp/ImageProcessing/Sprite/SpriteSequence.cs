using DispelTools.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace DispelTools.ImageProcessing.Sprite
{
    public class SpriteSequence : IDisposable
    {
        public bool Animated => frames.Length > 1;

        private readonly SpriteFrame[] frames;

        public SpriteSequence(int frames)
        {
            this.frames = new SpriteFrame[frames];
        }

        public void SaveAsImage(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path).ToUpper();
            if (Animated && extension != "GIF")
            {
                throw new ArgumentException("Sprite sequence is animated. Only supported type is GIF");
            }
            if (!Animated && extension != "PNG")
            {
                throw new ArgumentException("Sprite sequence is not animated. Only supported type is PNG");
            }
            SaveAsImage(directory, filename);
        }

        public void SaveAsImage(string directory, string filename)
        {
            if (!Animated)
            {
                frames[0].Bitmap.Bitmap.Save($"{directory}\\{filename}.png", ImageFormat.Png);
            }
            else
            {
                var gif = CreateGif();
                gif.Save($"{directory}\\{filename}.gif");
            }
        }

        public SpriteFrame GetFrame(int i) => frames[i];
        public void SetFrame(SpriteFrame frame, int i) => frames[i] = frame;

        private Image<Rgba32> CreateGif()
        {
            var dimensions = CalculateDimensions();
            var size = new Size(dimensions.Width, dimensions.Height);
            var center = new Point(dimensions.X, dimensions.Y);
            var gif = new Image<Rgba32>(size.Width, size.Height);

            foreach (var frame in frames)
            {
                var offset = CalculateFrameOffset(center, new Point(frame.OriginX, frame.OriginY));
                using (var bitmap = BoxImage(frame.Bitmap, size, offset))
                {
                    byte[] data = ConvertToRgbaByteArray(bitmap);
                    var img = Image.LoadPixelData<Rgba32>(data, bitmap.Width, bitmap.Height);
                    img.Mutate(x => x.BackgroundColor(Color.Black));
                    var gifFrame = img.Frames[0];
                    gifFrame.Metadata.GetGifMetadata().FrameDelay = 16;
                    gif.Frames.AddFrame(gifFrame);
                }
            }
            return gif;
        }
        private Rectangle CalculateDimensions()
        {
            int maxLeft = 1;
            int maxRight = 1;
            int maxUp = 1;
            int maxDown = 1;
            foreach (var frame in frames)
            {
                int left = frame.OriginX;
                int right = frame.Bitmap.Width - frame.OriginX;
                int up = frame.OriginY;
                int down = frame.Bitmap.Bitmap.Height - frame.OriginY;
                if (right > maxRight) { maxRight = right; }
                if (left > maxLeft) { maxLeft = left; }
                if (up > maxUp) { maxUp = up; }
                if (down > maxDown) { maxDown = down; }
            }
            return new Rectangle(maxLeft, maxUp, maxLeft + maxRight, maxUp + maxDown);
        }

        private Point CalculateFrameOffset(Point boxCenter, Point frameCenter) => new Point(boxCenter.X - frameCenter.X, boxCenter.Y - frameCenter.Y);

        private DirectBitmap BoxImage(DirectBitmap sourceImage, Size size, Point position)
        {
            var image = new DirectBitmap(size.Width, size.Height);
            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    image.SetPixel(x + position.X, y + position.Y, sourceImage.GetPixel(x, y));
                }
            }
            return image;
        }

        private byte[] ConvertToRgbaByteArray(DirectBitmap bitmap)
        {
            byte[] array = new byte[bitmap.Bits.Length * 4];
            for (int i = 0; i < bitmap.Bits.Length; i++)
            {
                byte[] pixel = BitConverter.GetBytes(bitmap.Bits[i]);
                int bytePixelNumber = (i * 4);
                array[bytePixelNumber + 0] = pixel[2];
                array[bytePixelNumber + 1] = pixel[1];
                array[bytePixelNumber + 2] = pixel[0];
                array[bytePixelNumber + 3] = pixel[3];
            }
            return array;
        }

        public void Dispose()
        {
            foreach (var frame in frames)
            {
                frame.Bitmap.Dispose();
            }
        }
    }
}
