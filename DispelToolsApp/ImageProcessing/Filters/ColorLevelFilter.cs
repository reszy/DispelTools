using System;
using System.Drawing;

namespace DispelTools.ImageProcessing.Filters
{
    public class ColorLevelFilter : IPerPixelFilter
    {
        private readonly byte low;
        private readonly byte high;
        public ColorLevelFilter(byte low, byte high)
        {
            this.low = low;
            this.high = high;
        }

        public Color Apply(Color color) => Color.FromArgb(color.A, ApplyFormula(color.R), ApplyFormula(color.G), ApplyFormula(color.B));

        private byte ApplyFormula(byte value) => (byte)Math.Min(255, Math.Max(0, (value - low) * 255 / (high - low)));
    }
}
