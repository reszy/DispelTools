using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections;
using DispelTools.Common;

namespace DispelTools.Common.Tests
{
    [TestClass()]
    public class FileReaderWriterExtensionTests
    {
        [TestMethod()]
        public void ReadIntsTest()
        {
            byte[] bytes = new byte[12] {
                8, 0, 0, 0,
                0, 0, 0, 0,
                7, 0, 0, 0
            };
            int[] expectedInts = new int[3] {
                8,
                0,
                7
            };
            using (var reader = new BinaryReader(new MemoryStream(bytes)))
            {
                var result = reader.ReadInts(3);
                CollectionAssert.AreEqual(expectedInts, result, $"\nE:{string.Join(", ", expectedInts)}\nR:{string.Join(", ", result)}\n");
            }
        }
    }
}