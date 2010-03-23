using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.AI;
using NetGore.EditorTools;
using NetGore.Graphics;
using Color = Microsoft.Xna.Framework.Graphics.Color;

namespace DemoGame.MapEditor
{
    sealed class AINodeCursor : EditorCursor <ScreenForm>
    {
        readonly ContextMenu _contextMenu;
        readonly MenuItem _mnuWeight;

        Vector2 _selectionOffset;
        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

        public AINodeCursor()
        {
            _mnuWeight = new MenuItem("Block", Menu_Clear_OnClick) { Checked = false };
            _contextMenu = new ContextMenu(new MenuItem[] {_mnuWeight});
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
            get { return "Set Node Weight"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public override int ToolbarPriority
        {
            get { return 10; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            if (!string.IsNullOrEmpty(_toolTip))
                spriteBatch.DrawStringShaded(Container.SpriteFont, _toolTip, _toolTipPos, Color.White, Color.Black);
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

        int[] GetMemoryCellUnderCursor(ScreenForm screen)
        {
            Vector2 CursorPos = screen.CursorPos;


            for (int X = 0; X < screen.Map.MemoryMap.MemoryCells.Count; X++)
            {
                for (int Y = 0; Y < screen.Map.MemoryMap.MemoryCells[X].Count; Y++)
                {
                    if (screen.Map.MemoryMap.MemoryCells[X][Y].Cell.Contains((int)CursorPos.X, (int)CursorPos.Y))
                    {
                        return new int[] {X, Y};
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the position to display the tooltip text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The tooltip text.</param>
        /// <param name="entity">The MemoryCell the tooltip is for.</param>
        /// <returns>The position to display the tooltip text.</returns>
        public static Vector2 GetToolTipPos(SpriteFont font, string text, MemoryCell memoryCell)
        {
            var pos = new Vector2(memoryCell.Cell.Right, memoryCell.Cell.Y);
            pos -= new Vector2(5, (font.LineSpacing * text.Split('\n').Length) + 5);
            return pos;
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int[] id = GetMemoryCellUnderCursor(Container);
            Container.SelectedObjs.SetSelected(Container.Map.MemoryMap.MemoryCells[id[0]][id[1]]);

            if (!_mnuWeight.Checked)
            {
                Container.Map.MemoryMap.MemoryCells[id[0]][id[1]].Weight += 10;
            }
            else
            {
                Container.Map.MemoryMap.MemoryCells[id[0]][id[1]].Weight = 0;
            }

        }

        /// <summary>
        /// When overridden in the derived class, handles when the cursor has moved.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseMove(MouseEventArgs e)
        {
            // Get the map and ensure a valid cursor position
            Map map = Container.Map;
            if (map == null || !map.IsInMapBoundaries(Container.CursorPos))
                return;

                // Set the tooltip to the entity under the cursor
                int[] id = GetMemoryCellUnderCursor(Container);
                MemoryCell hoverNode = Container.Map.MemoryMap.MemoryCells[id[0]][id[1]];

                if (hoverNode == null)
                {
                    _toolTip = string.Empty;
                    _toolTipObject = null;
                }
                else if (_toolTipObject != hoverNode)
                {
                    _toolTipObject = hoverNode;
                    _toolTip = string.Format("Weight({2})", hoverNode, hoverNode.Cell.Location, hoverNode.Weight);
                    _toolTipPos = GetToolTipPos(Container.SpriteFont, _toolTip, hoverNode);
                }
        }

        /// <summary>
        /// When overridden in the derived class, handles when the delete button has been pressed.
        /// </summary>
        public override void PressDelete()
        {
            int[] id = GetMemoryCellUnderCursor(Container);
            Container.Map.MemoryMap.MemoryCells[id[0]][id[1]].Weight = 0;
        }

        void Menu_Clear_OnClick(object sender, EventArgs e)
        {
            _mnuWeight.Checked = !_mnuWeight.Checked;
        }


    }
}
