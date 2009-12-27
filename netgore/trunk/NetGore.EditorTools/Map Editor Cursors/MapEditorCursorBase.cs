using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools.Properties;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Base handler for a cursor used to modify the map in different ways. All derived classes must have a constructor
    /// that takes no parameters.
    /// </summary>
    public abstract class MapEditorCursorBase<TForm> where TForm : Form
    {
        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public virtual Image CursorImage { get { return Resources.cursor_default; } }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public virtual int ToolbarPriority
        {
            get { return int.MaxValue; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void DrawInterface(TForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the cursor's selection layer, 
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void DrawSelection(TForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseDown(TForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseMove(TForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseUp(TForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void PressDelete(TForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void UpdateCursor(TForm screen)
        {
            screen.Cursor = Cursors.Default;
        }
    }
}