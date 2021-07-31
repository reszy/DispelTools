using DispelTools.DataExtractor.ExtractionStatus;

namespace DispelTools.DataExtractor
{
    public class ExtractionFile
    {
        private readonly string outputDirectory;
        private readonly string file;

        public ExtractionFile(string file, string outputDirectory)
        {
            this.file = file;
            this.outputDirectory = outputDirectory;
        }

        public string OuputDirectory { get => outputDirectory; }
        public string FileName { get => file; }

        public ExtractionFileProcess CreateProcess(ExtractionParams extractionParams, ExtractionWorkReporter workReporter) => new ExtractionFileProcess(extractionParams, file, outputDirectory, workReporter);
        public override string ToString() => $"[file: '{FileName}', out: '{OuputDirectory}']";
    }
}
