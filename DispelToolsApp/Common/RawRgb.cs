namespace DispelTools.Common
{
    public class RawRgb
    {
        public byte[] Bytes { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public RawRgb(int width, int height)
        {
            Width = width;
            Height = height;
            Bytes = new byte[width * height * 3];
        }

        internal System.Drawing.Color GetPixel(int x, int y)
        {
            int i = ((y * Width) + x) * 3;
            byte alpha = ((Bytes[i] + Bytes[i + 1] + Bytes[i + 2]) > 0) ? (byte)255 : (byte)0;
            return System.Drawing.Color.FromArgb(alpha, Bytes[i], Bytes[i + 1], Bytes[i + 2]);
        }
    }
}
