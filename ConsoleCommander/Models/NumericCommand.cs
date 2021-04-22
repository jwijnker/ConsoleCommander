using System;

namespace ConsoleCommander.Models
{
    public class NumericCommand : Command
    {
        public NumericCommand(int id, string description, Action action)
            : base(id.ToString(), description, action)
        {

        }
    }

}
