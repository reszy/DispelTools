using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.Common.DataProcessing
{
    internal class ErrorDetail : SimpleDetail
    {
        public string Errors { get => Details; }
        public ErrorDetail(string message, string secondaryMessage = null)
        {
            var sb = new StringBuilder();
            sb.Append("Error: ");
            sb.AppendLine(message);

            if (!string.IsNullOrEmpty(secondaryMessage))
            {
                sb.Append("    ");
                sb.AppendLine(secondaryMessage);
            }
            sb.AppendLine();
            Details = sb.ToString();
        }
    }
}
