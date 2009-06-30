using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Network;

namespace DemoGame.Client
{
    public class ItemInfoTooltip
    {
        /// <summary>
        /// Delay, in milliseconds, between hovering over an item and showing the item information display.
        /// </summary>
        const int _showItemInfoDelay = 300;

        readonly ItemInfo _itemInfo;
        object _hoverObject = null;
        int _hoverStartTime = int.MinValue;
        bool _sentItemInfoRequest;
        byte _slot;
        ItemInfoSource _source;

        /// <summary>
        /// Gets the ItemInfo containing the information drawn by this ItemInfoTooltip.
        /// </summary>
        public ItemInfo ItemInfo
        {
            get { return _itemInfo; }
        }

        public ItemInfoTooltip(ISocketSender socket)
        {
            _itemInfo = new ItemInfo(socket);
        }

        /// <summary>
        /// Draws the item's information
        /// </summary>
        /// <param name="pos">Position to draw at.</param>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="font">Font to use.</param>
        public void Draw(Vector2 pos, SpriteBatch sb, SpriteFont font)
        {
            if (!_sentItemInfoRequest || ItemInfo == null || !ItemInfo.IsUpdated)
                return;

            // Get all the non-zero stats and count them
            var nonZeroStats = ItemInfo.Stats.Where(stat => stat.Value != 0);
            int numNonZeroStats = nonZeroStats.Count();

            // Basic item information
            var lines = new List<string>(numNonZeroStats + 10) { ItemInfo.Name, ItemInfo.Description, "Value: " + ItemInfo.Value };

            // Item stats
            if (numNonZeroStats > 0)
            {
                lines.Add(null);
                lines.Add("Stats:");
                foreach (IStat stat in nonZeroStats)
                {
                    lines.Add(string.Format(" * {0}: {1}", stat.StatType, stat.Value));
                }
            }

            // Draw the background
            DrawBackground(pos, sb, font, lines);

            // Draw the item information
            foreach (string line in lines)
            {
                pos = DrawLine(pos, line, sb, font);
            }
        }

        /// <summary>
        /// Draws the background for the item's information to go on.
        /// </summary>
        /// <param name="pos">Position to draw at.</param>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="font">Font to use.</param>
        /// <param name="lines">The lines that will be drawn.</param>
        static void DrawBackground(Vector2 pos, SpriteBatch sb, SpriteFont font, IEnumerable<string> lines)
        {
            const int borderSize = 10;

            // Get the width of the longest line
            int width = (int)lines.Max(line => string.IsNullOrEmpty(line) ? 0 : font.MeasureString(line).X);

            // Get the height by just counting the lines and multiplying by the vertical spacing
            int height = lines.Count() * font.LineSpacing;

            // Create the dest rectangle
            Rectangle rect = new Rectangle((int)pos.X - borderSize, (int)pos.Y - borderSize, width + borderSize * 2,
                                           height + borderSize * 2);

            // Draw the background rectangle
            XNARectangle.Draw(sb, rect, new Color(0, 0, 0, 150), new Color(0, 0, 0, 225));
        }

        /// <summary>
        /// Draws a single string line.
        /// </summary>
        /// <param name="pos">Position to draw at.</param>
        /// <param name="text">String to write.</param>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="font">Font to use.</param>
        /// <returns>Position for the next line.</returns>
        static Vector2 DrawLine(Vector2 pos, string text, SpriteBatch sb, SpriteFont font)
        {
            if (!string.IsNullOrEmpty(text))
                sb.DrawString(font, text, pos, Color.White);

            return pos + new Vector2(0, font.LineSpacing);
        }

        static int GetTime()
        {
            // HACK: Not a very nice way to get the time. Should be using something with an IGetTime.
            return Environment.TickCount;
        }

        public void HandleMouseEnter(object sender, ItemInfoSource source, byte slot)
        {
            if (_hoverObject == sender)
                return;

            _hoverObject = sender;
            _sentItemInfoRequest = false;
            _hoverStartTime = GetTime();

            _source = source;
            _slot = slot;
        }

// ReSharper disable UnusedParameter.Global
        public void HandleMouseLeave(object sender, ItemInfoSource source, byte slot)
        {
// ReSharper restore UnusedParameter.Global
            // Only set the hoverObject to null if it came from the sender
            if (_hoverObject != sender)
                return;

            _hoverObject = null;
            _sentItemInfoRequest = false;
        }

// ReSharper disable UnusedParameter.Global
        public void HandleMouseMove(object sender, ItemInfoSource source, byte slot)
        {
// ReSharper restore UnusedParameter.Global
            _hoverStartTime = GetTime();
        }

        public void Update()
        {
            // Check if the ItemInfo is needed
            if (_sentItemInfoRequest || _hoverObject == null)
                return;

            // Check that enough time has elapsed
            int time = GetTime();
            if (time - _hoverStartTime < _showItemInfoDelay)
                return;

            // Send the request
            _sentItemInfoRequest = true;

            switch (_source)
            {
                case ItemInfoSource.Equipped:
                    ItemInfo.GetEquipmentItemInfo((EquipmentSlot)_slot);
                    break;

                case ItemInfoSource.Inventory:
                    ItemInfo.GetInventoryItemInfo(_slot);
                    break;
            }
        }
    }
}