using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    class EntityCursor : EditorCursorBase
    {
        Entity _selectedEntity = null;
        Vector2 _selectionOffset;

        string _toolTip = string.Empty;
        Vector2 _toolTipPos;

        public override void DrawInterface(ScreenForm screen)
        {
            base.DrawInterface(screen);

            screen.SpriteBatch.DrawStringShaded(screen.SpriteFont, _toolTip, _toolTipPos, Color.White, Color.Black);
        }

        public override void MouseDown(ScreenForm screen, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Vector2 cursorPos = screen.CursorPos;

            // Set the selected entity to the first entity we find at the cursor
            _selectedEntity = screen.Map.GetEntity(cursorPos, entity => !(entity is WallEntityBase || entity is CharacterEntity));

            // Set the offset
            if (_selectedEntity != null)
                _selectionOffset = cursorPos - _selectedEntity.Position;
        }

        public override void MouseMove(ScreenForm screen, MouseEventArgs e)
        {
            // Get the map
            Map map = screen.Map;
            if (map == null)
                return;

            // Check for a valid cursor position
            if (!map.Contains(screen.CursorPos))
                return;

            if (_selectedEntity != null)
            {
                if (screen.KeyEventArgs.Control)
                {
                    // Resize the entity
                    Vector2 size = screen.CursorPos - _selectedEntity.Position;
                    if (size.X < 4)
                        size.X = 4;
                    if (size.Y < 4)
                        size.Y = 4;
                    map.SafeResizeEntity(_selectedEntity, size);
                }
                else
                {
                    // Move the entity
                    map.SafeTeleportEntity(_selectedEntity, screen.CursorPos - _selectionOffset);
                }
            }
            else
            {
                // Set the tooltip to the entity under the cursor
                Entity hoverEntity = map.GetEntity(screen.CursorPos);
                if (hoverEntity == null)
                    _toolTip = string.Empty;
                else
                {
                    // Set the tooltip text
                    _toolTip = string.Format("{0}\n{1}", hoverEntity.GetType(), hoverEntity);

                    // Default text to the top-right corner of the entity
                    _toolTipPos = new Vector2(hoverEntity.CB.Max.X, hoverEntity.CB.Min.Y);

                    // Move slightly to the center
                    _toolTipPos.X -= 5;

                    // Move up to fit all the added lines
                    _toolTipPos.Y -= screen.SpriteFont.LineSpacing * _toolTip.Split('\n').Length;
                }
            }
        }

        public override void MouseUp(ScreenForm screen, MouseEventArgs e)
        {
            // Deselect the selected entity
            _selectedEntity = null;
        }
    }
}