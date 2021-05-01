using System.Drawing;

namespace DispelTools.ImageProcessing.Filters
{
    public class ChannelFilter : IPerPixelFilter
    {

        private readonly int mask;
        private readonly bool grey;
        private readonly int shift;
        public ChannelFilter(bool red, bool green, bool blue, bool alpha)
        {
            int ibRed = ToInt(red);
            int ibGreen = ToInt(green);
            int ibBlue = ToInt(blue);
            int ibAlpha = ToInt(alpha);
            grey = ToInt(red) + ToInt(green) + ToInt(blue) + ToInt(alpha) == 1;
            int ibGrey = ToInt(grey);

            int rMask = 0x00ff0000 * ibRed;
            int gMask = 0x0000ff00 * ibGreen;
            int bMask = 0x000000ff * ibBlue;
            int aMask = (int)(0xff000000 * ibAlpha);

            mask = rMask + gMask + bMask + aMask;
            shift = ibGrey * (ibRed * 2 + ibGreen + ibAlpha * 3) * 8;
        }

        private static int ToInt(bool b) => b ? 1 : 0;
        public Color Apply(Color color)
        {
            if (grey)
            {
                int filteredColorValue = (color.ToArgb() >> shift) & 0xFF;
                return Color.FromArgb(filteredColorValue, filteredColorValue, filteredColorValue);
            }
            else
            {
                return Color.FromArgb(color.ToArgb() & mask);
            }
        }

        public override string ToString() => GetType().Name + "#" + mask.ToString("X");
    }
}
