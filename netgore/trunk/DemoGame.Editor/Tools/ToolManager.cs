using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Editor.Tools
{
    public class ToolManager
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolManager"/> the event came from.</param>
        public delegate void EventHandler(ToolManager sender);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly ToolManager _instance;

        readonly Dictionary<Type, ToolBase> _tools = new Dictionary<Type, ToolBase>();

        /// <summary>
        /// Initializes the <see cref="ToolManager"/> class.
        /// </summary>
        static ToolManager()
        {
            _instance = new ToolManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolManager"/> class.
        /// </summary>
        ToolManager()
        {
            var tfc = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = new Type[] { typeof(ToolManager) },
                Subclass = typeof(ToolBase),
            };

            var filter = tfc.GetFilter();

            new TypeFactory(filter, TypeFactory_TypeLoaded);
        }

        /// <summary>
        /// Gets the <see cref="ToolManager"/> instance.
        /// </summary>
        public static ToolManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets all tools available.
        /// </summary>
        public IEnumerable<ToolBase> Tools
        {
            get { return _tools.Values; }
        }

        /// <summary>
        /// Sets the event listeners for a <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="tool">The <see cref="ToolBase"/> to set the events on.</param>
        /// <param name="add">True if the events should be added; false if they should be removed.</param>
        void SetToolListeners(ToolBase tool, bool add)
        {
            if (add)
            {
                // When adding the events, always remove them first just to make sure that we do not add the event listeners
                // twice. There is no harm in removing an event that is not there, but this is harm if we get invoked twice for
                // the same event.
                SetToolListeners(tool, false);

                // Add the events
                tool.Disposed += tool_Disposed;
            }
            else
            {
                // Remove the events
                tool.Disposed -= tool_Disposed;
            }
        }

        /// <summary>
        /// Tries to get a <see cref="ToolBase"/> from its <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type of the tool to get.</param>
        /// <returns>The tool of the given <paramref name="type"/>, or null if not found.</returns>
        public ToolBase TryGetTool(Type type)
        {
            ToolBase ret;
            if (!_tools.TryGetValue(type, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Tries to get a <see cref="ToolBase"/> from its type.
        /// </summary>
        /// <typeparam name="T">The type of the tool to get.</typeparam>
        /// <returns>The tool of type <typeparamref name="T"/>, or null if not found.</returns>
        public T TryGetTool<T>() where T : ToolBase
        {
            var ret = TryGetTool(typeof(T));
            Debug.Assert(ret is T);
            return ret as T;
        }

        /// <summary>
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="typeFactory"><see cref="TypeFactory"/> that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        void TypeFactory_TypeLoaded(TypeFactory typeFactory, Type loadedType, string name)
        {
            try
            {
                // Instantiate the type
                ToolBase tool;
                try
                {
                    tool = (ToolBase)TypeFactory.GetTypeInstance(loadedType);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to instantiate tool type `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, loadedType, ex);
                    Debug.Fail(string.Format(errmsg, loadedType, ex));
                    return;
                }

                Debug.Assert(tool.GetType() == loadedType);

                // Add the event hooks
                SetToolListeners(tool, true);

                // Add to the collection
                try
                {
                    _tools.Add(loadedType, tool);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to add tool `{0}` (type: {1}) to collection. Exception: {2}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, tool, tool.GetType(), ex);
                    Debug.Fail(string.Format(errmsg, tool, tool.GetType(), ex));

                    tool.Dispose();

                    return;
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Unexpected error while trying to load tool from type `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, loadedType, ex);
                Debug.Fail(string.Format(errmsg, loadedType, ex));
            }
        }

        /// <summary>
        /// Handles the <see cref="ToolBase.Disposed"/> event of a <see cref="ToolBase"/> in this <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolBase"/> the event came from.</param>
        void tool_Disposed(ToolBase sender)
        {
            if (sender == null)
            {
                Debug.Fail("How the hell was the sender null?");
                return;
            }

            try
            {
                // Remove the event hooks
                SetToolListeners(sender, false);

                // Remove the tool
                var wasRemoved = _tools.Remove(sender.GetType());
                Debug.Assert(wasRemoved);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to remove tool `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, sender, ex);
                Debug.Fail(string.Format(errmsg, sender, ex));
            }
        }
    }
}