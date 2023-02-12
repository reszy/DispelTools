using NUnit.Framework;
using DispelTools.DataEditor.Tests;
using System.IO.Abstractions.TestingHelpers;
using DispelTools.DataEditor.Export;

namespace DispelToolsTests.DataEditor.Export
{
    internal class ExportAsDocSchemeTests
    {
        private static readonly string ExpectedScheme =
            "struct Test\r\n" +
            "{\r\n" +
            "    INT32     entries_count\r\n" +
            "    //\r\n" +
            "    // First entry scheme\r\n" +
            "    //\r\n" +
            "    BYTE[30]  name      // 30 bytes    Test Description\r\n" +
            "    INT32     i32       // 4 bytes\r\n" +
            "    INT16     i16       // 2 bytes\r\n" +
            "    BYTE      byte      // 1 byte\r\n" +
            "    BYTE[13]  bytes     // 13 bytes\r\n" +
            "    //\r\n" +
            "    // ... Remaining entries\r\n" +
            "    //\r\n" +
            "}\r\n";


        [Test]
        public void ShouldCreateCorrectScheme()
        {
            //given
            var mockFs = new MockFileSystem();
            var mapper = new TestMapper();
            var exporter = new ExportAsDocScheme(mockFs);
            var filename = "testMapper.txt";

            exporter.Setup(new() { { filename, mapper } });

            //when
            exporter.Export();
             
            //then
            Assert.Multiple(() =>
            {
                Assert.That(mockFs.FileExists(filename), Is.True);
                Assert.That(mockFs.GetFile(filename).TextContents, Is.EqualTo(ExpectedScheme));
            });
        }
    }
}
