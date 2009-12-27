using System.Linq;
using System.Windows.Forms;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Base handler for a cursor used to modify the map in different ways.
    /// </summary>
    abstract class EditorCursorBase
    {
        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void DrawInterface(ScreenForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the cursor's selection layer, 
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void DrawSelection(ScreenForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void PressDelete(ScreenForm screen)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public virtual void UpdateCursor(ScreenForm screen)
        {
            screen.Cursor = Cursors.Default;
        }
    }
}