using DispelTools.Common;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.GameDataModels.Sprite
{
    public class SpriteSequence
    {
        public bool Animated => ImagesLoaded ? frames.Length > 1 : SequenceInfo.FrameInfos.Length > 1;
        public int FrameCount => frames.Length;

        public SpriteLoader.SequenceInfo SequenceInfo { get; }
        public bool ImagesLoaded { get; }

        private readonly SpriteFrame[] frames;

        public SpriteSequence(SpriteLoader.SequenceInfo sequenceInfo, bool imagesLoaded)
        {
            this.frames = new SpriteFrame[sequenceInfo.FrameInfos.Length];
            SequenceInfo = sequenceInfo;
            ImagesLoaded = imagesLoaded;
        }

        public string[] SaveAsImage(string directory, string filename, bool createGifs, bool blackAsTransparent)
        {
            List<string> savedFilenames = new List<string>();
            if (!Animated)
            {
                var savedFilename = $"{filename}.png";
                frames[0].RawRgb.SaveAsPng($"{directory}\\{savedFilename}", blackAsTransparent);
                savedFilenames.Add(savedFilename);
            }
            else
            {
                if (createGifs)
                {
                    var dimensions = CalculateDimensions();
                    var size = new Size(dimensions.Width, dimensions.Height);
                    var center = new Point(dimensions.X, dimensions.Y);
                    var gifFrames = new List<ImageProcessing.ImageConverter.AnimationFrame>();
                    for (int i = 0; i < frames.Length; i++)
                    {
                        var offset = CalculateFrameOffset(center, new Point(frames[i].OriginX, frames[i].OriginY));
                        gifFrames.Add(frames[i].RawRgb.ToFrame(offset.X, offset.Y));
                    }

                    var savedFilename = $"{filename}.gif";
                    ImageProcessing.ImageConverter.SaveAsGif(gifFrames, size.Width, size.Height, $"{directory}\\{savedFilename}", blackAsTransparent);
                    savedFilenames.Add(savedFilename);
                }
                else
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        var savedFilename = $"{filename}_f{i}.png";
                        frames[i].RawRgb.SaveAsPng($"{directory}\\{savedFilename}", blackAsTransparent);
                        savedFilenames.Add(savedFilename);
                    }
                }
            }
            return savedFilenames.ToArray();
        }

        public SpriteFrame GetFrame(int i) => frames[i];
        public void SetFrame(SpriteFrame frame, int i) => frames[i] = frame;
        private Rectangle CalculateDimensions()
        {
            int maxLeft = 1;
            int maxRight = 1;
            int maxUp = 1;
            int maxDown = 1;
            foreach (var frame in frames)
            {
                int left = frame.OriginX;
                int right = frame.RawRgb.Width - frame.OriginX;
                int up = frame.OriginY;
                int down = frame.RawRgb.Height - frame.OriginY;
                if (right > maxRight) { maxRight = right; }
                if (left > maxLeft) { maxLeft = left; }
                if (up > maxUp) { maxUp = up; }
                if (down > maxDown) { maxDown = down; }
            }
            return new Rectangle(maxLeft, maxUp, maxLeft + maxRight, maxUp + maxDown);
        }

        private Point CalculateFrameOffset(Point boxCenter, Point frameCenter) => new Point(boxCenter.X - frameCenter.X, boxCenter.Y - frameCenter.Y);

        private RawRgb BoxImage(RawRgb sourceImage, Size size, Point position)
        {
            var image = new RawRgb(size.Width, size.Height);
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
