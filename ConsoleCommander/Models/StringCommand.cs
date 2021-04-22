using System;

namespace ConsoleCommander.Models
{
    public class StringCommand : Command
    {
        public StringCommand(string id, string description, Action action)
            : base(id.ToString(), description, action)
        {
        }
    }
}
