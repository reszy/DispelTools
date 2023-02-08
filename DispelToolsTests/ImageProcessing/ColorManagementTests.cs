using DispelTools.ImageProcessing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static DispelTools.ImageProcessing.ColorManagement.ColorMode;

namespace DispelToolsTests.ImageProcessing
{
    public class ColorManagementTests
    {

        [Test]
        [TestCaseSource(nameof(GetModes))]
        public void ShouldThrowExceptionWhenExceededByteNumber(ColorManagement.ColorMode mode)
        {
            var colorManagement = ColorManagement.From(mode);

            byte[] tooBig = new byte[colorManagement.BytesConsumed + 1];
            Assert.Throws<ColorManagement.ColorBytesNumberException>(() => colorManagement.ProduceColor(tooBig));
        }

        [Test]
        [TestCaseSource(nameof(GetModes))]
        public void ShouldReturnCorrectByteNumber(ColorManagement.ColorMode mode)
        {
            var colorManagement = ColorManagement.From(mode);

            byte[] bytes = colorManagement.ProduceBytes(Color.Black);
            Assert.That(bytes, Has.Length.EqualTo(colorManagement.BytesConsumed));
        }

        [Test]
        [TestCaseSource(nameof(GetModesWithParams))]
        public void ShouldConvertBytesForthAndBack(ColorManagement.ColorMode mode, byte[] expected, Color expectedColor, byte[] actual)
        {
            var colorManagement = ColorManagement.From(mode);

            var colorResult = colorManagement.ProduceColor(actual);
            Assert.That(colorResult, Is.EqualTo(expectedColor));
            byte[] bytesResult = colorManagement.ProduceBytes(colorResult);
            CollectionAssert.AreEqual(expected, bytesResult, $"Expected {WriteBytes(expected)} but was {WriteBytes(bytesResult)}");
        }

        private static string WriteBytes(byte[] bytes) => "{" + string.Join(", ", bytes) + "}";

        public static IEnumerable<object[]> GetModes() => ((IEnumerable<ColorManagement.ColorMode>)Enum.GetValues(typeof(ColorManagement.ColorMode))).Select((mode) => (new object[] { mode }));
        public static IEnumerable<object[]> GetModesWithParams()
        {
            var modesWithParams = GetModes().ToList();
            SetModeParams(modesWithParams, RGB24,
                new byte[] { 128, 255, 0 }, Color.FromArgb(128, 255, 0), new byte[] { 128, 255, 0 });
            SetModeParams(modesWithParams, BGR24,
                new byte[] { 128, 255, 0 }, Color.FromArgb(0, 255, 128), new byte[] { 128, 255, 0 });
            SetModeParams(modesWithParams, PALLETE,
                new byte[] { 128 }, Color.FromArgb(128, 128, 128), new byte[] { 128 });
            SetModeParams(modesWithParams, BW8,
                new byte[] { 128 }, Color.FromArgb(128, 128, 128), new byte[] { 128 });
            SetModeParams(modesWithParams, YB16,
                new byte[] { 128, 255 }, Color.FromArgb(255, 255, 128), new byte[] { 128, 255 });
            SetModeParams(modesWithParams, RGB16_565,
                new byte[] { 128, 255 }, Color.FromArgb(248, 240, 0), new byte[] { 128, 255 });
            SetModeParams(modesWithParams, RGB16_565_Skip2,
                new byte[] { 128, 255, 0, 0 }, Color.FromArgb(248, 240, 0), new byte[] { 128, 255, 0, 0 });
            SetModeParams(modesWithParams, RGB16_555,
                new byte[] { 128, 127 }, Color.FromArgb(248, 224, 0), new byte[] { 128, 127 });
            SetModeParams(modesWithParams, DATA32,
                new byte[] { 255, 255, 255, 255 }, Color.White, new byte[] { 128, 255, 0, 255 });
            SetModeParams(modesWithParams, DATA32_TO_RGBA,
                new byte[] { 128, 255, 0, 10 }, Color.FromArgb(10, 0, 255, 128), new byte[] { 128, 255, 0, 10 });
            return modesWithParams;
        }

        private static void SetModeParams(List<object[]> modes, ColorManagement.ColorMode mode, byte[] expected, Color expectedColor, byte[] actual)
        {
            int i = GetModeIndex(modes, mode);
            modes[i] = new object[] { modes[i][0], expected, expectedColor, actual };
        }
        private static int GetModeIndex(List<object[]> modes, ColorManagement.ColorMode mode)
        {
            return modes.Select((value, index) => new { value, index })
                        .Where(pair => (ColorManagement.ColorMode)pair.value[0] == mode)
                        .Select(pair => pair.index + 1)
                        .FirstOrDefault() - 1;
        }
    }
}
