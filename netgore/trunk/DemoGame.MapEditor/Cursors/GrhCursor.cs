using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.MapEditor
{
    class GrhCursor : EditorCursorBase
    {
        /// <summary>
        /// Currently selected Grh already on the map
        /// </summary>
        readonly List<MapGrh> _selectedMapGrhs = new List<MapGrh>();

        /// <summary>
        /// MapGrh move box for moving multiple MapGrhs
        /// </summary>
        TransBox _mapGrhMoveBox = null;

        Vector2 _mouseDragStart = Vector2.Zero;

        Vector2 _selectedEntityOffset = Vector2.Zero;

        public override void DrawInterface(ScreenForm screen)
        {
            // Selected Grh move box
            if (_mapGrhMoveBox != null)
                _mapGrhMoveBox.Draw(screen.SpriteBatch);
        }

        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Vector2 cursorPos = screen.CursorPos;

                MapGrh cursorGrh = screen.Map.GetMapGrh(cursorPos);

                if (cursorGrh != null)
                {
                    // Single selection
                    _selectedEntityOffset = cursorPos - cursorGrh.Destination;
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

        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            if (_selectedMapGrhs.Count == 1)
            {
                // Move the selected single MapGrh
                foreach (MapGrh mg in _selectedMapGrhs)
                {
                    mg.Destination = cursorPos - _selectedEntityOffset;
                    if (screen.chkSnapGrhGrid.Checked)
                        mg.Destination = screen.Grid.AlignDown(mg.Destination);
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
                        mg.Destination -= offset;
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
                    CollisionBox cb = new CollisionBox(mg.Destination, mg.Destination + mg.Grh.Size);
                    if (CollisionBox.Intersect(cb, selectBox) && !_selectedMapGrhs.Contains(mg))
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

        public override void PressDelete(ScreenForm screen)
        {
            foreach (MapGrh mg in _selectedMapGrhs)
            {
                screen.Map.RemoveMapGrh(mg);
            }
            _selectedMapGrhs.Clear();
            _mapGrhMoveBox = null;
        }

        public override void UpdateCursor(ScreenForm screen)
        {
            bool isOverBox = false;
            if (_mapGrhMoveBox != null)
            {
                CollisionBox cursorCB = new CollisionBox(screen.CursorPos, 1, 1);
                CollisionBox boxCB = new CollisionBox(_mapGrhMoveBox.Position, _mapGrhMoveBox.Area.Width,
                                                      _mapGrhMoveBox.Area.Height);
                isOverBox = CollisionBox.Intersect(cursorCB, boxCB);
            }

            if (isOverBox || _selectedMapGrhs.Count != 0 && screen.MouseButton == MouseButtons.Left)
                screen.Cursor = Cursors.SizeAll;
            else
                screen.Cursor = Cursors.Default;
        }
    }
}