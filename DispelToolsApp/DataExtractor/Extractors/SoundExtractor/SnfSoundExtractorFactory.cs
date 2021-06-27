namespace DispelTools.DataExtractor.SoundExtractor
{
    public class SnfSoundExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "SoundExtractor (SNF)";

        public string FileFilter => "sound (*.snf)|*.SNF;*.snf";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.NoOptions;

        public Extractor CreateInstance() => new SnfSoundExtractor();
    }
}
