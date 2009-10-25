using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base class of all controls
    /// </summary>
    public abstract class Control : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly List<Control> _controls = new List<Control>(1);
        readonly GUIManagerBase _gui;
        readonly Control _parent;
        readonly Control _root;
        readonly ControlSettings _settings;
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
        /// States if a OnClick event will be raised with the OnMouseUp event
        /// </summary>
        bool _willRaiseClick = false;

        /// <summary>
        /// Notifies when the Control has begun being dragged
        /// </summary>
        public event ControlEventHandler OnBeginDrag;

        /// <summary>
        /// Notifies when the ControlBorder has changed
        /// </summary>
        public event ControlEventHandler OnChangeBorder;

        /// <summary>
        /// Notifies when the Control was clicked
        /// </summary>
        public event MouseClickEventHandler OnClick;

        /// <summary>
        /// Notifies when the Control has been disposed
        /// </summary>
        public event ControlEventHandler OnDispose;

        /// <summary>
        /// Notifies when the Control has ended being dragged
        /// </summary>
        public event ControlEventHandler OnEndDrag;

        /// <summary>
        /// Notifies when the Control has gained focus
        /// </summary>
        public event ControlEventHandler OnFocus;

        /// <summary>
        /// Notifies when a key has been pressed down while the Control has focus
        /// </summary>
        public event KeyboardEventHandler OnKeyDown;

        /// <summary>
        /// Notifies when a key is being pressed while the Control has focus
        /// </summary>
        public event KeyboardEventHandler OnKeyPress;

        /// <summary>
        /// Notifies when a key has been raised while the Control has focus
        /// </summary>
        public event KeyboardEventHandler OnKeyUp;

        /// <summary>
        /// Notifies when the Control has lost focus
        /// </summary>
        public event ControlEventHandler OnLostFocus;

        /// <summary>
        /// Notifies when a mouse button has been pressed down on the Control
        /// </summary>
        public event MouseClickEventHandler OnMouseDown;

        /// <summary>
        /// Notifies when the mouse has entered the area of the Control
        /// </summary>
        public event MouseEventHandler OnMouseEnter;

        /// <summary>
        /// Notifies when the mouse has left the area of the Control
        /// </summary>
        public event MouseEventHandler OnMouseLeave;

        /// <summary>
        /// Notifies when the mouse has moved over the Control
        /// </summary>
        public event MouseEventHandler OnMouseMove;

        /// <summary>
        /// Notifies when a mouse button has been raised on the Control
        /// </summary>
        public event MouseClickEventHandler OnMouseUp;

        /// <summary>
        /// Gets or sets the ControlBorder used with this control (can be null)
        /// </summary>
        public ControlBorder Border
        {
            get { return _border; }
            set
            {
                if (Border == value)
                    return;

                _border = value;

                // The border changed, thus the ClientArea probably changed and we need to update the KeepInParent
                foreach (Control child in Controls)
                {
                    child.KeepInParent();
                }

                if (OnChangeBorder != null)
                    OnChangeBorder(this);
            }
        }

        /// <summary>
        /// Gets or sets if the Control supports movement by dragging with the mouse
        /// </summary>
        public bool CanDrag
        {
            get { return _canDrag; }
            set { _canDrag = value; }
        }

        /// <summary>
        /// Gets or sets if the Control can get focus
        /// </summary>
        public bool CanFocus
        {
            get { return _canFocus; }
            set { _canFocus = value; }
        }

        /// <summary>
        /// Gets the size of the client area (internal area, not including the borders) of the Control.
        /// </summary>
        public Vector2 ClientSize
        {
            get
            {
                ControlBorder b = Border;

                if (b == null)
                    return Size;

                return Size - new Vector2(b.Width, b.Height);
            }

            set
            {
                ControlBorder b = Border;

                if (b == null)
                    Size = value;
                else
                    Size = value + new Vector2(b.Width, b.Height);
            }
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
        /// Gets the GUIManager used by this Control
        /// </summary>
        public GUIManagerBase GUIManager
        {
            get { return _gui; }
        }

        /// <summary>
        /// Gets if the control currently has focus
        /// </summary>
        public bool HasFocus
        {
            get { return GUIManager.FocusedControl == this; }
        }

        /// <summary>
        /// Gets or sets if the control is bound to the area of its parent (control must have a parent)
        /// </summary>
        public bool IsBoundToParentArea
        {
            get { return _isBoundToParentArea; }
            set { _isBoundToParentArea = value; }
        }

        /// <summary>
        /// Gets or sets if the Control is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// Gets if the mouse has entered the Control (OnMouseEnter has been raised) and the mouse
        /// is still location in the Control's area (OnMouseLeave has not been raised).
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
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
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
                    return pos + parent.ScreenPosition + parent.BorderOffset();
                else
                    return pos;
            }
        }

        /// <summary>
        /// Gets the <see cref="ControlSettings"/> used by this <see cref="Control"/>.
        /// </summary>
        public ControlSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets or sets the absolute size of the Control. The Size includes the Border, while the ClientSize
        /// includes only the area inside of the Control's borders.
        /// </summary>
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
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
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="gui">The <see cref="GUIManagerBase"/> this Control will be part of.</param>
        /// <param name="settings">Default settings for this Control.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        /// <param name="parent">Parent Control of this Control (or null for a root Control).</param>
        protected Control(GUIManagerBase gui, ControlSettings settings, Vector2 position, Vector2 size, Control parent)
        {
            _settings = settings;
            _gui = gui;
            _parent = parent;

            Position = position;
            Size = size;

            // Set the settings
            Border = settings.Border;
            CanDrag = settings.CanDrag;
            CanFocus = settings.CanFocus;
            IsBoundToParentArea = settings.IsBoundToParentArea;
            IsEnabled = settings.IsEnabled;
            IsVisible = settings.IsVisible;

            // Check for a valid GUIManager
            if (GUIManager == null)
                throw new ArgumentNullException("gui", "GUIManager cannot be null.");

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
                // This control is the root, so add it directly to the GUIManager
                _root = this;
                gui.Add(this);
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
                _setTopMostQueue = new Queue<Control>(1);

            // Add the control to the queue
            _setTopMostQueue.Enqueue(control);
        }

        /// <summary>
        /// Gets a Vector2 for the offset created by the Border.
        /// </summary>
        /// <returns>A Vector2 for the offset created by the Border.</returns>
        Vector2 BorderOffset()
        {
            ControlBorder b = Border;

            if (b == null)
                return Vector2.Zero;

            return new Vector2(b.LeftWidth, b.TopHeight);
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
            }

            if (OnDispose != null)
                OnDispose(this);
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
        /// Draws the Control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        protected virtual void DrawControl(SpriteBatch spriteBatch)
        {
            if (Border != null)
                Border.Draw(spriteBatch, this);
        }

        /// <summary>
        /// Draws this Control and all of its children Controls
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
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
        /// Gets the ButtonState for the current and last state for a given mouse button.
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
        /// Finds the child Control in this Control at a given screen location
        /// </summary>
        /// <param name="point">Screen location to check</param>
        /// <returns>Child found at the given point if any, else null</returns>
        public Control GetChild(Vector2 point)
        {
            return GetChild(point, false);
        }

        /// <summary>
        /// Finds the child Control in this Control at a given screen location
        /// </summary>
        /// <param name="point">Screen location to check</param>
        /// <param name="canFocusOnly">If true, only controls that can get focus will be checked</param>
        /// <returns>Child found at the given point if any, else null</returns>
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
        /// Finds the Control's root Control
        /// </summary>
        /// <returns>Root Control for this Control</returns>
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
        /// Notifies the Control it has received focus
        /// </summary>
        internal void HandleFocus()
        {
            if (Parent != null)
                Parent.SetTopMostChild(this);

            if (OnFocus != null)
                OnFocus(this);
        }

        /// <summary>
        /// Notifies the Control it has lost focus
        /// </summary>
        internal void HandleLostFocus()
        {
            if (OnLostFocus != null)
                OnLostFocus(this);

            // Send OnMouseLeave if needed
            if (_isMouseEntered)
            {
                _isMouseEntered = false;
                if (OnMouseLeave != null)
                    OnMouseLeave(this, new MouseEventArgs(GUIManager.CursorPosition - ScreenPosition));
            }

            // End dragging if needed
            if (_isDragging)
            {
                _isDragging = false;
                if (OnEndDrag != null)
                    OnEndDrag(this);
            }
        }

        /// <summary>
        /// Checks if a point is in a given region
        /// </summary>
        /// <param name="point">Point to check if the region contains</param>
        /// <param name="min">Top-left corner of the region</param>
        /// <param name="max">Bottom-right corner of the region</param>
        /// <returns>If true, the region contains the point, else false</returns>
        public static bool IsPointInRegion(Vector2 point, Vector2 min, Vector2 max)
        {
            return point.X >= min.X && point.Y >= min.Y && point.X <= max.X && point.Y <= max.Y;
        }

        /// <summary>
        /// Adjusts the Control's position to keep it in the area of its parent
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
        /// Sets this Control as the top-most Control, if possible.
        /// </summary>
        void SetAsTopMost()
        {
            Control parent = Parent;
            if (parent != null)
                parent.SetTopMostChild(this);
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
                if (OnMouseDown != null)
                    OnMouseDown(this, new MouseClickEventArgs(button, relativePos));
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
                            if (OnBeginDrag != null)
                                OnBeginDrag(this);
                        }
                    }
                }
            }
            else
            {
                // Mouse was raised
                if (OnMouseUp != null)
                    OnMouseUp(this, new MouseClickEventArgs(button, relativePos));

                // Check if to raise a click event, too
                if (_willRaiseClick)
                {
                    if (OnClick != null)
                        OnClick(this, new MouseClickEventArgs(button, relativePos));
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
        /// Updates the Control for anything other than the Mouse or Keyboard
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
            if (OnKeyPress != null)
            {
                var pressedKeys = GUIManager.KeysPressed;
                if (pressedKeys != null && pressedKeys.Count() > 0)
                    OnKeyPress(this, new KeyboardEventArgs(pressedKeys, keyboardState));
            }

            // KeyDown event
            if (OnKeyDown != null)
            {
                var keysDown = GUIManager.NewKeysDown;
                if (keysDown != null && keysDown.Count() > 0)
                    OnKeyDown(this, new KeyboardEventArgs(keysDown, keyboardState));
            }

            // KeyUp event
            if (OnKeyUp != null)
            {
                var keysUp = GUIManager.NewKeysUp;
                if (keysUp != null && keysUp.Count() > 0)
                    OnKeyUp(this, new KeyboardEventArgs(keysUp, keyboardState));
            }
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
                    if (OnEndDrag != null)
                        OnEndDrag(this);
                }
            }

            Vector2 currCursorPos = new Vector2(mouseState.X, mouseState.Y);
            Vector2 sp = ScreenPosition;

            // Check if this is the Control being pointed at
            if (GUIManager.UnderCursor == this)
            {
                // Raise the OnMouseMove event
                if (OnMouseMove != null)
                    OnMouseMove(this, new MouseEventArgs(currCursorPos - sp));

                // Check if the mouse has just entered the control
                if (!_isMouseEntered)
                {
                    _isMouseEntered = true;
                    if (OnMouseEnter != null)
                        OnMouseEnter(this, new MouseEventArgs(currCursorPos - sp));
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
                    if (OnMouseLeave != null)
                        OnMouseLeave(this, new MouseEventArgs(currCursorPos - sp));
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the Control and all its resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}