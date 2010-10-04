using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Graphics;
using SFML;
using SFML.Graphics;
using Image = System.Drawing.Image;

namespace DemoGame.Editor
{
    sealed class AddGrhCursor : EditorCursor<EditMapForm>
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
        /// Property to access the MSC. Provided purely for the means of shortening the
        /// code
        /// </summary>
        MapScreenControl MSC
        {
            get { return Container.MapScreenControl; }
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
        public override int ToolbarPriority
        {
            get { return 20; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor becomes the active cursor.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            ClearState();
        }

        /// <summary>
        /// Completely clears the state of the cursor.
        /// </summary>
        void ClearState()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor is no longer the active cursor.
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();

            ClearState();
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            var grhToPlace = GlobalState.Instance.Map.GrhToPlace;
            if (grhToPlace.GrhData == null)
                return;

            /*
            // TODO: !!!
            Vector2 drawPos;
            if (_mnuSnapToGrid.Checked)
                drawPos = MSC.Grid.AlignDown(MSC.CursorPos);
            else
                drawPos = MSC.CursorPos;
            */
            var drawPos = MSC.CursorPos; // HACK: !! Temp replacement for the above

            // If we fail to draw the selected Grh, just ignore it
            try
            {
                grhToPlace.Draw(spriteBatch, drawPos, _drawPreviewColor);
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
            catch (LoadingFailedException)
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
            var cursorPos = MSC.CursorPos;

            // On left-click place the Grh on the map
            switch (e.Button)
            {
                case MouseButtons.Left:
                    var grhToPlace = GlobalState.Instance.Map.GrhToPlace;

                    // Check for a valid MapGrh
                    if (grhToPlace.GrhData == null)
                        return;

                    // Find the position the MapGrh will be created at
                    /*
                    // TODO: !!!
                    Vector2 drawPos;
                    if (_mnuSnapToGrid.Checked)
                        drawPos = MSC.Grid.AlignDown(cursorPos);
                    else
                        drawPos = cursorPos;
                    */
                    var drawPos = cursorPos; // HACK: Temp replacement for the above

                    // Check if a MapGrh of the same type already exists at the location
                    var selGrhGrhIndex = grhToPlace.GrhData.GrhIndex;
                    if (MSC.Map.MapGrhs.Any(x => x.Position == drawPos && x.Grh.GrhData.GrhIndex == selGrhGrhIndex))
                        return;

                    // Add the MapGrh to the map
                    var g = new Grh(grhToPlace.GrhData, AnimType.Loop, MSC.GetTime());
                    MSC.Map.AddMapGrh(new MapGrh(g, drawPos, _mnuForeground.Checked));

                    break;

                case MouseButtons.Right:
                    while (true)
                    {
                        var mapGrh = MSC.Map.Spatial.Get<MapGrh>(cursorPos);
                        if (mapGrh == null)
                            break;

                        MSC.Map.RemoveMapGrh(mapGrh);
                    }

                    break;
            }
        }
    }
}