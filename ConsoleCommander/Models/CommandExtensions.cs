using System.Text.RegularExpressions;

namespace ConsoleCommander.Models
{
    public static class CommandExtensions
    {
        public static bool IsNumericCommand(this Command command)
        {
            return IsNumericCommand(command.Id);
        }

        public static bool IsNumericCommand(string commandId)
        {
            return Regex.IsMatch(commandId, @"^\d+$");
        }
    }

}
