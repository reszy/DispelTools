using DispelTools.DataEditor;
using DispelTools.GameDataModels.Map.External;
using DispelTools.GameDataModels.Map.External.Extra;
using DispelTools.GameDataModels.Map.External.Monster;
using DispelTools.GameDataModels.Map.External.Npc;
using NUnit.Framework;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace DispelToolsTests.GameDataModels.Map.External
{
    public class ExternalObjectMappingTests
    {
        [Test]
        public void TestExtraMapping()
        {
            //given
            var reader = new MapExtraReader();
            var mockedFile = new MockFileData(CreateFileContents(0xCD, reader.MapperDefinition.PropertyItemSize, 1));

            //then
            TestReader(reader, mockedFile);
        }

        [Test]
        public void TestMonsterMapping()
        {
            //given
            var reader = new MapMonsterReader();
            var mockedFile = new MockFileData(CreateFileContents(0xCD, reader.MapperDefinition.PropertyItemSize, 1));

            //then
            TestReader(reader, mockedFile);
        }

        [Test]
        public void TestNpcMapping()
        {
            //given
            var reader = new MapNpcReader();
            var mockedFile = new MockFileData(CreateFileContents(0xCD, reader.MapperDefinition.PropertyItemSize, 1));

            //then
            TestReader(reader, mockedFile);
        }

        private static void TestReader(ReferenceFileReader reader, MockFileData testFile)
        {
            //given
            var mockFS = new MockFileSystem();
            var mapper = new Mapper(mockFS, reader.MapperDefinition);
            var mappedValues = mapper.CreateMapping(reader.ValuesMapping);

            //when
            mockFS.AddFile("testFile", testFile);
            var items = mapper.ReadFile("testFile", new DispelTools.Common.DataProcessing.WorkReporter(new System.ComponentModel.BackgroundWorker()));

            //given
            Assert.That(items, Is.Not.Null);
            Assert.That(items, Is.Not.Empty);
            foreach (var item in items)
            {
                var results = mappedValues.Convert(item);
                Assert.That(results, Has.Length.EqualTo(reader.ValuesMapping.Length));
            }
        }

        private static byte[] CreateFileContents(byte filler, int itemSize, int itemCount)
        {
            var contents = new byte[itemCount * itemSize + 4];
            for (int i = 0; i < contents.Length; i++)
            {
                contents[i] = filler;
            }
            var itemSizeBytes = BitConverter.GetBytes(itemCount);
            for (int i = 0; i < itemSizeBytes.Length; i++)
            {
                contents[i] = itemSizeBytes[i];
            }
            return contents;
        }
    }
}
