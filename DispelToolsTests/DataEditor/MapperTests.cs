using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using NUnit.Framework;
using System.ComponentModel;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace DispelTools.DataEditor.Tests
{
    public partial class MapperTests
    {
        public static readonly WorkReporter workReporter = new(new BackgroundWorker());

        [SetUp]
        public void SetUp()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        public void ReadValueTest()
        {
            var mockFS = new MockFileSystem();

            var mapperDefinition = new TestMapper();
            var mockFile = new MockFileData(mapperDefinition.TestData);
            mockFS.AddFile("testfile", mockFile);

            var loader = new SimpleDataLoader(mockFS, mapperDefinition);

            var result = loader.LoadData("testFile", workReporter);
            Assert.Multiple(() =>
            {
                Assert.That(result[0][0].GetEncodedText(), Is.EqualTo(TestMapper.STR_DATA));
                Assert.That(result[0][1].Value, Is.EqualTo(TestMapper.I32_DATA));
                Assert.That(result[0][2].Value, Is.EqualTo(TestMapper.I16_DATA));
                Assert.That(result[0][3].Value, Is.EqualTo(TestMapper.BYTE_DATA));
            });
            CollectionAssert.AreEqual(TestMapper.BYA_DATA, (byte[])result[0][4].Value);
            Assert.Multiple(() =>
            {
                Assert.That(result[1][0].GetEncodedText(), Is.EqualTo(TestMapper.STR_DATA2));
                Assert.That(result[1][1].Value, Is.EqualTo(TestMapper.I32_DATA2));
                Assert.That(result[1][2].Value, Is.EqualTo(TestMapper.I16_DATA2));
                Assert.That(result[1][3].Value, Is.EqualTo(TestMapper.BYTE_DATA2));
            });
            CollectionAssert.AreEqual(TestMapper.BYA_DATA2, (byte[])result[1][4].Value);
        }

        [Test]
        public void SaveTest()
        {
            var mockFS = new MockFileSystem();

            var mapperDefinition = new TestMapper();
            var mockFile = new MockFileData(mapperDefinition.TestData);
            var filename = "testfile";
            mockFS.AddFile(filename, mockFile);

            var loader = new SimpleDataLoader(mockFS, mapperDefinition);

            var container = loader.LoadData(filename, workReporter);

            var descriptors = mapperDefinition.CreateDescriptors();
            for (int i = 0; i < descriptors.Count; i++)
            {
                SwitchField(container[0][i], container[1][i]);
            }

            loader.SaveElement(container, container[0]);
            loader.SaveElement(container, container[1]);

            CollectionAssert.AreEqual(mapperDefinition.TestDataSwitched, mockFile.Contents);
        }

        private static void SwitchField(IField o1, IField o2)
        {
            (o2.Value, o1.Value) = (o1.Value, o2.Value);
        }
    }
}