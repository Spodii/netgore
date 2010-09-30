using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// The base class for tools in the editor.
    /// </summary>
    public abstract class ToolBase : IDisposable
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolBase"/> the event came from.</param>
        public delegate void EventHandler(ToolBase sender);

        /// <summary>
        /// Delegate for handling events from the <see cref="ToolBase"/>.
        /// </summary>
        /// <param name="sender">The <see cref="ToolBase"/> the event came from.</param>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        public delegate void ValueChangedEventHandler<in T>(ToolBase sender, T oldValue, T newValue);

        readonly string _name;
        readonly ToolManager _toolManager;
        bool _canShowInToolbar;

        bool _isDisposed;
        bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolBase"/> class.
        /// </summary>
        /// <param name="name">The name of the tool.</param>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        protected ToolBase(string name, ToolManager toolManager)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (toolManager == null)
                throw new ArgumentNullException("toolManager");

            _name = name;
            _toolManager = toolManager;
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CanShowInToolbar"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler<bool> CanShowInToolbarChanged;

        /// <summary>
        /// Notifies listeners when this object has been disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Notifies listeners when the <see cref="IsEnabled"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler<bool> EnabledChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="ToolbarIcon"/> property has changed.
        /// </summary>
        public event ValueChangedEventHandler<Image> ToolbarIconChanged;

        /// <summary>
        /// Gets or sets if this tool can be shown in the toolbar. This does not mean that the tool will be shown in the toolbar,
        /// just if it is allowed to be.
        /// Default is true.
        /// </summary>
        [DefaultValue(true)]
        public bool CanShowInToolbar
        {
            get { return _canShowInToolbar; }
            set
            {
                if (_canShowInToolbar == value)
                    return;

                _canShowInToolbar = value;

                OnCanShowInToolbarChanged(!CanShowInToolbar, CanShowInToolbar);
                if (CanShowInToolbarChanged != null)
                    CanShowInToolbarChanged(this, !CanShowInToolbar, CanShowInToolbar);
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
        /// Gets if this tool is enabled.
        /// Default is false.
        /// </summary>
        [DefaultValue(false)]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;

                OnEnabledChanged(!IsEnabled, IsEnabled);
                if (EnabledChanged != null)
                    EnabledChanged(this, !IsEnabled, IsEnabled);
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
        /// <see cref="ToolBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~ToolBase()
        {
            _isDisposed = true;
            Dispose(false);
        }

        Image _toolbarIcon;

        /// <summary>
        /// Gets or sets (protected) the <see cref="Image"/> to use when displaying this tool in the toolbar.
        /// </summary>
        public Image ToolbarIcon
        {
            get { return _toolbarIcon; }
            set
            {
                if (_toolbarIcon == value)
                    return;

                var oldValue = _toolbarIcon;
                _toolbarIcon = value;

                OnToolbarIconChanged(oldValue, value);
                if (ToolbarIconChanged != null)
                    ToolbarIconChanged(this, oldValue, value);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="Image"/> to display for this <see cref="ToolBase"/> in
        /// the toolbar.
        /// </summary>
        /// <returns>The <see cref="Image"/> to display in the toolbar, or null if it cannot be added to the toolbar.</returns>
        protected virtual Image GetToolbarIcon()
        {
            return null;
        }

        protected virtual void OnCanShowInToolbarChanged(bool oldValue, bool newValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="ToolBase.EnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected virtual void OnEnabledChanged(bool oldValue, bool newValue)
        {
        }

        protected virtual void OnToolbarIconChanged(Image oldValue, Image newValue)
        {

        }

        /// <summary>
        /// When overridden in the derived class, handles when the tool is clicked in the toolbar.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected virtual void OnToolbarClicked(bool oldValue, bool newValue)
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
        /// <paramref name="enable"/> already equals the current <see cref="ToolBase.IsEnabled"/> state; false if the
        /// state failed to change.</returns>
        public bool TrySetEnabled(bool enable)
        {
            if (enable)
                return TryEnable();
            else
                return TryDisable();
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
    }
}