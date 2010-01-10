using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace DemoGame.MapEditor
{
    sealed class GrhCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly List<MapGrh> _selectedMapGrhs = new List<MapGrh>();
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;

        TransBox _mapGrhMoveBox = null;
        Vector2 _mouseDragStart = Vector2.Zero;
        Vector2 _selectedEntityOffset = Vector2.Zero;

        void Menu_SnapToGrid_Click(object sender, EventArgs e)
        {
            _mnuSnapToGrid.Checked = !_mnuSnapToGrid.Checked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhCursor"/> class.
        /// </summary>
        public GrhCursor()
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

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_grhs; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Select Grh"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 15; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void DrawInterface(ScreenForm screen)
        {
            // Selected Grh move box
            if (_mapGrhMoveBox != null)
                _mapGrhMoveBox.Draw(screen.SpriteBatch);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                _mouseDragStart = Vector2.Zero;
                return;
            }

            Vector2 cursorPos = screen.CursorPos;
            MapGrh cursorGrh = screen.Map.Spatial.GetEntity<MapGrh>(cursorPos);

            if (cursorGrh != null)
            {
                // Single selection
                _selectedEntityOffset = cursorPos - cursorGrh.Position;
                _selectedMapGrhs.Clear();
                _selectedMapGrhs.Add(cursorGrh);
            }
            else
            {
                // Batch selection
                if (_mapGrhMoveBox != null)
                {
                    Vector2 v = _mapGrhMoveBox.Position;
                    Vector2 cp = cursorPos;
                    float w = _mapGrhMoveBox.Area.Width;
                    float h = _mapGrhMoveBox.Area.Height;

                    if ((v.X <= cp.X) && (v.X + w >= cp.X) && (v.Y <= cp.Y) && (v.Y + h >= cp.Y))
                    {
                        screen.SelectedTransBox = _mapGrhMoveBox;
                        return;
                    }

                    _mapGrhMoveBox = null;
                    _selectedMapGrhs.Clear();
                }
                _mouseDragStart = screen.Camera.ToWorld(e.X, e.Y);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            if (_selectedMapGrhs.Count == 1)
            {
                // Move the selected single MapGrh
                foreach (MapGrh mg in _selectedMapGrhs)
                {
                    var pos = cursorPos - _selectedEntityOffset;
                    if (_mnuSnapToGrid.Checked)
                        pos = screen.Grid.AlignDown(pos);
                    mg.Position = pos;
                }
            }
            else if (e.Button == MouseButtons.Left && _mapGrhMoveBox != null)
            {
                if (screen.SelectedTransBox == _mapGrhMoveBox)
                {
                    Vector2 offset = _mapGrhMoveBox.Position - cursorPos;
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    foreach (MapGrh mg in _selectedMapGrhs)
                    {
                        mg.Position -= offset;
                    }

                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
                    screen.SelectedTransBox = _mapGrhMoveBox;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            if (e.Button == MouseButtons.Right)
            {
                if (_mapGrhMoveBox != null)
                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (_selectedMapGrhs.Count == 1)
                    _selectedMapGrhs.Clear();
                if (_selectedMapGrhs.Count > 0)
                    return;
                if (_mouseDragStart == Vector2.Zero)
                    return;

                // Select MapGrhs
                _selectedMapGrhs.Clear();

                Vector2 mouseDragEnd = screen.Camera.ToWorld(e.X, e.Y);
                var selectRectSize = _mouseDragStart + mouseDragEnd;
                var selectRect = new Rectangle((int)_mouseDragStart.X, (int)_mouseDragStart.Y, (int)selectRectSize.X,
                                               (int)selectRectSize.Y);
                foreach (MapGrh mg in screen.Map.MapGrhs)
                {
                    if (mg.Intersects(selectRect) && !_selectedMapGrhs.Contains(mg))
                        _selectedMapGrhs.Add(mg);
                }

                // Move transbox
                if (_selectedMapGrhs.Count > 1)
                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
                else
                {
                    _mapGrhMoveBox = null;
                    screen.SelectedTransBox = null;
                    _selectedMapGrhs.Clear();
                }
                _mouseDragStart = Vector2.Zero;
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void PressDelete(ScreenForm screen)
        {
            foreach (MapGrh mg in _selectedMapGrhs)
            {
                screen.Map.RemoveMapGrh(mg);
            }
            _selectedMapGrhs.Clear();
            _mapGrhMoveBox = null;
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void UpdateCursor(ScreenForm screen)
        {
            bool isOverBox = false;
            if (_mapGrhMoveBox != null)
            {
                var cursorRect = new Rectangle((int)screen.CursorPos.X, (int)screen.CursorPos.Y, 1, 1);
                var boxPos = _mapGrhMoveBox.Position;
                var boxRect = new Rectangle((int)boxPos.X, (int)boxPos.Y, _mapGrhMoveBox.Area.Width, _mapGrhMoveBox.Area.Height);
                isOverBox = cursorRect.Intersects(boxRect);
            }

            if (isOverBox || _selectedMapGrhs.Count != 0 && screen.MouseButton == MouseButtons.Left)
                screen.Cursor = Cursors.SizeAll;
            else
                screen.Cursor = Cursors.Default;
        }
    }
}