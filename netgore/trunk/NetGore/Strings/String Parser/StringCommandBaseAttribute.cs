using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Base Attribute for an Attribute used to mark a method that is handled by a StringCommandParser.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class StringCommandBaseAttribute : Attribute
    {
        readonly string _command;

        /// <summary>
        /// StringCommandBaseAttribute constructor.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        protected StringCommandBaseAttribute(string command)
        {
            _command = command;
        }

        /// <summary>
        /// Gets the name of the command, which is also the string entered to invoke the command.
        /// </summary>
        public string Command
        {
            get { return _command; }
        }
    }
}