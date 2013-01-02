using System;
using System.CodeDom.Compiler;
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
    /// <typeparam name="T">The Type of key.</typeparam>
    public abstract class MessageCollectionBase<T> : IMessageCollection<T>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEnumerable<CompilerError> _compilationErrors = Enumerable.Empty<CompilerError>();

        /// <summary>
        /// Dictionary of messages for this language.
        /// </summary>
        readonly Dictionary<T, string> _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollectionBase{T}"/> class.
        /// </summary>
        /// <param name="file">Path to the file to load the messages from.</param>
        /// <param name="secondary">Collection of messages to add missing messages from. If null, the
        /// collection will only contain messages specified in the file. Otherwise, any message that exists
        /// in this secondary collection but does not exist in the <paramref name="file"/> will be loaded
        /// to this collection from this secondary collection.</param>
        protected MessageCollectionBase(string file, IEnumerable<KeyValuePair<T, string>> secondary = null)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Loading MessageCollectionBase from file `{0}`.", file);

            // Load the script messages
            _messages = Load(file, secondary);
        }

        /// <summary>
        /// Gets the <see cref="CompilerError"/>s from trying to compile the messages. Will be empty if
        /// the compilation was successful or has not happened yet.
        /// </summary>
        public IEnumerable<CompilerError> CompilationErrors
        {
            get { return _compilationErrors; }
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
                if (log.IsDebugEnabled)
                    log.DebugFormat("Added message `{0}` from default messages.", sourceMsg.Key);
            }
        }

        /// <summary>
        /// Gets the IEqualityComparer to use for collections created by this collection.
        /// </summary>
        /// <returns>The IEqualityComparer to use for collections created by this collection.</returns>
        protected virtual IEqualityComparer<T> GetEqualityComparer()
        {
            return EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Checks if the given line is one that should be ignored. Typically, this means checking if a line starts with
        /// comment characters. By default, lines starting with a hash (#), slash (\ or /), and apostrophe (') are ignored.
        /// Blank lines are always ignored.
        /// </summary>
        /// <param name="fileLine">The line to check.</param>
        /// <returns>True if the line should be ignored; otherwise false.</returns>
        protected virtual bool IsLineToIgnore(string fileLine)
        {
            return fileLine.StartsWith("#") || fileLine.StartsWith("\\") || fileLine.StartsWith("'");
        }

        /// <summary>
        /// Loads the messages from a file.
        /// </summary>
        /// <param name="filePath">The full path of the file to load the message from.</param>
        /// <param name="secondary">The collection of messages to add missing messages from.</param>
        /// <returns>A dictionary containing the loaded messages.</returns>
        /// <exception cref="FileNotFoundException">No file was found at the <paramref name="filePath"/>.</exception>
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

            var loadedMessages = new Dictionary<T, string>(GetEqualityComparer());

            // Load all the lines in the file
            var lines = File.ReadAllLines(filePath);

            // Parse the lines
            foreach (var fileLine in lines)
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
                    if (!IsLineToIgnore(fileLine))
                    {
                        var colonIndex = fileLine.IndexOf(':');
                        if (colonIndex > 0)
                        {
                            // Split the message identifier and text
                            var idStr = fileLine.Substring(0, colonIndex).Trim();
                            msg = fileLine.Substring(colonIndex + 1).Trim();

                            // Find the corresponding type T for the id
                            if (TryParseID(idStr, out id))
                                return true;
                        }
                    }
                }
            }

            // Something went wrong somewhere
            id = default(T);
            msg = null;
            return false;
        }

        #region IMessageCollection<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<T, string>> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            string msg;
            if (!_messages.TryGetValue(id, out msg))
                return null;

            try
            {
                return string.Format(msg, args);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat("string.Format() failed on MsgId `{0}`. Exception: {1}", id, ex);
                return msg;
            }
        }

        #endregion
    }
}