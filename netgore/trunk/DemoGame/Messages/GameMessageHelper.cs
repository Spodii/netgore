using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Helper methods for the <see cref="GameMessage"/> enum.
    /// </summary>
    public static class GameMessageHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The delimiter used for delimiting the arguments of a <see cref="GameMessage"/> packed into a string
        /// </summary>
        public const string StringDelimiter = "|";

        /// <summary>
        /// A cache of an empty string array.
        /// </summary>
        static readonly string[] _emptyStringArray = new string[0];

        /// <summary>
        /// Constructs a <see cref="GameMessage"/> with arguments as a string.
        /// </summary>
        /// <param name="gameMessage">The <see cref="GameMessage"/>.</param>
        /// <param name="p">The message arguments.</param>
        /// <returns>The <see cref="GameMessage"/> with its arguments combined into a string.</returns>
        public static string AsString(GameMessage gameMessage, params object[] p)
        {
            Debug.Assert(EnumHelper<GameMessage>.IsDefined(gameMessage));

            // When no parameters passed, just do a quick conversion to reduce garbage generation
            if (p == null)
                return ((int)gameMessage).ToString();

            // Build the string
            var sb = new StringBuilder();
            sb.Append((int)gameMessage);

            // Append all the parameters
            for (var i = 0; i < p.Length; i++)
            {
                sb.Append(StringDelimiter);
                sb.Append(p[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Deconstructs a <see cref="GameMessage"/> and its arguments from a string.
        /// </summary>
        /// <param name="s">The <see cref="GameMessage"/> and its arguments packed into a string.</param>
        /// <returns>A <see cref="KeyValuePair{T,U}"/> where the key is the <see cref="GameMessage"/> and the value
        /// is an array of the arguments, or an empty string if no arguments were supplied.</returns>
        /// <exception cref="ArgumentException"><paramref name="s"/> is null or empty.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> does not start with a valid <see cref="GameMessage"/>.</exception>
        public static KeyValuePair<GameMessage, string[]> FromString(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("Argument cannot be null or empty.", "s");

            var split = s.Split(new string[] { StringDelimiter }, StringSplitOptions.None);

            // Get the GameMessage
            GameMessage gameMessage;
            if (!EnumHelper<GameMessage>.TryParse(split[0], out gameMessage))
            {
                const string errmsg = "The string `{0}` does not start with a valid GameMessage.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, s);
                Debug.Fail(string.Format(errmsg, s));
                throw new FormatException(string.Format(errmsg, s));
            }

            // Get the arguments
            string[] args;
            if (split.Length == 1)
            {
                // Use our empty array
                args = _emptyStringArray;
            }
            else
            {
                // Create an array to store the arguments
                args = new string[split.Length - 1];
                for (var i = 0; i < args.Length; i++)
                {
                    args[i] = split[i + 1];
                }
            }

            return new KeyValuePair<GameMessage, string[]>(gameMessage, args);
        }
    }
}