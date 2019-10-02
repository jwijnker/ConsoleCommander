using System;
using System.Collections.Generic;
using System.Text;

namespace WT.ConsoleCommander.Extensions
{
    public static class CommanderContextExtensions
    {
        #region Properties

        public static IDictionary<CommanderBase, IDictionary<string, object>> Context = new Dictionary<CommanderBase, IDictionary<string, object>>();

        #endregion

        public static IDictionary<string, object> UseContext(this CommanderBase commander, bool useGlobalContext = false)
        {
            if (!Context.ContainsKey(null)) // Check for GlobalContext
            {
                // Create the Global-context
                Context.Add(null, new Dictionary<string, object>());
            }

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
