using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using NetGore.Collections;

namespace NetGore.Features.ActionDisplays
{
    /// <summary>
    /// Contains all of the <see cref="ActionDisplayScriptHandler"/> delegate instances for methods that properly implement
    /// the <see cref="ActionDisplayScriptAttribute"/>. This class grabs all attributes from all loaded assemblies, and only
    /// includes methods from classes that implement the <see cref="ActionDisplayScriptCollectionAttribute"/>. This class
    /// should always be used when wanting to get the delegate to invoke a script.
    /// </summary>
    public static class ActionDisplayScriptManager
    {
        static readonly Dictionary<string, ActionDisplayScriptHandler> _scriptHandlers =
            new Dictionary<string, ActionDisplayScriptHandler>(StringComparer.Ordinal);

        /// <summary>
        /// Initializes the <see cref="ActionDisplayScriptManager"/> class.
        /// </summary>
        static ActionDisplayScriptManager()
        {
            new TypeFactory(x => !x.GetCustomAttributes(typeof(ActionDisplayScriptCollectionAttribute), true).IsEmpty(), TypeLoadedHandler);
        }

        /// <summary>
        /// Gets the <see cref="ActionDisplayScriptHandler"/> for the given script name.
        /// </summary>
        /// <param name="scriptName">The name of the script.</param>
        /// <returns>The <see cref="ActionDisplayScriptHandler"/> for the given <paramref name="scriptName"/>, or null
        /// if no such script exists.</returns>
        public static ActionDisplayScriptHandler GetHandler(string scriptName)
        {
            ActionDisplayScriptHandler ret;
            if (_scriptHandlers.TryGetValue(scriptName, out ret))
                return ret;

            return null;
        }

        /// <summary>
        /// Handles when a type is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ActionDisplayScriptHandlers")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ActionDisplayScriptHandler")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ActionDisplayScriptAttributes")]
        static void TypeLoadedHandler(TypeFactory sender, TypeFactoryLoadedEventArgs e)
        {
            var atbType = typeof(ActionDisplayScriptAttribute);
            var mpdType = typeof(ActionDisplayScriptHandler);

            const BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static;

            // Search through every method in the class
            foreach (var method in e.LoadedType.GetMethods(bindFlags))
            {
                // Get all of the ActionDisplayScriptAttributes for the method (should only be one)
                var atbs = (ActionDisplayScriptAttribute[])method.GetCustomAttributes(atbType, true);
                if (atbs.Length > 1)
                {
                    const string errmsg = "Multiple ActionDisplayScriptAttributes found for method `{0}`.";
                    Debug.Fail(string.Format(errmsg, method.Name));
                    throw new TypeException(string.Format(errmsg, method.Name));
                }

                // Create the delegates for the methods
                foreach (var atb in atbs)
                {
                    ActionDisplayScriptHandler del;
                    try
                    {
                        del = (ActionDisplayScriptHandler)Delegate.CreateDelegate(mpdType, null, method, true);
                        if (del == null)
                        {
                            const string errmsg = "Failed to create ActionDisplayScriptHandler delegate for `{0}`.";
                            throw new TypeException(string.Format(errmsg, atb));
                        }
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to create ActionDisplayScriptHandler delegate for method `{0}`. Make sure it is a static method and contains the correct parameters.";
                        Debug.Fail(string.Format(errmsg, method));
                        throw new InstantiateTypeException(string.Format(errmsg, method), ex);
                    }

                    // Ensure the name is unique
                    if (_scriptHandlers.ContainsKey(atb.Name))
                    {
                        const string errmsg = "Found multiple ActionDisplayScriptHandlers with the name `{0}`. Names must be unique.";
                        Debug.Fail(string.Format(errmsg, atb.Name));
                        throw new InstantiateTypeException(string.Format(errmsg, atb.Name), mpdType);
                    }

                    _scriptHandlers.Add(atb.Name, del);
                }
            }
        }
    }
}