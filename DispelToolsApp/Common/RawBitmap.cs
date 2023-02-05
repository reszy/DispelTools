using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.Common
{
    public abstract class RawBitmap
    {
        public byte[] Bytes { get; }
        public int Height { get; }
        public int Width { get; }
        public int ByteDepth { get; }

        public RawBitmap(int width, int height, byte byteDepth)
        {
            ValidateBitDepth(byteDepth);
            Width = width;
            Height = height;
            ByteDepth = byteDepth;
            Bytes = new byte[width * height * byteDepth];
        }

        private static void ValidateBitDepth(byte byteDepth)
        {
            switch (byteDepth)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return;
                default: throw new NotSupportedException("Image with byte depth of {byteDepth} is not supported");
            }
        }

        public abstract System.Drawing.Color GetPixel(int x, int y);
        public abstract void SetPixel(int x, int y, System.Drawing.Color color);

        public int GetIndexAt(int x, int y)
        {
            return ((y * Width) + x) * ByteDepth;
        }
    }
}
