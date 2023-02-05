using System.Drawing;

namespace DispelTools.Common
{
    public class RawRgba : RawBitmap
    {
        public RawRgba(int width, int height) : base(width, height, 4) { }

        public override Color GetPixel(int x, int y)
        {
            var i = GetIndexAt(x, y);
            return Color.FromArgb(Bytes[i + 3], Bytes[i], Bytes[i + 1], Bytes[i + 1]);
        }

        public override void SetPixel(int x, int y, Color color)
        {
            int i = GetIndexAt(x, y);
            Bytes[i] = color.R;
            Bytes[i + 1] = color.G;
            Bytes[i + 2] = color.B;
            Bytes[i + 3] = color.A;
        }
    }
}
