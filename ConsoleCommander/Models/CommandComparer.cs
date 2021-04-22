using System.Collections.Generic;

namespace ConsoleCommander.Models
{
    public class CommandComparer : IEqualityComparer<Command>
    {
        public bool Equals(Command x, Command y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Command obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
