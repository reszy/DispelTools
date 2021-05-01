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
            private readonly byte[] bytes = new byte[4] {0,0,0,0};
            private byte intPart1 => bytes[0];
            private int intPart2 => (bytes[1] << 8);
            private int intPart3 => (bytes[2] << 16);
            private int intPart4 => (bytes[3] << 24);

            public DataPixel(byte[] bytes)
            {
                bytes.CopyTo(this.bytes, 0);
            }

            public byte Byte => intPart1;
            public int Word => intPart1 + intPart2;
            public int DWord => intPart1 + intPart2 + intPart3;
            public int QWord => intPart1 + intPart2 + intPart3 + intPart4;
        }

        public void SetPixel(int x, int y, byte[] bytes) => data[x, y] = new DataPixel(bytes);

        public DataPixel GetPixel(int x, int y) => data[x, y];
    }
}
