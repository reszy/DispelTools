using System;

namespace DispelTools.Common
{
    public class ReadFileException : Exception
    {
        public string LongMessage { get; }
        public string Title { get; } = "Error reading file";
        public ReadFileException(string filename) : base(CreateMessageException(filename))
        {
            LongMessage = CreateLongMessage(filename);
        }
        public ReadFileException(string filename, string message) : base(CreateMessageException(filename, message))
        {
            LongMessage = CreateLongMessage(filename, message);
        }
        public ReadFileException(string filename, string message, Exception e) : base(CreateMessageException(filename, message), e)
        {
            LongMessage = CreateLongMessage(filename, message);
        }
        public ReadFileException(string filename, Exception e) : base(CreateMessageException(filename), e)
        {
            LongMessage = CreateLongMessage(filename);
        }

        public static string CreateLongMessage(string filename) => $"Error while reading file {filename}\nFile structure unrecognized";

        public static string CreateLongMessage(string filename, string message) => $"Error while reading file {filename}\nFile structure unrecognized\n{message}";

        public static string CreateMessageException(string filename) => $"Error reading {filename}";

        public static string CreateMessageException(string filename, string message) => $"Error reading {filename} : {message}";
    }
}
