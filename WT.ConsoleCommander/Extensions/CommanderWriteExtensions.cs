using System;
using System.Collections.Generic;
using System.Linq;

namespace WT.ConsoleCommander
{
    public static class CommanderWriteExtensions
    {
        public static void Info(this CommanderBase commander, string message)
        {
            commander.Write(message, ConsoleColor.Cyan);
        }

        public static void Verbose(this CommanderBase commander, string message)
        {
            commander.Write(message, ConsoleColor.Magenta);
        }

        public static void Debug(this CommanderBase commander, string message)
        {
            commander.Write(message, ConsoleColor.White);
        }

        public static void Warning(this CommanderBase commander, string message)
        {
            commander.Write(message, ConsoleColor.Yellow);
        }

        public static void Error(this CommanderBase commander, string message)
        {
            commander.Write(message, ConsoleColor.Red);
        }

        public static void Write(this CommanderBase commander, int message, ConsoleColor color = ConsoleColor.Gray)
        {
            commander.Write(message.ToString(), color);
        }

        public static void WriteEmptyLine(this CommanderBase commander)
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Write the items list using the given format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commander"></param>
        /// <param name="items"></param>
        /// <param name="writeFormat"></param>
        public static void WriteList<T>(this CommanderBase commander, IEnumerable<T> items, Func<T, string> writeFormat)
        {
            foreach (var i in items)
            {
                if (writeFormat == null)
                {
                    commander.Write(i.ToString());
                }
                else
                {
                    commander.Write(writeFormat(i));
                }
            }

            commander.Info($"There are {items.Count()} items found.");
            commander.WriteEmptyLine();
        }
    }
}
