using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools.Properties;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    // TODO: Remove when done with the new editor

    /// <summary>
    /// Base handler for a cursor used to modify the map in different ways. All derived classes must have a constructor
    /// that takes no parameters.
    /// </summary>
    public abstract class EditorCursor<TContainer>
    {
        EditorCursorManager<TContainer> _cursorManager;

        /// <summary>
        /// Gets the object that the cursors are controlling or are contained in.
        /// </summary>
        public TContainer Container
        {
            get { return CursorManager.Container; }
        }

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public virtual Image CursorImage
        {
            get { return Resources.cursor_default; }
        }

        /// <summary>
        /// Gets the cursor manager that this cursor belongs to.
        /// </summary>
        public EditorCursorManager<TContainer> CursorManager
        {
            get { return _cursorManager; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public virtual int ToolbarPriority
        {
            get { return int.MaxValue; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor becomes the active cursor.
        /// </summary>
        public virtual void Activate()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor is no longer the active cursor.
        /// </summary>
        public virtual void Deactivate()
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public virtual void DrawInterface(ISpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the cursor's selection layer, 
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public virtual void DrawSelection(ISpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="ContextMenu"/> used by this cursor
        /// to display additional functions and settings.
        /// </summary>
        /// <returns>
        /// The <see cref="ContextMenu"/> used by this cursor to display additional functions and settings,
        /// or null for no <see cref="ContextMenu"/>.
        /// </returns>
        public virtual ContextMenu GetContextMenu()
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, allows for setting up the cursor. This is always called
        /// after the constructor but before any of the other virtual methods, and is only called once.
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// Invokes the initialize method.
        /// </summary>
        /// <param name="cursorManager">The owner cursor manager.</param>
        internal void InvokeInitialize(EditorCursorManager<TContainer> cursorManager)
        {
            if (_cursorManager != null)
                return;

            _cursorManager = cursorManager;

            Initialize();
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseDown(MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseMove(MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public virtual void MouseUp(MouseEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the mouse wheel has moved.
        /// </summary>
        /// <param name="amount">How much the mouse wheel has scrolled, and which direction.</param>
        public virtual void MoveMouseWheel(int amount)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public virtual void PressDelete()
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        public virtual void UpdateCursor()
        {
        }
    }
}