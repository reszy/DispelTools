using NUnit.Framework;

namespace DispelTools.ImageAnalyzer.Tests
{
    public class DataPixelTests
    {


        [Test]
        public void DataPixel4ByteTest()
        {
            byte[] bytes = new byte[4] { 2, 120, 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.That(dataPixel.Byte, Is.EqualTo(2));
            Assert.That(dataPixel.Word, Is.EqualTo(30722));
            Assert.That(dataPixel.DWord, Is.EqualTo(3962882));
            Assert.That(dataPixel.QWord, Is.EqualTo(20740098));
        }

        [Test]
        public void DataPixel3ByteTest()
        {
            byte[] bytes = new byte[3] { 120, 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.That(dataPixel.Byte, Is.EqualTo(120));
            Assert.That(dataPixel.Word, Is.EqualTo(15480));
            Assert.That(dataPixel.DWord, Is.EqualTo(81016));
            Assert.That(dataPixel.QWord, Is.EqualTo(81016));
        }

        [Test]
        public void DataPixel2ByteTest()
        {
            byte[] bytes = new byte[2] { 60, 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.That(dataPixel.Byte, Is.EqualTo(60));
            Assert.That(dataPixel.Word, Is.EqualTo(316));
            Assert.That(dataPixel.DWord, Is.EqualTo(316));
            Assert.That(dataPixel.QWord, Is.EqualTo(316));
        }

        [Test]
        public void DataPixel1ByteTest()
        {
            byte[] bytes = new byte[1] { 1 };
            var dataPixel = new DataAnalyzedBitmap.DataPixel(bytes);
            Assert.That(dataPixel.Byte, Is.EqualTo(1));
            Assert.That(dataPixel.Word, Is.EqualTo(1));
            Assert.That(dataPixel.DWord, Is.EqualTo(1));
            Assert.That(dataPixel.QWord, Is.EqualTo(1));
        }
    }
}