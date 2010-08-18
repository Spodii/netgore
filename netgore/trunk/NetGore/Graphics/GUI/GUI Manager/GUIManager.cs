using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A generic implementation of <see cref="IGUIManager"/> to manage GUI components that should be suitable for most
    /// all GUI manager needs.
    /// </summary>
    public class GUIManager : IGUIManager
    {
        static Color _draggedItemColor = new Color(255, 255, 255, 150);

        readonly List<Control> _controls = new List<Control>(2);
        readonly ISkinManager _skinManager;
        readonly Tooltip _tooltip;
        readonly Window _window;

        IDragDropProvider _draggedDragDropProvider;
        IDragDropProvider _dropOntoControl;
        Control _focusedControl = null;
        Control _lastPressedControl;
        Vector2 _screenSize;
        Control _underCursor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GUIManager"/> class.
        /// </summary>
        /// <param name="window">The <see cref="Window"/> that provides the input.</param>
        /// <param name="font">Default SpriteFont to use for controls added to this <see cref="GUIManager"/>.</param>
        /// <param name="skinManager">The <see cref="ISkinManager"/> that handles the skinning for this
        /// <see cref="GUIManager"/>.</param>
        /// <param name="screenSize">The initial screen size value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="skinManager"/> is null.</exception>
        public GUIManager(Window window, Font font, ISkinManager skinManager, Vector2 screenSize)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (skinManager == null)
                throw new ArgumentNullException("skinManager");

            ScreenSize = screenSize;

            _window = window;
            _skinManager = skinManager;

            Font = font;

            // Create the tooltip
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _tooltip = CreateTooltip();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
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
            foreach (var child in root.Controls.Reverse())
            {
                var c = GetControlAtPoint(point, child);
                if (c != null)
                    return c;
            }

            // No child controls contained the point, so we found the deepest control containing the point
            return root;
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
        /// Updates what <see cref="Control"/> is currently under the cursor.
        /// </summary>
        /// <param name="e">The <see cref="SFML.Window.MouseMoveEventArgs"/> instance containing the event data.</param>
        void UpdateControlUnderCursor(MouseMoveEventArgs e)
        {
            var lastUnderCursor = UnderCursor;
            _underCursor = GetControlAtPoint(CursorPosition);

            // When the control under the cursor changes, handle the mouse enter/leave events
            var c = UnderCursor;
            if (lastUnderCursor != c)
            {
                if (lastUnderCursor != null)
                    lastUnderCursor.SendMouseLeaveEvent(e);

                if (c != null)
                    c.SendMouseEnterEvent(e);
            }
        }

        /// <summary>
        /// Updates what <see cref="Control"/> is currently under the cursor.
        /// </summary>
        /// <param name="x">The mouse X position.</param>
        /// <param name="y">The mouse Y position.</param>
        void UpdateControlUnderCursor(int x, int y)
        {
            MouseMoveEventArgs eArgs = null;

            var lastUnderCursor = UnderCursor;
            _underCursor = GetControlAtPoint(CursorPosition);

            // When the control under the cursor changes, handle the mouse enter/leave events
            var c = UnderCursor;
            if (lastUnderCursor != c)
            {
                if (lastUnderCursor != null)
                {
                    if (eArgs == null)
                        eArgs = new MouseMoveEventArgs(new MouseMoveEvent { X = x, Y = y });

                    lastUnderCursor.SendMouseLeaveEvent(eArgs);
                }

                if (c != null)
                {
                    if (eArgs == null)
                        eArgs = new MouseMoveEventArgs(new MouseMoveEvent { X = x, Y = y });

                    c.SendMouseEnterEvent(eArgs);
                }
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
        /// Gets the cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        public Vector2 CursorPosition
        {
            get { return new Vector2(_window.Input.GetMouseX(), _window.Input.GetMouseY()); }
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
        /// Gets the <see cref="Control"/> that was last the <see cref="IGUIManager.PressedControl"/>. Unlike
        /// <see cref="IGUIManager.PressedControl"/>, this value will not be set to null when the mouse button is raised.
        /// </summary>
        public Control LastPressedControl
        {
            get { return _lastPressedControl; }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> that the left mouse button was pressed down on. Will be null if the cursor
        /// was not over any <see cref="Control"/> when the left mouse button was pressed, or if the left mouse button is
        /// currently up.
        /// </summary>
        public Control PressedControl
        {
            get
            {
                if (!IsMouseButtonDown(MouseButton.Left))
                    return null;

                return _lastPressedControl;
            }
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
            foreach (var control in Controls)
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
            foreach (var child in Controls.Reverse())
            {
                // Start a recursive crawl through controls at the given point
                var control = GetControlAtPoint(point, child);

                // If the control returned is non-null, we have a control under the cursor
                if (control != null)
                    return control;
            }

            // No control at all was under the cursor
            return null;
        }

        /// <summary>
        /// Gets if a given <see cref="KeyCode"/> is currently being pressed.
        /// </summary>
        /// <param name="key">The <see cref="KeyCode"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="key"/> is currently being pressed; otherwise false.</returns>
        public bool IsKeyDown(KeyCode key)
        {
            return _window.Input.IsKeyDown(key);
        }

        /// <summary>
        /// Gets if a given <see cref="MouseButton"/> is currently being pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="button"/> is currently being pressed; otherwise false.</returns>
        public bool IsMouseButtonDown(MouseButton button)
        {
            return _window.Input.IsMouseButtonDown(button);
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
        /// Sends an event for a key being pressed to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventKeyPressed(KeyEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendKeyPressedEvent(e);
        }

        /// <summary>
        /// Sends an event for a key being released to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventKeyReleased(KeyEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendKeyReleasedEvent(e);
        }

        /// <summary>
        /// Sends an event for a mouse button being pressed to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventMouseButtonPressed(MouseButtonEventArgs e)
        {
            UpdateControlUnderCursor(e.X, e.Y);

            var c = UnderCursor;

            // Handle a left mouse button press
            if (e.Button == MouseButton.Left)
            {
                // Update the focus control
                var lastFocused = FocusedControl;
                FocusedControl = c;

                if (c != null)
                    _lastPressedControl = c;

                // If the focused control changed, send the corresponding events
                if (lastFocused != FocusedControl)
                {
                    // Tell the old control it lost focus
                    if (lastFocused != null)
                        lastFocused.SendLostFocusEvent(e);
                }

                // Tell the new control is acquired focus
                if (FocusedControl != null)
                {
                    FocusedControl.SendFocusedEvent(e);

                    // Check if the new focused control supports drag-and-drop
                    var ddp = _underCursor as IDragDropProvider;
                    if (ddp != null && ddp.CanDragContents && !_underCursor.CanDrag)
                        _draggedDragDropProvider = ddp;
                }
            }

            // Forward the event to the focused control, if any
            if (c != null)
                c.SendMouseButtonPressedEvent(e);
        }

        /// <summary>
        /// Sends an event for a mouse button being released to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventMouseButtonReleased(MouseButtonEventArgs e)
        {
            UpdateControlUnderCursor(e.X, e.Y);

            // If it was the left mouse button that was released, then stop dragging the drag-and-drop provider
            // control (if we have one)
            if (e.Button == MouseButton.Left && DraggedDragDropProvider != null)
            {
                if (DropOntoControl != null && DropOntoControl.CanDrop(DraggedDragDropProvider))
                    DropOntoControl.Drop(DraggedDragDropProvider);

                _draggedDragDropProvider = null;
            }

            // Forward the event to the control under the cursor
            var c = UnderCursor;
            if (c != null)
                c.SendMouseButtonReleasedEvent(e);
        }

        /// <summary>
        /// Sends an event for the mouse moving to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventMouseMoved(MouseMoveEventArgs e)
        {
            UpdateControlUnderCursor(e);

            var c = UnderCursor;

            // Update the DropOntoContorl
            if (DraggedDragDropProvider != null)
            {
                // Take the control we already found as being under the cursor. Then, create a loop that will check if
                // the control implements IDragDropProvider. If not, grab the parent. This will either give us the first
                // control to implement IDragDropProvider, or null.
                var tmpDropOntoControl = c;
                while (tmpDropOntoControl != null)
                {
                    var asDDP = tmpDropOntoControl as IDragDropProvider;

                    // If the control implements IDragDropProvider, and we can drop onto it, use that. Otherwise, move onto the parent.
                    if (asDDP != null && asDDP.CanDrop(DraggedDragDropProvider))
                        break;

                    tmpDropOntoControl = tmpDropOntoControl.Parent;
                }

                // Store the results of the above loop, which will be either a valid control we can drop on, or null
                _dropOntoControl = tmpDropOntoControl as IDragDropProvider;
            }
            else
            {
                // Nothing is being dragged, so don't take the time to calculate the control to drop onto
                _dropOntoControl = null;
            }

            // Send the mouse moved event
            if (c != null)
                c.SendMouseMoveEvent(e);
        }

        /// <summary>
        /// Sends an event for text being entered to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void SendEventTextEntered(TextEventArgs e)
        {
            var f = FocusedControl;
            if (f != null)
                f.SendTextEnteredEvent(e);
        }

        /// <summary>
        /// Updates the <see cref="IGUIManager"/> and all of the <see cref="Control"/>s in it.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(TickCount currentTime)
        {
            // Update the controls
            foreach (var control in Controls.Reverse())
            {
                control.Update(currentTime);
            }

            // If we have a cache of the control under the cursor, make sure that the control is valid. If not, then
            // update what control is under the cursor using the current cache of the cursor position.
            if (UnderCursor != null && (!UnderCursor.IsVisible || UnderCursor.IsDisposed))
                UpdateControlUnderCursor((int)CursorPosition.X, (int)CursorPosition.Y);

            // Update the tooltip
            Tooltip.Update(currentTime);
        }

        #endregion
    }
}