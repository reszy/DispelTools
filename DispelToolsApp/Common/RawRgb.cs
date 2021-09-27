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
    }
}
