using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Attribute used to mark a method has being the method for a command parsed by the SayHandler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class SayCommandAttribute : Attribute
    {
        /// <summary>
        /// Name of the command handled by this SayCommandAttribute.
        /// </summary>
        public readonly string Command;

        /// <summary>
        /// If the method attached to this SayCommandAttribute is thread-safe or not.
        /// </summary>
        public readonly bool ThreadSafe;

        /// <summary>
        /// SayCommandAttribute constructor.
        /// </summary>
        /// <param name="command">Name of the command handled by this SayCommandAttribute.</param>
        public SayCommandAttribute(string command) : this(command, false)
        {
        }

        /// <summary>
        /// SayCommandAttribute constructor.
        /// </summary>
        /// <param name="command">Name of the command handled by this SayCommandAttribute.</param>
        /// <param name="threadSafe">If the method attached to this SayCommandAttribute is thread-safe or not. Default
        /// is false.</param>
        public SayCommandAttribute(string command, bool threadSafe)
        {
            Command = command;
            ThreadSafe = threadSafe;
        }
    }
}