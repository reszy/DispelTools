namespace DispelTools.DataExtractor
{
    public interface IExtractorFactory
    {
        string ExtractorName { get; }
        string FileFilter { get; }

        ExtractionManager.ExtractorType Type { get; }

        ExtractionParams.OptionNames AcceptedOptions { get; }

        Extractor CreateInstance();
    }
}
