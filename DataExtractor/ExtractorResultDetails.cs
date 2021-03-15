namespace DispelTools.DataExtractor
{
    public class ExtractorResultDetails
    {
        public int FilesCreated { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public long LastPosition { get; internal set; }
    }
}
