using System;
using System.Drawing;

namespace DispelTools.ImageProcessing
{
    public abstract class ColorManagement
    {
        public enum ColorMode { RGB24, BGR24, PALLETE, BW8, YB16, RGB16_565, RGB16_565_Skip2, RGB16_555, DATA32, DATA32_TO_RGBA };

        public static ColorManagement From(ColorMode mode)
        {
            switch (mode)
            {
                case ColorMode.RGB24:
                    return new RGB24();
                case ColorMode.BGR24:
                    return new BGR24();
                case ColorMode.PALLETE:
                    return new Palette();
                case ColorMode.BW8:
                    return new BW8();
                case ColorMode.YB16:
                    return new YB16();
                case ColorMode.RGB16_565:
                    return new RGB16_565();
                case ColorMode.RGB16_565_Skip2:
                    return new RGB16_565_Skip2();
                case ColorMode.RGB16_555:
                    return new RGB16_555();
                case ColorMode.DATA32:
                    return new DATA32();
                case ColorMode.DATA32_TO_RGBA:
                    return new DATA32_TO_RGBA();
                default:
                    return new RGB24();
            }
        }

        public byte BytesConsumed { get; private set; }
        public bool HasTransparency { get; private set; }
        public bool HasPalette { get; private set; }
        public abstract void SetPallete(Color[] bytes);
        public abstract Color ProduceColor(byte[] bytes);

        internal class RGB24 : ColorManagement
        {
            internal RGB24()
            {
                BytesConsumed = 3;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class BGR24 : ColorManagement
        {
            internal BGR24()
            {
                BytesConsumed = 3;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return Color.FromArgb(bytes[2], bytes[1], bytes[0]);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class BW8 : ColorManagement
        {
            internal BW8()
            {
                BytesConsumed = 1;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return Color.FromArgb(bytes[0], bytes[0], bytes[0]);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class YB16 : ColorManagement
        {
            internal YB16()
            {
                BytesConsumed = 2;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return Color.FromArgb(bytes[1], bytes[1], bytes[0]);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class RGB16_565 : ColorManagement
        {

            private static ushort red_mask = 0xF800;
            private static ushort green_mask = 0x7E0;
            private static ushort blue_mask = 0x1F;

            internal RGB16_565()
            {
                BytesConsumed = 2;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    ushort pixel = (ushort)(bytes[1] << 8 | bytes[0]);
                    byte red_value = (byte)((pixel & red_mask) >> 11);
                    byte green_value = (byte)((pixel & green_mask) >> 5);
                    byte blue_value = (byte)(pixel & blue_mask);


                    byte red = (byte)(red_value << 3);
                    byte green = (byte)(green_value << 2);
                    byte blue = (byte)(blue_value << 3);
                    return Color.FromArgb(red, green, blue);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class RGB16_565_Skip2 : ColorManagement
        {

            private static ushort red_mask = 0xF800;
            private static ushort green_mask = 0x7E0;
            private static ushort blue_mask = 0x1F;

            internal RGB16_565_Skip2()
            {
                BytesConsumed = 4;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    ushort pixel = (ushort)(bytes[1] << 8 | bytes[0]);
                    byte red_value = (byte)((pixel & red_mask) >> 11);
                    byte green_value = (byte)((pixel & green_mask) >> 5);
                    byte blue_value = (byte)(pixel & blue_mask);


                    byte red = (byte)(red_value << 3);
                    byte green = (byte)(green_value << 2);
                    byte blue = (byte)(blue_value << 3);
                    return Color.FromArgb(red, green, blue);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class RGB16_555 : ColorManagement
        {

            private static ushort red_mask = 0x7C00;
            private static ushort green_mask = 0x3E0;
            private static ushort blue_mask = 0x1F;

            internal RGB16_555()
            {
                BytesConsumed = 2;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    ushort pixel = (ushort)(bytes[1] << 8 | bytes[0]);
                    byte red_value = (byte)((pixel & red_mask) >> 10);
                    byte green_value = (byte)((pixel & green_mask) >> 5);
                    byte blue_value = (byte)(pixel & blue_mask);


                    byte red = (byte)(red_value << 3);
                    byte green = (byte)(green_value << 3);
                    byte blue = (byte)(blue_value << 3);
                    return Color.FromArgb(red, green, blue);
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }
        internal class Palette : ColorManagement
        {
            internal Palette()
            {
                BytesConsumed = 1;
                HasPalette = true;
                HasTransparency = false;
                palette = new Color[256];
                for (int i = 0; i < 256; i++)
                {
                    palette[i] = Color.FromArgb(i, i, i);
                }
            }

            private Color[] palette;

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return palette[bytes[0]];
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] colors)
            {
                if (colors.Length == 256)
                {
                    palette = colors;
                }
                else
                {
                    throw new ArgumentException($"Palette bytes number is incorrect. Got {colors.Length} expected 256");
                }
            }
        }
        internal class DATA32 : ColorManagement
        {
            internal DATA32()
            {
                BytesConsumed = 4;
                HasPalette = false;
                HasTransparency = false;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    if (bytes[3] > 0)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.FromArgb(bytes[2], bytes[1], bytes[0]);
                    }
                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }

        internal class DATA32_TO_RGBA : ColorManagement
        {
            internal DATA32_TO_RGBA()
            {
                BytesConsumed = 4;
                HasPalette = false;
                HasTransparency = true;
            }

            public override Color ProduceColor(byte[] bytes)
            {
                if (bytes.Length == BytesConsumed)
                {
                    return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);

                }
                else
                {
                    throw ColorBytesNumberException.Create(bytes.Length, BytesConsumed);
                }
            }
            public override void SetPallete(Color[] bytes) => throw new NotImplementedException();
        }

        internal class ColorBytesNumberException : ArgumentException
        {
            internal static ColorBytesNumberException Create(int bytesLength, int bytesConsumed) => new ColorBytesNumberException($"Color bytes number is incorrect. Got {bytesLength} expected {bytesConsumed}");

            private ColorBytesNumberException(string message) : base(message) { }
        }
    }
}
