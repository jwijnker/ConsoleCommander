using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCommander
{
    public static class CommanderWriteExtensions
    {
        //
        // Logging-like write methods.
        //
        public static void Trace(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Blue);
        }

        public static void Info(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Cyan);
        }

        public static void Verbose(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Magenta);
        }

        public static void Debug(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.White);
        }

        public static void Warning(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Yellow);
        }

        public static void Error(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Red);
        }

        public static void Write(this CommanderBase commander, int message, ConsoleColor color = ConsoleColor.Gray)
        {
            commander.WriteLine(message.ToString(), color);
        }

        //
        // (Un-)Succesfull writers
        //
        public static void Success(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Green);
        }

        public static void Failed(this CommanderBase commander, string message)
        {
            commander.WriteLine(message, ConsoleColor.Red);
        }

        //
        // Specifics
        //

        public static void WriteEmptyLine(this CommanderBase commander)
        {
            commander.WriteLine(string.Empty);
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
                    commander.WriteLine(i.ToString());
                }
                else
                {
                    commander.WriteLine(writeFormat(i));
                }
            }

            commander.Info($"There are {items.Count()} items found.");
            commander.WriteEmptyLine();
        }

        public static void WriteAsTable<T>(this CommanderBase commander, IEnumerable<T> items, IDictionary<string, Func<T, object>> columns, string nullReplacementValue = "NULL")
        {
            // Dictionary to hold the max lenght of a column-value.
            var lenDict = new Dictionary<string, int>();

            foreach (var c in columns)
            {
                // Determine the max lenght of values of the column
                var len = ((items == null | !items.Any())
                        ? 0
                        : items.Max(i => {
                            var r = c.Value.Invoke(i);
                            return (r == null) ? nullReplacementValue.Length : r.ToString().Length;
                        })
                        );

                // Store the max len
                lenDict.Add(c.Key, Math.Max(len, c.Key.Length));
            }

            // Title
            var lenTotal = lenDict.Values.Sum()
                + ((lenDict.Count() -1) * 3);
            var titleLen = typeof(T).Name.Length;

            commander.WriteLine($"| {typeof(T).Name}{new String(' ', lenTotal - titleLen)} |", ConsoleColor.White);

            // Table Headers
            foreach (var header in columns)
            {
                var len = lenDict[header.Key];
                commander.Write($"| {header.Key.PadRight(len)} ", ConsoleColor.White);
            }
            commander.WriteLine($"|", ConsoleColor.White);

            // Separator-line/row
            foreach (var i in columns)
            {
                commander.Write($"|{new String('=', lenDict[i.Key] + 2)}");
            }
            commander.WriteLine($"|");

            // Values
            foreach (var i in items)
            {
                foreach (var c in columns)
                {
                    // Get the value first
                    var result = c.Value.Invoke(i);
                    // .. check if not null
                    //var value = (result == null) ? string.Empty : result;
                    var value = (result == null)
                        ? nullReplacementValue
                        : result.ToString()
                            .Replace(Environment.NewLine, string.Empty);

                    commander.Write($"| {value.ToString().PadRight(lenDict[c.Key])} ");
                }
                commander.WriteLine($"|");
            }

            // Footer
            var message = $"Lines: {items.Count()}, Columns: {columns.Count}";
            commander.WriteLine($"| {message}{new String(' ', lenTotal - message.Length)} |", ConsoleColor.White);
        }

    }
}
