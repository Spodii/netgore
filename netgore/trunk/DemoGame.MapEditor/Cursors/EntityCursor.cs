using System;
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

namespace DemoGame.MapEditor
{
    sealed class EntityCursor : MapEditorCursorBase<ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuIgnoreWalls;

        Vector2 _selectionOffset;
        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCursor"/> class.
        /// </summary>
        public EntityCursor()
        {
            _mnuIgnoreWalls = new MenuItem("Ignore Walls", Menu_IgnoreWalls_Click) { Checked = true };
            _contextMenu =
                new ContextMenu(new MenuItem[] { _mnuIgnoreWalls });
        }

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_select; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Select Entity"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        /// <value></value>
        public override int ToolbarPriority
        {
            get { return 0; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        public override void DrawInterface(ScreenForm screen)
        {
            base.DrawInterface(screen);

            screen.SpriteBatch.DrawStringShaded(screen.SpriteFont, _toolTip, _toolTipPos, Color.White, Color.Black);
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

        Entity GetEntityUnderCursor(ScreenForm screen)
        {
            Vector2 cursorPos = screen.CursorPos;
            return screen.Map.Spatial.GetEntity<Entity>(cursorPos, GetEntityUnderCursorFilter);
        }

        bool GetEntityUnderCursorFilter(Entity entity)
        {
            if (entity is CharacterEntity)
                return false;

            if (_mnuIgnoreWalls.Checked && entity is WallEntityBase)
                return false;

            return true;
        }

        void Menu_IgnoreWalls_Click(object sender, EventArgs e)
        {
            _mnuIgnoreWalls.Checked = !_mnuIgnoreWalls.Checked;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // Set the selected entity to the first entity we find at the cursor
            screen.SelectedObjectsManager.SetSelected(GetEntityUnderCursor(screen));

            // Set the offset
            var focusedEntity = screen.SelectedObjectsManager.Focused as Entity;
            if (focusedEntity != null)
                _selectionOffset = screen.CursorPos - focusedEntity.Position;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="screen">Screen that the cursor is on.</param>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            var focusedEntity = screen.SelectedObjectsManager.Focused as Entity;

            // Get the map
            Map map = screen.Map;
            if (map == null)
                return;

            // Check for a valid cursor position
            if (!map.IsInMapBoundaries(screen.CursorPos))
                return;

            if (focusedEntity != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (screen.KeyEventArgs.Control)
                    {
                        // Resize the entity
                        Vector2 size = screen.CursorPos - focusedEntity.Position;
                        if (size.X < 4)
                            size.X = 4;
                        if (size.Y < 4)
                            size.Y = 4;
                        map.SafeResizeEntity(focusedEntity, size);
                    }
                    else
                    {
                        // Move the entity
                        map.SafeTeleportEntity(focusedEntity, screen.CursorPos - _selectionOffset);
                    }
                }
            }
            else
            {
                // Set the tooltip to the entity under the cursor
                Entity hoverEntity = GetEntityUnderCursor(screen);

                if (hoverEntity == null)
                {
                    _toolTip = string.Empty;
                    _toolTipObject = null;
                }
                else if (_toolTipObject != hoverEntity)
                {
                    _toolTipObject = hoverEntity;
                    _toolTip = string.Format("{0}\n{1} ({2}x{3})", hoverEntity, hoverEntity.Position, hoverEntity.Size.X,
                                             hoverEntity.Size.Y);
                    _toolTipPos = new Vector2(hoverEntity.Max.X, hoverEntity.Position.Y);
                    _toolTipPos -= new Vector2(5, screen.SpriteFont.LineSpacing * _toolTip.Split('\n').Length);
                }
            }
        }
    }
}