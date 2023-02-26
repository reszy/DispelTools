using DispelTools.DataEditor;
using DispelTools.DataEditor.Data;
using DispelTools.DataEditor.Export;
using DispelTools.DataEditor.Tests;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace DispelToolsTests.DataEditor.Export
{
    internal class ExportAsDataTests
    {
        private static readonly string ExpectedScheme =
            "[\r\n" +
            "  {\r\n" +
            "    \"name\": \"Item1 name\",\r\n" +
            "    \"i32\": 8,\r\n" +
            "    \"i16\": 16,\r\n" +
            "    \"byte\": 3,\r\n" +
            "    \"bytes\": \"0x010305\"\r\n" +
            "  },\r\n" +
            "  {\r\n" +
            "    \"name\": \"Item2 name\",\r\n" +
            "    \"i32\": 10,\r\n" +
            "    \"i16\": 20,\r\n" +
            "    \"byte\": 5,\r\n" +
            "    \"bytes\": \"0x010305\"\r\n" +
            "  }\r\n" +
            "]";

        [OneTimeSetUp] 
        public void OneTimeSetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        public void ShouldCreateCorrectScheme()
        {
            //given
            var mockFs = new MockFileSystem();
            var mapper = new TestMapper();
            var exporter = new ExportAsData
                (
                    mockFs,
                    new(new()),
                    new
                    (
                        mapper,
                        new()
                        {
                            CreateData(1, mapper),
                            CreateData(2, mapper)
                        },
                        "test.db",
                        "test.db"
                    )
                )
            {
                AllProperties = true
            };
            var filename = "Test.json";

            //when
            exporter.Export(filename);

            //then
            Assert.Multiple(() =>
            {
                Assert.That(mockFs.FileExists(filename), Is.True);
                Assert.That(mockFs.GetFile(filename).TextContents, Is.EqualTo(ExpectedScheme));
            });
        }

        private static DataItem CreateData(int number, MapperDefinition mapper)
        {
            var descriptors = mapper.CreateDescriptors();
            return new DataItem()
            {
                new TextField(descriptors[0].Name, $"Item{number} name", Field.SupportedEncoding.ASCII),
                new PrimitiveField<short>(descriptors[1].Name, (short)(6+number*2), Field.DisplayType.DEC),
                new PrimitiveField<int>(descriptors[2].Name, 12+number*4, Field.DisplayType.DEC),
                new PrimitiveField<byte>(descriptors[3].Name, (byte)(1+number*2), Field.DisplayType.DEC),
                new ByteArrayField(descriptors[4].Name, new byte[] { 1, 3, 5 }, Field.DisplayType.DEC),
            };
        }
    }
}
