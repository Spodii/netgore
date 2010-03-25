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
using Color=Microsoft.Xna.Framework.Graphics.Color;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace DemoGame.MapEditor
{
    sealed class GrhCursor : EditorCursor<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuSnapToGrid;
        readonly List<MapGrh> _selectedMapGrhs = new List<MapGrh>();

        TransBox _mapGrhMoveBox = null;
        Vector2 _mouseDragStart = Vector2.Zero;
        Vector2 _selectedEntityOffset = Vector2.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhCursor"/> class.
        /// </summary>
        public GrhCursor()
        {
            _mnuSnapToGrid = new MenuItem("Snap to grid", Menu_SnapToGrid_Click) { Checked = true };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuSnapToGrid });
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
        public override int ToolbarPriority
        {
            get { return 15; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            // Selected Grh move box
            if (_mapGrhMoveBox != null)
                _mapGrhMoveBox.Draw(spriteBatch);
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the cursor's selection layer,
        /// which displays a selection box for when selecting multiple objects.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawSelection(ISpriteBatch spriteBatch)
        {
            Vector2 cursorPos = Container.CursorPos;

            if (_mouseDragStart == Vector2.Zero || Container.SelectedTransBox != null)
                return;

            var drawColor = new Color(0, 255, 0, 150);

            Vector2 min = new Vector2(Math.Min(cursorPos.X, _mouseDragStart.X), Math.Min(cursorPos.Y, _mouseDragStart.Y));
            Vector2 max = new Vector2(Math.Max(cursorPos.X, _mouseDragStart.X), Math.Max(cursorPos.Y, _mouseDragStart.Y));

            Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
            XNARectangle.Draw(spriteBatch, dest, drawColor);
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
            if (e.Button != MouseButtons.Left)
            {
                _mouseDragStart = Vector2.Zero;
                return;
            }

            Vector2 cursorPos = Container.CursorPos;
            MapGrh cursorGrh = Container.Map.Spatial.Get<MapGrh>(cursorPos);

            if (cursorGrh != null)
            {
                // Single selection
                _selectedEntityOffset = cursorPos - cursorGrh.Position;
                _selectedMapGrhs.Clear();
                _selectedMapGrhs.Add(cursorGrh);
                Container.SelectedObjs.SetSelected(cursorGrh);
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
                        Container.SelectedTransBox = _mapGrhMoveBox;
                        return;
                    }

                    _mapGrhMoveBox = null;
                    _selectedMapGrhs.Clear();
                }
                _mouseDragStart = Container.Camera.ToWorld(e.X, e.Y);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            Vector2 cursorPos = Container.CursorPos;

            if (_selectedMapGrhs.Count == 1)
            {
                // Move the selected single MapGrh
                foreach (MapGrh mg in _selectedMapGrhs)
                {
                    var pos = cursorPos - _selectedEntityOffset;
                    if (_mnuSnapToGrid.Checked)
                        pos = Container.Grid.AlignDown(pos);
                    mg.Position = pos;
                }
            }
            else if (e.Button == MouseButtons.Left && _mapGrhMoveBox != null)
            {
                if (Container.SelectedTransBox == _mapGrhMoveBox)
                {
                    Vector2 offset = _mapGrhMoveBox.Position - cursorPos;
                    offset.X = (float)Math.Round(offset.X);
                    offset.Y = (float)Math.Round(offset.Y);

                    foreach (MapGrh mg in _selectedMapGrhs)
                    {
                        mg.Position -= offset;
                    }

                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
                    Container.SelectedTransBox = _mapGrhMoveBox;
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been released.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseUp(MouseEventArgs e)
        {
            Vector2 cursorPos = Container.CursorPos;

            if (e.Button == MouseButtons.Right)
            {
                if (_mapGrhMoveBox != null)
                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (_selectedMapGrhs.Count == 1)
                    _selectedMapGrhs.Clear();

                if (_selectedMapGrhs.Count > 0 || _mouseDragStart == Vector2.Zero)
                    return;

                _selectedMapGrhs.Clear();

                Vector2 mouseDragEnd = Container.Camera.ToWorld(e.X, e.Y);
                Vector2 min = _mouseDragStart.Min(mouseDragEnd);
                Vector2 max = _mouseDragStart.Max(mouseDragEnd);
                Vector2 size = max - min;

                var rect = new Rectangle((int)min.X, (int)min.Y, (int)size.X, (int)size.Y);
                var selectAreaObjs = Container.Map.Spatial.GetMany<MapGrh>(rect);
                _selectedMapGrhs.AddRange(selectAreaObjs);

                Container.SelectedObjs.SetManySelected(_selectedMapGrhs.OfType<object>());

                // Move transbox
                if (_selectedMapGrhs.Count > 1)
                    _mapGrhMoveBox = new TransBox(TransBoxType.Move, null, cursorPos);
                else
                {
                    _mapGrhMoveBox = null;
                    Container.SelectedTransBox = null;
                    _selectedMapGrhs.Clear();
                }

                _mouseDragStart = Vector2.Zero;
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the mouse wheel has moved.
        /// </summary>
        /// <param name="amount">How much the mouse wheel has scrolled, and which direction.</param>
        public override void MoveMouseWheel(int amount)
        {
            // Change the layer for the focused MapGrh
            var mapGrh = Container.SelectedObjs.Focused as MapGrh;
            if (mapGrh == null)
                return;

            mapGrh.LayerDepth = (short)(mapGrh.LayerDepth + amount).Clamp(short.MinValue, short.MaxValue);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            // Find the stuff to delete
            var toDelete = Container.SelectedObjs.SelectedObjects.OfType<MapGrh>().ToImmutable();

            // Clear selection
            Container.SelectedObjs.Clear();
            _selectedMapGrhs.Clear();
            _mapGrhMoveBox = null;

            // Delete all selected
            foreach (var mg in toDelete)
            {
                Container.Map.RemoveMapGrh(mg);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles generic updating of the cursor. This is
        /// called every frame.
        /// </summary>
        public override void UpdateCursor()
        {
            bool isOverBox = false;
            if (_mapGrhMoveBox != null)
            {
                var cursorRect = new Rectangle((int)Container.CursorPos.X, (int)Container.CursorPos.Y, 1, 1);
                var boxPos = _mapGrhMoveBox.Position;
                var boxRect = new Rectangle((int)boxPos.X, (int)boxPos.Y, _mapGrhMoveBox.Area.Width, _mapGrhMoveBox.Area.Height);
                isOverBox = cursorRect.Intersects(boxRect);
            }

            if (isOverBox || _selectedMapGrhs.Count != 0 && Container.MouseButton == MouseButtons.Left)
                Container.Cursor = Cursors.SizeAll;
            else
                Container.Cursor = Cursors.Default;
        }
    }
}