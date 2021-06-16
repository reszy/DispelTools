using System;
using System.Linq;
using System.IO;
using DispelTools.Common;

namespace DispelTools.ImageProcessing.Sprite
{
    public class SpriteLoader
    {

        private BinaryReader reader;
        public SpriteLoader(BinaryReader reader)
        {
            this.reader = reader;
        }

        public void SkipSequence()
        {
            var info = SequenceInfo.CreateAndReadInfo(reader);
            reader.BaseStream.Seek(info.SequenceEndPosition, SeekOrigin.Begin);
        }

        public SpriteSequence LoadSequence()
        {
            var info = SequenceInfo.CreateAndReadInfo(reader);
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

        private class SequenceInfo
        {
            public ImageInfo[] FrameInfos { get; private set; }
            public long SequenceStartPosition { get; private set; }
            public long SequenceEndPosition { get; private set; }
            public static SequenceInfo CreateAndReadInfo(BinaryReader reader)
            {
                var info = new SequenceInfo();
                info.ReadInfo(reader);
                return info;
            }
            private void ReadInfo(BinaryReader reader)
            {
                int stamp = reader.ReadInt32();
                if(stamp == 8)
                {
                    stamp = reader.ReadInt32();
                }
                if(stamp == 0)
                {
                    int framesCount = reader.ReadInt32();
                    int stamp0_2 = reader.ReadInt32();
                    FrameInfos = new ImageInfo[framesCount];
                }
                SequenceStartPosition = reader.BaseStream.Position;
                for (int i = 0; i < FrameInfos.Length; i++)
                {
                    FrameInfos[i] = ImageInfo.CreateAndReadInfo(reader);
                    reader.Skip(FrameInfos[i].SizeBytes);
                }
                SequenceEndPosition = reader.BaseStream.Position;
                reader.BaseStream.Seek(SequenceStartPosition, SeekOrigin.Begin);
            }
        }

        private class ImageInfo
        {
            public int OriginX { get; private set; }
            public int OriginY { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public long SizeBytes { get; private set; }
            public long ImageStartPosition { get; private set; }

            public static ImageInfo CreateAndReadInfo(BinaryReader reader)
            {
                var info = new ImageInfo();
                info.ReadInfo(reader);
                return info;
            }
            private void ReadInfo(BinaryReader reader)
            {
                reader.Skip(6 * 4);//some data
                OriginX = reader.ReadInt32();
                OriginY = reader.ReadInt32();
                Width = reader.ReadInt32();
                Height = reader.ReadInt32();
                SizeBytes = reader.ReadUInt32() * 2;
                ImageStartPosition = reader.BaseStream.Position;
            }
        }
    }
}
