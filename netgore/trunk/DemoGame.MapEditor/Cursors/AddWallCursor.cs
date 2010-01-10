using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;

namespace DemoGame.MapEditor
{
    sealed class AddWallCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddWallCursor"/> class.
        /// </summary>
        public AddWallCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid });
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="ContextMenu"/> used by this cursor
        /// to display additional functions and settings.
        /// </summary>
        /// <param name="cursorManager">The cursor manager.</param>
        /// <returns>
        /// The <see cref="ContextMenu"/> used by this cursor to display additional functions and settings,
        /// or null for no <see cref="ContextMenu"/>.
        /// </returns>
        public override ContextMenu GetContextMenu(MapEditorCursorManager<ScreenForm> cursorManager)
        {
            return _contextMenu;
        }

        void Menu_SnapToGrid_Click(object sender, EventArgs e)
        {
            _mnuSnapToGrid.Checked = !_mnuSnapToGrid.Checked;
        }

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_wallsadd; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Wall"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 10; }
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            // Switch to the wall editing tool
            screen.CursorManager.SelectedCursor = screen.CursorManager.TryGetCursor<WallCursor>();

            // Create the new wall
            WallEntity w = new WallEntity(screen.Camera.ToWorld(e.X, e.Y), Vector2.One);
            screen.Map.AddEntity(w);
            if (_mnuSnapToGrid.Checked)
                screen.Grid.Align(w);

            // Create the transformation boxes for the wall and select the bottom/right one
            screen.TransBoxes.Clear();
            TransBox.SurroundEntity(w, screen.TransBoxes);
            foreach (TransBox tBox in screen.TransBoxes)
            {
                if (tBox.TransType == TransBoxType.BottomRight)
                {
                    screen.SelectedTransBox = tBox;
                    break;
                }
            }

            screen.SelectedObjectsManager.SetSelected(w);
        }
    }
}