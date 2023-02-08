using NUnit.Framework;
using System.Drawing;

namespace DispelTools.ImageProcessing.Filters.Tests
{
    public class ValueFilterTests
    {
        [Test]
        [TestCaseSource(nameof(GetTestData))]
        public void ValueFilterTest(Color input, Color expected, ValueFilter filter)
        {
            var result = filter.Apply(input);
            Assert.That(result.ToArgb(), Is.EqualTo(expected.ToArgb()), $"Expected:{expected}. Actual:{result}");
        }

        public static IEnumerable<object[]> GetTestData()
        {
            var list = new List<object[]>
            {
                new object[] { Color.Red, Color.White, new ValueFilter(255, ValueFilter.ColorChannel.R) },
                new object[] { Color.Red, Color.Black, new ValueFilter(255, ValueFilter.ColorChannel.B) },
                new object[] { Color.Red, Color.White, new ValueFilter(255, ValueFilter.ColorChannel.A) },
                new object[] { Color.FromArgb(0,255,0,0), Color.Black, new ValueFilter(255, ValueFilter.ColorChannel.A) },
                new object[] { Color.Red, Color.White, new ValueFilter(255, ValueFilter.ColorChannel.Any) },
                new object[] { Color.FromArgb(130, 150, 100), Color.White, new ValueFilter(150, ValueFilter.ColorChannel.Any) },
                new object[] { Color.FromArgb(130, 150, 100), Color.Black, new ValueFilter(250, ValueFilter.ColorChannel.Any) }
            };
            return list;
        }
    }
}