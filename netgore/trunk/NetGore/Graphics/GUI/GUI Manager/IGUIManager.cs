using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a class that manages multiple GUI components.
    /// </summary>
    public interface IGUIManager
    {
        /// <summary>
        /// Notifies listeners when the focused <see cref="Control"/> has changed.
        /// </summary>
        event TypedEventHandler<GUIManager> FocusedControlChanged;

        /// <summary>
        /// Gets an IEnumerable of all the root <see cref="Control"/>s handled by this <see cref="IGUIManager"/>. This
        /// only contains the top-level <see cref="Control"/>s, not any of the child <see cref="Control"/>s.
        /// </summary>
        IEnumerable<Control> Controls { get; }

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        Vector2 CursorPosition { get; }

        /// <summary>
        /// Gets the <see cref="IDragDropProvider"/> that is currently being dragged for drag-and-drop. Not to
        /// be confused with dragging a <see cref="Control"/> that supports being dragged.
        /// </summary>
        IDragDropProvider DraggedDragDropProvider { get; }

        /// <summary>
        /// Gets the <see cref="Control"/> that implements <see cref="IDragDropProvider"/> that is under the cursor
        /// and for which <see cref="IDragDropProvider.CanDrop"/> returns true for the
        /// <see cref="IGUIManager.DraggedDragDropProvider"/>.
        /// Only valid for when <see cref="IGUIManager.DraggedDragDropProvider"/> is not null. Will be null if there
        /// is no <see cref="Control"/> under the cursor, or none of the <see cref="Control"/>s under the cursor
        /// implement the <see cref="IDragDropProvider"/> interface.
        /// </summary>
        IDragDropProvider DropOntoControl { get; }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> that currently has focus. If the <paramref name="value"/> is null
        /// or <see cref="Control.CanFocus"/> is false, the setter will do nothing.
        /// </summary>
        Control FocusedControl { get; set; }

        /// <summary>
        /// Gets the top-level <see cref="Control"/> that currently has focus, or has a child <see cref="Control"/>
        /// that has focus.
        /// </summary>
        Control FocusedRoot { get; }

        /// <summary>
        /// Gets or sets the default font for new <see cref="Control"/>s added to this <see cref="IGUIManager"/>.
        /// Can be null, but having this value null may result in certain <see cref="Control"/>s that require
        /// a default font throwing an <see cref="Exception"/>.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="IGUIManager"/> is enabled. When disabled, it can still draw, but it will
        /// not handle any input.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets the <see cref="Control"/> that was last the <see cref="IGUIManager.PressedControl"/>. Unlike
        /// <see cref="IGUIManager.PressedControl"/>, this value will not be set to null when the mouse button is raised.
        /// </summary>
        Control LastPressedControl { get; }

        /// <summary>
        /// Gets the <see cref="Control"/> that the left mouse button was pressed down on. Will be null if the cursor
        /// was not over any <see cref="Control"/> when the left mouse button was pressed, or if the left mouse button is
        /// currently up.
        /// </summary>
        Control PressedControl { get; }

        /// <summary>
        /// Gets or sets the size of the screen.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Either the x- or y-coordinate of the <paramref name="value"/>
        /// is less than or equal to zero.</exception>
        Vector2 ScreenSize { get; set; }

        /// <summary>
        /// Gets the <see cref="ISkinManager"/> used by this <see cref="IGUIManager"/> to perform
        /// all of the GUI skinning.
        /// </summary>
        ISkinManager SkinManager { get; }

        /// <summary>
        /// Gets the <see cref="ITooltip"/> to use for displaying tooltips for <see cref="Control"/>s in
        /// this <see cref="IGUIManager"/>.
        /// </summary>
        ITooltip Tooltip { get; }

        /// <summary>
        /// Gets the <see cref="Control"/> currently under the cursor, or null if no <see cref="Control"/> managed 
        /// by this <see cref="IGUIManager"/> is currently under the cursor.
        /// </summary>
        Control UnderCursor { get; }

        Window Window { get; set; }

        /// <summary>
        /// Adds a <see cref="Control"/> to this <see cref="IGUIManager"/> at the root level. This should only be called
        /// by the <see cref="Control"/>'s constructor.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="control"/> is not a root <see cref="Control"/>.</exception>
        void Add(Control control);

        /// <summary>
        /// Draws all of the <see cref="Control"/>s in this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing the <see cref="Control"/>s.</param>
        void Draw(ISpriteBatch spriteBatch);

        /// <summary>
        /// Gets all of the <see cref="Control"/>s in this <see cref="GUIManager"/>, including all
        /// child <see cref="Control"/>s.
        /// </summary>
        /// <returns>All of the <see cref="Control"/>s in this <see cref="GUIManager"/>.</returns>
        IEnumerable<Control> GetAllControls();

        /// <summary>
        /// Gets the top-most <see cref="Control"/> at the given point.
        /// </summary>
        /// <param name="point">Point to find the top-most <see cref="Control"/> at.</param>
        /// <returns>The <see cref="Control"/> at the given <paramref name="point"/>, or null if no
        /// <see cref="Control"/> was found at the given <paramref name="point"/>.</returns>
        Control GetControlAtPoint(Vector2 point);

        /// <summary>
        /// Gets if a given <see cref="Keyboard.Key"/> is currently being pressed.
        /// </summary>
        /// <param name="key">The <see cref="Keyboard.Key"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="key"/> is currently being pressed; otherwise false.</returns>
        bool IsKeyDown(Keyboard.Key key);

        /// <summary>
        /// Gets if a given <see cref="Mouse.Button"/> is currently being pressed.
        /// </summary>
        /// <param name="button">The <see cref="Mouse.Button"/> to check if pressed.</param>
        /// <returns>True if the <paramref name="button"/> is currently being pressed; otherwise false.</returns>
        bool IsMouseButtonDown(Mouse.Button button);

        /// <summary>
        /// Remove a <see cref="Control"/> from this <see cref="IGUIManager"/> from the root level. This should only be called
        /// by the <see cref="Control"/>'s constructor.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to remove.</param>
        /// <returns>True if the <paramref name="control"/> was successfully removed; false if the <paramref name="control"/>
        /// could not be removed or was not in this <see cref="IGUIManager"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is null.</exception>
        bool Remove(Control control);

        /// <summary>
        /// Sends an event for a key being pressed to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventKeyPressed(KeyEventArgs e);

        /// <summary>
        /// Sends an event for a key being released to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventKeyReleased(KeyEventArgs e);

        /// <summary>
        /// Sends an event for a mouse button being pressed to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventMouseButtonPressed(MouseButtonEventArgs e);

        /// <summary>
        /// Sends an event for a mouse button being released to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventMouseButtonReleased(MouseButtonEventArgs e);

        /// <summary>
        /// Sends an event for the mouse moving to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventMouseMoved(MouseMoveEventArgs e);

        /// <summary>
        /// Sends an event for text being entered to this <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void SendEventTextEntered(TextEventArgs e);

        /// <summary>
        /// Updates the <see cref="IGUIManager"/> and all of the <see cref="Control"/>s in it.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        void Update(TickCount currentTime);
    }
}