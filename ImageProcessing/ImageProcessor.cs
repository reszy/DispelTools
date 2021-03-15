using System.Drawing;

namespace DispelTools.ImageProcessing
{
    internal class ImageProcessor
    {
        private uint currentPosition = 0;

        public static Bitmap Process(uint width, uint height, byte[] data) => new ImageProcessor().InternalProcess(width, height, data);

        private Bitmap InternalProcess(uint width, uint height, byte[] data)
        {
            var colorManager = ColorManagement.From(ColorManagement.ColorMode.RGB16_565);

            var bitmap = new Bitmap((int)width, (int)height);
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
