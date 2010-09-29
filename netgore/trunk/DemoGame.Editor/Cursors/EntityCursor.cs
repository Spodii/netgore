using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;
using Image = System.Drawing.Image;

namespace DemoGame.Editor
{
    sealed class EntityCursor : EditorCursor<EditMapForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuIgnoreWalls;

        Vector2 _selectionOffset;
        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

        /// <summary>
        /// Property to access the <see cref="MapScreenControl"/>. Provided purely for convenience.
        /// </summary>
        MapScreenControl MSC { get { return Container.MapScreenControl; } }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM { get { return GlobalState.Instance.Map.SelectedObjsManager; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCursor"/> class.
        /// </summary>
        public EntityCursor()
        {
            _mnuIgnoreWalls = new MenuItem("Ignore Walls", Menu_IgnoreWalls_Click) { Checked = true };
            _contextMenu = new ContextMenu(new MenuItem[] { _mnuIgnoreWalls });
        }

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_entities; }
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
        public override int ToolbarPriority
        {
            get { return 0; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_toolTip))
                spriteBatch.DrawStringShaded(GlobalState.Instance.DefaultRenderFont, _toolTip, _toolTipPos, Color.White, Color.Black);
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

        Entity GetEntityUnderCursor(EditMapForm screen)
        {
            var cursorPos = screen.MapScreenControl.CursorPos;
            return screen.MapScreenControl.Map.Spatial.Get<Entity>(cursorPos, GetEntityUnderCursorFilter);
        }

        bool GetEntityUnderCursorFilter(Entity entity)
        {
            if (entity is CharacterEntity)
                return false;

            if (_mnuIgnoreWalls.Checked && entity is WallEntityBase)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the position to display the tooltip text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The tooltip text.</param>
        /// <param name="entity">The entity the tooltip is for.</param>
        /// <returns>The position to display the tooltip text.</returns>
        public static Vector2 GetToolTipPos(Font font, string text, ISpatial entity)
        {
            var pos = new Vector2(entity.Max.X, entity.Position.Y);
            pos -= new Vector2(5, (font.GetLineSpacing() * text.Split('\n').Length) + 5);
            return pos;
        }

        void Menu_IgnoreWalls_Click(object sender, EventArgs e)
        {
            _mnuIgnoreWalls.Checked = !_mnuIgnoreWalls.Checked;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // Set the selected entity to the first entity we find at the cursor
            SOM.SetSelected(GetEntityUnderCursor(Container));

            // Set the offset
            var focusedEntity = SOM.Focused as Entity;
            if (focusedEntity != null)
                _selectionOffset = MSC.CursorPos - focusedEntity.Position;
        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            // Get the map and ensure a valid cursor position
            var map = MSC.Map;
            if (map == null || !map.IsInMapBoundaries(MSC.CursorPos))
                return;

            var focusedEntity = SOM.Focused as Entity;

            if (focusedEntity != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Input.IsCtrlDown)
                    {
                        // Resize the entity
                        var size = MSC.CursorPos - focusedEntity.Position;
                        if (size.X < 4)
                            size.X = 4;
                        if (size.Y < 4)
                            size.Y = 4;
                        map.SafeResizeEntity(focusedEntity, size);
                    }
                    else
                    {
                        // Move the entity
                        map.SafeTeleportEntity(focusedEntity, MSC.CursorPos - _selectionOffset);
                    }
                }
            }
            else
            {
                // Set the tooltip to the entity under the cursor
                var hoverEntity = GetEntityUnderCursor(Container);

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
                    _toolTipPos = GetToolTipPos(GlobalState.Instance.DefaultRenderFont, _toolTip, hoverEntity);
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            foreach (var selected in SOM.SelectedObjects.OfType<Entity>())
            {
                selected.Dispose();
            }

            SOM.Clear();
        }
    }
}