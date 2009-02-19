using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Class containing all of the messages for the correspoding GameMessage.
    /// </summary>
    public class GameMessages : IEnumerable<KeyValuePair<GameMessage, string>>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Name of the default language to use.
        /// </summary>
        const string _defaultLanguageName = "English";

        /// <summary>
        /// Reference to the GameMessages of the default language.
        /// </summary>
        static readonly GameMessages _defaultMessages = new GameMessages();

        /// <summary>
        /// Suffix for the language files.
        /// </summary>
        const string _languageFileSuffix = ".txt";
        
        /// <summary>
        /// Dictionary of messages for this language.
        /// </summary>
        readonly Dictionary<GameMessage, string> _messages;

        /// <summary>
        /// GameMessages constructor.
        /// </summary>
        public GameMessages() : this(_defaultLanguageName)
        {
        }

        /// <summary>
        /// GameMessages constructor.
        /// </summary>
        /// <param name="language">Name of the language to load.</param>
        public GameMessages(string language)
        {
            _language = language;
            _messages = Load(language);
        }

        readonly string _language;

        /// <summary>
        /// Gets the name of this language.
        /// </summary>
        public string Language { get { return _language; } }

        /// <summary>
        /// Gets the specified message, parsed using the supplied parameters.
        /// </summary>
        /// <param name="id">GameMessage to get.</param>
        /// <param name="args">Parameters used to parse the message.</param>
        /// <returns>Parsed GameMessage, or null if the <paramref name="id"/> is not found or invalid.</returns>
        public string GetMessage(GameMessage id, params string[] args)
        {
            // Try to get the message
            string ret;
            if (!_messages.TryGetValue(id, out ret))
            {
                if (log.IsInfoEnabled)
                    log.WarnFormat("Failed to load message `{0}` for language `{1}`.", id, Language);
                return null;
            }

            // Parse the message if needed
            if (args == null || args.Length == 0)
                return ret;

            string parsed;

            try
            {
                parsed = string.Format(ret, args);
            }
            catch (FormatException)
            {
                // Invalid number of arguments - return the unparsed string
                const string errmsg = "Too few arguments supplied for GameMessage `{0}`.";
                Debug.Fail(string.Format(errmsg, id));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, id);
                return ret;
            }

#if DEBUG
            // Check if any parameters were missed
            bool valid;
            try
            {
                var subArgs = args.Take(args.Length - 1);
                string parsed2 = (subArgs.Count() > 0) ? string.Format(ret, subArgs) : ret;
                valid = (parsed2 == parsed);
            }
            catch (FormatException)
            {
                valid = true;
            }

            if (!valid)
            {
                const string errmsg = "Too many arguments supplied for GameMessage `{0}`.";
                Debug.Fail(string.Format(errmsg, id));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, id);
            }
#endif

            return parsed;
        }

        /// <summary>
        /// Adds all GameMessages from the <paramref name="source"/> that do not exist in the <paramref name="dest"/>.
        /// </summary>
        /// <param name="dest">Dictionary to add the messages to.</param>
        /// <param name="source">Source to get the missing messages from.</param>
        static void AddMissingMessages(IDictionary<GameMessage, string> dest, IEnumerable<KeyValuePair<GameMessage, string>> source)
        {
            foreach (var sourceMsg in source)
            {
                if (!dest.ContainsKey(sourceMsg.Key))
                {
                    dest.Add(sourceMsg.Key, sourceMsg.Value);
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Added message `{0}` from default language.", sourceMsg.Key);
                }
            }
        }

        static Dictionary<GameMessage, string> Load(string language)
        {
            language = language.ToLower();

            // Check if the default language and already loaded
            if (_defaultMessages != null && language == _defaultLanguageName.ToLower())
                return _defaultMessages._messages;

            // Get the file path
            var filePath = ContentPaths.Build.Languages.Join(language + _languageFileSuffix);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                const string errmsg = "Could not find the file for language `{0}`";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, language);
                throw new FileNotFoundException();
            }

            Dictionary<GameMessage, string> loadedMessages = new Dictionary<GameMessage, string>();

            // Load all the lines in the file
            string[] lines = File.ReadAllLines(filePath);

            // Parse the lines
            foreach (var fileLine in lines)
            {
                // Check for a valid line
                if (string.IsNullOrEmpty(fileLine))
                    continue;

                // Trim the line and remove tabs
                var line = fileLine.Replace('\t'.ToString(), string.Empty).Trim();

                // Check for a still-valid line
                if (string.IsNullOrEmpty(line))
                    continue;

                int colonIndex = line.IndexOf(':');
                if (colonIndex < 0)
                    continue;

                // Split the message identifier and text
                string id = line.Substring(0, colonIndex).Trim();
                string msg = line.Substring(colonIndex + 1).Trim();

                // Find the corresponding ServerMessage for the id
                GameMessage gameMsg = (GameMessage)Enum.Parse(typeof(GameMessage), id, true);

                // Make sure the message is defined
                if (!Enum.IsDefined(typeof(GameMessage), gameMsg))
                {
                    const string errmsg = "Languages file contains id `{0}`, but this is not in the ServerMessage enum.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, id);
                    Debug.Fail(string.Format(errmsg, id));
                    continue;
                }

                // Add the message to the dictionary of loaded messages
                loadedMessages.Add(gameMsg, msg);
            }

            // If not the default language, add values missing in this language from the default language
            if (language != _defaultLanguageName.ToLower())
                AddMissingMessages(loadedMessages, _defaultMessages);

            return loadedMessages;
        }

        public IEnumerator<KeyValuePair<GameMessage, string>> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
