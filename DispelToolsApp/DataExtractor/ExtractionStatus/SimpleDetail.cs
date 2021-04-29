namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class SimpleDetail
    {
        public string Details { get; private set; }
        public static SimpleDetail NewDetails(params string[] results)
        {
            string details = "";

            foreach (string s in results)
            {
                details += s + "\r\n";
            }
            return new SimpleDetail()
            {
                Details = details
            };
        }
    }
}
