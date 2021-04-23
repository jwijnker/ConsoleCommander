using ConsoleCommander.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleCommander.Models
{
    [DebuggerStepThrough]
    public class CommandsContainer
    {
        #region Properties

        private IList<Command> registeredCommands = new List<Command>();

        public IEnumerable<Command> AllCommands { get { return registeredCommands.OrderBy(c => c.Id); } }

        public IEnumerable<SystemCommand> SystemCommands { get { return AllCommands.OfType<SystemCommand>(); } }
        public IEnumerable<NumericCommand> NumericCommands { get { return AllCommands.OfType<NumericCommand>(); } }
        public IEnumerable<StringCommand> StringCommands { get { return AllCommands.OfType<StringCommand>(); } }

        #endregion

        public void Add(Command command)
        {
            if (!IsRegistered(command))
            {
                registeredCommands.Add(command);
            }
        }

        public bool IsRegistered(Command command)
        {
            return IsRegistered(command.Id);
        }

        public bool IsRegistered(string commandId)
        {
            return registeredCommands.Select(r => r.Id).Contains(commandId);
        }

        public bool CanRegister(Command command)
        {
            return !IsRegistered(command);
        }

        public Command GetCommand(string id)
        {
            var command =  AllCommands.SingleOrDefault(c => c.Id == id);

            if (command == null)
            {
                throw new CommandNotRegisteredException(id);
            }

            return command;
        }
    }
}
