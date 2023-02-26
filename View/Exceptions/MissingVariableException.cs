namespace View.Exceptions
{
    internal class MissingVariableException : MessageException
    {
        public MissingVariableException(string variableName) : base($"Variable {variableName} has no value", MessageException.MessageType.ERROR)
        {

        }
    }
}
