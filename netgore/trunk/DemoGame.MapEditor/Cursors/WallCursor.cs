using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using Color=Microsoft.Xna.Framework.Graphics.Color;
using Point=System.Drawing.Point;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace DemoGame.MapEditor
{
    class WallCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly List<WallEntityBase> _selectedWalls = new List<WallEntityBase>();
        MouseButtons _mouseDragButton = MouseButtons.None;
        Vector2 _mouseDragStart = Vector2.Zero;

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_walls; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Select Wall"; }
        }

        /// <summary>
        /// Gets the List of all the currently selected walls
        /// </summary>
        public List<WallEntityBase> SelectedWalls
        {
            get { return _selectedWalls; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 5; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the cursor's selection layer,
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void DrawSelection(ScreenForm screen)
        {
            Vector2 cursorPos = screen.CursorPos;

            if (_mouseDragStart == Vector2.Zero || _mouseDragButton == MouseButtons.None || screen.SelectedTransBox != null)
                return;

            Color drawColor;
            if (_mouseDragButton == MouseButtons.Left)
                drawColor = new Color(0, 255, 0, 150);
            else
                drawColor = new Color(255, 0, 0, 150);

            Vector2 min = new Vector2(Math.Min(cursorPos.X, _mouseDragStart.X), Math.Min(cursorPos.Y, _mouseDragStart.Y));
            Vector2 max = new Vector2(Math.Max(cursorPos.X, _mouseDragStart.X), Math.Max(cursorPos.Y, _mouseDragStart.Y));

            Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            XNARectangle.Draw(screen.SpriteBatch, dest, drawColor);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 cursorPos = screen.CursorPos;

            // Set the dragging values
            _mouseDragStart = screen.Camera.ToWorld(e.X, e.Y);
            _mouseDragButton = e.Button;

            if (e.Button != MouseButtons.Left)
                return;

            if (screen.TransBoxes.Count > 0)
            {
                // Check for resize box collision
                Rectangle r = new Rectangle((int)_mouseDragStart.X, (int)_mouseDragStart.Y, 1, 1);
                screen.SelectedTransBox = null;
                foreach (TransBox box in screen.TransBoxes)
                {
                    if (!r.Intersects(box.Area))
                        continue;

                    screen.SelectedTransBox = box;
                    break;
                }
            }
            else
            {
                // Check for wall collision for quick dragging
                var spatial = screen.Map.GetSpatial<WallEntityBase>();
                WallEntityBase w = spatial.GetEntity<WallEntityBase>(cursorPos);
                if (w == null)
                    return;

                _selectedWalls.Clear();
                _selectedWalls.Add(w);
                TransBox.SurroundEntity(w, screen.TransBoxes);
                foreach (TransBox transBox in screen.TransBoxes)
                {
                    if (transBox.TransType != TransBoxType.Move)
                        continue;

                    cursorPos = transBox.Position;
                    cursorPos.X -= (TransBox.MoveSize.X / 2) + 16f;
                    GameScreenControl gameScreen = screen.GameScreenControl;
                    Point pts = gameScreen.PointToScreen(gameScreen.Location);
                    Vector2 screenPos = screen.Camera.ToScreen(cursorPos) + new Vector2(pts.X, pts.Y);

                    Cursor.Position = new Point((int)screenPos.X, (int)screenPos.Y);
                    screen.SelectedTransBox = transBox;
                    screen.CursorPos = cursorPos;
                    break;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            if (screen.SelectedTransBox == null)
                return;

            Vector2 cursorPos = screen.CursorPos;
            Map map = screen.Map;
            Entity selEntity = screen.SelectedTransBox.Entity;

            if (screen.SelectedTransBox.TransType == TransBoxType.Move)
            {
                if (selEntity == null)
                {
                    // Move multiple
                    Vector2 offset = cursorPos - _mouseDragStart;
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    // Keep the walls in the map area
                    foreach (WallEntityBase selWall in _selectedWalls)
                    {
                        map.KeepInMap(selWall, ref offset);
                    }

                    // Move the walls
                    foreach (WallEntityBase selWall in _selectedWalls)
                    {
                        selWall.Move(offset);
                    }
                }
                else
                {
                    // Move one
                    Vector2 offset = selEntity.Position - screen.SelectedTransBox.Position - (TransBox.MoveSize / 2);
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    // Move the wall
                    map.SafeTeleportEntity(selEntity, cursorPos + offset);

                    // Wall-to-wall snapping
                    if (screen.chkSnapWallWall.Checked)
                        map.SafeTeleportEntity(selEntity, map.SnapToWalls(selEntity));

                    // Wall-to-grid snapping
                    if (screen.chkSnapWallGrid.Checked)
                        screen.Grid.Align(selEntity);
                }
            }
            else
            {
                // Handle scaling
                if (((screen.SelectedTransBox.TransType & TransBoxType.Top) > 0) && selEntity.CB.Max.Y > cursorPos.Y)
                {
                    float oldMaxY = selEntity.CB.Max.Y;
                    map.SafeTeleportEntity(selEntity, new Vector2(selEntity.CB.Min.X, cursorPos.Y));
                    if (screen.chkSnapWallGrid.Checked)
                        screen.Grid.SnapToGridPosition(selEntity);

                    selEntity.Resize(new Vector2(selEntity.Size.X, oldMaxY - selEntity.Position.Y));
                }

                if (((screen.SelectedTransBox.TransType & TransBoxType.Left) > 0) && selEntity.CB.Max.X > cursorPos.X)
                {
                    float oldMaxX = selEntity.CB.Max.X;
                    map.SafeTeleportEntity(selEntity, new Vector2(cursorPos.X, selEntity.CB.Min.Y));
                    if (screen.chkSnapWallGrid.Checked)
                        screen.Grid.SnapToGridPosition(selEntity);
                    selEntity.Resize(new Vector2(oldMaxX - selEntity.Position.X, selEntity.Size.Y));
                }

                if (((screen.SelectedTransBox.TransType & TransBoxType.Bottom) > 0) && selEntity.CB.Min.Y < cursorPos.Y)
                {
                    selEntity.Resize(new Vector2(selEntity.Size.X, cursorPos.Y - selEntity.Position.Y));
                    if (screen.chkSnapWallGrid.Checked)
                        screen.Grid.SnapToGridSize(selEntity);
                }

                if (((screen.SelectedTransBox.TransType & TransBoxType.Right) > 0) && selEntity.CB.Min.X < cursorPos.X)
                {
                    selEntity.Resize(new Vector2(cursorPos.X - selEntity.CB.Min.X, selEntity.Size.Y));
                    if (screen.chkSnapWallGrid.Checked)
                        screen.Grid.SnapToGridSize(selEntity);
                }
            }

            // Recreate the transformation boxes
            screen.TransBoxes.Clear();
            if (selEntity == null)
            {
                TransBox newTransBox = new TransBox(TransBoxType.Move, null, cursorPos);
                screen.SelectedTransBox = newTransBox;
                screen.TransBoxes.Add(newTransBox);
            }
            else
            {
                TransBox.SurroundEntity(selEntity, screen.TransBoxes);

                // Find the new selected transformation box
                foreach (TransBox box in screen.TransBoxes)
                {
                    if (box.TransType == screen.SelectedTransBox.TransType)
                    {
                        screen.SelectedTransBox = box;
                        break;
                    }
                }
            }

            // Clear the drag start value
            _mouseDragStart = cursorPos;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
            Vector2 mouseDragEnd = screen.Camera.ToWorld(e.X, e.Y);

            if (_mouseDragStart != Vector2.Zero && screen.SelectedTransBox == null)
            {
                Vector2 min = _mouseDragStart.Min(mouseDragEnd);
                Vector2 max = _mouseDragStart.Max(mouseDragEnd);
                Vector2 size = max - min;

                var rect = new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y);
                var spatial = screen.Map.GetSpatial<WallEntityBase>();
                var walls = spatial.GetEntities<WallEntityBase>(rect);
                if (e.Button == MouseButtons.Left)
                {
                    // Selection dragging
                    // When holding down control, add to the selection by not deleting current selection
                    if (!screen.KeyEventArgs.Control)
                        _selectedWalls.Clear();

                    // Add all selected to the list if they are not already in it
                    foreach (WallEntityBase wall in walls)
                    {
                        if (!_selectedWalls.Contains(wall))
                            _selectedWalls.Add(wall);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // Deselection dragging
                    foreach (WallEntityBase wall in walls)
                    {
                        _selectedWalls.Remove(wall);
                    }
                }
            }

            // Release the selected resize box
            screen.SelectedTransBox = null;

            screen.TransBoxes.Clear();

            // With multiple selected walls, add a single move box for all of them
            if (_selectedWalls.Count > 1)
                screen.TransBoxes.Add(new TransBox(TransBoxType.Move, null, mouseDragEnd));

            // Create the transformation boxes
            foreach (WallEntityBase wall in _selectedWalls)
            {
                TransBox.SurroundEntity(wall, screen.TransBoxes);
            }

            // Update the selected walls list
            if (_selectedWalls.Count > 0)
                screen.UpdateSelectedWallsList(_selectedWalls);
            else if (_selectedWalls.Count == 0)
                screen.lstSelectedWalls.SelectedItems.Clear();

            _mouseDragStart = Vector2.Zero;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void PressDelete(ScreenForm screen)
        {
            foreach (WallEntityBase selectedWall in _selectedWalls)
            {
                screen.Map.RemoveEntity(selectedWall);
            }
            _selectedWalls.Clear();
            screen.UpdateSelectedWallsList(_selectedWalls);
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void UpdateCursor(ScreenForm screen)
        {
            TransBox hoverBox = null;

            if (screen.SelectedTransBox != null)
                hoverBox = screen.SelectedTransBox; // Set to the selected TransBox
            else
            {
                // Find the TransBox being hovered over (if any)
                CollisionBox cursorCB = new CollisionBox(screen.CursorPos, 1, 1);
                foreach (TransBox box in screen.TransBoxes)
                {
                    CollisionBox boxCB = new CollisionBox(box.Position, box.Area.Width, box.Area.Height);
                    if (CollisionBox.Intersect(cursorCB, boxCB))
                    {
                        hoverBox = box;
                        break;
                    }
                }
            }

            // If we have a box, set the cursor based on the TransType, else just set to default
            if (hoverBox != null)
            {
                switch (hoverBox.TransType)
                {
                    case TransBoxType.Move:
                        screen.Cursor = Cursors.SizeAll;
                        return;

                    case TransBoxType.Left:
                    case TransBoxType.Right:
                        screen.Cursor = Cursors.SizeWE;
                        return;

                    case TransBoxType.Top:
                    case TransBoxType.Bottom:
                        screen.Cursor = Cursors.SizeNS;
                        return;

                    case TransBoxType.BottomLeft:
                    case TransBoxType.TopRight:
                        screen.Cursor = Cursors.SizeNESW;
                        return;

                    case TransBoxType.BottomRight:
                    case TransBoxType.TopLeft:
                        screen.Cursor = Cursors.SizeNWSE;
                        return;
                }
            }

            screen.Cursor = Cursors.Default;
        }
    }
}