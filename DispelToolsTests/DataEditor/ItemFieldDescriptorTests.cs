using DispelTools.Common;
using static DispelToolsTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace DispelTools.DataEditor.Tests
{
    [TestClass()]
    public class ItemFieldDescriptorTests
    {

        [TestMethod()]
        public void ShouldWriteStringWithZeroWhenStringIsEqualToMax()
        {
            //given
            int max = 12;
            byte filler = 0xcd;

            string str = "Name of item";

            byte[] byteStr = Encoding.ASCII.GetBytes(str);

            byte[] expected = Encoding.ASCII.GetBytes(str.Substring(0, max));
            expected[str.Length - 1] = 0;

            var ifd = ItemFieldDescriptor.AsFixedString(max, filler);

            var memory = new MemoryStream();
            var writer = new BinaryWriter(memory);
            var reader = new BinaryReader(memory);

            //when
            ifd.Write(writer, byteStr);
            reader.SetPosition(0);
            byte[] result = reader.ReadBytes(expected.Length);

            //then
            Assert.AreEqual(expected.Length, memory.Length);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void ShouldWriteStringWithZeroAndFillerWhenStringIsShorterThanMax()
        {
            //given
            int max = 20;
            byte filler = 0xcd;

            string str = "Name of item";

            byte[] byteStr = Encoding.ASCII.GetBytes(str);
            byte[] byteZero = new byte[] {0};

            byte[] expected = CombineByteArrays(new byte[][]{
                byteStr,
                byteZero,
                CreateByteArray(filler, 7)
            });

            var ifd = ItemFieldDescriptor.AsFixedString(max, filler);

            var memory = new MemoryStream();
            var writer = new BinaryWriter(memory);
            var reader = new BinaryReader(memory);

            //when
            ifd.Write(writer, byteStr);
            reader.SetPosition(0);
            byte[] result = reader.ReadBytes(expected.Length);

            //then
            Assert.AreEqual(expected.Length, memory.Length);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void ShouldWriteStringWithZeroWhenStringIsLongerThanMax()
        {
            //given
            int max = 15;
            byte filler = 0xcd;

            string str = "Name of item longer than max";

            byte[] byteStr = Encoding.ASCII.GetBytes(str);

            byte[] expected = Encoding.ASCII.GetBytes(str.Substring(0, max));
            expected[max - 1] = 0;

            var ifd = ItemFieldDescriptor.AsFixedString(max, filler);

            var memory = new MemoryStream();
            var writer = new BinaryWriter(memory);
            var reader = new BinaryReader(memory);

            //when
            ifd.Write(writer, byteStr);
            reader.SetPosition(0);
            byte[] result = reader.ReadBytes(expected.Length);

            //then
            Assert.AreEqual(expected.Length, memory.Length);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}