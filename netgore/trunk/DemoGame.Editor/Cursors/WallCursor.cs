using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;

namespace DemoGame.Editor
{
    sealed class WallCursor : EditorCursor<EditMapForm>
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
            get { return "Select Wall"; }
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public override int ToolbarPriority
        {
            get { return 5; }
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
            _selectedWalls.Clear();
            _mouseDragButton = MouseButtons.None;
            _mouseDragStart = Vector2.Zero;
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
        /// When overridden in the derived class, handles drawing the cursor's selection layer,
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawSelection(ISpriteBatch spriteBatch)
        {
            var cursorPos = MSC.CursorPos;

            if (_mouseDragStart == Vector2.Zero || _mouseDragButton == MouseButtons.None || Container.SelectedTransBox != null)
                return;

            Color drawColor;
            if (_mouseDragButton == MouseButtons.Left)
                drawColor = new Color(0, 255, 0, 150);
            else
                drawColor = new Color(255, 0, 0, 150);

            var min = new Vector2(Math.Min(cursorPos.X, _mouseDragStart.X), Math.Min(cursorPos.Y, _mouseDragStart.Y));
            var max = new Vector2(Math.Max(cursorPos.X, _mouseDragStart.X), Math.Max(cursorPos.Y, _mouseDragStart.Y));

            var dest = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            RenderRectangle.Draw(spriteBatch, dest, drawColor);
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
            var cursorPos = MSC.CursorPos;

            // Set the dragging values
            _mouseDragStart = MSC.Camera.ToWorld(e.X, e.Y);
            _mouseDragButton = e.Button;

            if (e.Button != MouseButtons.Left)
                return;

            if (Container.TransBoxes.Count > 0)
            {
                // Check for resize box collision
                var r = new Rectangle((int)_mouseDragStart.X, (int)_mouseDragStart.Y, 1, 1);
                Container.SelectedTransBox = null;
                foreach (var box in Container.TransBoxes)
                {
                    if (!r.Intersects(box.Area))
                        continue;

                    Container.SelectedTransBox = box;
                    break;
                }
            }
            else
            {
                // Check for wall collision for quick dragging
                var w = MSC.Map.Spatial.Get<WallEntityBase>(cursorPos);
                if (w == null)
                    return;

                _selectedWalls.Clear();
                _selectedWalls.Add(w);
                TransBox.SurroundEntity(w, Container.TransBoxes);

                // Grab the move box
                var moveBox = Container.TransBoxes.FirstOrDefault(x => x.TransType == TransBoxType.Move);
                if (moveBox == null)
                    return;

                // Set the cursor to the center of the move box
                cursorPos = moveBox.Position;
                cursorPos += (moveBox.Size / 2f).Ceiling();
                MSC.CursorPos = cursorPos;

                // Get the system cursor to the center of the box, too
                var pts = MSC.PointToScreen(MSC.Location);
                var screenPos = MSC.Camera.ToScreen(cursorPos) + new Vector2(pts.X, pts.Y);
                Cursor.Position = new Point((int)screenPos.X, (int)screenPos.Y);

                // Set the move box as selected
                Container.SelectedTransBox = moveBox;
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            if (Container.SelectedTransBox == null)
                return;

            var cursorPos = MSC.CursorPos;
            var map = MSC.Map;
            var selEntity = Container.SelectedTransBox.Entity;

            if (Container.SelectedTransBox.TransType == TransBoxType.Move)
            {
                if (selEntity == null)
                {
                    // Move multiple
                    var offset = cursorPos - _mouseDragStart;
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    // Keep the walls in the map area
                    foreach (var selWall in _selectedWalls)
                    {
                        map.KeepInMap(selWall, ref offset);
                    }

                    // Move the walls
                    foreach (var selWall in _selectedWalls)
                    {
                        selWall.Move(offset);
                    }
                }
                else
                {
                    // Move one
                    var offset = selEntity.Position - Container.SelectedTransBox.Position - (TransBox.MoveSize / 2);
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    // Move the wall
                    map.SafeTeleportEntity(selEntity, cursorPos + offset);

                    // Wall-to-wall snapping
                    if (_mnuSnapToWalls.Checked)
                        map.SafeTeleportEntity(selEntity, map.SnapToWalls(selEntity));

                    // Wall-to-grid snapping
                    // TODO: !!
                    /*
                    if (_mnuSnapToGrid.Checked)
                        MSC.Grid.Align(selEntity);
                    */
                }
            }
            else
            {
                // Handle scaling
                if (((Container.SelectedTransBox.TransType & TransBoxType.Top) > 0) && selEntity.Max.Y > cursorPos.Y)
                {
                    var oldMaxY = selEntity.Max.Y;
                    map.SafeTeleportEntity(selEntity, new Vector2(selEntity.Position.X, cursorPos.Y));
                    // TODO: !!
                    /*
                    if (_mnuSnapToGrid.Checked)
                        MSC.Grid.SnapToGridPosition(selEntity);
                    */

                    selEntity.Resize(new Vector2(selEntity.Size.X, oldMaxY - selEntity.Position.Y));
                }

                if (((Container.SelectedTransBox.TransType & TransBoxType.Left) > 0) && selEntity.Max.X > cursorPos.X)
                {
                    var oldMaxX = selEntity.Max.X;
                    map.SafeTeleportEntity(selEntity, new Vector2(cursorPos.X, selEntity.Position.Y));
                    // TODO: !!
                    /*
                    if (_mnuSnapToGrid.Checked)
                        MSC.Grid.SnapToGridPosition(selEntity);
                    */

                    selEntity.Resize(new Vector2(oldMaxX - selEntity.Position.X, selEntity.Size.Y));
                }

                if (((Container.SelectedTransBox.TransType & TransBoxType.Bottom) > 0) && selEntity.Position.Y < cursorPos.Y)
                {
                    selEntity.Resize(new Vector2(selEntity.Size.X, cursorPos.Y - selEntity.Position.Y));
                    // TODO: !!
                    /*
                    if (_mnuSnapToGrid.Checked)
                        MSC.Grid.SnapToGridSize(selEntity);
                    */
                }

                if (((Container.SelectedTransBox.TransType & TransBoxType.Right) > 0) && selEntity.Position.X < cursorPos.X)
                {
                    selEntity.Resize(new Vector2(cursorPos.X - selEntity.Position.X, selEntity.Size.Y));
                    // TODO: !!
                    /*
                    if (_mnuSnapToGrid.Checked)
                        MSC.Grid.SnapToGridSize(selEntity);
                    */
                }
            }

            // Recreate the transformation boxes
            Container.TransBoxes.Clear();
            if (selEntity == null)
            {
                var newTransBox = new TransBox(TransBoxType.Move, null, cursorPos);
                Container.SelectedTransBox = newTransBox;
                Container.TransBoxes.Add(newTransBox);
            }
            else
            {
                TransBox.SurroundEntity(selEntity, Container.TransBoxes);

                // Find the new selected transformation box
                foreach (var box in Container.TransBoxes)
                {
                    if (box.TransType == Container.SelectedTransBox.TransType)
                    {
                        Container.SelectedTransBox = box;
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
            var mouseDragEnd = MSC.Camera.ToWorld(e.X, e.Y);

            if (_mouseDragStart != Vector2.Zero && Container.SelectedTransBox == null)
            {
                var min = _mouseDragStart.Min(mouseDragEnd);
                var max = _mouseDragStart.Max(mouseDragEnd);
                var size = max - min;

                var rect = new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y);
                var selectAreaObjs = MSC.Map.Spatial.GetMany<WallEntityBase>(rect);
                if (e.Button == MouseButtons.Left)
                {
                    // Selection dragging
                    // When holding down control, add to the selection by not deleting current selection
                    if (!Input.IsCtrlDown)
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
            Container.SelectedTransBox = null;

            Container.TransBoxes.Clear();

            // With multiple selected walls, add a single move box for all of them
            if (_selectedWalls.Count > 1)
                Container.TransBoxes.Add(new TransBox(TransBoxType.Move, null, mouseDragEnd));

            // Create the transformation boxes
            foreach (var wall in _selectedWalls)
            {
                TransBox.SurroundEntity(wall, Container.TransBoxes);
            }

            // Update the selected walls list
            SOM.SetManySelected(_selectedWalls.OfType<object>());

            _mouseDragStart = Vector2.Zero;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            foreach (var selectedWall in _selectedWalls)
            {
                MSC.Map.RemoveEntity(selectedWall);
            }

            _selectedWalls.Clear();
            SOM.Clear();
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        public override void UpdateCursor()
        {
            TransBox hoverBox = null;

            if (Container.SelectedTransBox != null)
                hoverBox = Container.SelectedTransBox; // Set to the selected TransBox
            else
            {
                // Find the TransBox being hovered over (if any)
                var cursorRect = new Rectangle((int)MSC.CursorPos.X, (int)MSC.CursorPos.Y, 1, 1);
                foreach (var box in Container.TransBoxes)
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
                        Container.Cursor = Cursors.SizeAll;
                        return;

                    case TransBoxType.Left:
                    case TransBoxType.Right:
                        Container.Cursor = Cursors.SizeWE;
                        return;

                    case TransBoxType.Top:
                    case TransBoxType.Bottom:
                        Container.Cursor = Cursors.SizeNS;
                        return;

                    case TransBoxType.BottomLeft:
                    case TransBoxType.TopRight:
                        Container.Cursor = Cursors.SizeNESW;
                        return;

                    case TransBoxType.BottomRight:
                    case TransBoxType.TopLeft:
                        Container.Cursor = Cursors.SizeNWSE;
                        return;
                }
            }

            Container.Cursor = Cursors.Default;
        }
    }
}