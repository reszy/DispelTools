using System.Text;

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
            Details = string.Join("\r\n", results);
        }
    }
}
