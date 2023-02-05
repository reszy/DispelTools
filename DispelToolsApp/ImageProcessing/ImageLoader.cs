using DispelTools.Common;

namespace DispelTools.ImageProcessing
{
    internal class ImageLoader
    {
        private uint currentPosition = 0;

        public static RawRgba Load(uint width, uint height, byte[] data) => new ImageLoader().InternalProcess(width, height, data);

        private RawRgba InternalProcess(uint width, uint height, byte[] data)
        {
            var colorManager = ColorManagement.From(ColorManagement.ColorMode.RGB16_565);

            var bitmap = new RawRgba((int)width, (int)height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte[] colorBytes = ReadBytes(data, colorManager.BytesConsumed);
                    var color = colorManager.ProduceColor(colorBytes);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }

        private byte[] ReadBytes(byte[] data, int n)
        {
            byte[] array = new byte[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = data[currentPosition++];
            }
            return array;
        }
    }
}
