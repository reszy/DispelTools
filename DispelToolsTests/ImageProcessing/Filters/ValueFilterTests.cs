using Microsoft.VisualStudio.TestTools.UnitTesting;
using DispelTools.ImageProcessing.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.ImageProcessing.Filters.Tests
{
    [TestClass()]
    public class ValueFilterTests
    {

        [TestMethod()]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void ValueFilterTest(Color input, Color expected, ValueFilter filter)
        {
            var result = filter.Apply(input);
            Assert.AreEqual(expected.ToArgb(), result.ToArgb(), $"Expected:{expected}. Actual:{result}");
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