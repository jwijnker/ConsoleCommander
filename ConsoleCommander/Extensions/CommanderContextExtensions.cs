using System.Collections.Generic;

namespace ConsoleCommander.Extensions
{
    public static class CommanderContextExtensions
    {
        #region Properties

        public static IDictionary<string, object> GlobalContext = new Dictionary<string, object>();
        public static IDictionary<CommanderBase, IDictionary<string, object>> Context = new Dictionary<CommanderBase, IDictionary<string, object>>();

        #endregion

        public static IDictionary<string, object> UseContext(this CommanderBase commander, bool useGlobalContext = false)
        {
            // Check for commander specific context
            if (!commander.HasContext())
            {
                Context.Add(commander, new Dictionary<string, object>());
            }

            return (useGlobalContext) ? Context[null] : Context[commander];
        }

        public static bool HasContext(this CommanderBase commander)
        {
            return Context.ContainsKey(commander);
        }
    }
}
