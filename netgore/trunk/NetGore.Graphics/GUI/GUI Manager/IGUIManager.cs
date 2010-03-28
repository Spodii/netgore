using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        event GUIEventHandler FocusedControlChanged;

        /// <summary>
        /// Notifies listeners when the focused root <see cref="Control"/> has changed.
        /// </summary>
        event GUIEventHandler FocusedRootChanged;

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
        /// Gets an IEnumerable of all the root <see cref="Control"/>s handled by this <see cref="IGUIManager"/>. This
        /// only contains the top-level <see cref="Control"/>s, not any of the child <see cref="Control"/>s.
        /// </summary>
        IEnumerable<Control> Controls { get; }

        /// <summary>
        /// Gets the screen coordinates of the cursor.
        /// </summary>
        Vector2 CursorPosition { get; }

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
        SpriteFont Font { get; set; }

        /// <summary>
        /// Gets the latest <see cref="KeyboardState"/>. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        KeyboardState KeyboardState { get; }

        /// <summary>
        /// Gets an IEnumerable of all keys that are currently down. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        IEnumerable<Keys> KeysPressed { get; }

        /// <summary>
        /// Gets the <see cref="KeyboardState"/> that was used immediately before the current
        /// <see cref="IGUIManager.KeyboardState"/>.
        /// </summary>
        KeyboardState LastKeyboardState { get; }

        /// <summary>
        /// Gets the IEnumerable of <see cref="Keys"/> that was used immediately before the current
        /// <see cref="IGUIManager.KeysPressed"/>.
        /// </summary>
        IEnumerable<Keys> LastKeysPressed { get; }

        /// <summary>
        /// Gets the <see cref="MouseState"/> that was used immediately before the current
        /// <see cref="IGUIManager.MouseState"/>.
        /// </summary>
        MouseState LastMouseState { get; }

        /// <summary>
        /// Gets the latest <see cref="MouseState"/>. This value is updated on each call to
        /// <see cref="IGUIManager.Update"/>.
        /// </summary>
        MouseState MouseState { get; }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="Keys"/> that were up during the previous call to
        /// <see cref="IGUIManager.Update"/> but are down on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        IEnumerable<Keys> NewKeysDown { get; }

        /// <summary>
        /// Gets an IEnumerable of all the <see cref="Keys"/> that were down during the previous call to
        /// <see cref="IGUIManager.Update"/> but are up on the latest call to <see cref="IGUIManager.Update"/>.
        /// This value is updated on each call to <see cref="IGUIManager.Update"/>.
        /// </summary>
        IEnumerable<Keys> NewKeysUp { get; }

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
        /// Remove a <see cref="Control"/> from this <see cref="IGUIManager"/> from the root level. This should only be called
        /// by the <see cref="Control"/>'s constructor.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to remove.</param>
        /// <returns>True if the <paramref name="control"/> was successfully removed; false if the <paramref name="control"/>
        /// could not be removed or was not in this <see cref="IGUIManager"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is null.</exception>
        bool Remove(Control control);

        /// <summary>
        /// Updates the <see cref="IGUIManager"/> and all of the <see cref="Control"/>s in it.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        void Update(int currentTime);
    }
}