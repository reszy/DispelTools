using System;
using System.Drawing;

namespace DispelTools.ImageProcessing.Filters
{
    public class ValueFilter : IPerPixelFilter
    {
        private readonly byte expectedColorValue;
        private readonly ColorChannel channel;
        public enum ColorChannel { Any, R, G, B, A, CH1 = R, CH2 = G, CH3 = B, CH4 = A };
        public ValueFilter(byte expectedColorValue, ColorChannel channel)
        {
            this.expectedColorValue = expectedColorValue;
            this.channel = channel;
        }

        private bool IsExpectedColor(Color givenColor)
        {
            switch (channel)
            {
                case ColorChannel.Any:
                    return givenColor.R == expectedColorValue
                        || givenColor.G == expectedColorValue
                        || givenColor.B == expectedColorValue
                        || givenColor.A == expectedColorValue;
                case ColorChannel.R:
                    return givenColor.R == expectedColorValue;
                case ColorChannel.G:
                    return givenColor.G == expectedColorValue;
                case ColorChannel.B:
                    return givenColor.B == expectedColorValue;
                case ColorChannel.A:
                    return givenColor.A == expectedColorValue;
                default:
                    throw new ArgumentException($"Unexpected channel value {channel}");
            }
        }

        public Color Apply(Color color) => IsExpectedColor(color) ? Color.White : Color.Black;
    }
}
