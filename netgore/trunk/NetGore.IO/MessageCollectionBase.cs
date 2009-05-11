using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.IO
{
    /// <summary>
    /// Base class for a collection of messages loaded from a file.
    /// </summary>
    /// <typeparam name="T">Type of the key.</typeparam>
    public abstract class MessageCollectionBase<T> : IEnumerable<KeyValuePair<T, string>>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary of messages for this language.
        /// </summary>
        readonly Dictionary<T, string> _messages;

        /// <summary>
        /// MessageCollectionBase constructor.
        /// </summary>
        /// <param name="file">Path to the file to load the messages from.</param>
        protected MessageCollectionBase(string file) : this(file, null)
        {
        }

        /// <summary>
        /// MessageCollectionBase constructor.
        /// </summary>
        /// <param name="file">Path to the file to load the messages from.</param>
        /// <param name="secondary">Collection of messages to add missing messages from. If null, the
        /// collection will only contain messages specified in the file. Otherwise, any message that exists
        /// in this secondary collection but does not exist in the <paramref name="file"/> will be loaded
        /// to this collection from this secondary collection.</param>
        protected MessageCollectionBase(string file, IEnumerable<KeyValuePair<T, string>> secondary)
        {
            _messages = Load(file, secondary);
        }

        /// <summary>
        /// Adds all messages from the <paramref name="source"/> that do not exist in the <paramref name="dest"/>.
        /// </summary>
        /// <param name="dest">Dictionary to add the messages to.</param>
        /// <param name="source">Source to get the missing messages from.</param>
        static void AddMissingMessages(IDictionary<T, string> dest, IEnumerable<KeyValuePair<T, string>> source)
        {
            foreach (var sourceMsg in source)
            {
                if (dest.ContainsKey(sourceMsg.Key))
                    continue;

                dest.Add(sourceMsg.Key, sourceMsg.Value);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Added message `{0}` from default messages.", sourceMsg.Key.ToString());
            }
        }

        /// <summary>
        /// Gets the specified message, parsed using the supplied parameters.
        /// </summary>
        /// <param name="id">ID of the message to get.</param>
        /// <param name="args">Parameters used to parse the message.</param>
        /// <returns>Parsed message for the <paramref name="id"/>, or null if the <paramref name="id"/> 
        /// is not found or invalid.</returns>
        public virtual string GetMessage(T id, params string[] args)
        {
            // Try to get the message
            string ret;
            if (!_messages.TryGetValue(id, out ret))
            {
                if (log.IsInfoEnabled)
                    log.WarnFormat("Failed to load message `{0}`.", id.ToString());
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
                const string errmsg = "Too few arguments supplied for message `{0}`.";
                Debug.Fail(string.Format(errmsg, id));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, id.ToString());
                return ret;
            }

#if DEBUG
            // Check if any parameters were missed
            bool valid;
            try
            {
                var subArgs = args.Take(args.Length - 1);
                string parsed2 = (subArgs.Count() > 0) ? string.Format(ret, subArgs) : ret;
                valid = (parsed2 != parsed);
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

        Dictionary<T, string> Load(string filePath, IEnumerable<KeyValuePair<T, string>> secondary)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                const string errmsg = "Failed to load the MessageCollection because file does not exist: `{0}`";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath);
                throw new FileNotFoundException();
            }

            var loadedMessages = new Dictionary<T, string>();

            // Load all the lines in the file
            var lines = File.ReadAllLines(filePath);

            // Parse the lines
            foreach (string fileLine in lines)
            {
                T id;
                string msg;
                if (TryParseLine(fileLine, out id, out msg))
                    loadedMessages.Add(id, msg);
            }

            // Add missing messages from the secondary collection if specified
            if (secondary != null)
                AddMissingMessages(loadedMessages, secondary);

            return loadedMessages;
        }

        /// <summary>
        /// Helper for parsing an enum. <typeparamref name="T"/> must be an Enum. Returns false if
        /// the parse failed, or if the <paramref name="id"/> does not exist in the Enum.
        /// </summary>
        /// <param name="str">String to parse.</param>
        /// <param name="id">Parsed ID from the <paramref name="str"/>.</param>
        /// <returns>True if the ID was parsed successfully and exists in the Enum, else false.</returns>
        protected bool ParseEnumHelper(string str, out T id)
        {
            // Parse the string
            id = (T)Enum.Parse(typeof(T), str, true);

            // Check if it is part of the enum
            if (!Enum.IsDefined(typeof(T), id))
            {
                const string errmsg = "Languages file contains id `{0}`, but this is not in the ServerMessage enum.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, id);
                Debug.Fail(string.Format(errmsg, id));
                return false;
            }

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, tries to parse a string to get the ID.
        /// </summary>
        /// <param name="str">String to parse.</param>
        /// <param name="id">Parsed ID.</param>
        /// <returns>True if the ID was parsed successfully, else false.</returns>
        protected abstract bool TryParseID(string str, out T id);

        /// <summary>
        /// Parses a single line from the file.
        /// </summary>
        /// <param name="fileLine">File line to parse.</param>
        /// <param name="id">Parsed ID of the message.</param>
        /// <param name="msg">Parsed message for the corresponding <paramref name="id"/>.</param>
        /// <returns>True if the line was parsed successfully, else false.</returns>
        protected virtual bool TryParseLine(string fileLine, out T id, out string msg)
        {
            // Check for a valid line
            if (!string.IsNullOrEmpty(fileLine))
            {
                // Trim the line and remove tabs
                fileLine = fileLine.Replace('\t'.ToString(), string.Empty).Trim();

                // Check for a still-valid line
                if (!string.IsNullOrEmpty(fileLine))
                {
                    int colonIndex = fileLine.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        // Split the message identifier and text
                        string idStr = fileLine.Substring(0, colonIndex).Trim();
                        msg = fileLine.Substring(colonIndex + 1).Trim();

                        // Find the corresponding ServerMessage for the id
                        if (TryParseID(idStr, out id))
                            return true;
                    }
                }
            }

            // Something went wrong somewhere
            id = default(T);
            msg = null;
            return false;
        }

        #region IEnumerable<KeyValuePair<T,string>> Members

        public IEnumerator<KeyValuePair<T, string>> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}