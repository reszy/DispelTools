namespace DispelTools.DataExtractor.StringExtractor
{
    public class StringExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "StringExtractor";

        public string FileFilter => "Any file|*.*";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.NoOptions;

        public Extractor CreateInstance() => new StringExtractor();
    }
}
