using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
//
// DirectBitmap from https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
//
namespace DispelTools.Common
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public byte[] Data { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public static DirectBitmap From(DirectBitmap bitmap)
        {
            var newBitmap = new DirectBitmap(bitmap.Width, bitmap.Height);
            Array.Copy(bitmap.Data, newBitmap.Data, bitmap.Data.Length);
            return newBitmap;
        }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new byte[width * height * 4];
            BitsHandle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = x + (y * Width);
            var i = index * 4;
            Data[i+0] = color.A;
            Data[i+1] = color.R;
            Data[i+2] = color.G;
            Data[i+3] = color.B;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            var i = index * 4;
            return Color.FromArgb(Data[i + 0], Data[i + 1], Data[i + 2], Data[i + 3]);
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
            Data = null;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
