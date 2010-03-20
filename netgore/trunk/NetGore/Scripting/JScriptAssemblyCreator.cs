using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.JScript;

namespace NetGore.Scripting
{
    /// <summary>
    /// Helps with creating an <see cref="Assembly"/> using JScript.
    /// </summary>
    public class JScriptAssemblyCreator
    {
        static readonly ScriptAssemblyCache _scriptAssemblyCache = ScriptAssemblyCache.Instance;
        readonly List<string> _members = new List<string>();

        readonly Regex _regexGetSafeFunction = new Regex(@"function\s+GetSafe\s*\([^,]+,[^,]+\)\s*:\s*String",
                                                         RegexOptions.IgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="JScriptAssemblyCreator"/> class.
        /// </summary>
        public JScriptAssemblyCreator()
        {
            RequireGetSafeFunction = true;
        }

        /// <summary>
        /// Gets or sets the name of the class to generate. This value must be set before calling Compile().
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets if the JScript GetSafe function is required to be defined. If true, then it will
        /// be added to the generated source code if no GetSafe function is found. Default is true.
        /// </summary>
        public bool RequireGetSafeFunction { get; set; }

        /// <summary>
        /// Adds the default GetSafe JScript function.
        /// </summary>
        protected virtual void AddDefaultGetSafeFunction()
        {
            AddMethod("GetSafe", "public", "String", "p, i : int", "return p.Length > i ? p[i] : \"<Param Missing>\";");
        }

        /// <summary>
        /// Adds a method for a message script.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="messageScript">The raw message script.</param>
        public void AddMessageScriptMethod(string name, string messageScript)
        {
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
        /// Adds raw code to the generated code on the member level.
        /// </summary>
        /// <param name="memberCode">The code to add.</param>
        public void AddRawMember(string memberCode)
        {
            _members.Insert(0, memberCode);
        }

        /// <summary>
        /// Compiles the source code.
        /// </summary>
        /// <returns>A <see cref="AssemblyClassInvoker"/> to invoke the compiled <see cref="Assembly"/>.</returns>
        public AssemblyClassInvoker Compile()
        {
            Assembly asm;
            return Compile(out asm);
        }

        /// <summary>
        /// Compiles the source code.
        /// </summary>
        /// <param name="asm">The created <see cref="Assembly"/>.</param>
        /// <returns>A <see cref="AssemblyClassInvoker"/> to invoke the compiled <see cref="Assembly"/>, or
        /// null if the compilation failed.</returns>
        public virtual AssemblyClassInvoker Compile(out Assembly asm)
        {
            var sourceCode = GetSourceCode(_members);

            asm = _scriptAssemblyCache.CreateInCache(sourceCode, x => CompileSourceToAssembly(sourceCode, x));

            if (asm == null)
                return null;

            return CreateAssemblyClassInvoker(asm, ClassName);
        }

        /// <summary>
        /// Performs the actual compiling of the <see cref="Assembly"/>. Will be called by the
        /// <see cref="ScriptAssemblyCache"/> if the source didn't exist in the cache.
        /// </summary>
        /// <param name="sourceCode">The source code to compile.</param>
        /// <param name="filePath">The file path to give the generated <see cref="Assembly"/>.</param>
        /// <returns>The generated <see cref="Assembly"/>. Can be null if there was any errors generating it.</returns>
        protected virtual Assembly CompileSourceToAssembly(string sourceCode, string filePath)
        {
            var provider = new JScriptCodeProvider();
            
            // Set up the compiler parameters
            var p = new CompilerParameters
            { GenerateInMemory = false, IncludeDebugInformation = false, OutputAssembly = filePath };
            
            // Compile
            var results = provider.CompileAssemblyFromSource(p, sourceCode);

            // Store the compilation errors
            if (results.Errors.Count > 0)
                _compilationErrors = results.Errors.OfType<CompilerError>().ToImmutable();
            else
                _compilationErrors = _emptyCompilerErrors;

            // Return the compiled assembly
            return results.CompiledAssembly;
        }

        static readonly CompilerError[] _emptyCompilerErrors = new CompilerError[0];

        IEnumerable<CompilerError> _compilationErrors = _emptyCompilerErrors;

        /// <summary>
        /// Gets the <see cref="CompilerError"/>s that resulted from the last compilation. Will be empty if the
        /// <see cref="Assembly"/> has not yet been compiled, if the <see cref="Assembly"/> was loaded from cache
        /// instead of compiling, or if the compilation completed without error.
        /// </summary>
        public IEnumerable<CompilerError> CompilationErrors { get { return _compilationErrors; } }

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
            StringBuilder sb = new StringBuilder(1024);

            // Class header
            sb.AppendLine("class " + ClassName);
            sb.AppendLine("{");

            // Check for the GetSafe function
            if (RequireGetSafeFunction)
            {
                if (!members.Any(x => _regexGetSafeFunction.IsMatch(x)))
                    AddDefaultGetSafeFunction();
            }

            // Add the members
            foreach (var member in members)
            {
                sb.AppendLine(member);
            }

            // Close the class
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