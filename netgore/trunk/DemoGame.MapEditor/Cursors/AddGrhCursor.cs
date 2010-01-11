using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore.EditorTools;
using NetGore.Graphics;
using Color=Microsoft.Xna.Framework.Graphics.Color;

namespace DemoGame.MapEditor
{
    sealed class AddGrhCursor : MapEditorCursorBase<ScreenForm>
    {
        /// <summary>
        /// Color of the Grh preview when placing new Grhs.
        /// </summary>
        static readonly Color _drawPreviewColor = new Color(255, 255, 255, 150);

        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuForeground;
        readonly MenuItem _mnuSnapToGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddGrhCursor"/> class.
        /// </summary>
        public AddGrhCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _mnuForeground = new MenuItem("Foreground", Menu_Foreground_Click) { Checked = false };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid, _mnuForeground });
        }

        public bool AddToForeground
        {
            get { return _mnuForeground.Checked; }
        }

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_grhsadd; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Grh"; }
        }

        public bool SnapToGrid
        {
            get { return SnapToGridMenuItem.Checked; }
        }

        public MenuItem SnapToGridMenuItem
        {
            get { return _mnuSnapToGrid; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 20; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        public override void DrawInterface()
        {
            var grh = Screen.SelectedGrh;
            if (grh.GrhData == null)
                return;

            Vector2 drawPos;
            if (_mnuSnapToGrid.Checked)
                drawPos = Screen.Grid.AlignDown(Screen.CursorPos);
            else
                drawPos = Screen.CursorPos;

            // If we fail to draw the selected Grh, just ignore it
            try
            {
                grh.Draw(Screen.SpriteBatch, drawPos, _drawPreviewColor);
            }
            catch (Exception)
            {
            }
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

        void Menu_Foreground_Click(object sender, EventArgs e)
        {
            _mnuForeground.Checked = !_mnuForeground.Checked;
        }

        void Menu_SnapToGrid_Click(object sender, EventArgs e)
        {
            _mnuSnapToGrid.Checked = !_mnuSnapToGrid.Checked;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            if (_mnuSnapToGrid.Checked)
                MouseUp(e);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(MouseEventArgs e)
        {
            Vector2 cursorPos = Screen.CursorPos;

            // On left-click place the Grh on the map
            if (e.Button == MouseButtons.Left)
            {
                // Check for a valid MapGrh
                if (Screen.SelectedGrh.GrhData == null)
                    return;

                // Find the position the MapGrh will be created at
                Vector2 drawPos;
                if (_mnuSnapToGrid.Checked)
                    drawPos = Screen.Grid.AlignDown(cursorPos);
                else
                    drawPos = cursorPos;

                // Check if a MapGrh of the same type already exists at the location
                foreach (MapGrh grh in Screen.Map.MapGrhs)
                {
                    if (grh.Position == drawPos && grh.Grh.GrhData.GrhIndex == Screen.SelectedGrh.GrhData.GrhIndex)
                        return;
                }

                // Add the MapGrh to the map
                Grh g = new Grh(Screen.SelectedGrh.GrhData, AnimType.Loop, Screen.GetTime());
                Screen.Map.AddMapGrh(new MapGrh(g, drawPos, _mnuForeground.Checked));
            }
            else if (e.Button == MouseButtons.Right)
            {
                // On right-click delete any Grhs under the cursor
                while (true)
                {
                    MapGrh mapGrh = Screen.Map.Spatial.GetEntity<MapGrh>(cursorPos);
                    if (mapGrh == null)
                        break;

                    Screen.Map.RemoveMapGrh(mapGrh);
                }
            }
        }
    }
}