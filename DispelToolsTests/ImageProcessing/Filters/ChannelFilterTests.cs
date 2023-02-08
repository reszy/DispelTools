using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.ImageProcessing.Filters.Tests
{
    public class ChannelFilterTests
    {
        [Test]
        [TestCaseSource(nameof(GetTestData))]
        public void ChannelFilterTest(Color input, Color expected, ChannelFilter filter)
        {
            var result = filter.Apply(input);
            Assert.That(result.ToArgb(), Is.EqualTo(expected.ToArgb()));
        }

        public static IEnumerable<object[]> GetTestData()
        {
            var list = new List<object[]>
            {
                new object[] { Color.Yellow, Color.Red, new ChannelFilter(true, false, false, true) },
                new object[] { Color.Yellow, Color.Black, new ChannelFilter(false, false, true, true) },
                new object[] { Color.FromArgb(130, 150, 100), Color.FromArgb(150, 150, 150), new ChannelFilter(false, true, false, false) },
                new object[] { Color.FromArgb(130, 150, 100), Color.FromArgb(255, 255, 255), new ChannelFilter(false, false, false, true) }
            };
            return list;
        }
    }
}