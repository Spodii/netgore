using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Base class for a handler of Say commands.
    /// </summary>
    /// <typeparam name="T">The Type of User.</typeparam>
    public abstract class SayHandlerBase<T> where T : DynamicEntity
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly StringCommandParser<SayCommandAttribute> _parser;
        readonly ISayCommands<T> _sayCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandlerBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="sayCommands">The object containing the Say commands.</param>
        protected SayHandlerBase(ISayCommands<T> sayCommands)
        {
            _sayCommands = sayCommands;
            _parser = new StringCommandParser<SayCommandAttribute>(sayCommands.GetType());
        }

        /// <summary>
        /// Handles a command from a <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="text">The command string.</param>
        void HandleCommand(T user, string text)
        {
            // Remove the command symbol from the text
            text = RemoveCommandSymbol(text);

            // Parse
            string output;
            lock (_sayCommands)
            {
                _sayCommands.User = user;
                _parser.TryParse(_sayCommands, text, out output);
            }

            // Handle the output
            if (!string.IsNullOrEmpty(output))
                HandleCommandOutput(user, output);
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        protected abstract void HandleCommandOutput(T user, string text);

        /// <summary>
        /// When overridden in the derived class, handles text that was not a command.
        /// </summary>
        /// <param name="user">The user the <paramref name="text"/> came from.</param>
        /// <param name="text">The text that wasn't a command.</param>
        protected abstract void HandleNonCommand(T user, string text);

        /// <summary>
        /// Checks if a Say string is formatted to be a command.
        /// </summary>
        /// <param name="text">String to check.</param>
        /// <returns>True if a command, else false.</returns>
        protected virtual bool IsCommand(string text)
        {
            // Must be at least 2 characters long to be a command
            if (text.Length < 2)
                return false;

            // Check for a command character on the first character
            switch (text[0])
            {
                case '/':
                case '\\':
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if a string is a valid Say string.
        /// </summary>
        /// <param name="text">String to check.</param>
        /// <returns>True if a valid Say string, else false.</returns>
        protected virtual bool IsValidSayString(string text)
        {
            // Check if empty
            if (string.IsNullOrEmpty(text))
                return false;

            // Check each character in the string to make sure they are valid
            foreach (char letter in text)
            {
                int i = Convert.ToInt32(letter);
                if (i > 126 || i < 32)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Processes a string of text.
        /// </summary>
        /// <param name="user">User that the <paramref name="text"/> came from.</param>
        /// <param name="text">Text to process.</param>
        public void Process(T user, string text)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (log.IsInfoEnabled)
                log.InfoFormat("Processing Say string from User `{0}`: {1}", user, text);

            // Check for an invalid string
            if (!IsValidSayString(text))
            {
                const string errmsg = "Invalid Say string from User `{0}`: {1}";
                Debug.Fail(string.Format(errmsg, user, text));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, user, text);
                return;
            }

            // Check if a command
            if (IsCommand(text))
            {
                HandleCommand(user, text);
                return;
            }

            // Not a command, so just do a normal, to-area chat
            HandleNonCommand(user, text);
        }

        /// <summary>
        /// Removes the command symbol from the <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The string to remove the command symbol from.</param>
        /// <returns>The <paramref name="text"/> without the command symbol.</returns>
        protected virtual string RemoveCommandSymbol(string text)
        {
            return text.Substring(1);
        }
    }
}
