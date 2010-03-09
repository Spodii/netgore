using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.JScript;

namespace NetGore.IO
{
    public class AssemblyClassInvoker
    {
        readonly Type _classType;
        readonly object _classInstance;

        public AssemblyClassInvoker(Assembly assembly, string className)
        {
            _classType = assembly.GetType(className);
            _classInstance = Activator.CreateInstance(_classType);
        }

        public object Invoke(string method, params object[] args)
        {
            var result = _classType.InvokeMember(method, BindingFlags.InvokeMethod, null, _classInstance, args );
            return result;
        }

        public string InvokeAsString(string method, params object[] args)
        {
            return Invoke(method, args).ToString();
        }
    }

    public class JScriptAssemblyCreator
    {
        readonly List<string> _methods = new List<string>();

        bool _hasAddedMessageScriptMethods = false;

        public string Namespace { get; set; }
        public string ClassName { get; set; }

        public void AddMessageScriptMethod(string name, string messageScript)
        {
            if (!_hasAddedMessageScriptMethods)
            {
                _hasAddedMessageScriptMethods = true;
                AddSpecialMessageScriptMethods();
            }

            var convertedScript = MessageScriptToJScript(messageScript, 0, messageScript.Length);
            AddMethod(name, "public", "String", "p", "return " + convertedScript + ";");
        }

        public virtual AssemblyClassInvoker Compile()
        {
            var sourceCode = GetSourceCode(_methods);
            
            var provider = new JScriptCodeProvider();
            var compiler = provider.CreateCompiler();

            var p = new CompilerParameters { GenerateInMemory = true };
            var r = compiler.CompileAssemblyFromSource(p, sourceCode);

            return CreateAssemblyClassInvoker(r.CompiledAssembly, ClassName);
        }

        protected virtual AssemblyClassInvoker CreateAssemblyClassInvoker(Assembly assembly, string className)
        {
            return new AssemblyClassInvoker(assembly, className);
        }

        protected virtual void AddSpecialMessageScriptMethods()
        {
            AddSpecialMessageScriptMethod_GetSafe();
        }

        protected virtual void AddSpecialMessageScriptMethod_GetSafe()
        {
            AddMethod("GetSafe", "private", "String", "p, i : int", "return p.Length > i ? p[i] : \"<Param Missing>\";");
        }

        protected virtual string GetSourceCode(List<string> methods)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("class " + ClassName);
            sb.AppendLine("{");

            foreach (var method in methods)
                sb.AppendLine(method);

            sb.AppendLine("}");

            return sb.ToString();
        }

        public void AddMethod(string name, string visibility, string returnType, string args, string innerCode)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.Append(visibility);
            sb.Append(" function ");
            sb.Append(name);
            sb.Append("(");
            sb.Append(args);
            sb.Append(")");
            if (!string.IsNullOrEmpty(returnType))
                sb.Append(" : " + returnType);

            sb.AppendLine();

            // Body
            sb.AppendLine("{");
            sb.AppendLine(innerCode);
            sb.AppendLine("}");

            _methods.Add(sb.ToString());
        }

        protected virtual string MessageScriptToJScript(string s, int start, int length)
        {
            // TODO: Comments
            StringBuilder sb = new StringBuilder((int)((s.Length + 8) * 1.5));

            bool inQuoteBlock = false;
            var chars = s.ToCharArray();
            for (int i = start; i < start + length; i++)
            {
                var c = chars[i];
                switch (c)
                {
                    case '"':

                        // Keep track of whether or not we are in a quoted block
                        if (i == start || chars[i - 1] != '\\')
                        {
                            inQuoteBlock = !inQuoteBlock;
                        }
                        sb.Append('"');
                        break;

                    case '$':
                        if (i > start && chars[i - 1] == '\\')
                        {
                            sb.Length--;
                            sb.Append('$');
                            break;
                        }

                        int pieceStart = i;
                        while (++i < (start + length))
                        {
                            if (!char.IsDigit(chars[i]))
                                break;
                        }
                        i--;

                        if (inQuoteBlock)
                            sb.Append("\" + ");

                        sb.Append("GetSafe(p,");
                        sb.Append(chars, pieceStart + 1, i - pieceStart);
                        sb.Append(")");

                        if (inQuoteBlock)
                            sb.Append(" + \"");

                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }

            if (inQuoteBlock)
                sb.Append("\"");

            return sb.ToString();
        }
    }

    /// <summary>
    /// Base class for a collection of messages loaded from a file.
    /// </summary>
    /// <typeparam name="T">The Type of key.</typeparam>
    public abstract class MessageCollectionBase<T> : IMessageCollection<T>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary of messages for this language.
        /// </summary>
        readonly Dictionary<T, string> _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollectionBase{T}"/> class.
        /// </summary>
        /// <param name="file">Path to the file to load the messages from.</param>
        protected MessageCollectionBase(string file) : this(file, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollectionBase{T}"/> class.
        /// </summary>
        /// <param name="file">Path to the file to load the messages from.</param>
        /// <param name="secondary">Collection of messages to add missing messages from. If null, the
        /// collection will only contain messages specified in the file. Otherwise, any message that exists
        /// in this secondary collection but does not exist in the <paramref name="file"/> will be loaded
        /// to this collection from this secondary collection.</param>
        protected MessageCollectionBase(string file, IEnumerable<KeyValuePair<T, string>> secondary)
        {
            if (log.IsDebugEnabled)
                log.DebugFormat("Loading MessageCollectionBase from file `{0}`.", file);

            _messages = Load(file, secondary);

            var assemblyCreator = new JScriptAssemblyCreator();
            assemblyCreator.Namespace = "GameMessages";
            assemblyCreator.ClassName = "English"; // TODO: Proper name

            foreach (var msg in _messages)
                assemblyCreator.AddMessageScriptMethod(msg.Key.ToString(), msg.Value);

            _invoker = assemblyCreator.Compile();
        }

        readonly AssemblyClassInvoker _invoker;

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
                    log.DebugFormat("Added message `{0}` from default messages.", sourceMsg.Key.ToString());
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
        /// Loads the messages from a file.
        /// </summary>
        /// <param name="filePath">The full path of the file to load the message from.</param>
        /// <param name="secondary">The collection of messages to add missing messages from.</param>
        /// <returns>A dictionary containing the loaded messages.</returns>
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
            return _invoker.InvokeAsString(id.ToString(), new object[] { args });
        }

        #endregion
    }
}