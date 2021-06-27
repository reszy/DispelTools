namespace DispelTools.DataExtractor.ImageExtractor
{
    public class SprImageExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "ImageExtractor (SPR)";

        public string FileFilter => "sprite (*.SPR)|*.SPR;*.spr|All files (*.*)|*.*";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.OptionNames.AnimatedGifs | ExtractionParams.OptionNames.ColorMode;

        public Extractor CreateInstance() => new SprImageExtractor();
    }
}
