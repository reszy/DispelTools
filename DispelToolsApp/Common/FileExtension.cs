using System.IO;

namespace DispelTools.Common
{
    public static class FileReaderWriterExtension
    {
        public static void Skip(this BinaryWriter writer, int bytesToSkip) => writer.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        public static void Skip(this BinaryReader reader, int bytesToSkip) => reader.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
    }
}
