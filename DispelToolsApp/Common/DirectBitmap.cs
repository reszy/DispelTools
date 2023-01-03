using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
//
// DirectBitmap from https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
//
namespace DispelTools.Common
{
    public partial class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public int[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public static DirectBitmap From(DirectBitmap bitmap)
        {
            var newBitmap = new DirectBitmap(bitmap.Width, bitmap.Height);
            Array.Copy(bitmap.Bits, newBitmap.Bits, bitmap.Bits.Length);
            return newBitmap;
        }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new int[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            var result = Color.FromArgb(col);

            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
            Bits = null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Stream Stream()
        {
            return new DirectBitmapStream(this);
        }

        private class DirectBitmapStream : Stream
        {
            private DirectBitmap bitmap;
            private readonly long length;
            private long position;
            private int bitmapPosition;
            internal DirectBitmapStream(DirectBitmap bitmap)
            {
                this.bitmap = bitmap;
                position = 0;
                bitmapPosition = 0;
                length = bitmap.Bits.Length * 4;
            }
            public override bool CanRead => true;

            public override bool CanSeek => true;

            public override bool CanWrite => false;

            public override long Length => length;

            public override long Position { get => position; set { bitmapPosition = (int)Math.Floor(value / 4.0f); position = value; } }

            public override void Flush() { }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (position >= length) return 0;
                var readCount = 0;
                var bufferIndex = offset;
                byte[] currentPixel = ReadPixel(bitmapPosition);
                var pixelOffset = (int)(position - (bitmapPosition * 4));
                while (bufferIndex < buffer.Length && position < length && readCount < count)
                {
                    buffer[bufferIndex] = currentPixel[pixelOffset];
                    readCount++;
                    bufferIndex++;
                    pixelOffset++;
                    position++;
                    if (pixelOffset > 3)
                    {
                        pixelOffset = 0;
                        bitmapPosition++;
                        if (bitmapPosition < bitmap.Bits.Length) currentPixel = ReadPixel(bitmapPosition);
                    }
                }
                return bufferIndex - offset;
            }

            private byte[] ReadPixel(int offset) => BitConverter.GetBytes((uint)bitmap.Bits[offset]);

            public override long Seek(long offset, SeekOrigin origin)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        Position = offset;
                        break;
                    case SeekOrigin.Current:
                        Position += offset;
                        break;
                    case SeekOrigin.End:
                        Position = length + offset;
                        break;
                    default:
                        break;
                }
                return Position;
            }

            public override void SetLength(long value) { }

            public override void Write(byte[] buffer, int offset, int count) { }

            protected override void Dispose(bool disposing)
            {
                bitmap = null;
                base.Dispose(disposing);
            }
        }
    }
}
