using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataEditor.Export
{
    internal class ExportException : Exception
    {
        public ExportException()
        {
        }

        public ExportException(string? message) : base(message)
        {
        }

        public ExportException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExportException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
