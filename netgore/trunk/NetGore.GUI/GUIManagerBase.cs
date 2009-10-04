using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base control for managing all of the GUI controls for a screen
    /// </summary>
    public class GUIManagerBase
    {
        readonly List<Control> _controls = new List<Control>(2);
        ButtonSettings _buttonSettings = ButtonSettings.Default;
        CheckBoxSettings _checkBoxSettings = CheckBoxSettings.Default;
        Control _focusedControl = null;
        FormSettings _formSettings = FormSettings.Default;
        bool _isKeysDownSet = false;
        bool _isKeysUpSet = false;
        KeyboardState _keyboardState;
        List<Keys> _keysDown = null;
        Keys[] _keysPressed = null;
        List<Keys> _keysUp = null;
        LabelSettings _labelSettings = LabelSettings.Default;
        KeyboardState _lastKeyboardState;
        MouseState _lastMouseState;
        Keys[] _lastPressedKeys = null;
        MouseState _mouseState;
        PanelSettings _panelSettings = PanelSettings.Default;
        PictureBoxSettings _pictureBoxSettings = PictureBoxSettings.Default;
        ISprite _spriteBlank = null;
        TextBoxSettings _textBoxSettings = TextBoxSettings.Default;
        Tooltip _tooltip;
        Control _underCursor;

        /// <summary>
        /// Raised when the focused control has changed
        /// </summary>
        public event GUIEventHandler OnChangeFocusedControl;

        /// <summary>
        /// Raised when the focused root control has changed
        /// </summary>
        public event GUIEventHandler OnChangeFocusedRoot;

        /// <summary>
        /// Gets or sets the default settings for new Buttons under this GUIManager
        /// </summary>
        public ButtonSettings ButtonSettings
        {
            get { return _buttonSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _buttonSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the default settings for new CheckBoxes under this GUIManager
        /// </summary>
        public CheckBoxSettings CheckBoxSettings
        {
            get { return _checkBoxSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _checkBoxSettings = value;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all the root Controls handled by this GUIManager.
        /// </summary>
        public IEnumerable<Control> Controls
        {
            get { return _controls; }
        }

        /// <summary>
        /// Gets the position of the cursor
        /// </summary>
        public Vector2 CursorPosition
        {
            get { return new Vector2(_mouseState.X, _mouseState.Y); }
        }

        /// <summary>
        /// Gets or sets the currently focused control
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

                if (OnChangeFocusedControl != null)
                    OnChangeFocusedControl(this);
            }
        }

        /// <summary>
        /// Gets the focused root control
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
        /// Gets or sets the default SpriteFont for new controls added to this GUIManager
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Gets or sets the default settings for new Forms under this GUIManager
        /// </summary>
        public FormSettings FormSettings
        {
            get { return _formSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _formSettings = value;
            }
        }

        /// <summary>
        /// Gets the current KeyboardState
        /// </summary>
        public KeyboardState KeyboardState
        {
            get { return _keyboardState; }
        }

        /// <summary>
        /// Gets an IEnumerable of Keys that are currently down.
        /// </summary>
        public IEnumerable<Keys> KeysPressed
        {
            get { return _keysPressed; }
        }

        /// <summary>
        /// Gets or sets the default settings for new Labels under this GUIManager
        /// </summary>
        public LabelSettings LabelSettings
        {
            get { return _labelSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _labelSettings = value;
            }
        }

        /// <summary>
        /// Gets the KeyboardState used for the previous update
        /// </summary>
        public KeyboardState LastKeyboardState
        {
            get { return _lastKeyboardState; }
        }

        /// <summary>
        /// Gets the MouseState used for the previous update
        /// </summary>
        public MouseState LastMouseState
        {
            get { return _lastMouseState; }
        }

        /// <summary>
        /// Gets an IEnumerable of Keys that were down the previous update
        /// </summary>
        public IEnumerable<Keys> LastPressedKeys
        {
            get { return _lastPressedKeys; }
        }

        /// <summary>
        /// Gets the current MouseState
        /// </summary>
        public MouseState MouseState
        {
            get { return _mouseState; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the keys that were up the last frame and down this frame.
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
        /// Gets an IEnumerable of all keys that were down the last frame and up this frame.
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
        /// Gets or sets the default settings for new Panels under this GUIManager
        /// </summary>
        public PanelSettings PanelSettings
        {
            get { return _panelSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _panelSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets the default settings for new PictureBoxes under this GUIManager
        /// </summary>
        public PictureBoxSettings PictureBoxSettings
        {
            get { return _pictureBoxSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _pictureBoxSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets a Sprite that is only Color.White (size does not matter). Used by multiple Controls
        /// for a variety of reasons.
        /// </summary>
        public ISprite SpriteBlank
        {
            get { return _spriteBlank; }
            set
            {
                if (value == null)
                {
                    Debug.Fail("Cannot set SpriteBlank to null.");

                    // If the _spriteBlank is already set to non-null, we can try to recover
                    // from the exception by just not setting the value
                    if (_spriteBlank != null)
                        return;

                    throw new Exception("Cannot set SpriteBlank to null.");
                }

                _spriteBlank = value;
            }
        }

        /// <summary>
        /// Gets or sets the default settings for new TextBoxes under this GUIManager
        /// </summary>
        public TextBoxSettings TextBoxSettings
        {
            get { return _textBoxSettings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _textBoxSettings = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Tooltip"/> used by this <see cref="GUIManagerBase"/>.
        /// </summary>
        public Tooltip Tooltip
        {
            get { return _tooltip; }
            internal set { _tooltip = value; }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> currently under the cursor, or null if no <see cref="Control"/> managed 
        /// by this GUIManager is currently under the cursor.
        /// </summary>
        public Control UnderCursor
        {
            get { return _underCursor; }
        }

        /// <summary>
        /// GUIManager constructor
        /// </summary>
        /// <param name="font">Default SpriteFont to use for controls added to this GUIManager</param>
        /// <param name="blank">A Sprite that is only Color.White (size does not matter). Used by multiple Controls
        /// for a variety of reasons.</param>
        public GUIManagerBase(SpriteFont font, ISprite blank)
        {
            Font = font;
            SpriteBlank = blank;

            new Tooltip(this);

            // Store the input state from now so we have something to worth with on the first Update() call
            _mouseState = Mouse.GetState();
            _keyboardState = Keyboard.GetState();
            _keysPressed = _keyboardState.GetPressedKeys();
        }

        /// <summary>
        /// GUIManager constructor
        /// </summary>
        /// <param name="gui">GUIManager to copy the settings from. This will not copy over
        /// the individual GUI components.</param>
        public GUIManagerBase(GUIManagerBase gui) : this(gui.Font, gui.SpriteBlank)
        {
            ButtonSettings = gui.ButtonSettings;
            CheckBoxSettings = gui.CheckBoxSettings;
            FormSettings = gui.FormSettings;
            LabelSettings = gui.LabelSettings;
            PanelSettings = gui.PanelSettings;
            TextBoxSettings = gui.TextBoxSettings;
        }

        /// <summary>
        /// Adds a control to this GUIMangaer
        /// </summary>
        /// <param name="c">Control to add</param>
        internal void Add(Control c)
        {
            if (c == null)
            {
                Debug.Fail("c is null.");
                return;
            }
            if (c.Parent != null)
            {
                Debug.Fail("Only root controls (controls with no parent) may be added.");
                return;
            }

            _controls.Add(c);
            c.SetFocus();
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
        /// Draws all of the controls in this GUIManager
        /// </summary>
        /// <param name="sb">SpriteBatch to draw the controls to</param>
        public void Draw(SpriteBatch sb)
        {
            // Iterate forward through the list so the last control is on top
            foreach (Control control in Controls)
            {
                control.Draw(sb);
            }

            // Draw the tooltip
            Tooltip.Draw(sb);
        }

        /// <summary>
        /// Gets all of the <see cref="Control"/>s in this <see cref="GUIManagerBase"/>.
        /// </summary>
        /// <returns>All of the <see cref="Control"/>s in this <see cref="GUIManagerBase"/>.</returns>
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
        /// Gets the top-most Control at the given point.
        /// </summary>
        /// <param name="point">Point to find the top-most Control at.</param>
        /// <returns>The Control at the given <paramref name="point"/>, or null if no Control was found
        /// at the given point.</returns>
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
        /// Gets the top-most Control at a given point from a root control. This is intended as a sub-method
        /// for GetControlAtPoint().
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
            if (lastPressed == null)
            {
                Debug.Fail("lastPressed is null.");
                return null;
            }
            if (pressed.Count == 0)
                return null;

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
            if (lastPressed == null)
            {
                Debug.Fail("lastPressed is null.");
                return null;
            }

            if (lastPressed.Count == 0)
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
        /// Remove a control from this GUIManager
        /// </summary>
        /// <param name="c">Control to remove</param>
        internal void Remove(Control c)
        {
            if (c == null)
            {
                Debug.Fail("c is null.");
                return;
            }
            Debug.Assert(c.Parent == null, "Only root controls (controls with no parent) need to be removed.");

            _controls.Remove(c);
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

                if (OnChangeFocusedRoot != null)
                    OnChangeFocusedRoot(this);
            }
        }

        /// <summary>
        /// Updates all of the controls in the GUIManager.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
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
    }
}