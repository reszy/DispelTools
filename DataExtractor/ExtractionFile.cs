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

        public ExtractionFileProcess CreateProcess(ExtractionManager extractor) => new ExtractionFileProcess(extractor, file, outputDirectory);
    }
}
