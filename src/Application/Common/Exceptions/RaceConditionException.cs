namespace Application.Common.Exceptions
{
    using System;

    public class RaceConditionException : Exception
    {
        public RaceConditionException()
        {
        }

        public RaceConditionException(string message)
            : base(message)
        {
        }

        public RaceConditionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RaceConditionException(string name, object key)
            : base($"Unable to save Entity \"{name}\" with the key: ({key}).")
        {
        }
    }
}