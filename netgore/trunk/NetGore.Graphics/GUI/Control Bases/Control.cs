using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore.IO;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base class of all controls
    /// </summary>
    public abstract class Control : IDisposable, IPersistable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly object _eventBeginDrag = new object();
        static readonly object _eventChangeBorder = new object();
        static readonly object _eventClick = new object();
        static readonly object _eventDispose = new object();
        static readonly object _eventEndDrag = new object();
        static readonly object _eventGetFocus = new object();
        static readonly object _eventKeyDown = new object();
        static readonly object _eventKeyPress = new object();
        static readonly object _eventKeyUp = new object();
        static readonly object _eventLostFocus = new object();
        static readonly object _eventMouseDown = new object();
        static readonly object _eventMouseEnter = new object();
        static readonly object _eventMouseLeave = new object();
        static readonly object _eventMouseMove = new object();
        static readonly object _eventMouseUp = new object();
        static readonly object _eventResize = new object();

        readonly List<Control> _controls = new List<Control>(1);
        readonly EventHandlerList _eventHandlerList = new EventHandlerList();
        readonly IGUIManager _gui;
        readonly Control _parent;
        readonly Control _root;
        ControlBorder _border;
        bool _canDrag = true;
        bool _canFocus = true;
        Vector2 _dragOffset;
        bool _isBoundToParentArea = true;
        bool _isDisposed = false;
        bool _isDragging = false;
        bool _isEnabled = true;
        bool _isMouseEntered = false;
        bool _isVisible = true;
        Vector2 _position;
        Queue<Control> _setTopMostQueue = null;
        Vector2 _size;

        /// <summary>
        /// States if a OnClick event will be raised with the OnMouseUp event.
        /// </summary>
        bool _willRaiseClick = false;

        /// <summary>
        /// Notifies listeners when the Control has begun being dragged.
        /// </summary>
        public event ControlEventHandler OnBeginDrag
        {
            add { Events.AddHandler(_eventBeginDrag, value); }
            remove { Events.RemoveHandler(_eventBeginDrag, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.Border"/> has changed.
        /// </summary>
        public event ControlEventHandler OnChangeBorder
        {
            add { Events.AddHandler(_eventChangeBorder, value); }
            remove { Events.RemoveHandler(_eventChangeBorder, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> was clicked.
        /// </summary>
        public event MouseClickEventHandler OnClick
        {
            add { Events.AddHandler(_eventClick, value); }
            remove { Events.RemoveHandler(_eventClick, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has been disposed.
        /// </summary>
        public event ControlEventHandler OnDispose
        {
            add { Events.AddHandler(_eventDispose, value); }
            remove { Events.RemoveHandler(_eventDispose, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has ended being dragged.
        /// </summary>
        public event ControlEventHandler OnEndDrag
        {
            add { Events.AddHandler(_eventEndDrag, value); }
            remove { Events.RemoveHandler(_eventEndDrag, value); }
        }

        /// <summary>
        /// Notifies listeners when this <see cref="Control"/> has gained focus.
        /// </summary>
        public event ControlEventHandler OnGetFocus
        {
            add { Events.AddHandler(_eventGetFocus, value); }
            remove { Events.RemoveHandler(_eventGetFocus, value); }
        }

        /// <summary>
        /// Notifies listeners when a key has been pressed down while the <see cref="Control"/> has focus.
        /// </summary>
        public event KeyboardEventHandler OnKeyDown
        {
            add { Events.AddHandler(_eventKeyDown, value); }
            remove { Events.RemoveHandler(_eventKeyDown, value); }
        }

        /// <summary>
        /// Notifies listeners when a key is being pressed while the <see cref="Control"/> has focus.
        /// </summary>
        public event KeyboardEventHandler OnKeyPress
        {
            add { Events.AddHandler(_eventKeyPress, value); }
            remove { Events.RemoveHandler(_eventKeyPress, value); }
        }

        /// <summary>
        /// Notifies listeners when a key has been raised while the <see cref="Control"/> has focus.
        /// </summary>
        public event KeyboardEventHandler OnKeyUp
        {
            add { Events.AddHandler(_eventKeyUp, value); }
            remove { Events.RemoveHandler(_eventKeyUp, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control"/> has lost focus.
        /// </summary>
        public event ControlEventHandler OnLostFocus
        {
            add { Events.AddHandler(_eventLostFocus, value); }
            remove { Events.RemoveHandler(_eventLostFocus, value); }
        }

        /// <summary>
        /// Notifies listeners when a mouse button has been pressed down on this <see cref="Control"/>.
        /// </summary>
        public event MouseClickEventHandler OnMouseDown
        {
            add { Events.AddHandler(_eventMouseDown, value); }
            remove { Events.RemoveHandler(_eventMouseDown, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has entered the area of the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler OnMouseEnter
        {
            add { Events.AddHandler(_eventMouseEnter, value); }
            remove { Events.RemoveHandler(_eventMouseEnter, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has left the area of the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler OnMouseLeave
        {
            add { Events.AddHandler(_eventMouseLeave, value); }
            remove { Events.RemoveHandler(_eventMouseLeave, value); }
        }

        /// <summary>
        /// Notifies listeners when the mouse has moved over the <see cref="Control"/>.
        /// </summary>
        public event MouseEventHandler OnMouseMove
        {
            add { Events.AddHandler(_eventMouseMove, value); }
            remove { Events.RemoveHandler(_eventMouseMove, value); }
        }

        /// <summary>
        /// Notifies listeners when a mouse button has been raised on the <see cref="Control"/>.
        /// </summary>
        public event MouseClickEventHandler OnMouseUp
        {
            add { Events.AddHandler(_eventMouseUp, value); }
            remove { Events.RemoveHandler(_eventMouseUp, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// </summary>
        public event ControlEventHandler OnResize
        {
            add { Events.AddHandler(_eventResize, value); }
            remove { Events.RemoveHandler(_eventResize, value); }
        }

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

            if (Parent != null)
            {
                // Check that the parent isn't disposed
                if (parent._isDisposed)
                    throw new ArgumentException("Parent control is disposed and cannot be used.", "parent");

                // Add the Control to the parent
                Parent._controls.Add(this);
                _root = GetRootControl();
                KeepInParent();
            }
            else
            {
                // This control is the root, so add it directly to the GUI manager
                _root = this;
                GUIManager.Add(this);
            }

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            SetDefaultValues();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
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
                foreach (Control child in Controls)
                {
                    child.KeepInParent();
                }

                InvokeChangeBorder();
            }
        }

        /// <summary>
        /// Gets or sets if the Control supports movement by dragging with the mouse
        /// </summary>
        [SyncValue]
        public bool CanDrag
        {
            get { return _canDrag; }
            set { _canDrag = value; }
        }

        /// <summary>
        /// Gets or sets if the Control can get focus
        /// </summary>
        [SyncValue]
        public bool CanFocus
        {
            get { return _canFocus; }
            set { _canFocus = value; }
        }

        /// <summary>
        /// Gets the size of the client area (internal area, not including the borders) of the Control.
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
        /// Gets or sets if the <see cref="Control"/> is bound to the area of its parent. Only valid if the
        /// <see cref="Control"/> has a parent.
        /// </summary>
        [SyncValue]
        public bool IsBoundToParentArea
        {
            get { return _isBoundToParentArea; }
            set { _isBoundToParentArea = value; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Control"/> is enabled.
        /// </summary>
        [SyncValue]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// Gets if the cursor is currently over this <see cref="Control"/>.
        /// </summary>
        public bool IsMouseEntered
        {
            get { return _isMouseEntered; }
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
                KeepInParent();
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
                Control parent = Parent;
                Vector2 pos = Position;

                if (parent != null)
                    return pos + parent.ScreenPosition + parent.GetBorderOffset();
                else
                    return pos;
            }
        }

        /// <summary>
        /// Gets or sets the absolute size of the Control. The Size includes the Border, while the ClientSize
        /// includes only the area inside of the Control's borders.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size == value)
                    return;

                _size = value;

                InvokeResize();

                KeepInParent();
            }
        }

        /// <summary>
        /// Gets or sets an optional tag object for the Control.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TooltipHandler"/> for this <see cref="Control"/>. If null, this
        /// <see cref="Control"/> will not display a <see cref="Tooltip"/>.
        /// </summary>
        public TooltipHandler Tooltip { get; set; }

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
                _setTopMostQueue = new Queue<Control>(1);

            // Add the control to the queue
            _setTopMostQueue.Enqueue(control);
        }

        /// <summary>
        /// Handles when a <see cref="Control"/> has begun being dragged.
        /// This is called immediately before <see cref="Control.OnBeginDrag"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnBeginDrag"/> when possible.
        /// </summary>
        protected virtual void BeginDrag()
        {
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.OnChangeBorder"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnChangeBorder"/> when possible.
        /// </summary>
        protected virtual void ChangeBorder()
        {
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> was clicked.
        /// This is called immediately before <see cref="Control.OnClick"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnClick"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void Click(MouseClickEventArgs e)
        {
        }

        /// <summary>
        /// Checks if the control contains a given point on the screen
        /// </summary>
        /// <param name="screenPoint">Point on the screen</param>
        /// <returns>True if the Control contains the given screen point, else false</returns>
        public bool ContainsPoint(Vector2 screenPoint)
        {
            Vector2 sp = ScreenPosition;
            return IsPointInRegion(screenPoint, sp, sp + Size);
        }

        /// <summary>
        /// Disposes of the Control and all its resources
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
                // Dispose of all the child controls. A temporary list must be
                // created so we can iterate through them since the list will be modifying as it goes.
                var tmpControls = new List<Control>(Controls);
                foreach (Control child in tmpControls)
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

            var eventHandler = Events[_eventDispose] as ControlEventHandler;
            if (eventHandler != null)
                eventHandler(this);
        }

        /// <summary>
        /// Primary drawing routine
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to</param>
        public void Draw(SpriteBatch sb)
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
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected virtual void DrawControl(SpriteBatch spriteBatch)
        {
            Border.Draw(spriteBatch, this);
        }

        /// <summary>
        /// Draws this Control and all of its children Controls.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to</param>
        void DrawControlStart(SpriteBatch spriteBatch)
        {
            if (_isDisposed)
            {
                const string errmsg = "Attempted to draw disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            if (!IsVisible)
                return;

            DrawControl(spriteBatch);

            foreach (Control c in Controls)
            {
                c.DrawControlStart(spriteBatch);
            }
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> has ended being dragged.
        /// This is called immediately before <see cref="Control.OnEndDrag"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnEndDrag"/> when possible.
        /// </summary>
        protected virtual void EndDrag()
        {
        }

        /// <summary>
        /// Gets all of the child <see cref="Control"/>s from this <see cref="Control"/>.
        /// </summary>
        /// <returns>All of the child <see cref="Control"/>s from this <see cref="Control"/>. Does not include
        /// this <see cref="Control"/>.</returns>
        public IEnumerable<Control> GetAllChildren()
        {
            foreach (Control c in Controls)
            {
                yield return c;
                foreach (Control c2 in c.GetAllChildren())
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
        /// Gets the <see cref="ButtonState"/> for the current and last state for a given mouse button.
        /// </summary>
        /// <param name="button">The mouse button to get the states for.</param>
        /// <param name="state">The current mouse state.</param>
        /// <param name="lastState">The last mouse state.</param>
        /// <param name="buttonState">The output current ButtonState for the given <paramref name="button"/>.</param>
        /// <param name="lastButtonState">The output last ButtonState for the given <paramref name="button"/>.</param>
        static void GetButtonStates(MouseButtons button, MouseState state, MouseState lastState, out ButtonState buttonState,
                                    out ButtonState lastButtonState)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    buttonState = state.LeftButton;
                    lastButtonState = lastState.LeftButton;
                    break;

                case MouseButtons.Right:
                    buttonState = state.RightButton;
                    lastButtonState = lastState.RightButton;
                    break;

                case MouseButtons.Middle:
                    buttonState = state.MiddleButton;
                    lastButtonState = lastState.MiddleButton;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("button", "Invalid button value!");
            }
        }

        /// <summary>
        /// Finds the child Control in this Control at a given screen location.
        /// </summary>
        /// <param name="point">Screen location to check.</param>
        /// <returns>Child found at the given point if any, else null.</returns>
        public Control GetChild(Vector2 point)
        {
            return GetChild(point, false);
        }

        /// <summary>
        /// Finds the child Control in this Control at a given screen location.
        /// </summary>
        /// <param name="point">Screen location to check.</param>
        /// <param name="canFocusOnly">If true, only controls that can get focus will be checked.</param>
        /// <returns>Child found at the given point if any, else null.</returns>
        public Control GetChild(Vector2 point, bool canFocusOnly)
        {
            foreach (Control c in Controls)
            {
                if (canFocusOnly && !c.CanFocus)
                    continue;

                Vector2 sp = c.ScreenPosition;
                if (IsPointInRegion(point, sp, sp + c.Size))
                    return c;
            }
            return null;
        }

        /// <summary>
        /// Handles when this <see cref="Control"/> has gained focus.
        /// This is called immediately before <see cref="Control.OnGetFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnGetFocus"/> when possible.
        /// </summary>
        protected virtual void GetFocus()
        {
        }

        /// <summary>
        /// Finds the Control's root Control.
        /// </summary>
        /// <returns>Root Control for this Control.</returns>
        Control GetRootControl()
        {
            Control root = this;
            while (root.Parent != null)
            {
                root = root.Parent;
            }
            return root;
        }

        /// <summary>
        /// Notifies the Control it has received focus.
        /// </summary>
        internal void HandleFocus()
        {
            if (Parent != null)
                Parent.SetTopMostChild(this);

            InvokeGetFocus();
        }

        /// <summary>
        /// Notifies the Control it has lost focus.
        /// </summary>
        internal void HandleLostFocus()
        {
            InvokeLostFocus();

            // Send OnMouseLeave if needed
            if (_isMouseEntered)
            {
                _isMouseEntered = false;
                InvokeMouseLeave(new MouseEventArgs(GUIManager.CursorPosition - ScreenPosition));
            }

            // End dragging if needed
            if (_isDragging)
            {
                _isDragging = false;
                InvokeEndDrag();
            }
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeBeginDrag()
        {
            BeginDrag();
            var handler = Events[_eventBeginDrag] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeBorder()
        {
            ChangeBorder();
            var handler = Events[_eventChangeBorder] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeClick(MouseClickEventArgs e)
        {
            Click(e);
            var handler = Events[_eventClick] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeEndDrag()
        {
            EndDrag();
            var handler = Events[_eventEndDrag] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeGetFocus()
        {
            GetFocus();
            var handler = Events[_eventGetFocus] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeKeyDown(KeyboardEventArgs e)
        {
            KeyDown(e);
            var handler = Events[_eventKeyDown] as KeyboardEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeKeyPress(KeyboardEventArgs e)
        {
            KeyPress(e);
            var handler = Events[_eventKeyPress] as KeyboardEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeKeyUp(KeyboardEventArgs e)
        {
            KeyUp(e);
            var handler = Events[_eventKeyUp] as KeyboardEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeLostFocus()
        {
            LostFocus();
            var handler = Events[_eventLostFocus] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseDown(MouseClickEventArgs e)
        {
            MouseDown(e);
            var handler = Events[_eventMouseDown] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseEnter(MouseEventArgs e)
        {
            MouseEnter(e);
            var handler = Events[_eventMouseEnter] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseLeave(MouseEventArgs e)
        {
            MouseLeave(e);
            var handler = Events[_eventMouseLeave] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseMove(MouseEventArgs e)
        {
            MouseMove(e);
            var handler = Events[_eventMouseMove] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="e">The event args.</param>
        void InvokeMouseUp(MouseClickEventArgs e)
        {
            MouseUp(e);
            var handler = Events[_eventMouseUp] as MouseClickEventHandler;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeResize()
        {
            Resize();
            var handler = Events[_eventResize] as ControlEventHandler;
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

            Control parent = Parent;
            if (parent == null)
                return;

            Vector2 min = Vector2.Zero;
            Vector2 max = parent.ClientSize - Size;
            Position = Vector2.Min(Vector2.Max(Position, min), max);
        }

        /// <summary>
        /// Handles when a key has been pressed down while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.OnKeyDown"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnKeyDown"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void KeyDown(KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a key is being pressed while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.OnKeyPress"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnKeyPress"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void KeyPress(KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a key has been raised while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.OnKeyUp"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnKeyUp"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void KeyUp(KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public virtual void LoadSkin(ISkinManager skinManager)
        {
            Border = skinManager.GetBorder(GetType().Name);
        }

        /// <summary>
        /// Handles when the <see cref="Control"/> has lost focus.
        /// This is called immediately before <see cref="Control.OnLostFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnLostFocus"/> when possible.
        /// </summary>
        protected virtual void LostFocus()
        {
        }

        /// <summary>
        /// Handles when a mouse button has been pressed down on this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseDown"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseDown"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void MouseDown(MouseClickEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse has entered the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseEnter"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void MouseEnter(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse has left the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseLeave"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void MouseLeave(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse has moved over the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseMove"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseMove"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void MouseMove(MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a mouse button has been raised on the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseUp"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseUp"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void MouseUp(MouseClickEventArgs e)
        {
        }

        /// <summary>
        /// Gets a Vector2 for the offset created by the Parent's Border.
        /// </summary>
        /// <returns>A Vector2 for the offset created by the Parent's Border.</returns>
        Vector2 ParentBorderOffset()
        {
            Control p = Parent;
            if (p == null)
                return Vector2.Zero;

            ControlBorder b = p.Border;
            if (b == null)
                return Vector2.Zero;

            return new Vector2(b.LeftWidth, b.TopHeight);
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
                Control child = _setTopMostQueue.Dequeue();

                // Remove the control from the list, then add it to the top (end of the list)
                _controls.Remove(child);
                _controls.Add(child);
            }
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.OnResize"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnResize"/> when possible.
        /// </summary>
        protected virtual void Resize()
        {
        }

        /// <summary>
        /// Sets this Control as the top-most Control, if possible.
        /// </summary>
        void SetAsTopMost()
        {
            Control parent = Parent;
            if (parent != null)
                parent.SetTopMostChild(this);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
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
            Control parent = Parent;
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
        /// Tests for if the MouseState has changed for a given button
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <param name="state">Current MouseState</param>
        /// <param name="lastState">Previous MouseState</param>
        /// <param name="relativePos">Relative position of the Control</param>
        void TestMouseStateChange(MouseButtons button, MouseState state, MouseState lastState, Vector2 relativePos)
        {
            // Store the ButtonStates for the given button
            ButtonState buttonState;
            ButtonState lastButtonState;
            GetButtonStates(button, state, lastState, out buttonState, out lastButtonState);

            // Check that the state has changed
            if (lastButtonState == buttonState)
                return;

            // Check if it was a press or release
            if (buttonState == ButtonState.Pressed)
            {
                // Mouse was clicked down
                InvokeMouseDown(new MouseClickEventArgs(button, relativePos));
                _willRaiseClick = true;

                // Drag the control if possible
                if ((CanFocus || CanDrag) && GetChild(new Vector2(state.X, state.Y), true) == null)
                {
                    GUIManager.FocusedControl = this;
                    if (CanDrag)
                    {
                        _dragOffset = relativePos + ParentBorderOffset();
                        if (!_isDragging)
                        {
                            _isDragging = true;
                            InvokeBeginDrag();
                        }
                    }
                }
            }
            else
            {
                // Mouse was raised
                InvokeMouseUp(new MouseClickEventArgs(button, relativePos));

                // Check if to raise a click event, too
                if (_willRaiseClick)
                {
                    InvokeClick(new MouseClickEventArgs(button, relativePos));
                    _willRaiseClick = false;
                }
            }
        }

        /// <summary>
        /// Updates the Control.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public void Update(int currentTime)
        {
            if (Parent != null)
                throw new MethodAccessException("Update() may only be called from the root control.");

            if (_isDisposed)
            {
                const string errmsg = "Attempted to update disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // Do not update the mouse or keyboard for the Control unless it is the focused root
            if (GUIManager.FocusedRoot == this)
            {
                UpdateMouse();
                UpdateKeyboard();
            }

            // Perform misc updating
            UpdateControl(currentTime);
        }

        /// <summary>
        /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected virtual void UpdateControl(int currentTime)
        {
            if (_isDisposed)
            {
                const string errmsg = "Attempted to update disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // Update all the child controls
            foreach (Control c in Controls)
            {
                c.UpdateControl(currentTime);
            }

            // Set the top-most controls if needed
            ProcessTopMostQueue();
        }

        /// <summary>
        /// Updates the Control with keyboard related events
        /// </summary>
        protected virtual void UpdateKeyboard()
        {
            if (_isDisposed)
            {
                const string errmsg = "Attempted to update disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // Skip disabled or invisible controls
            if (!IsEnabled || !IsVisible)
                return;

            // Update the child controls
            foreach (Control c in Controls)
            {
                c.UpdateKeyboard();
            }

            // Only raise events for the control with focus
            if (!HasFocus)
                return;

            KeyboardState keyboardState = GUIManager.KeyboardState;

            // KeyPress event
            var pressedKeys = GUIManager.KeysPressed;
            if (pressedKeys != null && pressedKeys.Count() > 0)
                InvokeKeyPress(new KeyboardEventArgs(pressedKeys, keyboardState));

            // KeyDown event
            var keysDown = GUIManager.NewKeysDown;
            if (keysDown != null && keysDown.Count() > 0)
                InvokeKeyDown(new KeyboardEventArgs(keysDown, keyboardState));

            // KeyUp event
            var keysUp = GUIManager.NewKeysUp;
            if (keysUp != null && keysUp.Count() > 0)
                InvokeKeyUp(new KeyboardEventArgs(keysUp, keyboardState));
        }

        /// <summary>
        /// Updates the Control with mouse related events.
        /// </summary>
        protected virtual void UpdateMouse()
        {
            if (_isDisposed)
            {
                const string errmsg = "Attempted to update disposed control `{0}`.";
                Debug.Fail(string.Format(errmsg, this));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                return;
            }

            // Skip disabled or invisible controls
            if (!IsEnabled || !IsVisible)
                return;

            MouseState mouseState = GUIManager.MouseState;
            MouseState lastMouseState = GUIManager.LastMouseState;

            // Check if the mouse state has even changed before updating
            if (mouseState == lastMouseState)
                return;

            // Update the child controls
            foreach (Control c in Controls)
            {
                c.UpdateMouse();
            }

            // Update the control's position if it is being dragged
            if (_isDragging)
            {
                Position = new Vector2(mouseState.X, mouseState.Y) - _dragOffset;
                if (Parent != null)
                {
                    Position -= Parent.ScreenPosition;
                    KeepInParent();
                }

                // Check for if the dragging has stopped
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    _isDragging = false;
                    InvokeEndDrag();
                }
            }

            Vector2 currCursorPos = new Vector2(mouseState.X, mouseState.Y);
            Vector2 sp = ScreenPosition;

            // Check if this is the Control being pointed at
            if (GUIManager.UnderCursor == this)
            {
                // Raise the OnMouseMove event
                InvokeMouseMove(new MouseEventArgs(currCursorPos - sp));

                // Check if the mouse has just entered the control
                if (!_isMouseEntered)
                {
                    _isMouseEntered = true;
                    InvokeMouseEnter(new MouseEventArgs(currCursorPos - sp));
                }

                // Perform updates based on the buttons
                TestMouseStateChange(MouseButtons.Left, mouseState, lastMouseState, currCursorPos - sp);
                TestMouseStateChange(MouseButtons.Right, mouseState, lastMouseState, currCursorPos - sp);
                TestMouseStateChange(MouseButtons.Middle, mouseState, lastMouseState, currCursorPos - sp);
            }
            else
            {
                // Mouse is not over the control anymore
                if (_isMouseEntered)
                {
                    _isMouseEntered = false;
                    _willRaiseClick = false;
                    InvokeMouseLeave(new MouseEventArgs(currCursorPos - sp));
                }
            }
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
    }
}