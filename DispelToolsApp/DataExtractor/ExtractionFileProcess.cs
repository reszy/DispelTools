using System;
using System.IO;

namespace DispelTools.DataExtractor
{
    public class ExtractionFileProcess : IDisposable
    {
        private readonly BinaryReader file;
        private readonly string filename;
        private readonly string outputDirectory;
        private readonly ExtractionManager extractor;

        public string ErrorMessage { get; set; } = null;
        public BinaryReader File => file;
        public Stream Stream => file.BaseStream;
        public string OutputDirectory => outputDirectory;
        public string Filename => filename;
        public string Extension { get; private set; }
        public int FilesCreated { get; set; } = 0;

        public ExtractionManager Extractor => extractor;
        public ExtractionParams Options => extractor.ExtractionParams;

        public ExtractionFileProcess(ExtractionManager extractor, string file, string outputDirectory)
        {
            this.file = new BinaryReader(new FileStream(file, FileMode.Open));
            this.outputDirectory = outputDirectory;
            filename = Path.GetFileNameWithoutExtension(file);
            Extension = Path.GetExtension(file);
            this.extractor = extractor;
        }

        public void Dispose() => file.Close();
        public string ExceptionMessagePrefix() => $"File {filename} at( {file.BaseStream.Position} )";

        public ExtractorResultDetails ResultDetails => new ExtractorResultDetails()
        {
            FilesCreated = FilesCreated,
            ErrorMessage = ErrorMessage,
            LastPosition = file.BaseStream.Position
        };
    }
}
