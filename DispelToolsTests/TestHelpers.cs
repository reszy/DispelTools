namespace DispelToolsTests
{
    public static class TestHelpers
    {
        public static byte[] CombineByteArrays(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] CreateByteArray(byte filler, int size)
        {
            byte[] array = new byte[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = filler;
            }
            return array;
        }
    }
}
