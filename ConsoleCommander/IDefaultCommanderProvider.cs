using ConsoleCommander.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCommander
{
    public interface IDefaultCommanderProvider
    {
        /// <summary>
        /// Gets the Type of the commander to be used by default.
        /// </summary>
        /// <returns></returns>
        Type GetCommanderType { get;  }
    }

    public class DefinedDefaultCommanderProvider : IDefaultCommanderProvider
    {
        #region Properties

        private Type commanderType;

        #endregion

        #region Constructor(s)

        public DefinedDefaultCommanderProvider(Type commanderType)
        {
            if (!commanderType.IsCommander())

            {
                throw new ArgumentException($"'{commanderType.Name}' is no Commander. (It should derive from CommanderBase.)");
            }

            this.commanderType = commanderType;
        }

        #endregion

        #region IDefaultCommanderProvider implementation

        public Type GetCommanderType { get { return commanderType; } }

        #endregion
    }

    public class ConfiguredDefaultCommanderProvider : IDefaultCommanderProvider
    {
        #region Properties

        private Type commanderType;

        #endregion

        #region Constructor(s)

        public ConfiguredDefaultCommanderProvider(IConfigurationRoot configuration, string configurationKey)
        {
            if (string.IsNullOrEmpty(configurationKey))
            {
                throw new ArgumentNullException(nameof(configurationKey));
            }

            // Check if there was a defaultCommander defined.
            var assemblyQualifiedName = configuration.GetValue<string>(configurationKey);
            if (string.IsNullOrEmpty(assemblyQualifiedName))
            {
                throw new ApplicationException($"There is no '{configurationKey}' defined.");
            }

            // Get the Fully Qualified typeName for the commander to start.
            commanderType = Type.GetType(assemblyQualifiedName);
            if (!commanderType.IsCommander())
            {
                throw new ApplicationException($"The defined type for defaultCommander '{commanderType.Name}' is no Commander.");
            }
        }

        #endregion

        #region IDefaultCommanderProvider implementation

        public Type GetCommanderType { get { return commanderType; } }

        #endregion
    }
}
