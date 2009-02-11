using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class SayCommandAttribute : Attribute
    {
        /// <summary>
        /// Name of the command handled by this SayCommandAttribute.
        /// </summary>
        public readonly string Command;

        public SayCommandAttribute(string command)
        {
            Command = command;
        }
    }
}