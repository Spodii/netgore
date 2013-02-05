using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Base class for a handler of Say commands.
    /// </summary>
    /// <typeparam name="T">The type of the object that the commands are coming from (the user).</typeparam>
    /// <typeparam name="U">The type of <see cref="SayCommandAttribute"/>.</typeparam>
    public abstract class SayHandlerBase<T, U> where T : class where U : SayCommandAttribute
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly SayHandlerStringCommandParser _parser;
        readonly ISayCommands<T> _sayCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandlerBase{T,U}"/> class.
        /// </summary>
        /// <param name="sayCommands">The object containing the Say commands.</param>
        protected SayHandlerBase(ISayCommands<T> sayCommands)
        {
            _sayCommands = sayCommands;
            _parser = new SayHandlerStringCommandParser(this, sayCommands.GetType());
        }


        /// <summary>
        /// Gets if the given <paramref name="user"/> is allowed to invoke the given command.
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>True if the command can be invoked; otherwise false.</returns>
        protected virtual bool AllowInvokeCommand(T user, StringCommandParserCommandData<U> commandData)
        {
            return true;
        }

        /// <summary>
        /// Gets the message to display when the user is not allowed to invoke a command.
        /// </summary>
        /// <param name="user">The user invoking the command.</param>
        /// <param name="commandData">The information about the command to be invoked.</param>
        /// <returns>The message to display to the <paramref name="user"/>, or null or empty to display nothing.</returns>
        protected virtual string GetCommandNotAllowedMessage(T user, StringCommandParserCommandData<U> commandData)
        {
            return "You are not allowed to do that.";
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

            string orginalMessage = "/" + text;

            // Handle the output
            if (!string.IsNullOrEmpty(output))
                HandleCommandOutput(user, output, orginalMessage);
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        /// <param name="orginalMessage">The orginal message that was inititally input.</param>
        protected abstract void HandleCommandOutput(T user, string text, string orginalMessage);

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
            foreach (var letter in text)
            {
                // Only allow chars >= 32, and non-control chars
                if (letter < 32 || char.IsControl(letter))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Processes a string of text.
        /// </summary>
        /// <param name="user">User that the <paramref name="text"/> came from.</param>
        /// <param name="text">Text to process.</param>
        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <c>null</c>.</exception>
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

        sealed class SayHandlerStringCommandParser : StringCommandParser<U>
        {
            readonly SayHandlerBase<T, U> _sayHandlerBase;

            public SayHandlerStringCommandParser(SayHandlerBase<T, U> sayHandlerBase, params Type[] types)
                : base((IEnumerable<Type>)types)
            {
                _sayHandlerBase = sayHandlerBase;
            }

            /// <summary>
            /// When overridden in the derived class, gets if the <paramref name="binder"/> is allowed to invoke the method
            /// defined by the given <see cref="StringCommandParserCommandData{T}"/> using the given set of <paramref name="args"/>.
            /// </summary>
            /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
            /// this is ignored and can be null.</param>
            /// <param name="cmdData">The <see cref="StringCommandParserCommandData{T}"/>
            /// containing the method to invoke and the corresponding attribute
            /// that is attached to it.</param>
            /// <param name="args">The casted arguments to use to invoke the method.</param>
            /// <returns></returns>
            protected override bool AllowInvokeMethod(object binder, StringCommandParserCommandData<U> cmdData, object[] args)
            {
                var sayCommands = binder as ISayCommands<T>;
                if (sayCommands == null)
                {
                    const string errmsg = "Was expecting the binder `{0}` to be type ISayCommands<T>, but was `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, binder, binder != null ? binder.GetType().ToString() : "NULL");
                    Debug.Fail(string.Format(errmsg, binder, binder != null ? binder.GetType().ToString() : "NULL"));
                    return false;
                }

                return _sayHandlerBase.AllowInvokeCommand(sayCommands.User, cmdData);
            }

            /// <summary>
            /// Handles when a command with the valid parameters was found, but <see cref="StringCommandParser{T}.AllowInvokeMethod"/>
            /// returned false for the given <see cref="binder"/>.
            /// </summary>
            /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
            /// this is ignored and can be null.</param>
            /// <param name="cd">The <see cref="StringCommandParserCommandData{T}"/> for the command that the
            /// <paramref name="binder"/> was rejected from
            /// invoking.</param>
            /// <param name="args">Arguments used to invoke the command. Can be null.</param>
            /// <returns>A string containing a message about why the command failed to be handled.</returns>
            protected override string HandleCommandInvokeDenied(object binder, StringCommandParserCommandData<U> cd, string[] args)
            {
                var sayCommands = binder as ISayCommands<T>;
                if (sayCommands == null)
                {
                    const string errmsg = "Was expecting the binder `{0}` to be type ISayCommands<T>, but was `{1}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, binder, binder != null ? binder.GetType().ToString() : "NULL");
                    Debug.Fail(string.Format(errmsg, binder, binder != null ? binder.GetType().ToString() : "NULL"));
                    return string.Empty;
                }

                return _sayHandlerBase.GetCommandNotAllowedMessage(sayCommands.User, cd);
            }
        }
    }
}