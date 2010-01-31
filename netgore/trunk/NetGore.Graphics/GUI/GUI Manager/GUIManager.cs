using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A generic implementation of <see cref="IGUIManager"/> to manage GUI components that should be suitable for most
    /// all GUI manager needs.
    /// </summary>
    public class GUIManager : IGUIManager
    {
        readonly List<Control> _controls = new List<Control>(2);
        readonly ISkinManager _skinManager;
        readonly Tooltip _tooltip;
        Control _focusedControl = null;
        bool _isKeysDownSet = false;
        bool _isKeysUpSet = false;
        KeyboardState _keyboardState;
        List<Keys> _keysDown = null;
        Keys[] _keysPressed = null;
        List<Keys> _keysUp = null;
        KeyboardState _lastKeyboardState;
        MouseState _lastMouseState;
        Keys[] _lastPressedKeys = null;
        MouseState _mouseState;
        Control _underCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIManager"/> class.
        /// </summary>
        /// <param name="font">Default SpriteFont to use for controls added to this <see cref="GUIManager"/>.</param>
        /// <param name="skinManager">The <see cref="ISkinManager"/> that handles the skinning for this
        /// <see cref="GUIManager"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="skinManager"/> is null.</exception>
        public GUIManager(SpriteFont font, ISkinManager skinManager)
        {
            if (skinManager == null)
                throw new ArgumentNullException();

            _skinManager = skinManager;
            Font = font;

            // Store the input state from now so we have something to worth with on the first Update() call
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            _keysPressed = _keyboardState.GetPressedKeys();

            // Create the tooltip
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _tooltip = CreateTooltip();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Gets the <see cref="Tooltip"/> used by this <see cref="GUIManager"/>.
        /// </summary>
        public Tooltip Tooltip
        {
            get { return _tooltip; }
        }

        /// <summary>
        /// Checks if a collection of keys contains a given key
        /// </summary>
        /// <param name="array">Collection of keys to check</param>
        /// <param name="key">Key to check for</param>
        /// <returns>True if the array contains the requested key, else false</returns>
        static bool CollectionContainsKey(IEnumerable<Keys> array, Keys key)
        {
            if (array == null)
            {
                Debug.Fail("array is null.");
                return false;
            }

            // Iterate through every array element and check for a match
            foreach (Keys i in array)
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
        static List<Keys> GetNewKeysDown(ICollection<Keys> pressed, IEnumerable<Keys> lastPressed)
        {
            if (pressed == null)
            {
                Debug.Fail("pressed is null.");
                return null;
            }
            
            if (pressed.Count == 0)
                return null;

            if (lastPressed == null)
                return new List<Keys>(pressed);

            var ret = new List<Keys>(pressed.Count);

            foreach (Keys key in pressed)
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
        static List<Keys> GetNewKeysUp(IEnumerable<Keys> pressed, ICollection<Keys> lastPressed)
        {
            if (pressed == null)
            {
                Debug.Fail("pressed is null.");
                return null;
            }

            if (lastPressed == null || lastPressed.Count == 0)
                return null;

            var ret = new List<Keys>(lastPressed.Count);

            foreach (Keys lastKey in lastPressed)
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
            // Only check for a new root if the mouse was clicked
            if (LastMouseState.LeftButton != ButtonState.Released || MouseState.LeftButton != ButtonState.Pressed)
                return;

            // First root control to contain the mouse position is set as the focused root
            // Controls are iterated in reverse to respect the order they were drawn
            Vector2 screenPos = new Vector2(MouseState.X, MouseState.Y);

            foreach (Control control in Controls.Reverse())
            {
                if (!control.ContainsPoint(screenPos))
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
        /// Gets the screen coordinates of the cursor.
        /// </summary>
        /// <value></value>
        public Vector2 CursorPosition
        {
            get { return new Vector2(_mouseState.X, _mouseState.Y); }
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

                Control oldFocusedControl = _focusedControl;
                _focusedControl = value;

                // Set the new focus control
                if (_focusedControl != null)
                {
                    _focusedControl.HandleFocus();
                    SetFocusedRoot(_focusedControl.Root);
                }

                // Notify the old focused control that it has lost focus
                if (oldFocusedControl != null)
                    oldFocusedControl.HandleLostFocus();

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
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Gets the latest <see cref="KeyboardState"/>. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        public KeyboardState KeyboardState
        {
            get { return _keyboardState; }
        }

        /// <summary>
        /// Gets an IEnumerable of all keys that are currently down. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        public IEnumerable<Keys> KeysPressed
        {
            get { return _keysPressed; }
        }

        /// <summary>
        /// Gets the <see cref="KeyboardState"/> that was used immediately before the current
        /// <see cref="IGUIManager.KeyboardState"/>.
        /// </summary>
        public KeyboardState LastKeyboardState
        {
            get { return _lastKeyboardState; }
        }

        /// <summary>
        /// Gets the <see cref="MouseState"/> that was used immediately before the current
        /// <see cref="IGUIManager.MouseState"/>.
        /// </summary>
        public MouseState LastMouseState
        {
            get { return _lastMouseState; }
        }

        /// <summary>
        /// Gets the IEnumerable of <see cref="Keys"/> that was used immediately before the current
        /// <see cref="IGUIManager.KeysPressed"/>.
        /// </summary>
        public IEnumerable<Keys> LastKeysPressed
        {
            get { return _lastPressedKeys; }
        }

        /// <summary>
        /// Gets the latest <see cref="MouseState"/>. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        public MouseState MouseState
        {
            get { return _mouseState; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="Keys"/> that were up during the previous call to
        /// <see cref="IGUIManager.Update"/> but are down on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        public IEnumerable<Keys> NewKeysDown
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
                return _keysDown;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="Keys"/> that were down during the previous call to
        /// <see cref="IGUIManager.Update"/> but are up on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        public IEnumerable<Keys> NewKeysUp
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
                return _keysUp;
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
        /// Draws all of the <see cref="Control"/>s in this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to use for drawing the <see cref="Control"/>s.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Iterate forward through the list so the last control is on top
            foreach (Control control in Controls)
            {
                control.Draw(spriteBatch);
            }

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
        /// Updates the <see cref="IGUIManager"/> and all of the <see cref="Control"/>s in it.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(int currentTime)
        {
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

            // Update the controls
            _underCursor = GetControlAtPoint(CursorPosition);
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