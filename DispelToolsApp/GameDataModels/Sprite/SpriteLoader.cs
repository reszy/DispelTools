using DispelTools.Common;
using DispelTools.DebugTools.MetricTools;
using DispelTools.ImageProcessing;
using System;
using System.IO;
using System.Linq;

namespace DispelTools.GameDataModels.Sprite
{
    public class SpriteLoader
    {

        private readonly BinaryReader reader;
        private readonly string filename;
        private readonly ColorManagement.ColorMode colorMode;

        public SpriteLoader(BinaryReader reader, string filename, ColorManagement.ColorMode colorMode = ColorManagement.ColorMode.RGB16_565)
        {
            this.reader = reader;
            this.filename = filename;
            this.colorMode = colorMode;
        }

        public void SkipSequence()
        {
            var info = GetSequenceInfo();
            reader.BaseStream.Seek(info.SequenceEndPosition, SeekOrigin.Begin);
        }

        public SpriteSequence LoadSequence()
        {
            var info = GetSequenceInfo();
            var sequence = new SpriteSequence(info.FrameInfos.Length);
            for (int i = 0; i < info.FrameInfos.Length; i++)
            {
                var frameInfo = info.FrameInfos[i];
                reader.BaseStream.Seek(frameInfo.ImageStartPosition, SeekOrigin.Begin);
                var image = LoadImageFromFile(frameInfo.Width, frameInfo.Height);
                var frame = new SpriteFrame(frameInfo.OriginX, frameInfo.OriginY, image);
                sequence.SetFrame(frame, i);
            }
            return sequence;
        }

        private DirectBitmap LoadImageFromFile(int width, int height)
        {
            var colorManager = ColorManagement.From(colorMode);

            var bitmap = new DirectBitmap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte[] colorBytes = reader.ReadBytes(colorManager.BytesConsumed);
                    var color = colorManager.ProduceColor(colorBytes);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        private SequenceInfo GetSequenceInfo()
        {
            var info = new SequenceInfo();

            int stamp = reader.ReadInt32();
            if (stamp == 8)
            {
                stamp = reader.ReadInt32();
            }
            if (stamp == 0)
            {
                int framesCount = reader.ReadInt32();
                int stamp0_2 = reader.ReadInt32();
                info.FrameInfos = new ImageInfo[framesCount];
            }

            if (info.FrameInfos.Length == 0)
            {
                Metrics.Count(MetricFile.SpriteFileMetric, filename, "zeroFrame");
                Metrics.Count(MetricFile.SpriteFileMetric, "zeroFrames");
            }
            info.SequenceStartPosition = reader.BaseStream.Position;
            for (int i = 0; i < info.FrameInfos.Length; i++)
            {
                try
                {
                    info.FrameInfos[i] = GetImageInfo();
                    reader.Skip(info.FrameInfos[i].SizeBytes);
                    Metrics.Gauge(MetricFile.SpriteOffsetMetric, $"file.{filename}", info.FrameInfos[i].ImageStartPosition);
                }
                catch (FrameInfoException)
                {
                    var oldFrames = info.FrameInfos;
                    info.FrameInfos = new ImageInfo[i];
                    for (int j = 0; j < info.FrameInfos.Length; j++)
                    {
                        info.FrameInfos[j] = oldFrames[j];
                    }
                }
            }
            info.SequenceEndPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek(info.SequenceStartPosition, SeekOrigin.Begin);


            Metrics.Count(MetricFile.SpriteFileMetric, filename, "spriteFrameCount", info.FrameInfos.Length);
            Metrics.Count(MetricFile.SpriteFileMetric, "allFrames", info.FrameInfos.Length);
            return info;
        }

        private ImageInfo GetImageInfo()
        {
            var info = new ImageInfo();
            reader.Skip(6 * 4);//some data
            info.OriginX = reader.ReadInt32();
            info.OriginY = reader.ReadInt32();
            info.Width = reader.ReadInt32();
            info.Height = reader.ReadInt32();
            info.SizeBytes = reader.ReadUInt32() * 2;
            info.ImageStartPosition = reader.BaseStream.Position;
            if (info.Width < 1 || info.Height < 1)
            {
                throw new FrameInfoException();//fix for soulnet.spr missing one frame
            }
            return info;
        }

        public class FrameInfoException : Exception
        {
            public FrameInfoException() : base("Loading some garbage instead of frame info")
            {
            }
        }

        private class SequenceInfo
        {
            public ImageInfo[] FrameInfos { get; set; }
            public long SequenceStartPosition { get; set; }
            public long SequenceEndPosition { get; set; }
        }

        private class ImageInfo
        {
            public int OriginX { get; set; }
            public int OriginY { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public long SizeBytes { get; set; }
            public long ImageStartPosition { get; set; }
        }
    }
}
