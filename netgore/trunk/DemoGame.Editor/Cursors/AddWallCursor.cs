using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.EditorTools;
using SFML.Graphics;
using Image = System.Drawing.Image;

namespace DemoGame.Editor
{
    sealed class AddWallCursor : EditorCursor<EditMapForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;

        /// <summary>
        /// Property to access the MSC. Provided purely for the means of shortening the
        /// code
        /// </summary>
        MapScreenControl MSC { get { return Container.MapScreenControl; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddWallCursor"/> class.
        /// </summary>
        public AddWallCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid });
        }

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
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
        public override int ToolbarPriority
        {
            get { return 10; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="ContextMenu"/> used by this cursor
        /// to display additional functions and settings.
        /// </summary>
        /// <returns>
        /// The <see cref="ContextMenu"/> used by this cursor to display additional functions and settings,
        /// or null for no <see cref="ContextMenu"/>.
        /// </returns>
        public override ContextMenu GetContextMenu()
        {
            return _contextMenu;
        }

        void Menu_SnapToGrid_Click(object sender, EventArgs e)
        {
            _mnuSnapToGrid.Checked = !_mnuSnapToGrid.Checked;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            // Switch to the wall editing tool
            Container.CursorManager.SelectedCursor = Container.CursorManager.TryGetCursor<WallCursor>();

            // Create the new wall
            var w = new WallEntity(MSC.Camera.ToWorld(e.X, e.Y), Vector2.One);
            MSC.Map.AddEntity(w);
            if (_mnuSnapToGrid.Checked)
                MSC.Grid.Align(w);

            // Create the transformation boxes for the wall and select the bottom/right one
            Container.TransBoxes.Clear();
            TransBox.SurroundEntity(w, Container.TransBoxes);
            foreach (var tBox in Container.TransBoxes)
            {
                if (tBox.TransType == TransBoxType.BottomRight)
                {
                    Container.SelectedTransBox = tBox;
                    break;
                }
            }

            GlobalState.Instance.Map.SelectedObjsManager.SetSelected(w);
        }
    }
}