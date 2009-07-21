using System;
using System.Diagnostics;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Network;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    public class SayCommandAttribute : StringCommandBaseAttribute
    {
        public SayCommandAttribute(string command) : base(command)
        {
        }
    }

    /// <summary>
    /// Handles processing what Users say.
    /// </summary>
    public class SayHandler
    {
        /// <summary>
        /// The parser for the Say commands.
        /// </summary>
        static readonly SayCommandParser _parser = new SayCommandParser();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly SayCommands _sayCommands;

        /// <summary>
        /// SayHandler constructor.
        /// </summary>
        /// <param name="server">Server that the commands are coming from.</param>
        public SayHandler(Server server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _sayCommands = new SayCommands(server);
        }

        void HandleCommand(User user, string text)
        {
            // Remove the command symbol from the text
            text = text.Substring(1);

            // NOTE: This lock makes it so we can only parse one Say command at a time. Might want to use a pool in the future.
            string output;
            lock (_sayCommands)
            {
                _sayCommands.User = user;
                _parser.TryParse(_sayCommands, text, out output);
            }

            // Send the resulting message to the User
            if (!string.IsNullOrEmpty(output))
            {
                using (PacketWriter pw = ServerPacket.Chat(output))
                {
                    user.Send(pw);
                }
            }
        }

        /// <summary>
        /// Checks if a Say string is formatted to be a command.
        /// </summary>
        /// <param name="text">String to check.</param>
        /// <returns>True if a command, else false.</returns>
        static bool IsCommand(string text)
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
        static bool IsValidSayString(string text)
        {
            // Check if empty
            if (string.IsNullOrEmpty(text))
                return false;

            // Check each character in the string to make sure they are valid
            foreach (char letter in text)
            {
                int i = Convert.ToInt32(Convert.ToChar(letter));
                if (i > 126 || i < 32)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Processes a string of text.
        /// </summary>
        /// <param name="user">User that the text came from.</param>
        /// <param name="text">Text to process.</param>
        public void Process(User user, string text)
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
            using (PacketWriter pw = ServerPacket.ChatSay(user.Name, user.MapEntityIndex, text))
            {
                user.Map.SendToArea(user.Center, pw);
            }
        }

        /// <summary>
        /// Parser for the Say commands.
        /// </summary>
        class SayCommandParser : StringCommandParser<SayCommandAttribute>
        {
            public SayCommandParser() : base(typeof(SayCommands))
            {
            }
        }
    }
}