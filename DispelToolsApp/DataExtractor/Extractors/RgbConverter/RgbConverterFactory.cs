namespace DispelTools.DataExtractor.RgbConverter
{
    internal class RgbConverterFactory : IExtractorFactory
    {
        public string ExtractorName => "RgbConverter";

        public string FileFilter => "PNG file|*.png";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.NoOptions;

        public Extractor CreateInstance() => new Rgb24Converter();
    }
}
