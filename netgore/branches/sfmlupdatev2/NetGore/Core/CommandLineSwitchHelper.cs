using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Provides helper functions for parsing command-line switches and the arguments that go with
    /// each of those switches.
    /// </summary>
    public static class CommandLineSwitchHelper
    {
        /// <summary>
        /// The name of the primary key. This is the key used for arguments that come before any switches.
        /// This key is only present if it has any arguments.
        /// </summary>
        public const string PrimaryKeyName = "Main";

        /// <summary>
        /// The prefix used to denote a switch. Any word after a word with this prefix that does not
        /// have this prefix will be considered as an argument to the switch. For example:
        /// -switch1 arg1 arg2 -switch2 -switch3 arg1
        /// </summary>
        public const string SwitchPrefix = "-";

        /// <summary>
        /// Gets the switches and their arguments from the given string array.
        /// </summary>
        /// <param name="args">The array of strings.</param>
        /// <returns>The switches and their arguments from the given string array.</returns>
        public static IEnumerable<KeyValuePair<string, string[]>> GetCommands(string[] args)
        {
            return GroupValuesToSwitches(args);
        }

        /// <summary>
        /// Gets the switches and their arguments from the given string array. Only switches that can be parsed to
        /// type <typeparamref name="T"/> will be returned.
        /// </summary>
        /// <typeparam name="T">The Type of Enum to use as the key.</typeparam>
        /// <param name="args">The array of strings.</param>
        /// <returns>The switches and their arguments from the given string array.</returns>
        /// <exception cref="MethodAccessException">Generic type <typeparamref name="T"/> must be an Enum.</exception>
        public static IEnumerable<KeyValuePair<T, string[]>> GetCommandsUsingEnum<T>(string[] args)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (!typeof(T).IsEnum)
            {
                const string errmsg = "Generic type T (type: {0}) must be an Enum.";
                throw new MethodAccessException(string.Format(errmsg, typeof(T)));
            }

            var items = GetCommands(args);

            foreach (var item in items)
            {
                T parsed;
                if (EnumHelper<T>.TryParse(item.Key, true, out parsed))
                    yield return new KeyValuePair<T, string[]>(parsed, item.Value);
            }
        }

        /// <summary>
        /// Groups the values after a switch to a switch.
        /// </summary>
        /// <param name="args">The array of strings.</param>
        /// <returns>The switches grouped with their values.</returns>
        static IEnumerable<KeyValuePair<string, string[]>> GroupValuesToSwitches(IList<string> args)
        {
            if (args == null || args.Count == 0)
                return Enumerable.Empty<KeyValuePair<string, string[]>>();

            var switchPrefixAsCharArray = SwitchPrefix.ToCharArray();

            var ret = new List<KeyValuePair<string, string[]>>(args.Count);

            var currentKey = PrimaryKeyName;
            var currentArgs = new List<string>(args.Count);

            // Iterate through all the strings
            for (var i = 0; i < args.Count; i++)
            {
                var currentArg = args[i];
                var currentArgTrimmed = args[i].Trim();

                if (currentArgTrimmed.StartsWith(SwitchPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    // Empty out the current switch
                    if (currentKey != PrimaryKeyName || currentArgs.Count > 0)
                        ret.Add(new KeyValuePair<string, string[]>(currentKey, currentArgs.ToArray()));

                    // Remove the switch prefix and set as the new key
                    currentKey = currentArgTrimmed.TrimStart(switchPrefixAsCharArray);
                    currentArgs.Clear();
                }
                else
                {
                    // Add the argument only if its an actual string
                    if (currentArg.Length > 0)
                        currentArgs.Add(currentArg);
                }
            }

            // Empty out the remainder
            if (currentKey != PrimaryKeyName || currentArgs.Count > 0)
                ret.Add(new KeyValuePair<string, string[]>(currentKey, currentArgs.ToArray()));

            return ret;
        }
    }
}