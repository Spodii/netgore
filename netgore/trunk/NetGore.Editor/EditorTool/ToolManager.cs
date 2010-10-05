using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// The manager for the <see cref="Tool"/>s. Contains an instance of all the available <see cref="Tool"/>s.
    /// </summary>
    public sealed class ToolManager : IDisposable
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolManager"/> the event came from.</param>
        public delegate void EventHandler(ToolManager sender);

        /// <summary>
        /// Delegate for handling events from the <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolManager"/> the event came from.</param>
        /// <param name="tool">The <see cref="Tool"/> that the event relates to.</param>
        public delegate void ToolEventHandler(ToolManager sender, Tool tool);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How frequently to auto-save the tool settings.
        /// Default is every 3 minutes.
        /// </summary>
        const int _autoSaveSettingsRate = 1000 * 60 * 3;

        static readonly ToolManager _instance;
        readonly Timer _autoSaveSettingsTimer;

        readonly MapDrawingExtensionCollection _mapDrawingExtensions = new MapDrawingExtensionCollection();
        readonly ToolStateManager _toolState = new ToolStateManager();
        readonly Dictionary<Type, Tool> _tools = new Dictionary<Type, Tool>();

        bool _isDisposed;
        string _toolSettingsProfileName;

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
            // Create the type filter
            var tfc = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = new Type[] { typeof(ToolManager) },
                Subclass = typeof(Tool),
            };

            var filter = tfc.GetFilter();

            // Create the type factory to load the tools
            new TypeFactory(filter, typeFactory_TypeLoaded);

            // Change to the generic "user" profile
            ToolSettingsProfileName = "User";

            // Create the auto-saver
            _autoSaveSettingsTimer = new Timer(AutoSaveSettingsTimerCallback, _toolState, _autoSaveSettingsRate,
                                               _autoSaveSettingsRate);
        }

        /// <summary>
        /// Notifies listeners when a <see cref="Tool"/> instance has been added to this <see cref="ToolManager"/>.
        /// </summary>
        public event ToolEventHandler ToolAdded;

        /// <summary>
        /// Notifies listeners when a <see cref="Tool"/> instance has been removed from this <see cref="ToolManager"/>.
        /// </summary>
        public event ToolEventHandler ToolRemoved;

        /// <summary>
        /// Gets all of the enabled <see cref="Tool"/>s.
        /// </summary>
        public IEnumerable<Tool> EnabledTools
        {
            get { return _tools.Values.Where(x => x.IsEnabled && !x.IsDisposed).ToImmutable(); }
        }

        /// <summary>
        /// Gets the <see cref="ToolManager"/> instance.
        /// </summary>
        public static ToolManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        public MapDrawingExtensionCollection MapDrawingExtensions
        {
            get { return _mapDrawingExtensions; }
        }

        /// <summary>
        /// Gets or sets the name of the profile to use for the tool settings.
        /// </summary>
        public string ToolSettingsProfileName
        {
            get { return _toolSettingsProfileName; }
            set
            {
                if (StringComparer.Ordinal.Equals(_toolSettingsProfileName, value))
                    return;

                _toolSettingsProfileName = value;

                _toolState.CurrentSettingsFile = ToolStateManager.GetFilePath(ContentPaths.Build, _toolSettingsProfileName);
            }
        }

        /// <summary>
        /// Gets all of the <see cref="Tool"/>s.
        /// </summary>
        public IEnumerable<Tool> Tools
        {
            get { return _tools.Values.ToImmutable(); }
        }

        /// <summary>
        /// Callback for the <see cref="_autoSaveSettingsTimer"/>.
        /// </summary>
        /// <param name="state">The state object, which is the <see cref="ToolStateManager"/> to save.</param>
        static void AutoSaveSettingsTimerCallback(object state)
        {
            // Get the state object as a ToolSettingsManager
            var tsm = state as ToolStateManager;
            if (tsm == null)
            {
                const string errmsg = "Expected state to be a ToolSettingsManager instance, but was `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, state);
                Debug.Fail(string.Format(errmsg, state));
                return;
            }

            // Try to auto-save
            try
            {
                tsm.Save();
            }
            catch (Exception ex)
            {
                const string errmsg = "Error occured while auto-saving the ToolSettingsManager `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, tsm, ex);
                Debug.Fail(string.Format(errmsg, tsm, ex));
                return;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.</param>
        void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                _autoSaveSettingsTimer.Dispose();
                _toolState.Save();
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ToolManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~ToolManager()
        {
            _isDisposed = true;

            Dispose(false);
        }

        /// <summary>
        /// Forces the settings for the <see cref="Tool"/>s to be saved.
        /// </summary>
        public void SaveSettings()
        {
            _toolState.Save();
        }

        /// <summary>
        /// Sets the event listeners for a <see cref="Tool"/>.
        /// </summary>
        /// <param name="tool">The <see cref="Tool"/> to set the events on.</param>
        /// <param name="add">True if the events should be added; false if they should be removed.</param>
        void SetToolListeners(Tool tool, bool add)
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
        /// Tries to get a <see cref="Tool"/> from its <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type of the tool to get.</param>
        /// <returns>The tool of the given <paramref name="type"/>, or null if not found.</returns>
        public Tool TryGetTool(Type type)
        {
            Tool ret;
            if (!_tools.TryGetValue(type, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Tries to get a <see cref="Tool"/> from its type.
        /// </summary>
        /// <typeparam name="T">The type of the tool to get.</typeparam>
        /// <returns>The tool of type <typeparamref name="T"/>, or null if not found.</returns>
        public T TryGetTool<T>() where T : Tool
        {
            var ret = TryGetTool(typeof(T));
            Debug.Assert(ret is T);
            return ret as T;
        }

        /// <summary>
        /// Updates the <see cref="ToolManager"/> and the tools in it.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            foreach (var tool in _tools.Values)
            {
                if (tool.IsEnabled)
                    tool.Update(currentTime);
            }
        }

        /// <summary>
        /// Handles the <see cref="Tool.Disposed"/> event of a <see cref="Tool"/> in this <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
        void tool_Disposed(Tool sender)
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

                if (wasRemoved)
                {
                    if (ToolRemoved != null)
                        ToolRemoved(this, sender);
                }

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

        /// <summary>
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="typeFactory"><see cref="TypeFactory"/> that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        void typeFactory_TypeLoaded(TypeFactory typeFactory, Type loadedType, string name)
        {
            try
            {
                // Instantiate the type
                Tool tool;
                try
                {
                    tool = (Tool)TypeFactory.GetTypeInstance(loadedType, this);
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

                // Add to the settings manager
                _toolState.Add(tool);

                // Notify listeners
                if (ToolAdded != null)
                    ToolAdded(this, tool);
            }
            catch (Exception ex)
            {
                const string errmsg = "Unexpected error while trying to load tool from type `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, loadedType, ex);
                Debug.Fail(string.Format(errmsg, loadedType, ex));
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            GC.SuppressFinalize(this);

            _isDisposed = true;

            Dispose(true);
        }

        #endregion
    }
}