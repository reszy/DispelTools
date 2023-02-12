using DispelTools.Common.DataProcessing;
using NUnit.Framework;
using System.ComponentModel;
using System.IO.Abstractions.TestingHelpers;
using System.Text;

namespace DispelTools.DataEditor.Tests
{
    public partial class SimpleEditorTests
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

            var mapper = new TestMapper();
            var mockFile = new MockFileData(mapper.TestData);
            mockFS.AddFile("testfile", mockFile);

            var editor = new SimpleEditor("testfile", mockFS);
            editor.SetMapper(new Mapper(mockFS, mapper));

            var result1 = editor.ReadValue(0, workReporter);
            Assert.Multiple(() =>
            {
                Assert.That(result1[0].EncodedText, Is.EqualTo(TestMapper.STR_DATA));
                Assert.That(result1[1].Value, Is.EqualTo(TestMapper.I32_DATA));
                Assert.That(result1[2].Value, Is.EqualTo(TestMapper.I16_DATA));
                Assert.That(result1[3].Value, Is.EqualTo(TestMapper.BYTE_DATA));
            });
            CollectionAssert.AreEqual(TestMapper.BYA_DATA, result1[4].ByteArrayValue);
            var result2 = editor.ReadValue(1, workReporter);
            Assert.Multiple(() =>
            {
                Assert.That(result2[0].EncodedText, Is.EqualTo(TestMapper.STR_DATA2));
                Assert.That(result2[1].Value, Is.EqualTo(TestMapper.I32_DATA2));
                Assert.That(result2[2].Value, Is.EqualTo(TestMapper.I16_DATA2));
                Assert.That(result2[3].Value, Is.EqualTo(TestMapper.BYTE_DATA2));
            });
            CollectionAssert.AreEqual(TestMapper.BYA_DATA2, result2[4].ByteArrayValue);
        }

        [Test]
        public void SaveTest()
        {
            var mockFS = new MockFileSystem();

            var mapper = new TestMapper();
            var mockFile = new MockFileData(mapper.TestData);
            mockFS.AddFile("testfile", mockFile);

            var editor = new SimpleEditor("testfile", mockFS);
            editor.SetMapper(new Mapper(mockFS, mapper));

            //var result1 = editor.ReadValue(0);
            //result1[0].Value = TestMapper.STR_DATA2;
            //result1[1].Value = TestMapper.I32_DATA2;
            //result1[2].Value = TestMapper.I16_DATA2;
            //result1[3].Value = TestMapper.BYTE_DATA2;
            //var result2 = editor.ReadValue(1);
            //result2[0].Value = TestMapper.STR_DATA;
            //result2[1].Value = TestMapper.I32_DATA;
            //result2[2].Value = TestMapper.I16_DATA;
            //result2[3].Value = TestMapper.BYTE_DATA;

            editor.Save(editor.ReadValue(1, workReporter), 0);
            editor.Save(editor.ReadValue(0, workReporter), 1);

            CollectionAssert.AreEqual(mapper.TestDataSwitched, mockFile.Contents);
        }
    }
}