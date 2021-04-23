using System;
using System.Runtime.Serialization;

namespace ConsoleCommander.Exceptions
{
    [Serializable]
    public class CommandNotRegisteredException : Exception
    {
        public string CommandId { get; }

        public CommandNotRegisteredException(string commandId)
            : base($"There is no command '{commandId}' registered.")
        {
            if (string.IsNullOrEmpty(commandId))
            {
                throw new ArgumentNullException(nameof(commandId));
            }

            this.CommandId = commandId;
        }

        protected CommandNotRegisteredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
