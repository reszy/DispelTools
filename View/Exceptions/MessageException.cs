using System;
using System.Windows;

namespace View.Exceptions
{
    public class MessageException : Exception
    {
        private readonly MessageType messageType;

        public string Header { get; }

        public MessageBoxImage GetMessageBoxIcon() => messageType switch
        {
            MessageType.WARNING => MessageBoxImage.Warning,
            _ => MessageBoxImage.Error
        };
        public enum MessageType { WARNING, ERROR }
        public MessageException(string message, MessageType type) : this(message, Enum.GetName(typeof(MessageType), type) ?? "ERROR", type)
        {
        }
        public MessageException(string message, bool isWarning) : this(message, isWarning ? MessageType.WARNING : MessageType.ERROR)
        {
        }
        public MessageException(string message, string header, MessageType type ):base(message)
        {
            Header = header;
            messageType = type;
        }

        public MessageException(string message, string header, bool isWarning) : this(message, isWarning ? MessageType.WARNING : MessageType.ERROR)
        {
        }
    }
}
