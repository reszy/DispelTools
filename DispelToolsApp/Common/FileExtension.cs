using System.IO;

namespace DispelTools.Common
{
    public static class FileReaderWriterExtension
    {
        public static void Skip(this BinaryWriter writer, int bytesToSkip) => writer.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        public static void Skip(this BinaryWriter writer, long bytesToSkip) => writer.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        public static void Skip(this BinaryReader reader, int bytesToSkip) => reader.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        public static void Skip(this BinaryReader reader, long bytesToSkip) => reader.BaseStream.Seek(bytesToSkip, SeekOrigin.Current);
        public static void SetPosition(this BinaryReader reader, long position) => reader.BaseStream.Seek(position, SeekOrigin.Begin);
        public static void SetPosition(this BinaryReader reader, int position) => reader.BaseStream.Seek(position, SeekOrigin.Begin);
    }
}
