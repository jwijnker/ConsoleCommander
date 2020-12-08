using System;
using System.Collections.Generic;
using System.Linq;

namespace WT.ConsoleCommander
{
    public static class CommanderRequestExtensions
    {
        /// <summary>
        /// Asks the user for input. The user can see the options to pick from, and a default value is suggested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commander"></param>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static int requestValue<T>(this CommanderBase commander, string message, T defaultValue, IEnumerable<int> options = null)
        {
            string sval = string.Empty;

            if (options == null)
            {
                sval = commander.requestValue(message, defaultValue.ToString());
            }
            else
            {
                sval = commander.requestValue(message, defaultValue.ToString(), options.Select(v => v.ToString()));
            }

            return int.Parse(sval);
        }

        public static string requestValue(this CommanderBase commander, string message, string defaultValue = null, IEnumerable<string> options = null)
        {
            // Console.Write is used here because we want to stay on the current line.

            if (options != null)
            {
                Console.Write($"{message} ({defaultValue.ToString()}) [{string.Join(", ", options.ToString())}]: ");
            }
            else if (defaultValue != null)
            {
                Console.Write($"{message} ({defaultValue.ToString()}): ");
            }
            else
            {
                Console.Write($"{message}: ");
            }

            var userVal = Console.ReadLine();
            if (string.IsNullOrEmpty(userVal))
            {
                return defaultValue;
            }

            return userVal;
        }

        /// <summary>
        /// Request the user to pick an item from the list given.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commander"></param>
        /// <param name="items"></param>
        /// <param name="defaultIdx"></param>
        /// <returns></returns>
        public static T requestItem<T>(this CommanderBase commander, IEnumerable<T> items, string message = "item", int defaultIdx = 0)
        {
            return commander.requestItem(items, null, message, defaultIdx);
        }

        /// <summary>
        /// Request the user to pick an item from the list given.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commander"></param>
        /// <param name="items"></param>
        /// <param name="defaultIdx"></param>
        /// <returns></returns>
        public static T requestItem<T>(this CommanderBase commander, IEnumerable<T> items, Func<T, string> writeFormat, string message = "item", int defaultIdx = 0)
        {
            var idx = 0;
            foreach (var i in items)
            {
                if (writeFormat == null)
                {
                    commander.Write($"[{idx}]: {i.ToString()}");
                }
                else
                {
                    commander.Write($"[{idx}]: {writeFormat(i)}");
                }

                idx++;
            }

            var selectedItem = commander.requestValue(message, defaultIdx);
            return items.ElementAt(selectedItem);
        }

        /// <summary>
        /// Requests the user to choose a month number
        /// </summary>
        /// <param name="commander"></param>
        /// <param name="defaultMonth">By default the current month</param>
        /// <returns></returns>
        public static int requestMonth(this CommanderBase commander, int defaultMonth = 0)
        {
            if (defaultMonth == 0)
            {
                defaultMonth = DateTime.Now.Month;
            }
            return commander.requestValue("Month:", defaultMonth, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }.ToList());
        }

        public static bool requestBool(this CommanderBase commander, string message = "Yes or No?:", string trueText = "Yes", string falseText = "No", bool defaultValue = true)
        {
            var boolList = new Dictionary<string, bool>() {
                { falseText, false},
                { trueText, true}
            };
            var result = commander.requestItem(boolList.ToList(), kvp => $"{kvp.Value}", message, boolList.ToList().FindIndex(k => k.Value == defaultValue));

            return result.Value;
        }
    }
}
