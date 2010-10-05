using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Interface for a container that is the target for a <see cref="Tool"/>. This is usually for a container that holds
    /// a screen that a <see cref="Tool"/> can interact with.
    /// </summary>
    public interface IToolTargetContainer
    {
        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        event DragEventHandler DragDrop;

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        event DragEventHandler DragEnter;

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        event EventHandler DragLeave;

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        event DragEventHandler DragOver;

        /// <summary>
        /// Occurs when the control receives focus.
        /// </summary>
        event EventHandler GotFocus;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        event KeyPressEventHandler KeyPress;

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        event KeyEventHandler KeyUp;

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        event MouseEventHandler MouseClick;

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        event MouseEventHandler MouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        event MouseEventHandler MouseDown;

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        event EventHandler MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer rests on the control.
        /// </summary>
        event EventHandler MouseHover;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        event EventHandler MouseLeave;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        event MouseEventHandler MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        event MouseEventHandler MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        event MouseEventHandler MouseWheel;

        /// <summary>
        /// Occurs before the <see cref="IToolTargetContainer.KeyDown"/> event when a key is pressed while focus is on this control.
        /// </summary>
        event PreviewKeyDownEventHandler PreviewKeyDown;

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        /// <value>A System.Drawing.Color that represents the background color of the control.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultBackColor"/> property.</value>
        Color BackColor { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has captured the mouse.
        /// </summary>
        /// <value>true if the control has captured the mouse; otherwise, false.</value>
        bool Capture { get; }

        /// <summary>
        /// Gets the height and width of the client area of the control.
        /// </summary>
        /// <value>A <see cref="System.Drawing.Size"/> that represents the dimensions of the client area of the control.</value>
        Size ClientSize { get; }

        /// <summary>
        /// Gets a value indicating whether the control, or one of its child controls, currently has the input focus.
        /// </summary>
        /// <value>true if the control or one of its child controls currently has the input focus; otherwise, false.</value>
        bool ContainsFocus { get; }

        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        /// <value>A <see cref="System.Windows.Forms.Cursor"/> that represents the cursor to display
        /// when the mouse pointer is over the control.</value>
        Cursor Cursor { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <value>true if the control can respond to user interaction; otherwise, false. The default is true.</value>
        bool Enabled { get; }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        /// <value>true if the control has focus; otherwise, false.</value>
        bool Focused { get; }

        /// <summary>
        /// Gets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="System.Drawing.Font"/> to apply to the text displayed by the control.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultFont"/> property.</value>
        Font Font { get; }

        /// <summary>
        /// Gets the foreground color of the control.
        /// </summary>
        /// <value>The foreground <see cref="System.Drawing.Color"/> of the control.
        /// The default is the value of the <see cref="System.Windows.Forms.Control.DefaultForeColor"/> property.</value>
        Color ForeColor { get; }

        /// <summary>
        /// Gets the height and width of the control.
        /// </summary>
        /// <value>The <see cref="System.Drawing.Size"/> that represents the height and width of the control in pixels.</value>
        Size Size { get; }

        /// <summary>
        /// Computes the location of the specified screen point into client coordinates.
        /// </summary>
        /// <param name="p">The screen coordinate <see cref="System.Drawing.Point"/> to convert.</param>
        /// <returns>A <see cref="System.Drawing.Point"/> that represents the converted
        /// <see cref="System.Drawing.Point"/>, <paramref name="p"/>, in client coordinates.</returns>
        Point PointToClient(Point p);

        /// <summary>
        /// Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="p">The client coordinate <see cref="System.Drawing.Point"/> to convert.</param>
        /// <returns>A <see cref="System.Drawing.Point"/> that represents the converted
        /// <see cref="System.Drawing.Point"/>, <paramref name="p"/>, in screen coordinates.</returns>
        Point PointToScreen(Point p);
    }
}