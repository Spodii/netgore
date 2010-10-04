using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Editor
{
    /// <summary>
    /// The base class for tools in the editor. <see cref="Tool"/>s are the primary component in the editor for expanding on the
    /// functions and displays of the editor. They can be global to the whole editor, or valid only to specific screens. They can
    /// be shown in a <see cref="ToolBar"/>, but can also be completely invisible to the user interface and function purely in the
    /// background.
    /// </summary>
    public abstract class Tool : IDisposable, IPersistable
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="Tool"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
        public delegate void EventHandler(Tool sender);

        /// <summary>
        /// Delegate for handling events from the <see cref="Tool"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Tool"/> the event came from.</param>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        public delegate void ValueChangedEventHandler<in T>(Tool sender, T oldValue, T newValue);

        readonly IEnumerable<IMapDrawingExtension> _mapDrawingExtensions;

        readonly string _name;
        readonly IToolBarControl _toolBarControl;
        readonly ToolBarVisibility _toolBarVisibility;
        readonly ToolManager _toolManager;

        bool _isDisposed;
        bool _isEnabled;

        const bool _defaultIsEnabled = true;
        const bool _defaultIsOnToolBar = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="name">The name of the tool.</param>
        /// <param name="toolBarControlType">The <see cref="ToolBarControlType"/> to use for displaying this <see cref="Tool"/>
        /// in a toolbar.</param>
        /// <param name="toolBarVisibility">The visibility of this <see cref="Tool"/> in a <see cref="ToolBar"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="toolBarControlType"/> does not contain a defined value of the
        /// <see cref="ToolBarControlType"/> enum.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="toolBarVisibility"/> does not contain a defined value of the
        /// <see cref="ToolBarVisibility"/> enum.</exception>
        protected Tool(ToolManager toolManager, string name, ToolBarControlType toolBarControlType,
                       ToolBarVisibility toolBarVisibility)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (toolManager == null)
                throw new ArgumentNullException("toolManager");
            if (!EnumHelper<ToolBarControlType>.IsDefined(toolBarControlType))
                throw new ArgumentOutOfRangeException("toolBarControlType");
            if (!EnumHelper<ToolBarVisibility>.IsDefined(toolBarVisibility))
                throw new ArgumentOutOfRangeException("toolBarVisibility");

            _name = name;
            _toolManager = toolManager;
            _toolBarVisibility = toolBarVisibility;
            _toolBarControl = ToolBar.CreateToolControl(this, toolBarControlType);

            var exts = GetMapDrawingExtensions();
            if (exts == null || exts.IsEmpty())
                _mapDrawingExtensions = Enumerable.Empty<IMapDrawingExtension>();
            else
                _mapDrawingExtensions = exts.ToImmutable();
        }

        /// <summary>
        /// Notifies listeners when this object has been disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Notifies listeners when the <see cref="IsEnabled"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler<bool> IsEnabledChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IsOnToolBar"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler<bool> IsOnToolBarChanged;

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
        [DefaultValue(_defaultIsEnabled)]
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
                    IsEnabledChanged(this, !IsEnabled, IsEnabled);

                // Internal handling
                HandleEnabledChangedInternal();
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling resetting the default values. The overriding method should
        /// set the properties specific to the derived class, then call this base method. The parameters passed to the base method
        /// can just be repeated from the parameters passed to the overridden method to use the default behavior, or can be altered
        /// to provide custom values. For simplicity, all default values should be constant, no matter the current state.
        /// </summary>
        /// <param name="isEnabled">The default value for <see cref="Tool.IsEnabled"/>.</param>
        /// <param name="isOnToolBar">The default value for <see cref="Tool.IsOnToolBar"/>.</param>
        protected virtual void HandleResetValues(bool isEnabled, bool isOnToolBar)
        {
            IsEnabled = isEnabled;
            IsOnToolBar = isOnToolBar;
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Resets all of the values of the <see cref="Tool"/> back to the default value.
        /// </summary>
        public void ResetValues()
        {
            try
            {
                HandleResetValues(_defaultIsEnabled, _defaultIsOnToolBar);
            }
            catch (Exception ex)
            {
                const string errmsg = "Error while trying to reset values for tool `{0}`." + 
                    " Check HandleResetValues() on the derived class ({1}). Exception: {2}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this, GetType().FullName, ex);
                Debug.Fail(string.Format(errmsg, this, GetType().FullName, ex));
            }
        }

        /// <summary>
        /// Gets or sets if this tool is currently on the <see cref="ToolBar"/>. If <see cref="CanShowInToolbar"/> is false,
        /// then this property will always be false.
        /// Default value is true, but can be altered by the derived class to default to false.
        /// </summary>
        [DefaultValue(_defaultIsOnToolBar)]
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
                        IsOnToolBarChanged(this, oldValue, newValue);
                }
            }
        }

        /// <summary>
        /// Gets the name of the tool. While it is recommended that a tool's name is unique, it is not required.
        /// This property is immutable.
        /// </summary>
        public string Name
        {
            get { return _name; }
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
            get { return _toolBarVisibility; }
        }

        /// <summary>
        /// Gets the <see cref="ToolManager"/> that this tool is in.
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
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
        /// When overridden in the derived class, gets the <see cref="IMapDrawingExtension"/>s that are used by this
        /// <see cref="Tool"/>.
        /// </summary>
        /// <returns>The <see cref="IMapDrawingExtension"/>s used by this <see cref="Tool"/>. Can be null or empty if none
        /// are used. Default is null.</returns>
        protected virtual IEnumerable<IMapDrawingExtension> GetMapDrawingExtensions()
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing after the GUI for a <see cref="MapBase"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="MapBase"/> being drawn.</param>
        protected virtual void HandleAfterDrawMapGUI(ISpriteBatch spriteBatch, MapBase map)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="MapBase"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="MapBase"/> being drawn.</param>
        protected virtual void HandleBeforeDrawMapGUI(ISpriteBatch spriteBatch, MapBase map)
        {
        }

        /// <summary>
        /// Internally handles when <see cref="IsEnabled"/> has changed.
        /// </summary>
        void HandleEnabledChangedInternal()
        {
            // Ensure the update hook is removed
            GlobalState.Instance.Tick -= tickCallback;

            // Add the update hook if we became enabled (since we just removed it, we know we won't have it added twice)
            if (IsEnabled)
                GlobalState.Instance.Tick += tickCallback;

            // If we were disabled, remove the MapDrawingExtensions. Otherwise, add them.
            Debug.Assert(_mapDrawingExtensions != null,
                         "IsEnabled seems to have changed before the _mapDrawingExtensions were set.");
            if (IsEnabled)
                ToolManager.MapDrawingExtensions.Add(_mapDrawingExtensions);
            else
                ToolManager.MapDrawingExtensions.Remove(_mapDrawingExtensions);
        }

        /// <summary>
        /// When overridden in the derived class, handles updating the <see cref="Tool"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        protected virtual void HandleUpdate(TickCount currentTime)
        {
        }

        /// <summary>
        /// Notifies the <see cref="Tool"/> that after a <see cref="MapBase"/>'s GUI was drawn.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="MapBase"/> being drawn.</param>
        public void InvokeAfterDrawMapGUI(ISpriteBatch spriteBatch, MapBase map)
        {
            if (!IsEnabled || IsDisposed)
                return;

            HandleAfterDrawMapGUI(spriteBatch, map);
        }

        /// <summary>
        /// Notifies the <see cref="Tool"/> that before a <see cref="MapBase"/>'s GUI was drawn.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="MapBase"/> being drawn.</param>
        public void InvokeBeforeDrawMapGUI(ISpriteBatch spriteBatch, MapBase map)
        {
            if (!IsEnabled || IsDisposed)
                return;

            HandleBeforeDrawMapGUI(spriteBatch, map);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="IsEnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected virtual void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
        }

        protected virtual void OnIsOnToolBarChanged(bool oldValue, bool newValue)
        {
        }

        protected virtual void OnToolBarPriorityChanged(int oldValue, int newValue)
        {
        }

        protected virtual void OnToolbarIconChanged(Image oldValue, Image newValue)
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
        /// Tries to disable this tool.
        /// </summary>
        /// <returns>True if the tool was successfully disabled or was already disabled; otherwise false.</returns>
        public bool TryDisable()
        {
            if (!IsEnabled)
                return true;

            if (!CanDisable())
                return false;

            IsEnabled = false;

            return true;
        }

        /// <summary>
        /// Tries to enable this tool.
        /// </summary>
        /// <returns>True if the tool was successfully enabled or was already enabled; otherwise false.</returns>
        public bool TryEnable()
        {
            if (IsEnabled)
                return true;

            if (!CanEnable())
                return false;

            IsEnabled = true;

            return true;
        }

        /// <summary>
        /// Tries to set the enabled state of this tool.
        /// </summary>
        /// <param name="enable">When true, try to set to enabled. When false, try to set to disabled.</param>
        /// <returns>True if the tool's enabled state was changed to the value given by <paramref name="enable"/> or
        /// <paramref name="enable"/> already equals the current <see cref="Tool.IsEnabled"/> state; false if the
        /// state failed to change.</returns>
        public bool TrySetEnabled(bool enable)
        {
            if (enable)
                return TryEnable();
            else
                return TryDisable();
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

        /// <summary>
        /// Handles the <see cref="GlobalState.Tick"/> event.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void tickCallback(TickCount currentTime)
        {
            Debug.Assert(IsEnabled);

            HandleUpdate(currentTime);
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
                Disposed(this);
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