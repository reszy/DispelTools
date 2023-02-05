namespace DispelTools.Common
{
    public class RawRgb : RawBitmap
    {
        public RawRgb(int width, int height) : base(width, height, 3) { }

        public override System.Drawing.Color GetPixel(int x, int y)
        {
            int i = GetIndexAt(x, y);
            byte alpha = ((Bytes[i] + Bytes[i + 1] + Bytes[i + 2]) > 0) ? (byte)255 : (byte)0;
            return System.Drawing.Color.FromArgb(alpha, Bytes[i], Bytes[i + 1], Bytes[i + 2]);
        }
        public override void SetPixel(int x, int y, System.Drawing.Color color)
        {
            int i = GetIndexAt(x,y);
            Bytes[i] = color.R;
            Bytes[i + 1] = color.G;
            Bytes[i + 2] = color.B;
        }
    }
}
