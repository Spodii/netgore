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
    /// Non-generic base class for <see cref="StringCommandParser{T}"/>. See <see cref="StringCommandParser{T}"/>
    /// for more information.
    /// </summary>
    public abstract class StringCommandParser
    {
        public static readonly string UnknownCommandSpecified = "Unknown command specified.";

        /// <summary>
        /// Gets the parameter information for a method.
        /// </summary>
        /// <param name="method">Method to get the parameter information for.</param>
        /// <returns>The parameter information for the <paramref name="method"/>.</returns>
        public static string GetParameterInfo(MethodBase method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(128);
            foreach (var p in parameters)
            {
                sb.Append(p.ParameterType.Name);
                sb.Append(" ");
                sb.Append(p.Name);
                sb.Append(", ");
            }

            sb.Length -= 2;

            return sb.ToString();
        }
    }

    /// <summary>
    /// A class that finds methods marked with an <see cref="Attribute"/> of type <typeparamref name="T"/>, and allows them to be
    /// invoked by a string. The parameters for the method are respected, and a method will only be invoked if all
    /// of the arguments are valid for the method's parameters.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Attribute"/> to search for on the methods to handle. Only methods that
    /// contain an <see cref="Attribute"/> of this type will be handled.</typeparam>
    public class StringCommandParser<T> : StringCommandParser where T : StringCommandBaseAttribute
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sorter used for the <see cref="StringCommandParserCommandData{T}"/>s.
        /// </summary>
        static readonly CommandDataSorter _commandDataSorter = new CommandDataSorter();

        /// <summary>
        /// Dictionary containing the command name as the key, and a list of the MethodInfos for the methods
        /// that handle that command as the value.
        /// </summary>
        readonly IDictionary<string, IEnumerable<StringCommandParserCommandData<T>>> _commands =
            new Dictionary<string, IEnumerable<StringCommandParserCommandData<T>>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="StringCommandParser{T}"/> class.
        /// </summary>
        /// <param name="types">Array of Types to check for methods containing an Attribute of
        /// Type <typeparamref name="T"/>.</param>
        public StringCommandParser(params Type[] types) : this((IEnumerable<Type>)types)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringCommandParser{T}"/> class.
        /// </summary>
        /// <param name="types">IEnumerable of Types to check for methods containing an Attribute of
        /// Type <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="types" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">A type was found that does not contain the required
        /// <see cref="StringCommandBaseAttribute"/>.</exception>
        public StringCommandParser(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException("types");

            var methods = MethodInfoHelper.FindMethodsWithAttribute<T>(types);

            // Create the dictionary for building the commands
            var tmpDict = new Dictionary<string, List<StringCommandParserCommandData<T>>>(StringComparer.OrdinalIgnoreCase);

            // Set up the methods
            foreach (var method in methods)
            {
                var isValidMethod = true;

                // Get the StringCommandBaseAttributes
                var attributes = method.GetCustomAttributes(typeof(T), true).Cast<T>();
                if (attributes.IsEmpty())
                {
                    // This should never happen, but just in case...
                    const string errmsg = "Type `{0}` does not contain the required StringCommandBaseAttribute.";
                    var err = string.Format(errmsg, method);
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    Debug.Fail(err);
                    throw new ArgumentException(err, "types");
                }

                // Make sure the parameters are supported
                foreach (var param in method.GetParameters())
                {
                    if (!StringParser.CanParseType(param.ParameterType))
                    {
                        const string errmsg = "Cannot add method `{0}`. Parameter `{1}` has type `{2}`, which cannot be parsed.";
                        var err = string.Format(errmsg, method.Name, param.Name, param.ParameterType);
                        if (log.IsErrorEnabled)
                            log.Error(err);
                        Debug.Fail(err);
                        isValidMethod = false;
                    }
                }

                if (isValidMethod)
                {
                    // Add the commands
                    foreach (var attrib in attributes)
                    {
                        Add(tmpDict, attrib.Command, method, attrib);
                    }
                }
            }

            // Add the commands to the main dictionary
            foreach (var item in tmpDict)
            {
                _commands.Add(item.Key, item.Value.ToArray());
            }
        }

        /// <summary>
        /// Adds a command to a dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to add to.</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="methodInfo">Method for handling hte command.</param>
        /// <param name="attrib">The attribute bound to the method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandName"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="dict"/> is null.</exception>
        /// <exception cref="DuplicateKeyException">Multiple methods found for the same <paramref name="commandName"/>
        /// and same set of parameters.</exception>
        static void Add(IDictionary<string, List<StringCommandParserCommandData<T>>> dict, string commandName,
                        MethodInfo methodInfo, T attrib)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException("commandName");
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");
            if (dict == null)
                throw new ArgumentNullException("dict");

            // Grab the List for this commandName
            List<StringCommandParserCommandData<T>> cmdDatas;
            if (!dict.TryGetValue(commandName, out cmdDatas))
            {
                cmdDatas = new List<StringCommandParserCommandData<T>>(2);
                dict.Add(commandName, cmdDatas);
            }
            else
            {
                // Check for duplicate parameters for this command
                var p = methodInfo.GetParameters();
                foreach (var cd in cmdDatas)
                {
                    if (AreParameterTypesSame(p, cd.Method.GetParameters()))
                    {
                        const string errmsg = "Methods `{0}` and `{1}` both handle command `{2}` and contain the same parameters.";
                        var err = string.Format(errmsg, methodInfo, cd, commandName);
                        if (log.IsFatalEnabled)
                            log.Error(err);
                        Debug.Fail(err);
                        throw new DuplicateKeyException(err);
                    }
                }
            }

            // Add the method
            cmdDatas.Add(new StringCommandParserCommandData<T>(methodInfo, attrib));

            // Sort the methods so the ones with the most non-string parameters are first
            cmdDatas.Sort(_commandDataSorter);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="binder"/> is allowed to invoke the method
        /// defined by the given <see cref="StringCommandParserCommandData{T}"/> using the given set of <paramref name="args"/>.
        /// </summary>
        /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
        /// this is ignored and can be null.</param>
        /// <param name="cmdData">The <see cref="StringCommandParserCommandData{T}"/> containing the method to invoke and the corresponding attribute
        /// that is attached to it.</param>
        /// <param name="args">The casted arguments to use to invoke the method.</param>
        /// <returns></returns>
        protected virtual bool AllowInvokeMethod(object binder, StringCommandParserCommandData<T> cmdData, object[] args)
        {
            return true;
        }

        /// <summary>
        /// Checks if the parameter types in two lists are of the same Type. Length and order must also be same.
        /// </summary>
        /// <param name="a">First collection.</param>
        /// <param name="b">Second collection.</param>
        /// <returns>True if <paramref name="a"/> contains the same number of objects as <paramref name="b"/>, and
        /// the Type of each object is the same for each index.</returns>
        static bool AreParameterTypesSame(IList<ParameterInfo> a, IList<ParameterInfo> b)
        {
            if (a.Count != b.Count)
                return false;

            for (var i = 0; i < a.Count; i++)
            {
                if (a[i].ParameterType != b[i].ParameterType)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Combines the strings in an array to a single string.
        /// </summary>
        /// <param name="strings">Array of strings.</param>
        /// <param name="start">Index of the first string to combine.</param>
        /// <param name="end">Index of the last string to combine.</param>
        /// <param name="separator">Character used to separate the combined strings.</param>
        /// <returns>The combined strings.</returns>
        static string CombineStrings(IList<string> strings, int start, int end, char separator)
        {
            if (start == end)
                return strings[start];

            var sb = new StringBuilder();
            for (var i = start; i <= end; i++)
            {
                sb.Append(strings[i]);
                sb.Append(separator);
            }
            sb.Length--;
            return sb.ToString();
        }

        /// <summary>
        /// Gets a dictionary of each command and the method or methods that handle the command.
        /// </summary>
        /// <returns>A dictionary of each command and the method or methods that handle the command.</returns>
        public IEnumerable<KeyValuePair<string, IEnumerable<StringCommandParserCommandData<T>>>> GetCommands()
        {
            return _commands;
        }

        /// <summary>
        /// Handles when a command with the valid parameters was found, but <see cref="StringCommandParser{T}.AllowInvokeMethod"/>
        /// returned false for the given <see cref="binder"/>.
        /// </summary>
        /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
        /// this is ignored and can be null.</param>
        /// <param name="cd">The <see cref="StringCommandParserCommandData{T}"/> for the command that the <paramref name="binder"/> was rejected from
        /// invoking.</param>
        /// <param name="args">Arguments used to invoke the command. Can be null.</param>
        /// <returns>A string containing a message about why the command failed to be handled.</returns>
        protected virtual string HandleCommandInvokeDenied(object binder, StringCommandParserCommandData<T> cd, string[] args)
        {
            return string.Format("Object `{0}` was not allowed to invoke command `{1}`.",
                binder != null ? binder.ToString() : "NULL", cd.Attribute.Command);
        }


        /// <summary>
        /// Handles when an invalid command is called.
        /// </summary>
        /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
        /// this is ignored and can be null.</param>
        /// <param name="commandName">The name of the command called. Not guarenteed to be an existent command.</param>
        /// <param name="args">Arguments used to invoke the command. Can be null.</param>
        /// <returns>A string containing a message about why the command failed to be handled.</returns>
        protected virtual string HandleInvalidCommand(object binder, string commandName, string[] args)
        {
            if (string.IsNullOrEmpty(commandName))
                return "No command specified.";

            IEnumerable<StringCommandParserCommandData<T>> cmdDatas;
        
            if (!_commands.TryGetValue(commandName, out cmdDatas))
                return UnknownCommandSpecified;

            if (cmdDatas.IsEmpty())
            {
                Debug.Fail("Shouldn't ever have 0 methods in the list.");
                return UnknownCommandSpecified;
            }

            return "Expected usage: " + commandName + " (" + GetParameterInfo(cmdDatas.Select(x => x.Method).First()) + ")";
        }

        static bool MethodCanBeInvokedWithArgs(MethodInfo method, IList<string> args, out object[] convertedArgs)
        {
            convertedArgs = null;

            var parameters = method.GetParameters();

            // Args always has to be >= the number of parameters
            if (parameters.Length > args.Count)
                return false;

            // The number of parameters can only be less than the number of args if the last parameter is a string
            var lastParameter = parameters.LastOrDefault();
            if (parameters.Length < args.Count && (lastParameter == null || lastParameter.ParameterType != typeof(string)))
                return false;

            convertedArgs = new object[parameters.Length];

            // If args.Length > parameters.Length, then the last parameter is a string, so just join them all together
            // as one big string. Otherwise, we have to parse the last argument.
            var length = parameters.Length;
            if (parameters.Length < args.Count)
            {
                length--; // Makes it so we don't try to parse the last argument
                var combinedString = CombineStrings(args, convertedArgs.Length - 1, args.Count - 1, ' ');
                convertedArgs[convertedArgs.Length - 1] = combinedString;
            }

            // Try to perform the needed casts for all of the arguments
            for (var i = 0; i < length; i++)
            {
                object parsed;
                if (!StringParser.TryParse(args[i], parameters[i].ParameterType, out parsed) || parsed == null)
                    return false;

                convertedArgs[i] = parsed;
            }

            return true;
        }

        /// <summary>
        /// Parses the command string.
        /// </summary>
        /// <param name="commandString">The input command string.</param>
        /// <param name="command">If the parsing was successful, contains the name of the command; otherwise,
        /// contains an empty string.</param>
        /// <param name="args">If the parsing was successful, contains an array of the arguments; otherwise,
        /// contains an empty array of strings.</param>
        protected virtual void ParseCommandString(string commandString, out string command, out string[] args)
        {
            if (commandString == null || commandString.Length <= 1)
            {
                command = string.Empty;
                args = new string[0];
                return;
            }

            var splits = new List<string>();
            var isInQuoted = false;
            var start = 0;

            var cs = commandString.ToCharArray();

            // Just split the string at spaces except for when it is a quoted string
            for (var end = 1; end < commandString.Length; end++)
            {
                var endReached = false;

                if (end == commandString.Length - 1)
                    endReached = true;
                else
                {
                    switch (cs[end])
                    {
                        case ' ':
                            if (!isInQuoted)
                                endReached = true;
                            break;

                        case '\"':
                            if (isInQuoted)
                                endReached = true;
                            else
                                isInQuoted = true;
                            break;
                    }
                }

                if (endReached)
                {
                    isInQuoted = false;

                    var s = start;
                    var e = end;

                    if (cs[s] == '\"' || cs[s] == ' ')
                        s++;
                    if (cs[e] == '\"' || cs[e] == ' ')
                        e--;

                    var len = e - s + 1;
                    if (len < 1)
                        continue;

                    var subStr = commandString.Substring(s, len);

                    Debug.Assert(!subStr.StartsWith("\""));
                    Debug.Assert(!subStr.EndsWith("\""));

                    splits.Add(subStr);

                    start = end + 1;
                }
            }

            command = splits[0];
            splits.RemoveAt(0);
            args = splits.ToArray();
        }

        /// <summary>
        /// Tries to invoke a method.
        /// </summary>
        /// <param name="binder">The object to invoke the <paramref name="method"/> on. If <paramref name="method"/>
        /// is static, this is ignored and can be null.</param>
        /// <param name="method">MethodInfo to invoke.</param>
        /// <param name="convertedArgs">Arguments to send to the method.</param>
        /// <param name="result">If successfully invoked, this will contain the string returned from
        /// the <paramref name="method"/>. If unsuccessfully invoked, or the <paramref name="method"/> has
        /// a void return type, this will be an empty string.</param>
        /// <returns>True if the <paramref name="method"/> was invoked successfully; otherwise false.</returns>
        static bool TryInvokeMethod(object binder, MethodInfo method, object[] convertedArgs, out string result)
        {
            // Try to invoke the method
            object methodOutput;
            try
            {
                methodOutput = method.Invoke(binder, convertedArgs);
            }
            catch (Exception ex)
            {
                Debug.Fail("Caught exception when trying to execute method: " + ex);
                if (log.IsErrorEnabled)
                    log.ErrorFormat("Error executing string command: {0}", ex);

                result = string.Empty;
                return false;
            }

            // Set the return string
            if (methodOutput != null)
                result = methodOutput.ToString();
            else
                result = string.Empty;

            return true;
        }

        /// <summary>
        /// Tries to parse a command from the given <paramref name="commandString"/>.
        /// </summary>
        /// <param name="binder">The object to invoke the method on. If the method handling the command is static,
        /// this is ignored and can be null.</param>
        /// <param name="commandString">The string for the command. This should start with the name of the
        /// command followed by all of the space-delimited arguments.</param>
        /// <param name="result">If the parsing was successful, this will contain the return value from the
        /// function that handled the command. If the function has a void return type, this will be an
        /// empty string. If the specified arguments were invalid, or the command did not exist, this
        /// will contain an error message stating what went wrong.</param>
        /// <returns>True if the command was successfully parsed; otherwise false.</returns>
        public bool TryParse(object binder, string commandString, out string result)
        {
            // Split up the command and arguments
            string command;
            string[] args;
            ParseCommandString(commandString, out command, out args);

            // Check for a valid command
            IEnumerable<StringCommandParserCommandData<T>> cmdDatas;
            if (string.IsNullOrEmpty(command) || !_commands.TryGetValue(command, out cmdDatas))
            {
                result = HandleInvalidCommand(binder, command, args);
                return false;
            }

            // Try invoking all of the methods handling this command in order, stopping on the first
            // one that was invoked successfully
            StringCommandParserCommandData<T> commandExistsCD = null;

            foreach (var cd in cmdDatas)
            {
                object[] convertedArgs;

                // Check if the method can be invoked with the arguments given
                if (MethodCanBeInvokedWithArgs(cd.Method, args, out convertedArgs))
                {
                    // If we got this far, we know a valid method exists
                    commandExistsCD = cd;

                    // See if we are allowed to invoke it
                    if (!AllowInvokeMethod(binder, cd, convertedArgs))
                        continue;

                    // We are allowed to invoke it, so try and invoke
                    if (TryInvokeMethod(binder, cd.Method, convertedArgs, out result))
                        return true;
                }
            }

            if (commandExistsCD != null)
            {
                // The method existed, but we were not allowed to invoke it
                result = HandleCommandInvokeDenied(binder, commandExistsCD, args);
            }
            else
            {
                // None of the methods were able to handle the command
                result = HandleInvalidCommand(binder, command, args);
            }

            return false;
        }

        /// <summary>
        /// Provides sorting for the <see cref="StringCommandParserCommandData{T}"/>s based on which <see cref="MethodInfo"/> has the least number
        /// of parameters of type string or object.
        /// </summary>
        class CommandDataSorter : IComparer<StringCommandParserCommandData<T>>
        {
            static readonly Func<ParameterInfo, bool> _countFuncDelegate;

            /// <summary>
            /// Initializes the <see cref="StringCommandParser{T}.CommandDataSorter"/> class.
            /// </summary>
            static CommandDataSorter()
            {
                _countFuncDelegate = CountFunc;
            }

            static bool CountFunc(ParameterInfo p)
            {
                return p.ParameterType != typeof(string) && p.ParameterType != typeof(object);
            }

            #region IComparer<StringCommandParserCommandData<T>> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/> is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/> equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            public int Compare(StringCommandParserCommandData<T> x, StringCommandParserCommandData<T> y)
            {
                var a = x.Method;
                var b = y.Method;

                var nonStringX = a.GetParameters().Count(_countFuncDelegate);
                var nonStringY = b.GetParameters().Count(_countFuncDelegate);

                return nonStringX.CompareTo(nonStringY);
            }

            #endregion
        }
    }

    /// <summary>
    /// Describes a command in the <see cref="StringCommandParser{T}"/>.
    /// </summary>
    public class StringCommandParserCommandData<T> where T : StringCommandBaseAttribute
    {
        readonly T _attribute;
        readonly MethodInfo _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringCommandParserCommandData{T}"/> class.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/>.</param>
        /// <param name="attribute">The attribute.</param>
        public StringCommandParserCommandData(MethodInfo method, T attribute)
        {
            _attribute = attribute;
            _method = method;
        }

        /// <summary>
        /// Gets the attribute that resulted in this method being binded to the command.
        /// </summary>
        public T Attribute
        {
            get { return _attribute; }
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for the method to be invoked.
        /// </summary>
        public MethodInfo Method
        {
            get { return _method; }
        }
    }
}