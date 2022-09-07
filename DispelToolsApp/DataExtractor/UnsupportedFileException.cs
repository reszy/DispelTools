using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataExtractor
{
    public class UnsupportedFileException : Exception
    {
        public UnsupportedFileException(string message) : base(message)
        {
        }
    }
}
