using DispelTools.Common;
using DispelTools.DebugTools.Metrics;
using DispelTools.DebugTools.Metrics.Dto;
using System.IO;
using System.Linq;

namespace DispelTools.ImageProcessing.Sprite
{
    public class SpriteLoader
    {

        private readonly BinaryReader reader;
        private readonly string filename;

        public SpriteLoader(BinaryReader reader, string filename)
        {
            this.reader = reader;
            this.filename = filename;
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
            var colorManager = ColorManagement.From(ColorManagement.ColorMode.RGB16_565);

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
                FileMetrics.AddMetric(new GeneralFileCounterDto("spriteFirstStamp8", filename));
                stamp = reader.ReadInt32();
            }
            if (stamp == 0)
            {
                int framesCount = reader.ReadInt32();
                int stamp0_2 = reader.ReadInt32();
                info.FrameInfos = new ImageInfo[framesCount];

                FileMetrics.AddMetric(new GeneralFileCounterDto("spriteFrameCount", filename));
            }
            info.SequenceStartPosition = reader.BaseStream.Position;
            for (int i = 0; i < info.FrameInfos.Length; i++)
            {
                info.FrameInfos[i] = GetImageInfo();
                reader.Skip(info.FrameInfos[i].SizeBytes);
            }
            info.SequenceEndPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek(info.SequenceStartPosition, SeekOrigin.Begin);

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
            return info;
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
