using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataExtractor
{
    internal class ExtractException : Exception
    {
        public List<string> ResultSnapshot { get; private set; }
        public ExtractException(string message) : base(message)
        {
            ResultSnapshot = new List<string>();
        }

        public ExtractException(List<string> details, string message) : base(message)
        {
            ResultSnapshot = details;
        }
    }
}
