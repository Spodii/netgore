using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages all of the SayCommandCallbacks and provides the ability to find a SayCommandCallback
    /// from a given Say command string.
    /// </summary>
    class SayCommandManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<string, SayCommandCallback> _commands =
            new Dictionary<string, SayCommandCallback>(StringComparer.OrdinalIgnoreCase);

        public SayCommandManager(object source)
        {
            const BindingFlags bindFlags =
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod |
                BindingFlags.Static;

            // Enumerate through all the methods in the passed source object
            var methods = source.GetType().GetMethods(bindFlags);
            foreach (MethodInfo method in methods)
            {
                // Check for our custom attributes
                var attributes = method.GetCustomAttributes(typeof(SayCommandAttribute), true);
                if (attributes.Count() < 1)
                    continue;

                // Enumerate through all the attributes found on the method
                foreach (SayCommandAttribute attribute in attributes.Cast<SayCommandAttribute>())
                {
                    log.InfoFormat("Say command `{0}` being handled by `{1}`", attribute.Command, method.Name);

                    SayCommandCallback del =
                        (SayCommandCallback)Delegate.CreateDelegate(typeof(SayCommandCallback), source, method, true);
                    _commands.Add(attribute.Command, del);
                }
            }
        }

        /// <summary>
        /// Gets the SayCommandCallback for the specified command name.
        /// </summary>
        /// <param name="command">Name of the command.</param>
        /// <returns>SayCommandCallback for the specified command, or null if an invalid command.</returns>
        public SayCommandCallback GetCallback(string command)
        {
            SayCommandCallback ret;
            if (_commands.TryGetValue(command, out ret))
                return ret;

            return null;
        }
    }
}