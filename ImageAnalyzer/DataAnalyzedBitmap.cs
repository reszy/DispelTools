namespace DispelTools.ImageAnalyzer
{
    public class DataAnalyzedBitmap
    {
        private readonly DataPixel[,] data;

        public DataAnalyzedBitmap(int width, int height)
        {
            data = new DataPixel[width, height];
        }

        public class DataPixel
        {
            private readonly byte b1;
            private readonly byte b2;
            private readonly byte b3;
            private readonly byte b4;

            public DataPixel(byte[] bytes)
            {
                if (bytes.Length > 3)
                {
                    b1 = bytes[0];
                    b2 = bytes[1];
                    b3 = bytes[2];
                    b4 = bytes[3];
                }
                else if (bytes.Length > 2)
                {
                    b1 = bytes[0];
                    b2 = bytes[1];
                    b3 = bytes[2];
                    b4 = 0;
                }
                else if (bytes.Length > 1)
                {
                    b1 = bytes[0];
                    b2 = bytes[1];
                    b3 = 0;
                    b4 = 0;
                }
                else if(bytes.Length == 1)
                {
                    b1 = bytes[0];
                    b2 = 0;
                    b3 = 0;
                    b4 = 0;
                }
                else
                {
                    b1 = 0;
                    b2 = 0;
                    b3 = 0;
                    b4 = 0;
                }
            }

            public byte Byte => b1;
            public int Word => b1 + (b2 << 8);
            public int DWord => b1 + (b2 << 8) + (b3 << 16);
            public int QWord => b1 + (b2 << 8) + (b3 << 16) + (b4 << 24);
        }

        public void SetPixel(int x, int y, byte[] bytes) => data[x, y] = new DataPixel(bytes);

        public DataPixel GetPixel(int x, int y) => data[x, y];
    }
}
