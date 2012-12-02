using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using NetGore.Graphics;
using NetGore.IO;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// The base class for tools in the editor. <see cref="Tool"/>s are the primary component in the editor for expanding on the
    /// functions and displays of the editor. They can be global to the whole editor, or valid only to specific screens. They can
    /// be shown in a <see cref="ToolBar"/>, but can also be completely invisible to the user interface and function purely in the
    /// background.
    /// </summary>
    public abstract class Tool : IDisposable, IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal const bool defaultIsEnabled = true;
        internal const bool defaultIsOnToolBar = true;

        readonly ToolSettings _settings;
        readonly IToolBarControl _toolBarControl;
        readonly ToolManager _toolManager;

        bool _isDisposed;
        bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="settings">The <see cref="ToolSettings"/> to use to create this <see cref="Tool"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected Tool(ToolManager toolManager, ToolSettings settings)
        {
            if (toolManager == null)
                throw new ArgumentNullException("toolManager");
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.Lock();

            // Set the properties
            _toolManager = toolManager;
            _settings = settings;
            _toolBarControl = ToolBar.CreateToolControl(this, ToolSettings.ToolBarControlType);

            if (ToolBarControl != null)
            {
                // Set the default image
                ToolBarControl.ControlSettings.Image = ToolSettings.DisabledImage ?? ToolSettings.DefaultImage;

                // If an image is set, then default to showing only the image
                if (ToolBarControl.ControlSettings.Image != null)
                    ToolBarControl.ControlSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;

                // Bind help
                if (!string.IsNullOrEmpty(settings.HelpName) && !string.IsNullOrEmpty(settings.HelpWikiPage))
                {
                    EditorHelpManager.Instance.Add((ToolStripItem)ToolBarControl, settings.HelpName, settings.HelpWikiPage);
                }
            }
        }

        /// <summary>
        /// Notifies listeners when this object has been disposed.
        /// </summary>
        public event TypedEventHandler<Tool> Disposed;

        /// <summary>
        /// Notifies listeners when the <see cref="IsEnabled"/> property has changed.
        /// </summary>
        public event TypedEventHandler<Tool, ValueChangedEventArgs<bool>> IsEnabledChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IsOnToolBar"/> property has changed.
        /// </summary>
        public event TypedEventHandler<Tool, ValueChangedEventArgs<bool>> IsOnToolBarChanged;

        /// <summary>
        /// Gets if this tool can be shown in the <see cref="ToolBar"/>. This does not mean that the tool will be shown in the
        /// <see cref="ToolBar"/>, just if it is allowed to be.
        /// </summary>
        public bool CanShowInToolbar
        {
            get
            {
                if (ToolBarControl == null || ToolBarVisibility == ToolBarVisibility.None)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Gets the name of the group that this <see cref="Tool"/> is in for restricting the enabled status of <see cref="Tool"/>s.
        /// When this value is non-null, only one <see cref="Tool"/> from this group will be allowed to be enabled at a time. Enabling
        /// the <see cref="Tool"/> will disable all others in the group. When null, this feature will be disabled.
        /// Default value is null.
        /// </summary>
        [DefaultValue((string)null)]
        public string EnabledToolsGroup
        {
            get { return ToolSettings.EnabledToolsGroup; }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        [DefaultValue(false)]
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets if this tool is enabled. When disabled, this <see cref="Tool"/> will not perform regular updating and drawing.
        /// Default is true.
        /// </summary>
        [DefaultValue(defaultIsEnabled)]
        [SyncValue]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;

                // Raise event
                OnIsEnabledChanged(!IsEnabled, IsEnabled);
                if (IsEnabledChanged != null)
                    IsEnabledChanged.Raise(this, ValueChangedEventArgs.Create(!IsEnabled, IsEnabled));

                // Internal handling
                HandleEnabledChangedInternal();
            }
        }

        /// <summary>
        /// Gets or sets if this tool is currently on the <see cref="ToolBar"/>. If <see cref="CanShowInToolbar"/> is false,
        /// then this property will always be false.
        /// Default value is true, but can be altered by the derived class to default to false.
        /// </summary>
        [DefaultValue(defaultIsOnToolBar)]
        [SyncValue]
        public bool IsOnToolBar
        {
            get
            {
                if (!CanShowInToolbar)
                    return false;

                return ToolBarControl.IsOnToolBar;
            }
            set
            {
                if (!CanShowInToolbar)
                    value = false;

                var oldValue = IsOnToolBar;

                if (value)
                    ToolBar.AddToToolBar(this);
                else
                    ToolBar.RemoveFromToolBar(this);

                var newValue = IsOnToolBar;

                if (oldValue != newValue)
                {
                    OnIsOnToolBarChanged(oldValue, newValue);
                    if (IsOnToolBarChanged != null)
                        IsOnToolBarChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, newValue));
                }
            }
        }

        /// <summary>
        /// Gets the name of the tool. While it is recommended that a tool's name is unique, it is not required.
        /// This property is immutable.
        /// </summary>
        public string Name
        {
            get { return ToolSettings.Name; }
        }

        /// <summary>
        /// Gets the <see cref="IToolBarControl"/> to use for displaying this <see cref="Tool"/> in a <see cref="ToolBar"/>.
        /// This property is set in the <see cref="Tool"/>'s constructor and remains the same throughout the life of the object.
        /// This can be null when the derived class sets the <see cref="ToolBarControlType"/> to
        /// <see cref="ToolBarControlType.None"/>.
        /// </summary>
        public IToolBarControl ToolBarControl
        {
            get { return _toolBarControl; }
        }

        /// <summary>
        /// Gets the visibility of this <see cref="Tool"/> in a <see cref="ToolBar"/>.
        /// </summary>
        public ToolBarVisibility ToolBarVisibility
        {
            get { return ToolSettings.ToolBarVisibility; }
        }

        /// <summary>
        /// Gets the <see cref="ToolManager"/> that this tool is in.
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
        }

        /// <summary>
        /// Gets the <see cref="ToolSettings"/> instance that this <see cref="Tool"/> was created with.
        /// </summary>
        protected ToolSettings ToolSettings
        {
            get { return _settings; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this tool is allowed to be disabled at this time.
        /// </summary>
        /// <returns>True if the tool can be enabled; otherwise false.</returns>
        protected virtual bool CanDisable()
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets if this tool is allowed to be enabled at this time.
        /// </summary>
        /// <returns>True if the tool can be enabled; otherwise false.</returns>
        protected virtual bool CanEnable()
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, handles disposing this object.
        /// </summary>
        /// <param name="disposeManaged">When true, <see cref="IDisposable.Dispose"/> was called directly and managed resources need to be
        /// disposed. When false, this object was garbage collected and managed resources should not be disposed.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Tool"/> is reclaimed by garbage collection.
        /// </summary>
        ~Tool()
        {
            _isDisposed = true;
            Dispose(false);
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing after the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        protected virtual void HandleAfterDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        protected virtual void HandleBeforeDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
        }

        /// <summary>
        /// Internally handles when <see cref="IsEnabled"/> has changed.
        /// </summary>
        void HandleEnabledChangedInternal()
        {
            // If we were disabled, remove the MapDrawingExtensions. Otherwise, add them.
            if (IsEnabled)
                ToolManager.MapDrawingExtensions.Add(ToolSettings.MapDrawingExtensions);
            else
                ToolManager.MapDrawingExtensions.Remove(ToolSettings.MapDrawingExtensions);

            // If we were enabled, and an EnabledToolsGroup is provided, disable all other tools in the group
            if (IsEnabled && EnabledToolsGroup != null)
            {
                var toDisable =
                    ToolManager.Tools.Where(
                        x =>
                        x.IsEnabled && x != this && ToolSettings.GroupNameComparer.Equals(EnabledToolsGroup, x.EnabledToolsGroup));
                foreach (var groupTool in toDisable)
                {
                    groupTool.IsEnabled = false;
                }
            }

            // Update the ToolBarControl image, if possible
            if (ToolBarControl != null)
            {
                if (IsEnabled)
                {
                    // Change to Enabled image
                    if (ToolSettings.EnabledImage != null)
                        ToolBarControl.ControlSettings.Image = ToolSettings.EnabledImage;
                }
                else
                {
                    // Change to Disabled image
                    if (ToolSettings.DisabledImage != null)
                        ToolBarControl.ControlSettings.Image = ToolSettings.DisabledImage;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling resetting the state of this <see cref="Tool"/>.
        /// For simplicity, all default values should be constant, no matter the current state.
        /// </summary>
        protected virtual void HandleResetState()
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles updating the <see cref="Tool"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        protected virtual void HandleUpdate(TickCount currentTime)
        {
        }

        /// <summary>
        /// Notifies the <see cref="Tool"/> after a <see cref="IDrawableMap"/>'s GUI was drawn.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        internal void InvokeAfterDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            if (IsDisposed)
                return;

            HandleAfterDrawMapGUI(spriteBatch, map);
        }

        /// <summary>
        /// Notifies the <see cref="Tool"/> before a <see cref="IDrawableMap"/>'s GUI was drawn.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        internal void InvokeBeforeDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            if (IsDisposed)
                return;

            HandleBeforeDrawMapGUI(spriteBatch, map);
        }

        /// <summary>
        /// Invokes the <see cref="Tool.ToolTargetContainerAdded"/> method.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to pass.</param>
        internal void InvokeToolTargetContainerAdded(IToolTargetContainer c)
        {
            ToolTargetContainerAdded(c);
        }

        /// <summary>
        /// Invokes the <see cref="Tool.ToolTargetContainerRemoved"/> method.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to pass.</param>
        internal void InvokeToolTargetContainerRemoved(IToolTargetContainer c)
        {
            ToolTargetContainerRemoved(c);
        }

        /// <summary>
        /// Updates the <see cref="Tool"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        internal void InvokeUpdate(TickCount currentTime)
        {
            HandleUpdate(currentTime);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="Tool.IsEnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected virtual void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="Tool.IsOnToolBarChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected virtual void OnIsOnToolBarChanged(bool oldValue, bool newValue)
        {
        }

        /// <summary>
        /// Handles reading custom state information for this <see cref="Tool"/> from an <see cref="IValueReader"/> for when
        /// persisting the <see cref="Tool"/>'s state.
        /// When possible, it is preferred that you use the <see cref="SyncValueAttribute"/> instead of manually handling
        /// reading and writing the state.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        protected virtual void ReadCustomToolState(IValueReader reader)
        {
        }

        /// <summary>
        /// Resets all of the state values of the <see cref="Tool"/> back to the default value.
        /// </summary>
        public void ResetState()
        {
            // Reset the state
            try
            {
                IsEnabled = ToolSettings.EnabledByDefault;
                IsOnToolBar = ToolSettings.OnToolBarByDefault;
            }
            catch (Exception ex)
            {
                const string errmsg = "Error while trying to reset values for tool `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, ex);
                Debug.Fail(string.Format(errmsg, this, ex));
            }

            // Reset the state in the derived class
            try
            {
                HandleResetState();
            }
            catch (Exception ex)
            {
                const string errmsg =
                    "Error while trying to reset values for tool `{0}`." +
                    " Check HandleResetValues() on the derived class ({1}). Exception: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, GetType().FullName, ex);
                Debug.Fail(string.Format(errmsg, this, GetType().FullName, ex));
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles setting up event listeners for a <see cref="IToolTargetContainer"/>.
        /// This will be invoked once for every <see cref="Tool"/> instance for every <see cref="IToolTargetContainer"/> available.
        /// When the <see cref="Tool"/> is newly added to the <see cref="ToolManager"/>, all existing <see cref="IToolTargetContainer"/>s
        /// will be sent through this method. As new ones are added while this <see cref="Tool"/> exists, those new
        /// <see cref="IToolTargetContainer"/>s will also be passed through. What events to listen to and on what instances is
        /// purely up to the derived <see cref="Tool"/>.
        /// Make sure that all attached event listeners are also removed in the <see cref="Tool.ToolTargetContainerRemoved"/> method.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected virtual void ToolTargetContainerAdded(IToolTargetContainer c)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles tearing down event listeners for a <see cref="IToolTargetContainer"/>.
        /// Any event listeners set up in <see cref="Tool.ToolTargetContainerAdded"/> should be torn down here.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected virtual void ToolTargetContainerRemoved(IToolTargetContainer c)
        {
        }

        /// <summary>
        /// Handles writing custom state information for this <see cref="Tool"/> to an <see cref="IValueWriter"/> for when
        /// persisting the <see cref="Tool"/>'s state.
        /// When possible, it is preferred that you use the <see cref="SyncValueAttribute"/> instead of manually handling
        /// reading and writing the state.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        protected virtual void WriteCustomToolState(IValueWriter writer)
        {
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

            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);

            ReadCustomToolState(reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);

            WriteCustomToolState(writer);
        }

        #endregion
    }
}