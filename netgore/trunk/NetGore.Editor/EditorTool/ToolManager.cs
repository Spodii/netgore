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
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// How frequently to auto-save the tool settings.
        /// Default is every 3 minutes.
        /// </summary>
        const int _autoSaveSettingsRate = 1000 * 60 * 3;

        static readonly ToolManager _instance;

        readonly Timer _autoSaveSettingsTimer;
        readonly ToolTargetContainerCollection _containers = new ToolTargetContainerCollection();
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

            // Listen for containers being added and removed
            ToolTargetContainers.Added -= ToolTargetContainers_Added;
            ToolTargetContainers.Added += ToolTargetContainers_Added;

            ToolTargetContainers.Removed -= ToolTargetContainers_Removed;
            ToolTargetContainers.Removed += ToolTargetContainers_Removed;

            // Load the default state settings
            _toolState.CurrentSettingsFile = ToolStateManager.GetFilePath(ContentPaths.Build, null);

            // Change to the generic "user" profile
            ToolSettingsProfileName = "User";

            // Create the auto-saver
            _autoSaveSettingsTimer = new Timer(AutoSaveSettingsTimerCallback, _toolState, _autoSaveSettingsRate, _autoSaveSettingsRate);
        }

        /// <summary>
        /// Notifies listeners when a <see cref="Tool"/> instance has been added to this <see cref="ToolManager"/>.
        /// </summary>
        public event TypedEventHandler<ToolManager, EventArgs<Tool>> ToolAdded;

        /// <summary>
        /// Notifies listeners when a <see cref="Tool"/> instance has been removed from this <see cref="ToolManager"/>.
        /// </summary>
        public event TypedEventHandler<ToolManager, EventArgs<Tool>> ToolRemoved;

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

        /// <summary>
        /// Gets the <see cref="MapDrawingExtensionCollection"/> used by this <see cref="ToolManager"/> to manage the
        /// <see cref="IMapDrawingExtension"/>s implemented by the <see cref="Tool"/>s in this <see cref="ToolManager"/>.
        /// </summary>
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
        /// Gets the <see cref="ToolTargetContainerCollection"/> that holds the <see cref="IToolTargetContainer"/>s to be used
        /// by the <see cref="Tool"/>s in this <see cref="ToolManager"/>.
        /// </summary>
        public ToolTargetContainerCollection ToolTargetContainers
        {
            get { return _containers; }
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
            // Try to cast to the desired type
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
        /// Notifies the <see cref="Tool"/>s in this <see cref="ToolManager"/> about the corresponding event.
        /// It is the responsibility of the application to make sure this method is called at the appropriate time.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/>.</param>
        /// <param name="map">The <see cref="IDrawableMap"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="spriteBatch"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> is null.</exception>
        public void InvokeAfterDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            if (map == null)
                throw new ArgumentNullException("map");

            foreach (var t in Tools)
            {
                t.InvokeAfterDrawMapGUI(spriteBatch, map);
            }
        }

        /// <summary>
        /// Notifies the <see cref="Tool"/>s in this <see cref="ToolManager"/> about the corresponding event.
        /// It is the responsibility of the application to make sure this method is called at the appropriate time.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/>.</param>
        /// <param name="map">The <see cref="IDrawableMap"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="spriteBatch"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> is null.</exception>
        public void InvokeBeforeDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            if (map == null)
                throw new ArgumentNullException("map");

            foreach (var t in Tools)
            {
                t.InvokeBeforeDrawMapGUI(spriteBatch, map);
            }
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
                tool.Disposed -= tool_Disposed;
                tool.Disposed += tool_Disposed;
            }
            else
            {
                // Remove the events
                tool.Disposed -= tool_Disposed;
            }
        }

        /// <summary>
        /// Handles the <see cref="ToolTargetContainerCollection.Added"/> event for the <see cref="_containers"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="c">The <see cref="NetGore.EventArgs{IToolTargetContainer}"/> instance containing the event data.</param>
        void ToolTargetContainers_Added(ToolTargetContainerCollection sender, EventArgs<IToolTargetContainer> c)
        {
            // Notify all tools that it was added
            foreach (var tool in Tools)
            {
                try
                {
                    tool.InvokeToolTargetContainerAdded(c.Item1);
                }
                catch (Exception ex)
                {
                    // When there is an error, log it but move on to the next tool
                    const string errmsg = "Error while adding container `{0}` to tool `{1}` in ToolManager `{2}`. Exception: {3}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, c, tool, this, ex);
                    Debug.Fail(string.Format(errmsg, c, tool, this, ex));
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="ToolTargetContainerCollection.Removed"/> event for the <see cref="_containers"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="c">The <see cref="IToolTargetContainer"/> that was removed.</param>
        void ToolTargetContainers_Removed(ToolTargetContainerCollection sender, EventArgs<IToolTargetContainer> c)
        {
            // Notify all tools that it was removed
            foreach (var tool in Tools)
            {
                try
                {
                    tool.InvokeToolTargetContainerRemoved(c.Item1);
                }
                catch (Exception ex)
                {
                    // When there is an error, log it but move on to the next tool
                    const string errmsg =
                        "Error while removing container `{0}` from tool `{1}` in ToolManager `{2}`. Exception: {3}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, c, tool, this, ex);
                    Debug.Fail(string.Format(errmsg, c, tool, this, ex));
                }
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
                    tool.InvokeUpdate(currentTime);
            }
        }

        /// <summary>
        /// Handles the <see cref="Tool.Disposed"/> event of a <see cref="Tool"/> in this <see cref="ToolManager"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void tool_Disposed(Tool sender, EventArgs e)
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
                        ToolRemoved.Raise(this, EventArgsHelper.Create(sender));
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
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        void typeFactory_TypeLoaded(TypeFactory sender, TypeFactoryLoadedEventArgs e)
        {
            try
            {
                // Instantiate the type
                Tool tool;
                try
                {
                    tool = (Tool)TypeFactory.GetTypeInstance(e.LoadedType, this);
                }
                catch (Exception ex)
                {
                    const string errmsg = "Failed to instantiate tool type `{0}`. Exception: {1}";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, e.LoadedType, ex);
                    Debug.Fail(string.Format(errmsg, e.LoadedType, ex));
                    return;
                }

                Debug.Assert(tool.GetType() == e.LoadedType);

                // Add the event hooks
                SetToolListeners(tool, true);

                // Add to the collection
                try
                {
                    _tools.Add(e.LoadedType, tool);
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

                // Notify the Tool about all the containers already in this ToolManager
                foreach (var container in ToolTargetContainers)
                {
                    try
                    {
                        tool.InvokeToolTargetContainerAdded(container);
                    }
                    catch (Exception ex)
                    {
                        // When there is an error when adding, log it but move on
                        const string errmsg = "Error when notifying `{0}` about container `{1}`. Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, tool, container, ex);
                        Debug.Fail(string.Format(errmsg, tool, container, ex));
                    }
                }

                // Notify listeners
                if (ToolAdded != null)
                    ToolAdded.Raise(this, EventArgsHelper.Create(tool));
            }
            catch (Exception ex)
            {
                const string errmsg = "Unexpected error while trying to load tool from type `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, e.LoadedType, ex);
                Debug.Fail(string.Format(errmsg, e.LoadedType, ex));
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