using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DispelTools.ImageAnalyzer.Tests
{
    [TestClass()]
    public class DataPixelTests
    {


        [TestMethod()]
        public void DataPixel4ByteTest()
        {
            byte[] bytes = new byte[4] { 2, 120, 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.AreEqual(2, dataPixel.Byte);
            Assert.AreEqual(30722, dataPixel.Word);
            Assert.AreEqual(3962882, dataPixel.DWord);
            Assert.AreEqual(20740098, dataPixel.QWord);
        }

        [TestMethod()]
        public void DataPixel3ByteTest()
        {
            byte[] bytes = new byte[3] { 120, 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.AreEqual(120, dataPixel.Byte);
            Assert.AreEqual(15480, dataPixel.Word);
            Assert.AreEqual(81016, dataPixel.DWord);
            Assert.AreEqual(81016, dataPixel.QWord);
        }

        [TestMethod()]
        public void DataPixel2ByteTest()
        {
            byte[] bytes = new byte[2] { 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.AreEqual(60, dataPixel.Byte);
            Assert.AreEqual(316, dataPixel.Word);
            Assert.AreEqual(316, dataPixel.DWord);
            Assert.AreEqual(316, dataPixel.QWord);
        }

        [TestMethod()]
        public void DataPixel1ByteTest()
        {
            byte[] bytes = new byte[1] { 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.AreEqual(1, dataPixel.Byte);
            Assert.AreEqual(1, dataPixel.Word);
            Assert.AreEqual(1, dataPixel.DWord);
            Assert.AreEqual(1, dataPixel.QWord);
        }
    }
}