using DispelTools.Common;
using System.IO;

namespace DispelTools.DataExtractor.SoundExtractor
{
    public class SnfSoundExtractor : Extractor
    {
        private class SoundHeader
        {
            public int DataSize { get; set; }
            public short PCMAudioFormat { get; set; }
            public short NumberOfChannels { get; set; }
            public int SampleRate { get; set; }
            public int ByteRate { get; set; }
            public short BlockAlign { get; set; }
            public short BitsPerSample { get; set; }
        }

        public override void ExtractFile(ExtractionFileProcess process)
        {
            var file = process.File;
            var header = new SoundHeader();
            header.DataSize = file.ReadInt32();
            if (header.DataSize == file.BaseStream.Length - 22)
            {
                header.PCMAudioFormat = file.ReadInt16();
                header.NumberOfChannels = file.ReadInt16();
                header.SampleRate = file.ReadInt32();
                header.ByteRate = file.ReadInt32();
                header.BlockAlign = file.ReadInt16();
                header.BitsPerSample = file.ReadInt16();
                file.ReadInt16();
                if (!Settings.ExtractorReadOnly)
                {
                    SaveToWaveFormat(process, file, header);
                }
            }
            else
            {
                throw new ExtractException($"{process.ExceptionMessagePrefix()} expected data size of ({file.BaseStream.Length - 22}) was ({header.DataSize})");
            }
        }

        private void SaveToWaveFormat(ExtractionFileProcess process, BinaryReader reader, SoundHeader header)
        {
            var outputFileName = $"{process.Filename}.wav";
            using (var writer = new BinaryWriter(new FileStream($"{process.OutputDirectory}\\{outputFileName}", FileMode.Create)))
            {
                WriteRiff(writer, header.DataSize);
                WriteFmt(writer, header);
                WriteData(writer, reader, header.DataSize);
            }
            process.WorkReporter.ReportFileCreated(process, outputFileName);
        }

        private void WriteRiff(BinaryWriter writer, int dataSize)
        {
            writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
            writer.Write(44 + dataSize);
            writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
        }

        private void WriteFmt(BinaryWriter writer, SoundHeader header)
        {
            writer.Write(new char[4] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write(header.PCMAudioFormat);
            writer.Write(header.NumberOfChannels);
            writer.Write(header.SampleRate);
            writer.Write(header.ByteRate);
            writer.Write(header.BlockAlign);
            writer.Write(header.BitsPerSample);
        }

        private void WriteData(BinaryWriter writer, BinaryReader reader, int dataSize)
        {
            writer.Write(new char[4] { 'd', 'a', 't', 'a' });
            writer.Write(dataSize);
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                writer.Write(reader.ReadBytes(2));
            }
        }
    }
}
