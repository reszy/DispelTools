using System.IO;
using DispelTools.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace DispelTools.ImageProcessing.Sprite
{
    public class SpriteSequence
    {
        public bool Animated => frames.Length > 1;

        private readonly SpriteFrame[] frames;

        public SpriteSequence(int frames)
        {
            this.frames = new SpriteFrame[frames];
        }

        public void SaveAsImage(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path).ToUpper();
            if (Animated && extension != "GIF")
            {
                throw new System.ArgumentException("Sprite sequence is animated. Only supported type is GIF");
            }
            if (!Animated && extension != "PNG")
            {
                throw new System.ArgumentException("Sprite sequence is not animated. Only supported type is PNG");
            }
            SaveAsImage(directory, filename);
        }

        public void SaveAsImage(string directory, string filename)
        {
            if (!Animated)
            {
                frames[0].Bitmap.Bitmap.Save($"{directory}\\{filename}.png");
            }
            else
            {
                var gif = CreateGif();
                gif.Save($"{directory}\\{filename}.gif");
            }
        }

        public SpriteFrame GetFrame(int i) => frames[i];
        public void SetFrame(SpriteFrame frame, int i) => frames[i] = frame;

        private Image<Argb32> CreateGif()
        {
            var dimensions = CalculateDimensions();
            var size = new Size(dimensions.Width, dimensions.Height);
            var center = new Point(dimensions.X, dimensions.Y);
            var gif = new Image<Argb32>(size.Width, size.Height);

            foreach (var frame in frames)
            {
                var offset = CalculateFrameOffset(center, new Point(frame.OriginX, frame.OriginY));
                using (var bitmap = BoxImage(frame.Bitmap, size, offset))
                {
                    Image<Argb32> img = Image.LoadPixelData<Argb32>(bitmap.Data, size.Width, size.Height);
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
                var left = frame.OriginX;
                var right = frame.Bitmap.Width - frame.OriginX;
                var up = frame.OriginY;
                var down = frame.Bitmap.Bitmap.Height - frame.OriginY;
                if (right > maxRight) { maxRight = right; }
                if (left > maxLeft) { maxLeft = left; }
                if (up > maxUp) { maxUp = up; }
                if (down > maxDown) { maxDown = down; }
            }
            return new Rectangle(maxLeft, maxUp ,maxLeft + maxRight, maxUp + maxDown);
        }

        private Point CalculateFrameOffset(Point boxCenter, Point frameCenter)
        {
            return new Point(boxCenter.X - frameCenter.X, boxCenter.Y - frameCenter.Y);
        }

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
    }
}
