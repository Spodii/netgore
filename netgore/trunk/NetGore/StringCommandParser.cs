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
    /// A class that finds methods marked with an Attribute of Type <typeparamref name="T"/>, and allows them to be
    /// invoked by a string. The parameters for the method are respected, and a method will only be invoked if all
    /// of the arguments are valid for the method's parameters.
    /// </summary>
    /// <typeparam name="T">The Type of Attribute to search for on the methods to handle. Only methods that
    /// contain an Attribute of this Type will be handled.</typeparam>
    public class StringCommandParser<T> where T : StringCommandBaseAttribute
    {
        /// <summary>
        /// Sorter used for the MethodInfos.
        /// </summary>
        static readonly MethodInfoSorter _methodInfoSorter = new MethodInfoSorter();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictionary containing the command name as the key, and a list of the MethodInfos for the methods
        /// that handle that command as the value.
        /// </summary>
        readonly Dictionary<string, IEnumerable<MethodInfo>> _commands =
            new Dictionary<string, IEnumerable<MethodInfo>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// StringCommandParser constructor.
        /// </summary>
        /// <param name="types">Array of Types to check for methods containing an Attribute of
        /// Type <typeparamref name="T"/>.</param>
        public StringCommandParser(params Type[] types) : this((IEnumerable<Type>)types)
        {
        }

        /// <summary>
        /// StringCommandParser constructor.
        /// </summary>
        /// <param name="types">IEnumerable of Types to check for methods containing an Attribute of
        /// Type <typeparamref name="T"/>.</param>
        public StringCommandParser(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException("types");

            var methods = MethodInfoHelper.FindMethodsWithAttribute<T>(types);

            // Create the dictionary for building the commands
            var dict = new Dictionary<string, List<MethodInfo>>(StringComparer.OrdinalIgnoreCase);

            // Set up the methods
            foreach (MethodInfo method in methods)
            {
                // Get the StringCommandBaseAttributes
                var attributes = method.GetCustomAttributes(typeof(T), true).Cast<T>();
                if (attributes.IsEmpty())
                {
                    // This should never happen, but just in case...
                    const string errmsg = "Type `{0}` does not contain the required StringCommandBaseAttribute.";
                    string err = string.Format(errmsg, method);
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    Debug.Fail(err);
                    throw new ArgumentException(err, "types");
                }

                // Make sure the parameters are supported
                foreach (ParameterInfo param in method.GetParameters())
                {
                    if (!StringParser.CanParseType(param.ParameterType))
                    {
                        const string errmsg = "Cannot add method `{0}`. Parameter `{1}` has type `{2}`, which cannot be parsed.";
                        string err = string.Format(errmsg, method.Name, param.Name, param.ParameterType);
                        if (log.IsErrorEnabled)
                            log.Error(err);
                        Debug.Fail(err);
                        throw new ArgumentException(err, "types");
                    }
                }

                // Add the commands
                foreach (T attrib in attributes)
                {
                    Add(dict, attrib.Command, method);
                }
            }

            // Add the commands to the main dictionary, converting them to an array to reduce the memory footprint
            foreach (var item in dict)
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
        static void Add(IDictionary<string, List<MethodInfo>> dict, string commandName, MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException("commandName");
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");
            if (dict == null)
                throw new ArgumentNullException("dict");

            // Grab the List for this commandName
            List<MethodInfo> methods;
            if (!dict.TryGetValue(commandName, out methods))
            {
                methods = new List<MethodInfo>(2);
                dict.Add(commandName, methods);
            }
            else
            {
                // Check for duplicate parameters for this command
                var p = methodInfo.GetParameters();
                foreach (MethodInfo m in methods)
                {
                    if (AreParameterTypesSame(p, m.GetParameters()))
                    {
                        const string errmsg = "Methods `{0}` and `{1}` both handle command `{2}` and contain the same parameters.";
                        string err = string.Format(errmsg, methodInfo, m, commandName);
                        if (log.IsFatalEnabled)
                            log.Error(err);
                        Debug.Fail(err);
                        throw new Exception(err);
                    }
                }
            }

            // Add the method
            methods.Add(methodInfo);

            // Sort the methods so the ones with the most non-string parameters are first
            methods.Sort(_methodInfoSorter);
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

            for (int i = 0; i < a.Count; i++)
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
        static string CombineStrings(string[] strings, int start, int end, char separator)
        {
            if (start == end)
                return strings[start];

            StringBuilder sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                sb.Append(strings[i]);
                sb.Append(separator);
            }
            sb.Length--;
            return sb.ToString();
        }

        /// <summary>
        /// Gets the parameter information for a method.
        /// </summary>
        /// <param name="method">Method to get the parameter information for.</param>
        /// <returns>The parameter information for the <paramref name="method"/>.</returns>
        static string GetParameterInfo(MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(128);
            foreach (ParameterInfo p in parameters)
            {
                sb.Append(p.ParameterType.Name);
                sb.Append(" ");
                sb.Append(p.Name);
                sb.Append(", ");
            }

            sb.Length -= 2;

            return sb.ToString();
        }

        /// <summary>
        /// Handles when an invalid command is called.
        /// </summary>
        /// <param name="commandName">The name of the command called. Not guarenteed to be an existant command.</param>
        /// <param name="args">Arguments used to invoke the command. Can be null.</param>
        /// <returns>A string containing a message about why the command failed to be handled.</returns>
        // ReSharper disable UnusedParameter.Global
        protected virtual string HandleInvalidCommand(string commandName, string[] args)
            // ReSharper restore UnusedParameter.Global
        {
            if (string.IsNullOrEmpty(commandName))
                return "No command specified.";

            IEnumerable<MethodInfo> methods;
            if (!_commands.TryGetValue(commandName, out methods))
                return "Unknown command specified.";

            if (methods.Count() == 0)
            {
                Debug.Fail("Shouldn't ever have 0 methods in the list.");
                return "Unknown command specified.";
            }

            return "Expected usage: " + commandName + " (" + GetParameterInfo(methods.First()) + ")";
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
            bool isInQuoted = false;
            int start = 0;

            var cs = commandString.ToCharArray();

            // Just split the string at spaces except for when it is a quoted string
            for (int end = 1; end < commandString.Length; end++)
            {
                bool endReached = false;

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

                    int s = start;
                    int e = end;

                    if (cs[s] == '\"' || cs[s] == ' ')
                        s++;
                    if (cs[e] == '\"' || cs[e] == ' ')
                        e--;

                    int len = e - s + 1;
                    if (len < 1)
                        continue;

                    string subStr = commandString.Substring(s, len);

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
        /// <param name="args">Arguments to send to the method.</param>
        /// <param name="result">If successfully invoked, this will contain the string returned from
        /// the <paramref name="method"/>. If unsuccessfully invoked, or the <paramref name="method"/> has
        /// a void return type, this will be an empty string.</param>
        /// <returns>True if the <paramref name="method"/> was invoked successfully; otherwise false.</returns>
        static bool TryInvokeMethod(object binder, MethodInfo method, string[] args, out string result)
        {
            var parameters = method.GetParameters();

            // Args always has to be >= the number of parameters
            if (parameters.Length > args.Length)
            {
                result = string.Empty;
                return false;
            }

            // The number of parameters can only be less than the number of args if the last parameter is a string
            ParameterInfo lastParameter = parameters.LastOrDefault();
            if (parameters.Length < args.Length && (lastParameter == null || lastParameter.ParameterType != typeof(string)))
            {
                result = string.Empty;
                return false;
            }

            var convertedArgs = new object[parameters.Length];

            // If args.Length > parameters.Length, then the last parameter is a string, so just join them all together
            // as one big string. Otherwise, we have to parse the last argument.
            int length = parameters.Length;
            if (parameters.Length < args.Length)
            {
                length--; // Makes it so we don't try to parse the last argument
                string combinedString = CombineStrings(args, convertedArgs.Length - 1, args.Length - 1, ' ');
                convertedArgs[convertedArgs.Length - 1] = combinedString;
            }

            // Try to perform the needed casts for all of the arguments
            for (int i = 0; i < length; i++)
            {
                object parsed;
                if (!StringParser.TryParse(args[i], parameters[i].ParameterType, out parsed) || parsed == null)
                {
                    result = string.Empty;
                    return false;
                }

                convertedArgs[i] = parsed;
            }

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
            IEnumerable<MethodInfo> methods;
            if (string.IsNullOrEmpty(command) || !_commands.TryGetValue(command, out methods))
            {
                result = HandleInvalidCommand(command, args);
                return false;
            }

            // Try invoking all of the methods handling this command in order, stopping on the first
            // one that was invoked successfully
            foreach (MethodInfo method in methods)
            {
                if (TryInvokeMethod(binder, method, args, out result))
                    return true;
            }

            // None of the methods were able to handle the command
            result = HandleInvalidCommand(command, args);
            return false;
        }

        /// <summary>
        /// Provides sorting for the MethodInfo based on which one has the least number of parameters
        /// of type string or object.
        /// </summary>
        class MethodInfoSorter : IComparer<MethodInfo>
        {
            #region IComparer<MethodInfo> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <returns>
            /// Value 
            ///                     Condition 
            ///                     Less than zero
            ///                 <paramref name="x"/> is less than <paramref name="y"/>.
            ///                     Zero
            ///                 <paramref name="x"/> equals <paramref name="y"/>.
            ///                     Greater than zero
            ///                 <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <param name="x">The first object to compare.
            ///                 </param><param name="y">The second object to compare.
            ///                 </param>
            public int Compare(MethodInfo x, MethodInfo y)
            {
                int nonStringX =
                    x.GetParameters().Count(p => p.ParameterType != typeof(string) && p.ParameterType != typeof(object));
                int nonStringY =
                    x.GetParameters().Count(p => p.ParameterType != typeof(string) && p.ParameterType != typeof(object));

                return nonStringX.CompareTo(nonStringY);
            }

            #endregion
        }
    }
}