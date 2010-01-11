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
    sealed class WallCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;
        readonly MenuItem _mnuSnapToWalls;
        readonly List<WallEntityBase> _selectedWalls = new List<WallEntityBase>();
        MouseButtons _mouseDragButton = MouseButtons.None;
        Vector2 _mouseDragStart = Vector2.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="WallCursor"/> class.
        /// </summary>
        public WallCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _mnuSnapToWalls = new MenuItem("Snap to walls", Menu_SnapToWalls_Click) { Checked = true };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid, _mnuSnapToWalls });
        }

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
        /// When overridden in the derived class, allows for handling when the cursor becomes the active cursor.
        /// </summary>
        public override void Activate()
        {
            _selectedWalls.Clear();
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor is no longer the active cursor.
        /// </summary>
        public override void Deactivate()
        {
            _selectedWalls.Clear();
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
        public override void DrawSelection()
        {
            Vector2 cursorPos = Screen.CursorPos;

            if (_mouseDragStart == Vector2.Zero || _mouseDragButton == MouseButtons.None || Screen.SelectedTransBox != null)
                return;

            Color drawColor;
            if (_mouseDragButton == MouseButtons.Left)
                drawColor = new Color(0, 255, 0, 150);
            else
                drawColor = new Color(255, 0, 0, 150);

            Vector2 min = new Vector2(Math.Min(cursorPos.X, _mouseDragStart.X), Math.Min(cursorPos.Y, _mouseDragStart.Y));
            Vector2 max = new Vector2(Math.Max(cursorPos.X, _mouseDragStart.X), Math.Max(cursorPos.Y, _mouseDragStart.Y));

            Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            XNARectangle.Draw(Screen.SpriteBatch, dest, drawColor);
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

        void Menu_SnapToWalls_Click(object sender, EventArgs e)
        {
            _mnuSnapToWalls.Checked = !_mnuSnapToWalls.Checked;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            Vector2 cursorPos = Screen.CursorPos;

            // Set the dragging values
            _mouseDragStart = Screen.Camera.ToWorld(e.X, e.Y);
            _mouseDragButton = e.Button;

            if (e.Button != MouseButtons.Left)
                return;

            if (Screen.TransBoxes.Count > 0)
            {
                // Check for resize box collision
                Rectangle r = new Rectangle((int)_mouseDragStart.X, (int)_mouseDragStart.Y, 1, 1);
                Screen.SelectedTransBox = null;
                foreach (TransBox box in Screen.TransBoxes)
                {
                    if (!r.Intersects(box.Area))
                        continue;

                    Screen.SelectedTransBox = box;
                    break;
                }
            }
            else
            {
                // Check for wall collision for quick dragging
                WallEntityBase w = Screen.Map.Spatial.GetEntity<WallEntityBase>(cursorPos);
                if (w == null)
                    return;

                _selectedWalls.Clear();
                _selectedWalls.Add(w);
                TransBox.SurroundEntity(w, Screen.TransBoxes);
                foreach (TransBox transBox in Screen.TransBoxes)
                {
                    if (transBox.TransType != TransBoxType.Move)
                        continue;

                    cursorPos = transBox.Position;
                    cursorPos.X -= (TransBox.MoveSize.X / 2) + 16f;
                    GameScreenControl gameScreen = Screen.GameScreenControl;
                    Point pts = gameScreen.PointToScreen(gameScreen.Location);
                    Vector2 screenPos = Screen.Camera.ToScreen(cursorPos) + new Vector2(pts.X, pts.Y);

                    Cursor.Position = new Point((int)screenPos.X, (int)screenPos.Y);
                    Screen.SelectedTransBox = transBox;
                    Screen.CursorPos = cursorPos;
                    break;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            if (Screen.SelectedTransBox == null)
                return;

            Vector2 cursorPos = Screen.CursorPos;
            Map map = Screen.Map;
            Entity selEntity = Screen.SelectedTransBox.Entity;

            if (Screen.SelectedTransBox.TransType == TransBoxType.Move)
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
                    Vector2 offset = selEntity.Position - Screen.SelectedTransBox.Position - (TransBox.MoveSize / 2);
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    // Move the wall
                    map.SafeTeleportEntity(selEntity, cursorPos + offset);

                    // Wall-to-wall snapping
                    if (_mnuSnapToWalls.Checked)
                        map.SafeTeleportEntity(selEntity, map.SnapToWalls(selEntity));

                    // Wall-to-grid snapping
                    if (_mnuSnapToGrid.Checked)
                        Screen.Grid.Align(selEntity);
                }
            }
            else
            {
                // Handle scaling
                if (((Screen.SelectedTransBox.TransType & TransBoxType.Top) > 0) && selEntity.Max.Y > cursorPos.Y)
                {
                    float oldMaxY = selEntity.Max.Y;
                    map.SafeTeleportEntity(selEntity, new Vector2(selEntity.Position.X, cursorPos.Y));
                    if (_mnuSnapToGrid.Checked)
                        Screen.Grid.SnapToGridPosition(selEntity);

                    selEntity.Resize(new Vector2(selEntity.Size.X, oldMaxY - selEntity.Position.Y));
                }

                if (((Screen.SelectedTransBox.TransType & TransBoxType.Left) > 0) && selEntity.Max.X > cursorPos.X)
                {
                    float oldMaxX = selEntity.Max.X;
                    map.SafeTeleportEntity(selEntity, new Vector2(cursorPos.X, selEntity.Position.Y));
                    if (_mnuSnapToGrid.Checked)
                        Screen.Grid.SnapToGridPosition(selEntity);
                    selEntity.Resize(new Vector2(oldMaxX - selEntity.Position.X, selEntity.Size.Y));
                }

                if (((Screen.SelectedTransBox.TransType & TransBoxType.Bottom) > 0) && selEntity.Position.Y < cursorPos.Y)
                {
                    selEntity.Resize(new Vector2(selEntity.Size.X, cursorPos.Y - selEntity.Position.Y));
                    if (_mnuSnapToGrid.Checked)
                        Screen.Grid.SnapToGridSize(selEntity);
                }

                if (((Screen.SelectedTransBox.TransType & TransBoxType.Right) > 0) && selEntity.Position.X < cursorPos.X)
                {
                    selEntity.Resize(new Vector2(cursorPos.X - selEntity.Position.X, selEntity.Size.Y));
                    if (_mnuSnapToGrid.Checked)
                        Screen.Grid.SnapToGridSize(selEntity);
                }
            }

            // Recreate the transformation boxes
            Screen.TransBoxes.Clear();
            if (selEntity == null)
            {
                TransBox newTransBox = new TransBox(TransBoxType.Move, null, cursorPos);
                Screen.SelectedTransBox = newTransBox;
                Screen.TransBoxes.Add(newTransBox);
            }
            else
            {
                TransBox.SurroundEntity(selEntity, Screen.TransBoxes);

                // Find the new selected transformation box
                foreach (TransBox box in Screen.TransBoxes)
                {
                    if (box.TransType == Screen.SelectedTransBox.TransType)
                    {
                        Screen.SelectedTransBox = box;
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
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(MouseEventArgs e)
        {
            Vector2 mouseDragEnd = Screen.Camera.ToWorld(e.X, e.Y);

            if (_mouseDragStart != Vector2.Zero && Screen.SelectedTransBox == null)
            {
                Vector2 min = _mouseDragStart.Min(mouseDragEnd);
                Vector2 max = _mouseDragStart.Max(mouseDragEnd);
                Vector2 size = max - min;

                var rect = new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y);
                var selectAreaObjs = Screen.Map.Spatial.GetEntities<WallEntityBase>(rect);
                if (e.Button == MouseButtons.Left)
                {
                    // Selection dragging
                    // When holding down control, add to the selection by not deleting current selection
                    if (!Screen.KeyEventArgs.Control)
                        _selectedWalls.Clear();

                    // Add all selected to the list if they are not already in it
                    _selectedWalls.AddRange(selectAreaObjs);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // Deselection dragging
                    _selectedWalls.RemoveAll(x => selectAreaObjs.Contains(x));
                }
            }

            // Release the selected resize box
            Screen.SelectedTransBox = null;

            Screen.TransBoxes.Clear();

            // With multiple selected walls, add a single move box for all of them
            if (_selectedWalls.Count > 1)
                Screen.TransBoxes.Add(new TransBox(TransBoxType.Move, null, mouseDragEnd));

            // Create the transformation boxes
            foreach (WallEntityBase wall in _selectedWalls)
            {
                TransBox.SurroundEntity(wall, Screen.TransBoxes);
            }

            // Update the selected walls list
            Screen.SelectedObjectsManager.SetManySelected(_selectedWalls.OfType<object>());

            _mouseDragStart = Vector2.Zero;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            foreach (WallEntityBase selectedWall in _selectedWalls)
            {
                Screen.Map.RemoveEntity(selectedWall);
            }

            _selectedWalls.Clear();
            Screen.SelectedObjectsManager.Clear();
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        public override void UpdateCursor()
        {
            TransBox hoverBox = null;

            if (Screen.SelectedTransBox != null)
                hoverBox = Screen.SelectedTransBox; // Set to the selected TransBox
            else
            {
                // Find the TransBox being hovered over (if any)
                var cursorRect = new Rectangle((int)Screen.CursorPos.X, (int)Screen.CursorPos.Y, 1, 1);
                foreach (TransBox box in Screen.TransBoxes)
                {
                    var boxRect = new Rectangle((int)box.Position.X, (int)box.Position.Y, box.Area.Width, box.Area.Height);
                    if (cursorRect.Intersects(boxRect))
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
                        Screen.Cursor = Cursors.SizeAll;
                        return;

                    case TransBoxType.Left:
                    case TransBoxType.Right:
                        Screen.Cursor = Cursors.SizeWE;
                        return;

                    case TransBoxType.Top:
                    case TransBoxType.Bottom:
                        Screen.Cursor = Cursors.SizeNS;
                        return;

                    case TransBoxType.BottomLeft:
                    case TransBoxType.TopRight:
                        Screen.Cursor = Cursors.SizeNESW;
                        return;

                    case TransBoxType.BottomRight:
                    case TransBoxType.TopLeft:
                        Screen.Cursor = Cursors.SizeNWSE;
                        return;
                }
            }

            Screen.Cursor = Cursors.Default;
        }
    }
}