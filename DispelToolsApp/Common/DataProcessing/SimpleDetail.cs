namespace DispelTools.Common.DataProcessing
{
    public class SimpleDetail
    {
        public string Details { get; protected set; }

        public SimpleDetail(string detail)
        {
            Details = detail;
        }
        public SimpleDetail (params string[] results)
        {
            string details = "";

            foreach (string s in results)
            {
                details += s + "\r\n";
            }
            Details = details;
        }
    }
}
