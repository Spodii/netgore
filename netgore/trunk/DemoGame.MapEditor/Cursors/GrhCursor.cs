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

namespace DemoGame.MapEditor
{
    class GrhCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly List<MapGrh> _selectedMapGrhs = new List<MapGrh>();
        TransBox _mapGrhMoveBox = null;
        Vector2 _mouseDragStart = Vector2.Zero;
        Vector2 _selectedEntityOffset = Vector2.Zero;

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
            if (e.Button == MouseButtons.Left)
            {
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
            else
                _mouseDragStart = Vector2.Zero;
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
                    mg.Position = cursorPos - _selectedEntityOffset;
                    if (screen.chkSnapGrhGrid.Checked)
                        mg.Position = screen.Grid.AlignDown(mg.Position);
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
                Vector2 mouseDragEnd = screen.Camera.ToWorld(e.X, e.Y);
                CollisionBox selectBox = new CollisionBox(_mouseDragStart, mouseDragEnd);
                _selectedMapGrhs.Clear();
                foreach (MapGrh mg in screen.Map.MapGrhs)
                {
                    CollisionBox cb = new CollisionBox(mg.Position, mg.Position + mg.Grh.Size);
                    if (cb.Intersect(selectBox) && !_selectedMapGrhs.Contains(mg))
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
                CollisionBox cursorCB = new CollisionBox(screen.CursorPos, 1, 1);
                CollisionBox boxCB = new CollisionBox(_mapGrhMoveBox.Position, _mapGrhMoveBox.Area.Width,
                                                      _mapGrhMoveBox.Area.Height);
                isOverBox = cursorCB.Intersect(boxCB);
            }

            if (isOverBox || _selectedMapGrhs.Count != 0 && screen.MouseButton == MouseButtons.Left)
                screen.Cursor = Cursors.SizeAll;
            else
                screen.Cursor = Cursors.Default;
        }
    }
}