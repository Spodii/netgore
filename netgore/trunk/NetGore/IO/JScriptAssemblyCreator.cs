using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.JScript;

namespace NetGore.IO
{
    /// <summary>
    /// Helps with creating an <see cref="Assembly"/> using JScript.
    /// </summary>
    public class JScriptAssemblyCreator
    {
        readonly List<string> _members = new List<string>();

        bool _hasAddedMessageScriptMethods = false;

        /// <summary>
        /// Gets or sets the name of the class to generate. This value must be set before calling Compile().
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Adds a method for a message script.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="messageScript">The raw message script.</param>
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

        /// <summary>
        /// Adds a method to the generated source code.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="visibility">The method's visibility.</param>
        /// <param name="returnType">The method return type.</param>
        /// <param name="args">The arguments for the method, or null if no arguments.</param>
        /// <param name="innerCode">The method's body.</param>
        public void AddMethod(string name, string visibility, string returnType, string args, string innerCode)
        {
            StringBuilder sb = new StringBuilder();

            // Header
            sb.Append(visibility);
            sb.Append(" function ");
            sb.Append(name);
            sb.Append("(");
            if (!string.IsNullOrEmpty(args))
                sb.Append(args);
            sb.Append(")");
            if (!string.IsNullOrEmpty(returnType))
                sb.Append(" : " + returnType);

            sb.AppendLine();

            // Body
            sb.AppendLine("{");
            sb.AppendLine(innerCode);
            sb.AppendLine("}");

            _members.Add(sb.ToString());
        }

        /// <summary>
        /// Adds the GetSafe JScript method that ensures the parameter index being grabbed for the input
        /// arguments of a message script method is valid or, if its not, returns a message instead of throwing
        /// an exception.
        /// </summary>
        protected virtual void AddSpecialMessageScriptMethod_GetSafe()
        {
            AddMethod("GetSafe", "private", "String", "p, i : int", "return p.Length > i ? p[i] : \"<Param Missing>\";");
        }

        /// <summary>
        /// Adds the special methods requires for the message script methods. Override in the derived class
        /// to add additional messages.
        /// </summary>
        protected virtual void AddSpecialMessageScriptMethods()
        {
            AddSpecialMessageScriptMethod_GetSafe();
        }

        /// <summary>
        /// Compiles the source code.
        /// </summary>
        /// <returns>A <see cref="AssemblyClassInvoker"/> to invoke the compiled <see cref="Assembly"/>.</returns>
        public AssemblyClassInvoker Compile()
        {
            CompilerResults r;
            return Compile(out r);
        }

        /// <summary>
        /// Compiles the source code.
        /// </summary>
        /// <param name="results">The results of the compilation.</param>
        /// <returns>A <see cref="AssemblyClassInvoker"/> to invoke the compiled <see cref="Assembly"/>.</returns>
        public virtual AssemblyClassInvoker Compile(out CompilerResults results)
        {
            var sourceCode = GetSourceCode(_members);

            var provider = new JScriptCodeProvider();
            var p = new CompilerParameters { GenerateInMemory = true, IncludeDebugInformation = false };

            results = provider.CompileAssemblyFromSource(p, sourceCode);

            return CreateAssemblyClassInvoker(results.CompiledAssembly, ClassName);
        }

        /// <summary>
        /// Creates the <see cref="AssemblyClassInvoker"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to invoke.</param>
        /// <param name="className">The name of the class to invoke.</param>
        /// <returns>The <see cref="AssemblyClassInvoker"/>.</returns>
        protected virtual AssemblyClassInvoker CreateAssemblyClassInvoker(Assembly assembly, string className)
        {
            return new AssemblyClassInvoker(assembly, className);
        }

        /// <summary>
        /// Gets the complete source code for the class.
        /// </summary>
        /// <param name="members">The members to include.</param>
        /// <returns>The class source code.</returns>
        protected virtual string GetSourceCode(List<string> members)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("class " + ClassName);
            sb.AppendLine("{");

            foreach (var member in members)
            {
                sb.AppendLine(member);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Converts a message script string to JScript.
        /// </summary>
        /// <param name="s">The message script string.</param>
        /// <param name="start">The index to start at.</param>
        /// <param name="length">The number of characters to include.</param>
        /// <returns>The JScript code for the <paramref name="s"/>.</returns>
        protected virtual string MessageScriptToJScript(string s, int start, int length)
        {
            StringBuilder sb = new StringBuilder((int)((s.Length + 8) * 1.5));

            // Start not in a code block
            bool inQuoteBlock = false;

            // Get the chars
            var chars = s.ToCharArray();

            // Loop through all the chars
            for (int i = start; i < start + length; i++)
            {
                // Get the current character
                var c = chars[i];
                switch (c)
                {
                    case '"':

                        // Keep track of whether or not we are in a quoted block.
                        // Don't count it if the quote is prefixed by a \.
                        if (i == start || chars[i - 1] != '\\')
                            inQuoteBlock = !inQuoteBlock;
                        sb.Append('"');
                        break;

                    case '$':

                        // Handle a parameter reference.
                        // Don't count it if the $ is prefixed by a \ or the next character
                        // is not a digit.
                        if ((i > start && chars[i - 1] == '\\') || (i == start - length - 1) || !char.IsDigit(chars[i + 1]))
                        {
                            sb.Length--;
                            sb.Append('$');
                            break;
                        }

                        // Remember the index the value started at.
                        int pieceStart = i;

                        // Keep getting chars until we run out of digits.
                        while (++i < (start + length))
                        {
                            if (!char.IsDigit(chars[i]))
                                break;
                        }
                        i--;

                        // If in a quoted block, we have to break it
                        if (inQuoteBlock)
                            sb.Append("\" + ");

                        // Add the method call
                        sb.Append("GetSafe(p,");
                        sb.Append(chars, pieceStart + 1, i - pieceStart);
                        sb.Append(")");

                        // Resume the quoted block if we were in one
                        if (inQuoteBlock)
                            sb.Append(" + \"");

                        break;

                    default:

                        // Any other character, just output it as normal
                        sb.Append(c);
                        break;
                }
            }

            // Ensure a quoted block ends
            if (inQuoteBlock)
                sb.Append("\"");

            return sb.ToString();
        }
    }
}