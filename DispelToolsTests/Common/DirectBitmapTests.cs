using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace DispelTools.Common.Tests
{
    [TestClass()]
    public class DirectBitmapTests
    {
        private readonly Color CheckedColor = Color.FromArgb(255, 190, 120, 70);

        [TestMethod()]
        public void SetPixelGetPixelSmallTest()
        {
            var bitmap = new DirectBitmap(2, 2);
            bitmap.SetPixel(1, 1, CheckedColor);

            var result = bitmap.GetPixel(1, 1).ToArgb();

            Assert.AreEqual(CheckedColor.ToArgb(), result, $"color was #${result:X}");
        }

        [TestMethod()]
        public void SetPixelGetPixelMediumUnevenTest()
        {
            var bitmap = new DirectBitmap(20, 28);
            bitmap.SetPixel(15, 19, CheckedColor);

            var result = bitmap.GetPixel(15, 19).ToArgb();

            Assert.AreEqual(CheckedColor.ToArgb(), result, $"color was #${result:X}");
        }


        [TestMethod()]
        public void SetPixelDataCorrectOrderTest()
        {
            var bitmap = new DirectBitmap(1, 1);
            bitmap.SetPixel(0, 0, CheckedColor);

            Assert.AreEqual(CheckedColor.A, bitmap.Data[0]);
            Assert.AreEqual(CheckedColor.R, bitmap.Data[1]);
            Assert.AreEqual(CheckedColor.G, bitmap.Data[2]);
            Assert.AreEqual(CheckedColor.B, bitmap.Data[3]);
        }
    }
}