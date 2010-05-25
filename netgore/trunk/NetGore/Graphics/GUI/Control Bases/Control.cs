using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base class of all controls
    /// </summary>
    public abstract class Control : IDisposable, IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly object _eventBeginDrag = new object();
        static readonly object _eventBorderChanged = new object();
        static readonly object _eventBorderColorChanged = new object();
        static readonly object _eventCanDragChanged = new object();
        static readonly object _eventCanFocusChanged = new object();
        static readonly object _eventClicked = new object();
        static readonly object _eventDisposed = new object();
        static readonly object _eventEnabledChanged = new object();
        static readonly object _eventEndDrag = new object();
        static readonly object _eventFocused = new object();
        static readonly object _eventIsBoundToParentAreaChanged = new object();
        static readonly object _eventKeyPressed = new object();
        static readonly object _eventKeyReleased = new object();
        static readonly object _eventLostFocus = new object();
        static readonly object _eventMouseDown = new object();
        static readonly object _eventMouseEnter = new object();
        static readonly object _eventMouseLeave = new object();
        static readonly object _eventMouseMoved = new object();
        static readonly object _eventMouseUp = new object();
        static readonly object _eventMoved = new object();
        static readonly object _eventResizeToChildrenChanged = new object();
        static readonly object _eventResizeToChildrenPaddingChanged = new object();
        static readonly object _eventResized = new object();
        static readonly object _eventTextEntered = new object();
        static readonly object _eventTooltipChanged = new object();
        static readonly object _eventVisibleChanged = new object();

        readonly List<Control> _controls = new List<Control>(1);
        readonly EventHandlerList _eventHandlerList = new EventHandlerList();
        readonly IGUIManager _gui;
        readonly Control _parent;
        readonly Control _root;

        ControlBorder _border;
        Color _borderColor = Color.White;
        bool _canDrag = true;
        bool _canFocus = true;
        Vector2 _dragOffset;
        bool _includeInResizeToChildren = true;
        bool _isBoundToParentArea = true;
        bool _isDisposed = false;
        bool _isDragging = false;
        bool _isEnabled = true;
        bool _isVisible = true;
        Vector2 _position;
        bool _resizeToChildren;
        byte _resizeToChildrenPadding = 4;
        Queue<Control> _setTopMostQueue = null;
        Vector2 _size;
        TooltipHandler _toolTip;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected Control(Control parent, Vector2 position, Vector2 clientSize)
            : this(parent, parent.GUIManager, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected Control(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : this(null, guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        Control(Control parent, IGUIManager guiManager, Vector2 position, Vector2 clientSize)
        {
            if (guiManager == null)
                throw new ArgumentNullException("guiManager", "GUIManager cannot be null.");

            _gui = guiManager;
            _parent = parent;

            _border = ControlBorder.Empty;
            _position = position;
            _size = clientSize;

            // Get the root
            if (Parent == null)
                _root = this;
            else
                _root = Parent.Root;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Initialize();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            if (Parent != null)
            {
                // Check that the parent isn't disposed
                if (parent._isDisposed)
                    throw new ArgumentException("Parent control is disposed and cannot be used.", "parent");

                // Add the Control to the parent
                Parent._controls.Add(this);
                KeepInParent();

                if (Parent.ResizeToChildren)
                    Parent.UpdateResizeToChildren();
            }
            else
            {
                // This control is the root, so add it directly to the GUI manager
                GUIManager.Add(this);
            }

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            SetDefaultValues();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Notifies listeners when the Control has begun being dragged.
        /// </summary>
        public event ControlEventHandler BeginDrag
        {
            add { Events.AddHandler(_eventBeginDrag, value); }
            remove { Events.RemoveHandler(_eventBeginDrag, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.Border"/> has changed.
        /// </summary>
        public event ControlEventHandler BorderChanged
        {
            add { Events.AddHandler(_eventBorderChanged, value); }
            remove { Events.RemoveHandler(_eventBorderChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.BorderColor"/> has changed.
        /// </summary>
        public event ControlEventHandler BorderColorChanged
        {
            add { Events.AddHandler(_eventBorderColorChanged, value); }
            remove { Events.RemoveHandler(_eventBorderColorChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.CanDrag"/> value of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler CanDragChanged
        {
            add { Events.AddHandler(_eventCanDragChanged, value); }
            remove { Events.RemoveHandler(_eventCanDragChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.CanFocus"/> value of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler CanFocusChanged
        {
            add { Events.AddHandler(_eventCanFocusChanged, value); }
            remove { Events.RemoveHandler(_eventCanFocusChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> was clicked.
        /// </summary>
        public event MouseClickEventHandler Clicked
        {
            add { Events.AddHandler(_eventClicked, value); }
            remove { Events.RemoveHandler(_eventClicked, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has been disposed.
        /// </summary>
        public event ControlEventHandler Disposed
        {
            add { Events.AddHandler(_eventDisposed, value); }
            remove { Events.RemoveHandler(_eventDisposed, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.IsEnabled"/> value of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler EnabledChanged
        {
            add { Events.AddHandler(_eventEnabledChanged, value); }
            remove { Events.RemoveHandler(_eventEnabledChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has ended being dragged.
        /// </summary>
        public event ControlEventHandler EndDrag
        {
            add { Events.AddHandler(_eventEndDrag, value); }
            remove { Events.RemoveHandler(_eventEndDrag, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has gained focus.
        /// </summary>
        public event ControlEventHandler Focused
        {
            add { Events.AddHandler(_eventFocused, value); }
            remove { Events.RemoveHandler(_eventFocused, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.IsBoundToParentArea"/> value of this
        /// <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler IsBoundToParentAreaChanged
        {
            add { Events.AddHandler(_eventIsBoundToParentAreaChanged, value); }
            remove { Events.RemoveHandler(_eventIsBoundToParentAreaChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when a key is being pressed while the <see cref="Control"/> has focus.
        /// </summary>
        public event KeyboardEventHandler KeyPressed
        {
            add { Events.AddHandler(_eventKeyPressed, value); }
            remove { Events.RemoveHandler(_eventKeyPressed, value); }
        }

        /// <summary>
        /// Notifies listeners when a key has been released while the <see cref="Control"/> has focus.
        /// </summary>
        public event KeyboardEventHandler KeyReleased
        {
            add { Events.AddHandler(_eventKeyReleased, value); }
            remove { Events.RemoveHandler(_eventKeyReleased, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control"/> has lost focus.
        /// </summary>
        public event ControlEventHandler LostFocus
        {
            add { Events.AddHandler(_eventLostFocus, value); }
            remove { Events.RemoveHandler(_eventLostFocus, value); }
        }

        /// <summary>
        /// Notifies listeners when a mouse button has been pressed down on this <see cref="Control"/>.
        /// </summary>
        public event MouseClickEventHandler MouseDown
        {
            add { Events.AddHandler(_eventMouseDown, value); }
            remove { Events.RemoveHandler(_eventMouseDown, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has entered the area of the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler MouseEnter
        {
            add { Events.AddHandler(_eventMouseEnter, value); }
            remove { Events.RemoveHandler(_eventMouseEnter, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has left the area of the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler MouseLeave
        {
            add { Events.AddHandler(_eventMouseLeave, value); }
            remove { Events.RemoveHandler(_eventMouseLeave, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has moved over the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler MouseMoved
        {
            add { Events.AddHandler(_eventMouseMoved, value); }
            remove { Events.RemoveHandler(_eventMouseMoved, value); }
        }

        /// <summary>
        /// Notifies listeners when a mouse button has been raised on the <see cref="Control"/>.
        /// </summary>
        public event MouseClickEventHandler MouseUp
        {
            add { Events.AddHandler(_eventMouseUp, value); }
            remove { Events.RemoveHandler(_eventMouseUp, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has moved.
        /// </summary>
        public event ControlEventHandler Moved
        {
            add { Events.AddHandler(_eventMoved, value); }
            remove { Events.RemoveHandler(_eventMoved, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.ResizeToChildren"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler ResizeToChildrenChanged
        {
            add { Events.AddHandler(_eventResizeToChildrenChanged, value); }
            remove { Events.RemoveHandler(_eventResizeToChildrenChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.ResizeToChildrenPadding"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler ResizeToChildrenPaddingChanged
        {
            add { Events.AddHandler(_eventResizeToChildrenPaddingChanged, value); }
            remove { Events.RemoveHandler(_eventResizeToChildrenPaddingChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler Resized
        {
            add { Events.AddHandler(_eventResized, value); }
            remove { Events.RemoveHandler(_eventResized, value); }
        }

        /// <summary>
        /// Notifies listeners text has been entered into this <see cref="Control"/>.
        /// </summary>
        public event TextEnteredEventHandler TextEntered
        {
            add { Events.AddHandler(_eventTextEntered, value); }
            remove { Events.RemoveHandler(_eventTextEntered, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.Tooltip"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler TooltipChanged
        {
            add { Events.AddHandler(_eventTooltipChanged, value); }
            remove { Events.RemoveHandler(_eventTooltipChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.IsVisible"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler VisibleChanged
        {
            add { Events.AddHandler(_eventVisibleChanged, value); }
            remove { Events.RemoveHandler(_eventVisibleChanged, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> used with this control. If set to null,
        /// <see cref="ControlBorder.Empty"/> will be used instead. Will never be null.
        /// </summary>
        public ControlBorder Border
        {
            get
            {
                Debug.Assert(_border != null);
                return _border;
            }
            set
            {
                if (value == null)
                    value = ControlBorder.Empty;

                if (_border == value)
                    return;

                var oldBorderSize = _border != null ? _border.Size : Vector2.Zero;

                _border = value;

                // Resize the control so that the ClientSize remainst he same
                if (_border.Size != oldBorderSize)
                    Size += (_border.Size - oldBorderSize);

                // The border changed, thus the ClientArea probably changed and we need to update the KeepInParent
                for (var i = 0; i < _controls.Count; i++)
                {
                    _controls[i].KeepInParent();
                }

                InvokeBorderChanged();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to use when drawing the <see cref="Border"/>.
        /// </summary>
        [SyncValue]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value)
                    return;

                _borderColor = value;

                InvokeBorderColorChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the Control supports movement by dragging with the mouse
        /// </summary>
        [SyncValue]
        public bool CanDrag
        {
            get { return _canDrag; }
            set
            {
                if (CanDrag == value)
                    return;

                _canDrag = value;

                InvokeCanDragChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the Control can get focus
        /// </summary>
        [SyncValue]
        public bool CanFocus
        {
            get { return _canFocus; }
            set
            {
                if (CanFocus == value)
                    return;

                _canFocus = value;

                InvokeCanFocusChanged();
            }
        }

        /// <summary>
        /// Gets the size of the client area (internal area, not including the borders) of the Control.
        /// Can only be set if <see cref="Control.ResizeToChildren"/> is not set.
        /// </summary>
        [SyncValue]
        public Vector2 ClientSize
        {
            get { return Size - Border.Size; }
            set { Size = value + Border.Size; }
        }

        /// <summary>
        /// Gets the list of Controls that this Control contains. This includes only immediate child Controls
        /// from this Control.
        /// </summary>
        public IEnumerable<Control> Controls
        {
            get { return _controls; }
        }

        /// <summary>
        /// Gets the <see cref="EventHandlerList"/> for this <see cref="Control"/>.
        /// </summary>
        protected EventHandlerList Events
        {
            get { return _eventHandlerList; }
        }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that manages this <see cref="Control"/>.
        /// </summary>
        public IGUIManager GUIManager
        {
            get { return _gui; }
        }

        /// <summary>
        /// Gets if the <see cref="Control"/> currently has focus.
        /// </summary>
        public bool HasFocus
        {
            get { return GUIManager.FocusedControl == this; }
        }

        /// <summary>
        /// Gets or sets if this <see cref="Control"/> will be included when using <see cref="Control.ResizeToChildren"/>. The default
        /// value for this property is true, and should remain true for most all <see cref="Control"/>s. Only times you should set it false
        /// is if you have a <see cref="Control"/> that is technically a child control, but behaves like an addition to the parent. An
        /// example of this is the close button on a <see cref="Form"/>.
        /// </summary>
        public bool IncludeInResizeToChildren
        {
            get { return _includeInResizeToChildren; }
            set { _includeInResizeToChildren = value; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Control"/> is bound to the area of its parent. Only valid if the
        /// <see cref="Control"/> has a parent and if the parent does not have <see cref="Control.ResizeToChildren"/> set.
        /// </summary>
        [SyncValue]
        public bool IsBoundToParentArea
        {
            get
            {
                if (Parent == null)
                    return false;

                if (Parent.ResizeToChildren)
                    return false;

                return _isBoundToParentArea;
            }
            set
            {
                if (_isBoundToParentArea == value)
                    return;

                _isBoundToParentArea = value;

                InvokeIsBoundToParentAreaChanged();
            }
        }

        /// <summary>
        /// Gets if this <see cref="Control"/> has been disposed;
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Control"/> is enabled.
        /// </summary>
        [SyncValue]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;

                InvokeEnabledChanged();
            }
        }

        /// <summary>
        /// Gets if the cursor is currently over this <see cref="Control"/>.
        /// </summary>
        public bool IsMouseEntered
        {
            get { return GUIManager.UnderCursor == this; }
        }

        /// <summary>
        /// Gets if this Control is the root Control.
        /// </summary>
        public bool IsRoot
        {
            get { return Parent == null; }
        }

        /// <summary>
        /// Gets or sets if the Control is visible.
        /// </summary>
        [SyncValue]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;

                InvokeVisibleChanged();

                SetAsTopMost();
            }
        }

        /// <summary>
        /// Gets the parent Control for this Control. If the Control has no parent, this will be null.
        /// </summary>
        public Control Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets or sets the position of this Control reletive to its parent, where point (0,0) represents the top-left
        /// corner of the client area of the parent Control.
        /// </summary>
        [SyncValue]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)
                    return;

                _position = value;

                InvokeMoved();

                KeepInParent();
            }
        }

        /// <summary>
        /// Gets or sets if this <see cref="Control"/>'s size will automatically be set to the size needed to fit
        /// all the child <see cref="Control"/>s.
        /// </summary>
        public bool ResizeToChildren
        {
            get { return _resizeToChildren; }
            set
            {
                if (_resizeToChildren == value)
                    return;

                _resizeToChildren = value;

                InvokeResizeToChildrenChanged();

                if (ResizeToChildren)
                    UpdateResizeToChildren();
            }
        }

        /// <summary>
        /// Gets or sets the amount of pixels to pad to the <see cref="Control.ClientSize"/> when resizing when
        /// <see cref="Control.ResizeToChildren"/> is set. Only applicable when <see cref="Control.ResizeToChildren"/>
        /// is set.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is greater than 255.</exception>
        public int ResizeToChildrenPadding
        {
            get { return _resizeToChildrenPadding; }
            set
            {
                if (value < 0 || value > byte.MaxValue)
                    throw new ArgumentOutOfRangeException("value");

                if (ResizeToChildrenPadding == value)
                    return;

                unchecked
                {
                    _resizeToChildrenPadding = (byte)value;
                }

                InvokeResizeToChildrenPaddingChanged();
            }
        }

        /// <summary>
        /// Gets the root Control that this Control comes from. If this is the root Control, the value will be a reference
        /// to the Control itself.
        /// </summary>
        public Control Root
        {
            get { return _root; }
        }

        /// <summary>
        /// Gets the position of the control reletive to the screen.
        /// </summary>
        public Vector2 ScreenPosition
        {
            get
            {
                var parent = Parent;
                var pos = Position;

                if (parent != null)
                    return pos + parent.ScreenPosition + parent.GetBorderOffset();
                else
                    return pos;
            }
        }

        /// <summary>
        /// Gets or sets the absolute size of the Control. The Size includes the Border, while the ClientSize
        /// includes only the area inside of the Control's borders.
        /// Can only be set if <see cref="Control.ResizeToChildren"/> is not set.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { InternalSetSize(value, true); }
        }

        /// <summary>
        /// Gets or sets an optional tag object for the Control.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TooltipHandler"/> for this <see cref="Control"/>. If null, this
        /// <see cref="Control"/> will not display a <see cref="Tooltip"/>.
        /// </summary>
        public TooltipHandler Tooltip
        {
            get { return _toolTip; }
            set
            {
                if (_toolTip == value)
                    return;

                _toolTip = value;

                InvokeTooltipChanged();
            }
        }

        /// <summary>
        /// Adds a control to the queue to set a child control as the top-most control.
        /// </summary>
        /// <param name="control">Child control to be set as the top-most Control.</param>
        void AddToTopMostQueue(Control control)
        {
            // No other controls, so can't set as top-most
            if (_controls.Count < 2)
                return;

            // Create the queue if it has not yet been created
            if (_setTopMostQueue == null)
                _setTopMostQueue = new Queue<Control>(2);

            // Add the control to the queue
            _setTopMostQueue.Enqueue(control);
        }

        /// <summary>
        /// Checks if the control contains a given point on the screen.
        /// </summary>
        /// <param name="screenPoint">Point on the screen.</param>
        /// <returns>True if the Control contains the given screen point, else false.</returns>
        public bool ContainsPoint(Vector2 screenPoint)
        {
            var sp = ScreenPosition;
            return IsPointInRegion(screenPoint, sp, sp + Size);
        }

        /// <summary>
        /// Disposes of the Control and all its resources.
        /// </summary>
        /// <param name="disposeManaged">If true, managed resources need to be disposed. If false,
        /// this was raised by a destructor which means the managed resources are already disposed.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            if (disposeManaged)
            {
                // Dispose of all the child controls. An immutable collection must be
                // created so we can iterate through them since the list will be modifying as it goes.
                var tmpControls = Controls.ToImmutable();
                foreach (var child in tmpControls)
                {
                    child.Dispose();
                }

                // Remove the control from either the GUIManager if root or Parent if not root
                if (Parent == null)
                {
                    if (GUIManager != null)
                        GUIManager.Remove(this);
                }
                else
                    Parent._controls.Remove(this);

                Events.Dispose();
            }

            var eventHandler = Events[_eventDisposed] as ControlEventHandler;
            if (eventHandler != null)
                eventHandler(this);
        }

        /// <summary>
        /// Primary drawing routine.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (Parent != null)
                throw new MethodAccessException("Draw() may only be called from the root control.");

            if (_isDisposed)
            {
                const string errmsg = "Attempted to draw disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            DrawControlStart(sb);
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected virtual void DrawControl(ISpriteBatch spriteBatch)
        {
            Border.Draw(spriteBatch, this);
        }

        /// <summary>
        /// Draws this Control and all of its children Controls.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to</param>
        void DrawControlStart(ISpriteBatch spriteBatch)
        {
            if (_isDisposed)
            {
                const string errmsg = "Attempted to draw disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // If this control isn't visible, none of the child controls will be visible, either
            if (!IsVisible)
                return;

            // Draw the control
            DrawControl(spriteBatch);

            // Draw the highlighting if there is an object being dragged and it can be dropped onto this
            if (this == GUIManager.DropOntoControl && this != GUIManager.DraggedDragDropProvider && this is IDragDropProvider)
                ((IDragDropProvider)this).DrawDropHighlight(spriteBatch);

            // Draw all the child controls
            for (var i = 0; i < _controls.Count; i++)
            {
                _controls[i].DrawControlStart(spriteBatch);
            }
        }

        /// <summary>
        /// Finds the client size that this <see cref="Control"/> needs to be to be able to fit all of the child
        /// <see cref="Control"/>s.
        /// </summary>
        /// <returns>The client size that this <see cref="Control"/> needs to be to be able to fit all of the child
        /// <see cref="Control"/>s.</returns>
        Vector2 FindRequiredClientSizeToFitChildren()
        {
            // Use the initial values of 1 (so we will never have a size less than 1)
            float x = 1;
            float y = 1;

            // Loop through all child controls
            foreach (var t in _controls)
            {
                // Skip certain child controls
                if (!t.IncludeInResizeToChildren)
                    continue;

                // Get the position + size of the child control
                var max = t.Position + t.Size;

                // Update the x/y values to the greater of the two values
                if (max.X > x)
                    x = max.X;

                if (max.Y > y)
                    y = max.Y;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// Gets all of the child <see cref="Control"/>s from this <see cref="Control"/>.
        /// </summary>
        /// <returns>All of the child <see cref="Control"/>s from this <see cref="Control"/>. Does not include
        /// this <see cref="Control"/>.</returns>
        public IEnumerable<Control> GetAllChildren()
        {
            foreach (var c in Controls)
            {
                yield return c;
                foreach (var c2 in c.GetAllChildren())
                {
                    yield return c2;
                }
            }
        }

        /// <summary>
        /// Gets a Vector2 for the offset created by the Border.
        /// </summary>
        /// <returns>A Vector2 for the offset created by the Border.</returns>
        Vector2 GetBorderOffset()
        {
            return new Vector2(Border.LeftWidth, Border.TopHeight);
        }

        /// <summary>
        /// Finds the child Control in this Control at a given screen location.
        /// </summary>
        /// <param name="point">Screen location to check.</param>
        /// <param name="canFocusOnly">If true, only controls that can get focus will be checked.</param>
        /// <returns>Child found at the given point if any, else null.</returns>
        public Control GetChild(Vector2 point, bool canFocusOnly = false)
        {
            for (var i = 0; i < _controls.Count; i++)
            {
                var c = _controls[i];
                if (canFocusOnly && !c.CanFocus)
                    continue;

                var sp = c.ScreenPosition;
                if (IsPointInRegion(point, sp, sp + c.Size))
                    return c;
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the area occupied by this <see cref="Control"/> where the
        /// position is relative to the the parent control (that is, same position as <see cref="Control.Position"/>).
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the area occupied by this <see cref="Control"/>.</returns>
        public Rectangle GetRelativeArea()
        {
            var pos = Position;
            var size = Size;
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the area occupied by this <see cref="Control"/> where the
        /// position is the absolute screen position (that is, same position as <see cref="Control.ScreenPosition"/>).
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the area occupied by this <see cref="Control"/>.</returns>
        public Rectangle GetScreenArea()
        {
            var pos = ScreenPosition;
            var size = Size;
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Notifies the Control it has received focus.
        /// </summary>
        internal void HandleFocus()
        {
            if (Parent != null)
                Parent.SetTopMostChild(this);

            InvokeFocused();
        }

        /// <summary>
        /// When overridden in the derived class, provides a place to initialize the <see cref="Control"/> before any events for the
        /// <see cref="Control"/> are invoked. This will be called before the derived class's constructor, and only called once
        /// for each instance. Derived classes should call the base method at the start of the overridden method to ensure
        /// the call hierarchy is maintained in the expected order.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Sets the <see cref="Control.Size"/>.
        /// </summary>
        /// <param name="value">The new size.</param>
        /// <param name="checkResizeToChildren">If true, the size will not be allowed to be set if
        /// <see cref="Control.ResizeToChildren"/> is also true.</param>
        void InternalSetSize(Vector2 value, bool checkResizeToChildren)
        {
            if (checkResizeToChildren && ResizeToChildren)
                return;

            if (_size == value)
                return;

            _size = value;

            InvokeResized();

            KeepInParent();
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeBeginDrag()
        {
            OnBeginDrag();
            var handler = Events[_eventBeginDrag] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeBorderChanged()
        {
            OnBorderChanged();
            var handler = Events[_eventBorderChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeBorderColorChanged()
        {
            OnBorderColorChanged();
            var handler = Events[_eventBorderColorChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeCanDragChanged()
        {
            OnCanDragChanged();
            var handler = Events[_eventCanDragChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeCanFocusChanged()
        {
            OnCanFocusChanged();
            var handler = Events[_eventCanFocusChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeClicked(MouseButtonEventArgs e)
        {
            OnClick(e);
            var handler = Events[_eventClicked] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeEnabledChanged()
        {
            OnEnabledChanged();
            var handler = Events[_eventEnabledChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeEndDrag()
        {
            OnEndDrag();
            var handler = Events[_eventEndDrag] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeFocused()
        {
            OnFocused();
            var handler = Events[_eventFocused] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeIsBoundToParentAreaChanged()
        {
            OnIsBoundToParentAreaChanged();
            var handler = Events[_eventIsBoundToParentAreaChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeKeyPressed(KeyEventArgs e)
        {
            OnKeyPressed(e);
            var handler = Events[_eventKeyPressed] as KeyboardEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeKeyReleased(KeyEventArgs e)
        {
            OnKeyReleased(e);
            var handler = Events[_eventKeyReleased] as KeyboardEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeLostFocus()
        {
            OnLostFocus();
            var handler = Events[_eventLostFocus] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseDown(MouseButtonEventArgs e)
        {
            OnMouseDown(e);
            var handler = Events[_eventMouseDown] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseEnter(MouseMoveEventArgs e)
        {
            OnMouseEnter(e);
            var handler = Events[_eventMouseEnter] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseLeave(MouseMoveEventArgs e)
        {
            OnMouseLeave(e);
            var handler = Events[_eventMouseLeave] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseMoved(MouseMoveEventArgs e)
        {
            OnMouseMoved(e);
            var handler = Events[_eventMouseMoved] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseUp(MouseButtonEventArgs e)
        {
            OnMouseUp(e);
            var handler = Events[_eventMouseUp] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeMoved()
        {
            OnMoved();
            var handler = Events[_eventMoved] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeResizeToChildrenChanged()
        {
            OnResizeToChildrenChanged();
            var handler = Events[_eventResizeToChildrenChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeResizeToChildrenPaddingChanged()
        {
            OnResizeToChildrenPaddingChanged();
            var handler = Events[_eventResizeToChildrenPaddingChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeResized()
        {
            OnResized();
            var handler = Events[_eventResized] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeTextEntered(TextEventArgs e)
        {
            OnTextEntered(e);
            var handler = Events[_eventTextEntered] as TextEnteredEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeTooltipChanged()
        {
            OnTooltipChanged();
            var handler = Events[_eventTooltipChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeVisibleChanged()
        {
            OnVisibleChanged();
            var handler = Events[_eventVisibleChanged] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Checks if a point is in a given region.
        /// </summary>
        /// <param name="point">Point to check if the region contains.</param>
        /// <param name="min">Top-left corner of the region.</param>
        /// <param name="max">Bottom-right corner of the region.</param>
        /// <returns>If true, the region contains the point, else false.</returns>
        public static bool IsPointInRegion(Vector2 point, Vector2 min, Vector2 max)
        {
            return point.X >= min.X && point.Y >= min.Y && point.X <= max.X && point.Y <= max.Y;
        }

        /// <summary>
        /// Adjusts the Control's position to keep it in the area of its parent.
        /// </summary>
        void KeepInParent()
        {
            if (!IsBoundToParentArea)
                return;

            var parent = Parent;
            if (parent == null)
                return;

            var min = Vector2.Zero;
            var max = parent.ClientSize - Size;
            Position = Vector2.Min(Vector2.Max(Position, min), max);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public virtual void LoadSkin(ISkinManager skinManager)
        {
            var type = GetType();
            var name = type.Name;

            if (type.IsGenericType)
                name = name.Substring(0, name.IndexOf('`'));

            Border = skinManager.GetBorder(name);
        }

        /// <summary>
        /// Handles when a <see cref="Control"/> has begun being dragged.
        /// This is called immediately before <see cref="Control.OnBeginDrag"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnBeginDrag"/> when possible.
        /// </summary>
        protected virtual void OnBeginDrag()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.BorderChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderChanged"/> when possible.
        /// </summary>
        protected virtual void OnBorderChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.BorderColor"/> has changed.
        /// This is called immediately before <see cref="Control.BorderColorChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderColorChanged"/> when possible.
        /// </summary>
        protected virtual void OnBorderColorChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.CanDrag"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.CanDragChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.CanDragChanged"/> when possible.
        /// </summary>
        protected virtual void OnCanDragChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.CanFocus"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.CanFocusChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.CanFocusChanged"/> when possible.
        /// </summary>
        protected virtual void OnCanFocusChanged()
        {
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> was clicked.
        /// This is called immediately before <see cref="Control.OnClick"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnClick(MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.OnEnabledChanged"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.EnabledChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.EnabledChanged"/> when possible.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> has ended being dragged.
        /// This is called immediately before <see cref="Control.OnEndDrag"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnEndDrag"/> when possible.
        /// </summary>
        protected virtual void OnEndDrag()
        {
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> has gained focus.
        /// This is called immediately before <see cref="Control.Focused"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Focused"/> when possible.
        /// </summary>
        protected virtual void OnFocused()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.IsBoundToParentArea"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.IsBoundToParentAreaChanged"/>.
        /// Override this method instead of using an event hook on
        /// <see cref="Control.IsBoundToParentAreaChanged"/> when possible.
        /// </summary>
        protected virtual void OnIsBoundToParentAreaChanged()
        {
        }

        /// <summary>
        /// Handles when a key is being pressed while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.KeyPressed"/>.
        /// Override this method instead of using an event hook on <see cref="Control.KeyPressed"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a key has been pressed down while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.KeyReleased"/>.
        /// Override this method instead of using an event hook on <see cref="Control.KeyReleased"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnKeyReleased(KeyEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control"/> has lost focus.
        /// This is called immediately before <see cref="Control.OnLostFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.LostFocus"/> when possible.
        /// </summary>
        protected virtual void OnLostFocus()
        {
            // End dragging if needed
            if (_isDragging)
            {
                _isDragging = false;
                InvokeEndDrag();
            }
        }

        /// <summary>
        /// Handles when a mouse button has been pressed down on this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseDown"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseDown"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseDown(MouseButtonEventArgs e)
        {
            // Give the control the focus
            if ((CanFocus || CanDrag) && GetChild(e.Location(), true) == null)
            {
                GUIManager.FocusedControl = this;

                // Drag the control if possible
                if (CanDrag)
                {
                    _dragOffset = e.Location() - Position;
                    if (!_isDragging)
                    {
                        _isDragging = true;
                        InvokeBeginDrag();
                    }
                }
            }
        }

        /// <summary>
        /// Handles when the mouse has entered the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseEnter"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseEnter(MouseMoveEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse has left the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseLeave"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseLeave(MouseMoveEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse has moved over the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.MouseMoved"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseMoved"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseMoved(MouseMoveEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a mouse button has been raised on the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseUp"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseUp"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnMouseUp(MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                InvokeEndDrag();
            }
            else
            {
                if (GUIManager.LastPressedControl == this && GUIManager.UnderCursor == this)
                    InvokeClicked(e);
            }
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> has moved.
        /// This is called immediately before <see cref="Control.Moved"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Moved"/> when possible.
        /// </summary>
        protected virtual void OnMoved()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.ResizeToChildren"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.ResizeToChildrenChanged"/>.
        /// Override this method instead of using an event hook on
        /// <see cref="Control.ResizeToChildrenChanged"/> when possible.
        /// </summary>
        protected virtual void OnResizeToChildrenChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.ResizeToChildrenPadding"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.ResizeToChildrenPaddingChanged"/>.
        /// Override this method instead of using an event hook on
        /// <see cref="Control.ResizeToChildrenPaddingChanged"/> when possible.
        /// </summary>
        protected virtual void OnResizeToChildrenPaddingChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected virtual void OnResized()
        {
        }

        /// <summary>
        /// Handles when text has been entered into this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.TextEntered"/>.
        /// Override this method instead of using an event hook on <see cref="Control.TextEntered"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnTextEntered(TextEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Tooltip"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Tooltip"/>.
        /// Override this method instead of using an event hook on
        /// <see cref="Control.TooltipChanged"/> when possible.
        /// </summary>
        protected virtual void OnTooltipChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.IsVisible"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.VisibleChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.VisibleChanged"/> when possible.
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
        }

        /// <summary>
        /// Processes the SetTopMostQueue to update the top-most Controls. Must be called
        /// while Controls is not being enumerated over.
        /// </summary>
        void ProcessTopMostQueue()
        {
            if (_setTopMostQueue == null)
                return;

            // Loop through all the controls in the queue
            while (_setTopMostQueue.Count > 0)
            {
                // Get the control to process
                var child = _setTopMostQueue.Dequeue();

                // Remove the control from the list, then add it to the top (end of the list)
                _controls.Remove(child);
                _controls.Add(child);
            }
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendFocusedEvent(MouseButtonEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeFocused();
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendKeyPressedEvent(KeyEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeKeyPressed(e);
        }

        /// <summary>
        /// Gets of this <see cref="Control"/> is in a valid state to be receiving input events.
        /// </summary>
        bool CanReceiveInputEvent { get { return IsVisible && IsEnabled && !IsDisposed; } }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendKeyReleasedEvent(KeyEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeKeyReleased(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendLostFocusEvent(MouseButtonEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeLostFocus();
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendMouseButtonPressedEvent(MouseButtonEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeMouseDown(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendMouseButtonReleasedEvent(MouseButtonEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeMouseUp(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendMouseEnterEvent(MouseMoveEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeMouseEnter(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendMouseLeaveEvent(MouseMoveEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeMouseLeave(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendMouseMoveEvent(MouseMoveEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeMouseMoved(e);
        }

        /// <summary>
        /// Notifies this <see cref="Control"/> about an event that took place on it. These events should usually come from the
        /// <see cref="IGUIManager"/> that manages this <see cref="Control"/> instance.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        internal void SendTextEnteredEvent(TextEventArgs e)
        {
            if (!CanReceiveInputEvent)
                return;

            InvokeTextEntered(e);
        }

        /// <summary>
        /// Sets this Control as the top-most Control, if possible.
        /// </summary>
        void SetAsTopMost()
        {
            var parent = Parent;
            if (parent != null)
                parent.SetTopMostChild(this);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical and cascading.
        /// </summary>
        protected virtual void SetDefaultValues()
        {
            CanFocus = true;
            CanDrag = false;
            IsEnabled = true;
            IsVisible = true;
            Border = ControlBorder.Empty;

            LoadSkin(GUIManager.SkinManager);
        }

        /// <summary>
        /// Gives the control focus. CanFocus must be true for this to have any affect.
        /// </summary>
        public void SetFocus()
        {
            if (CanFocus)
                GUIManager.FocusedControl = this;
        }

        /// <summary>
        /// Set a child Control as the top-most Control in its parent's (this) list of children.
        /// </summary>
        /// <param name="control">Control to be set as the top-most Control.</param>
        void SetTopMostChild(Control control)
        {
            // Do this recursively all the way back to the lowest-level parent Control, which will make sure that
            // even a sub-control will cause it's parent to become the focus Control
            var parent = Parent;
            if (parent != null)
                parent.SetTopMostChild(this);

            // Make sure that this is actually the control's parent.
            if (!Controls.Contains(control))
            {
                const string errmsg =
                    "Attempted to set control `{0}` as the top-most control in `{1}`, " +
                    "but the control is not in the parent's list of Controls.";
                Debug.Fail(string.Format(errmsg, control, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, control, this);
                return;
            }

            // Queue the control to be set as the top-most
            AddToTopMostQueue(control);
        }

        /// <summary>
        /// Updates the Control and all the controls under it.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public void Update(TickCount currentTime)
        {
            // This method just handles the recursive updating. The real updating logic is done in UpdateControl.

            // Do not update disposed controls
            if (IsDisposed)
                return;

            // Update the children in a way that allows us to support when the collection is modified
            for (var i = 0; i < _controls.Count; i++)
            {
                var child = _controls[i];
                child.Update(currentTime);

                // If the control at the given index is no longer the child, then the child control probably
                // disposed itself, so decrement the counter by one to re-update the changed slot
                if (i < _controls.Count && _controls[i] != child)
                    --i;
            }

            // Perform the updating
            UpdateControl(currentTime);
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void UpdateControl(TickCount currentTime)
        {
            if (IsDisposed)
                return;

            // Set the top-most controls if needed
            ProcessTopMostQueue();

            // Update the control's position if it is being dragged
            if (_isDragging)
            {
                // Just as a safety net, make sure we NEVER drag when the mouse button is up
                // even though it shouldn't be up if our code is correct
                if (!GUIManager.IsMouseButtonDown(MouseButton.Left))
                {
                    // Button not down - stop dragging
                    _isDragging = false;
                    InvokeEndDrag();
                }
                else
                {
                    // Button down - continue dragging
                    Position = GUIManager.CursorPosition - _dragOffset;
                }
            }

            // Auto-resize if needed
            if (ResizeToChildren)
                UpdateResizeToChildren();
        }

        /// <summary>
        /// Updates the <see cref="Control.ClientSize"/> for when <see cref="Control.ResizeToChildren"/> is set.
        /// </summary>
        void UpdateResizeToChildren()
        {
            var newSize = FindRequiredClientSizeToFitChildren() + Border.Size + new Vector2(ResizeToChildrenPadding);
            InternalSetSize(newSize, false);
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of this <see cref="Control"/> and all its resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}