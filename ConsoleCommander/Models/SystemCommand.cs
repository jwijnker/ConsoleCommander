using System;

namespace ConsoleCommander.Models
{
    public class SystemCommand : Command
    {
        public SystemCommand(string id, string description, Action action)
            : base(id, description, action)
        {
        }
    }
}
