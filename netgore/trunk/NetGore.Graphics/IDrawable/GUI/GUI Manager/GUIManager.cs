using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    // TODO: ##2 Drag & Drop

    /// <summary>
    /// A generic implementation of <see cref="IGUIManager"/> to manage GUI components that should be suitable for most
    /// all GUI manager needs.
    /// </summary>
    public class GUIManager : IGUIManager
    {
        static readonly IEnumerable<KeyCode> _emptyKeys = new KeyCode[0];
        static Color _draggedItemColor = new Color(255, 255, 255, 150);

        readonly List<Control> _controls = new List<Control>(2);
        readonly ISkinManager _skinManager;
        readonly Tooltip _tooltip;

        IDragDropProvider _draggedDragDropProvider;
        IDragDropProvider _dropOntoControl;
        Control _focusedControl = null;
        bool _isKeysDownSet = false;
        bool _isKeysUpSet = false;
        // TODO: ## KeyboardState _keyboardState;
        List<KeyCode> _keysDown = null;
        KeyCode[] _keysPressed = null;
        List<KeyCode> _keysUp = null;
        // TODO: ## KeyboardState _lastKeyboardState;
        // TODO: ## MouseState _lastMouseState;
        KeyCode[] _lastPressedKeys = null;
        // TODO: ## MouseState _mouseState;
        Vector2 _screenSize;
        Control _underCursor;
        RenderWindow _rw;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIManager"/> class.
        /// </summary>
        /// <param name="font">Default SpriteFont to use for controls added to this <see cref="GUIManager"/>.</param>
        /// <param name="skinManager">The <see cref="ISkinManager"/> that handles the skinning for this
        /// <see cref="GUIManager"/>.</param>
        /// <param name="screenSize">The initial screen size value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="skinManager"/> is null.</exception>
        public GUIManager(RenderWindow rw, Font font, ISkinManager skinManager, Vector2 screenSize)
        {
            if (rw == null)
                throw new ArgumentNullException("rw");
            if (skinManager == null)
                throw new ArgumentNullException("skinManager");

            ScreenSize = screenSize;

            _rw = rw;
            _skinManager = skinManager;

            Font = font;

            // Create the tooltip
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _tooltip = CreateTooltip();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public void SendEventMouseButtonReleased(MouseButtonEventArgs e)
        {
            var c = UnderCursor;
            if (c != null)
                c.SendMouseButtonReleasedEvent(e);
        }

        public void SendEventMouseMoved(MouseMoveEventArgs e)
        {
            var lastUnderCursor = UnderCursor;
            _underCursor = GetControlAtPoint(CursorPosition);

            var c = UnderCursor;
            if (lastUnderCursor != c)
            {
                if (lastUnderCursor != null)
                    lastUnderCursor.SendMouseLeaveEvent(e);

                if (c != null)
                    c.SendMouseEnterEvent(e);
            }
        }

        public void SendEventMouseButtonPressed(MouseButtonEventArgs e)
        {
            var c = UnderCursor;

            if (e.Button == MouseButton.Left)
            {
                var lastFocused = FocusedControl;
                FocusedControl = c;

                if (c != null)
                    _lastPressedControl = c;

                if (lastFocused != FocusedControl)
                {
                    if (lastFocused != null)
                        lastFocused.SendLostFocusEvent(e);

                    if (FocusedControl != null)
                        FocusedControl.SendFocusedEvent(e);
                }
            }

            if (c != null)
                c.SendMouseButtonPressedEvent(e);
        }

        public void SendEventTextEntered(TextEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendTextEnteredEvent(e);
        }

        public void SendEventKeyReleased(KeyEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendKeyReleasedEvent(e);
        }

        public void SendEventKeyPressed(KeyEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendKeyPressedEvent(e);
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the <see cref="IDragDropProvider"/> item currently being dragged.
        /// </summary>
        public static Color DraggedItemColor
        {
            get { return _draggedItemColor; }
            set { _draggedItemColor = value; }
        }

        /// <summary>
        /// Checks if a collection of keys contains a given key
        /// </summary>
        /// <param name="array">Collection of keys to check</param>
        /// <param name="key">Key to check for</param>
        /// <returns>True if the array contains the requested key, else false</returns>
        static bool CollectionContainsKey(IEnumerable<KeyCode> array, KeyCode key)
        {
            if (array == null)
            {
                Debug.Fail("array is null.");
                return false;
            }

            // Iterate through every array element and check for a match
            foreach (KeyCode i in array)
            {
                if (i == key)
                    return true;
            }

            // No matches found
            return false;
        }

        /// <summary>
        /// Creates the <see cref="Tooltip"/> to be used with this <see cref="GUIManager"/>.
        /// </summary>
        /// <returns>The <see cref="Tooltip"/> to be used with this <see cref="GUIManager"/>. Can be null
        /// if no <see cref="Tooltip"/> is to be used.</returns>
        protected virtual Tooltip CreateTooltip()
        {
            return new Tooltip(this);
        }

        /// <summary>
        /// Gets the top-most <see cref="Control"/> at a given point from a root <see cref="Control"/>.
        /// </summary>
        /// <param name="point">Point to find the top-most Control at.</param>
        /// <param name="root">Root control to look from.</param>
        /// <returns>Returns the top-most Control at the given <paramref name="point"/> contained in the
        /// <paramref name="root"/> Control, or the <paramref name="root"/> Control if no child Controls
        /// were found at the given <paramref name="point"/>, or null if the <paramref name="root"/> was
        /// not even at the given <paramref name="point"/>.</returns>
        static Control GetControlAtPoint(Vector2 point, Control root)
        {
            // Do not return invisible controls
            if (!root.IsVisible)
                return null;

            // Check that the root contains the point
            if (!root.ContainsPoint(point))
                return null;

            // Crawl backwards through each child control until we find one that also contains the point
            foreach (Control child in root.Controls.Reverse())
            {
                Control c = GetControlAtPoint(point, child);
                if (c != null)
                    return c;
            }

            // No child controls contained the point, so we found the deepest control containing the point
            return root;
        }

        /// <summary>
        /// Gets the newly pressed keys (keys that are down this frame, but were not down last frame)
        /// </summary>
        /// <param name="pressed">Collection of currently pressed keys</param>
        /// <param name="lastPressed">Collection of previously pressed keys</param>
        /// <returns>List of all the newly pressed keys, or null if there are none</returns>
        static List<KeyCode> GetNewKeysDown(ICollection<KeyCode> pressed, IEnumerable<KeyCode> lastPressed)
        {
            if (pressed == null || pressed.Count == 0)
                return null;

            if (lastPressed == null)
                return new List<KeyCode>(pressed);

            var ret = new List<KeyCode>(pressed.Count);

            foreach (KeyCode key in pressed)
            {
                if (!CollectionContainsKey(lastPressed, key))
                    ret.Add(key);
            }

            return ret;
        }

        /// <summary>
        /// Gets the newly pressed released (keys that are up this frame, but were not up last frame)
        /// </summary>
        /// <param name="pressed">Collection of currently pressed keys</param>
        /// <param name="lastPressed">Collection of previously pressed keys</param>
        /// <returns>List of all the newly released keys, or null if there are none</returns>
        static List<KeyCode> GetNewKeysUp(IEnumerable<KeyCode> pressed, ICollection<KeyCode> lastPressed)
        {
            if (pressed == null || lastPressed == null || lastPressed.Count == 0)
                return null;

            var ret = new List<KeyCode>(lastPressed.Count);

            foreach (KeyCode lastKey in lastPressed)
            {
                if (!CollectionContainsKey(pressed, lastKey))
                    ret.Add(lastKey);
            }

            return ret;
        }

        /// <summary>
        /// Sets the focused root control
        /// </summary>
        /// <param name="newFocusedRoot">New focused root control</param>
        void SetFocusedRoot(Control newFocusedRoot)
        {
            if (newFocusedRoot == null)
            {
                Debug.Fail("newFocuesdRoot is null.");
                return;
            }
            if (!newFocusedRoot.IsRoot)
            {
                Debug.Fail("newFocuesdRoot is not a root control.");
                return;
            }

            // Check that the control is not already the focused root control
            if (newFocusedRoot != FocusedRoot)
            {
                // Remove the control then add it back to place it at the top of the list
                _controls.Remove(newFocusedRoot);
                _controls.Add(newFocusedRoot);

                if (FocusedRootChanged != null)
                    FocusedRootChanged(this);
            }
        }

        /// <summary>
        /// Finds the new focused root control (if any)
        /// </summary>
        void UpdateFocusedRoot()
        {
            // First root control to contain the mouse position is set as the focused root
            // Controls are iterated in reverse to respect the order they were drawn
            Vector2 pos = CursorPosition;

            foreach (Control control in Controls.Reverse())
            {
                if (!control.ContainsPoint(pos))
                    continue;

                SetFocusedRoot(control.Root);
                return;
            }
        }

        #region IGUIManager Members

        /// <summary>
        /// Notifies listeners when the focused <see cref="Control"/> has changed.
        /// </summary>
        public event GUIEventHandler FocusedControlChanged;

        /// <summary>
        /// Notifies listeners when the focused root <see cref="Control"/> has changed.
        /// </summary>
        public event GUIEventHandler FocusedRootChanged;

        /// <summary>
        /// Gets an IEnumerable of all the root <see cref="Control"/>s handled by this <see cref="IGUIManager"/>. This
        /// only contains the top-level <see cref="Control"/>s, not any of the child <see cref="Control"/>s.
        /// </summary>
        public IEnumerable<Control> Controls
        {
            get { return _controls; }
        }

        /// <summary>
        /// Gets the <see cref="IDragDropProvider"/> that is currently being dragged for drag-and-drop. Not to
        /// be confused with dragging a <see cref="Control"/> that supports being dragged.
        /// </summary>
        public IDragDropProvider DraggedDragDropProvider
        {
            get { return _draggedDragDropProvider; }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> that implements <see cref="IDragDropProvider"/> that is under the cursor
        /// and for which <see cref="IDragDropProvider.CanDrop"/> returns true for the
        /// <see cref="IGUIManager.DraggedDragDropProvider"/>.
        /// Only valid for when <see cref="IGUIManager.DraggedDragDropProvider"/> is not null. Will be null if there
        /// is no <see cref="Control"/> under the cursor, or none of the <see cref="Control"/>s under the cursor
        /// implement the <see cref="IDragDropProvider"/> interface.
        /// </summary>
        public IDragDropProvider DropOntoControl
        {
            get { return _dropOntoControl; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> that currently has focus. If the <paramref name="value"/> is null
        /// or <see cref="Control.CanFocus"/> is false, the setter will do nothing.
        /// </summary>
        public Control FocusedControl
        {
            get { return _focusedControl; }
            set
            {
                // Ensure the control is not already the focus control
                if (_focusedControl == value)
                    return;

                // Ensure the new control can receive focus
                if (value != null && !value.CanFocus)
                    return;

                _focusedControl = value;

                // Set the new focus control
                if (_focusedControl != null)
                {
                    _focusedControl.HandleFocus();
                    SetFocusedRoot(_focusedControl.Root);
                }

                if (FocusedControlChanged != null)
                    FocusedControlChanged(this);
            }
        }

        /// <summary>
        /// Gets the top-level <see cref="Control"/> that currently has focus, or has a child <see cref="Control"/>
        /// that has focus.
        /// </summary>
        public Control FocusedRoot
        {
            get
            {
                // If theres no root controls, there can't be a root control to has focus
                if (_controls.Count == 0)
                    return null;

                // Return the last control in the root controls list
                return _controls[_controls.Count - 1];
            }
        }

        /// <summary>
        /// Gets or sets the default font for new <see cref="Control"/>s added to this <see cref="IGUIManager"/>.
        /// Can be null, but having this value null may result in certain <see cref="Control"/>s that require
        /// a default font throwing an <see cref="Exception"/>.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets if a given <see cref="KeyCode"/> is currently being pressed.
        /// </summary>
        /// <param name="key">The <see cref="KeyCode"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="key"/> is currently being pressed; otherwise false.</returns>
        public bool IsKeyDown(KeyCode key)
        {
            return _rw.Input.IsKeyDown(key);
        }

        /// <summary>
        /// Gets if a given <see cref="MouseButton"/> is currently being pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="button"/> is currently being pressed; otherwise false.</returns>
        public bool IsMouseButtonDown(MouseButton button)
        {
            return _rw.Input.IsMouseButtonDown(button);
        }

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        public Vector2 CursorPosition
        {
            get { return new Vector2(_rw.Input.GetMouseX(), _rw.Input.GetMouseY()); }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="KeyCode"/> that were up during the previous call to
        /// <see cref="IGUIManager.Update"/> but are down on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        public IEnumerable<KeyCode> NewKeysDown
        {
            get
            {
                // If we have not found the pressed keys yet, then find them
                if (!_isKeysDownSet)
                {
                    _keysDown = GetNewKeysDown(_keysPressed, _lastPressedKeys);
                    _isKeysDownSet = true;
                }

                // Return the cached pressed keys list
                return _keysDown ?? _emptyKeys;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="KeyCode"/> that were down during the previous call to
        /// <see cref="IGUIManager.Update"/> but are up on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        public IEnumerable<KeyCode> NewKeysUp
        {
            get
            {
                // If we have not found the released keys yet, then find them
                if (!_isKeysUpSet)
                {
                    _keysUp = GetNewKeysUp(_keysPressed, _lastPressedKeys);
                    _isKeysUpSet = true;
                }

                // Return the cached released keys list
                return _keysUp ?? _emptyKeys;
            }
        }

        Control _lastPressedControl;

        /// <summary>
        /// Gets the <see cref="Control"/> that the left mouse button was pressed down on. Will be null if the cursor
        /// was not over any <see cref="Control"/> when the left mouse button was pressed, or if the left mouse button is
        /// currently up.
        /// </summary>
        public Control PressedControl
        {
            get {
                if (!IsMouseButtonDown(MouseButton.Left))
                    return null;

                return _lastPressedControl; }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> that was last the <see cref="IGUIManager.PressedControl"/>. Unlike
        /// <see cref="IGUIManager.PressedControl"/>, this value will not be set to null when the mouse button is raised.
        /// </summary>
        public Control LastPressedControl
        {
            get { return _lastPressedControl; }
        }

        /// <summary>
        /// Gets or sets the size of the screen.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Either the x- or y-coordinate of the <paramref name="value"/>
        /// is less than or equal to zero.</exception>
        public Vector2 ScreenSize
        {
            get { return _screenSize; }
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                    throw new ArgumentOutOfRangeException("value");

                _screenSize = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISkinManager"/> used by this <see cref="IGUIManager"/> to perform
        /// all of the GUI skinning.
        /// </summary>
        public ISkinManager SkinManager
        {
            get { return _skinManager; }
        }

        /// <summary>
        /// Gets the <see cref="Tooltip"/> used by this <see cref="GUIManager"/>.
        /// </summary>
        public ITooltip Tooltip
        {
            get { return _tooltip; }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> currently under the cursor, or null if no <see cref="Control"/> managed 
        /// by this <see cref="IGUIManager"/> is currently under the cursor.
        /// </summary>
        public Control UnderCursor
        {
            get { return _underCursor; }
        }

        /// <summary>
        /// Adds a <see cref="Control"/> to this <see cref="IGUIManager"/> at the root level. This should only be called
        /// by the <see cref="Control"/>'s constructor.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="control"/> is not a root <see cref="Control"/>.</exception>
        void IGUIManager.Add(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            if (!control.IsRoot)
                throw new ArgumentException("Only root controls may be added to the IGUIManager directly.");

            _controls.Add(control);
        }

        /// <summary>
        /// Draws all of the <see cref="Control"/>s in this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing the <see cref="Control"/>s.</param>
        public void Draw(ISpriteBatch spriteBatch)
        {
            // Iterate forward through the list so the last control is on top
            foreach (Control control in Controls)
            {
                control.Draw(spriteBatch);
            }

            // Draw the item being dragged
            if (DraggedDragDropProvider != null)
                DraggedDragDropProvider.DrawDraggedItem(spriteBatch, CursorPosition, DraggedItemColor);

            // Draw the tooltip
            Tooltip.Draw(spriteBatch);
        }

        /// <summary>
        /// Gets all of the <see cref="Control"/>s in this <see cref="GUIManager"/>, including all
        /// child <see cref="Control"/>s.
        /// </summary>
        /// <returns>All of the <see cref="Control"/>s in this <see cref="GUIManager"/>.</returns>
        public IEnumerable<Control> GetAllControls()
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
        /// Gets the top-most <see cref="Control"/> at the given point.
        /// </summary>
        /// <param name="point">Point to find the top-most <see cref="Control"/> at.</param>
        /// <returns>The <see cref="Control"/> at the given <paramref name="point"/>, or null if no
        /// <see cref="Control"/> was found at the given <paramref name="point"/>.</returns>
        public Control GetControlAtPoint(Vector2 point)
        {
            // Enumerate through the controls in reverse until we find the first Control containing
            // our desired point. We will have to enumerate in reverse because we want to check the
            // top-most controls first, which are contained at the end of the collection.
            foreach (Control child in Controls.Reverse())
            {
                // Start a recursive crawl through controls at the given point
                Control control = GetControlAtPoint(point, child);

                // If the control returned is non-null, we have a control under the cursor
                if (control != null)
                    return control;
            }

            // No control at all was under the cursor
            return null;
        }

        /// <summary>
        /// Remove a <see cref="Control"/> from this <see cref="IGUIManager"/> from the root level. This should only be called
        /// by the <see cref="Control"/>'s constructor.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to remove.</param>
        /// <returns>True if the <paramref name="control"/> was successfully removed; false if the <paramref name="control"/>
        /// could not be removed or was not in this <see cref="IGUIManager"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is null.</exception>
        bool IGUIManager.Remove(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");

            return _controls.Remove(control);
        }

        /// <summary>
        /// Updates the <see cref="IGUIManager"/> and all of the <see cref="Control"/>s in it.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(int currentTime)
        {
            // TODO: ## Input
            /*
            // Set the last state
            _lastMouseState = _mouseState;
            _lastKeyboardState = _keyboardState;
            _lastPressedKeys = _keysPressed;

            // Get the new current state
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            _keysPressed = _keyboardState.GetPressedKeys();

            // Since the state has changed, we will have to recalculate the KeysDown/KeysUp if requested
            _isKeysDownSet = false;
            _isKeysUpSet = false;

            // Update which root is focused
            UpdateFocusedRoot();
            _underCursor = GetControlAtPoint(CursorPosition);

            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                // Check if the left mouse button has been freshly pressed. If so, check if the control under the cursor
                // supports drag/drop.
                if (LastMouseState.LeftButton == ButtonState.Released)
                {
                    var ddp = _underCursor as IDragDropProvider;
                    if (ddp != null && ddp.CanDragContents && !_underCursor.CanDrag)
                        _draggedDragDropProvider = ddp;
                }
            }
            else
            {
                // Since the left mouse button is up, release the drag/drop provider if we have one
                if (DraggedDragDropProvider != null)
                {
                    if (DropOntoControl != null && DropOntoControl.CanDrop(DraggedDragDropProvider))
                        DropOntoControl.Drop(DraggedDragDropProvider);

                    _draggedDragDropProvider = null;
                }
            }
            */

            // Update the DropOntoContorl
            if (DraggedDragDropProvider != null)
            {
                // Take the control we already found as being under the cursor. Then, create a loop that will check if
                // the control implements IDragDropProvider. If not, grab the parent. This will either give us the first
                // control to implement IDragDropProvider, or null.
                var c = UnderCursor;
                IDragDropProvider asDDP;
                while (c != null && (asDDP = c as IDragDropProvider) != null && !asDDP.CanDrop(DraggedDragDropProvider))
                {
                    c = c.Parent;
                }

                _dropOntoControl = c as IDragDropProvider;
                if (_dropOntoControl != null && !_dropOntoControl.CanDrop(DraggedDragDropProvider))
                    _dropOntoControl = null;
            }
            else
            {
                // Nothing is being dragged, so don't take the time to calculate the control to drop onto
                _dropOntoControl = null;
            }

            // Update the controls
            foreach (Control control in Controls.Reverse())
            {
                control.Update(currentTime);
            }

            // Update the tooltip
            Tooltip.Update(currentTime);
        }

        #endregion
    }
}