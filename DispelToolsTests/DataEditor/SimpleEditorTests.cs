using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using static DispelToolsTests.TestHelpers;

namespace DispelTools.DataEditor.Tests
{
    [TestClass()]
    public class SimpleEditorTests
    {
        private class TestMapper : Mapper
        {
            private const int PROPERTY_ITEM_SIZE = 50;
            private static byte[] testData;
            private static byte[] testDataSwitched;

            public static readonly byte[] STR_ZERO = new byte[1] { 0 };
            public static readonly byte[] STR_FILLER = new byte[8] { 0xcd, 0xcd, 0xcd, 0xcd, 0xcd, 0xcd, 0xcd, 0xcd };
            public const string STR_DATA = "test field name data1";
            public const int I32_DATA = 291;
            public const short I16_DATA = 16;
            public const byte BYTE_DATA = 8;
            public static readonly byte[] BYA_DATA = new byte[13] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            public const string STR_DATA2 = "test field name data2";
            public const int I32_DATA2 = 300;
            public const short I16_DATA2 = 32;
            public const byte BYTE_DATA2 = 4;
            public static readonly byte[] BYA_DATA2 = new byte[13] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130 };

            public TestMapper(IFileSystem fs) : base(fs)
            {
            }

            internal override int PropertyItemSize => PROPERTY_ITEM_SIZE;

            protected override List<ItemFieldDescriptor> CreateDescriptors()
            {
                var builder = new FileDescriptorBuilder();
                builder.Add("name", ItemFieldDescriptor.AsFixedString(30, 0xcd));
                builder.Add("i32", ItemFieldDescriptor.AsInt32());
                builder.Add("i16", ItemFieldDescriptor.AsInt16());
                builder.Add("byte", ItemFieldDescriptor.AsByte());
                builder.Add("bytes", ItemFieldDescriptor.AsByteArray(13));
                return builder.Build();
            }

            public byte[] TestData { get { if (testData == null) { CreateTestData(); } return testData; } }
            public byte[] TestDataSwitched { get { if (testDataSwitched == null) { CreateTestDataSwitched(); } return testDataSwitched; } }

            private void CreateTestData()
            {
                const int DATA_SIZE = PROPERTY_ITEM_SIZE * 2 + 4;
                testData = CombineByteArrays(
                    BitConverter.GetBytes(2),
                    System.Text.Encoding.ASCII.GetBytes(STR_DATA), STR_ZERO, STR_FILLER,
                    BitConverter.GetBytes(I32_DATA),
                    BitConverter.GetBytes(I16_DATA),
                    new byte[1] { BYTE_DATA },
                    BYA_DATA,
                    System.Text.Encoding.ASCII.GetBytes(STR_DATA2), STR_ZERO, STR_FILLER,
                    BitConverter.GetBytes(I32_DATA2),
                    BitConverter.GetBytes(I16_DATA2),
                    new byte[1] { BYTE_DATA2 },
                    BYA_DATA2
                );
                if (testData.Length != DATA_SIZE)
                {
                    throw new InternalTestFailureException($"data Length {testData.Length} is not {DATA_SIZE}");
                }
            }
            private void CreateTestDataSwitched()
            {
                const int DATA_SIZE = PROPERTY_ITEM_SIZE * 2 + 4;
                testDataSwitched = CombineByteArrays(
                    BitConverter.GetBytes(2),
                    System.Text.Encoding.ASCII.GetBytes(STR_DATA2), STR_ZERO, STR_FILLER,
                    BitConverter.GetBytes(I32_DATA2),
                    BitConverter.GetBytes(I16_DATA2),
                    new byte[1] { BYTE_DATA2 },
                    BYA_DATA2,
                    System.Text.Encoding.ASCII.GetBytes(STR_DATA), STR_ZERO, STR_FILLER,
                    BitConverter.GetBytes(I32_DATA),
                    BitConverter.GetBytes(I16_DATA),
                    new byte[1] { BYTE_DATA },
                    BYA_DATA
                );
                if (testDataSwitched.Length != DATA_SIZE)
                {
                    throw new InternalTestFailureException($"data Length {testData.Length} is not {DATA_SIZE}");
                }
            }
        }

        [TestMethod()]
        public void ReadValueTest()
        {
            var mockFS = new MockFileSystem();

            var mapper = new TestMapper(mockFS);
            var mockFile = new MockFileData(mapper.TestData);
            mockFS.AddFile("testfile", mockFile);

            var editor = new SimpleEditor("testfile", mockFS);
            editor.SetMapper(mapper);

            var result1 = editor.ReadValue(0);
            Assert.AreEqual(TestMapper.STR_DATA, result1[0].EncodedText);
            Assert.AreEqual(TestMapper.I32_DATA, result1[1].Value);
            Assert.AreEqual(TestMapper.I16_DATA, result1[2].Value);
            Assert.AreEqual(TestMapper.BYTE_DATA, result1[3].Value);
            CollectionAssert.AreEqual(TestMapper.BYA_DATA, result1[4].ByteArrayValue);
            var result2 = editor.ReadValue(1);
            Assert.AreEqual(TestMapper.STR_DATA2, result2[0].EncodedText);
            Assert.AreEqual(TestMapper.I32_DATA2, result2[1].Value);
            Assert.AreEqual(TestMapper.I16_DATA2, result2[2].Value);
            Assert.AreEqual(TestMapper.BYTE_DATA2, result2[3].Value);
            CollectionAssert.AreEqual(TestMapper.BYA_DATA2, result2[4].ByteArrayValue);
        }

        [TestMethod()]
        public void SaveTest()
        {
            var mockFS = new MockFileSystem();

            var mapper = new TestMapper(mockFS);
            var mockFile = new MockFileData(mapper.TestData);
            mockFS.AddFile("testfile", mockFile);

            var editor = new SimpleEditor("testfile", mockFS);
            editor.SetMapper(mapper);

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

            editor.Save(editor.ReadValue(1), 0);
            editor.Save(editor.ReadValue(0), 1);

            CollectionAssert.AreEqual(mapper.TestDataSwitched, mockFile.Contents);
        }
    }
}