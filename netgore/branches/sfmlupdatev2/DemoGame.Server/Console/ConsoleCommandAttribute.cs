using System;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Attribute for a method that handles a console command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ConsoleCommandAttribute : StringCommandBaseAttribute
    {
        public ConsoleCommandAttribute(string command) : base(command)
        {
        }
    }
}