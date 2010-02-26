using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace DemoGame.Client
{
    /// <summary>
    /// Box that displays non-interactive, short-lived messages in a cycling manner.
    /// </summary>
    public class InfoBox
    {
        readonly List<InfoBoxItem> _items;
        readonly SpriteFont _sf;

        Color _defaultColor = Color.Green;
        int _maxItems = 20;
        int _messageLife = 5000;
        Vector2 _position;

        /// <summary>
        /// InfoBox constructor
        /// </summary>
        /// <param name="position">Position of the bottom-right corner of the InfoBox</param>
        /// <param name="sf">SpriteFont used to draw the InfoBox text</param>
        public InfoBox(Vector2 position, SpriteFont sf)
        {
            _items = new List<InfoBoxItem>(_maxItems);
            _position = position;
            _sf = sf;
        }

        /// <summary>
        /// Gets or sets the default color of the messages
        /// </summary>
        public Color DefaultColor
        {
            get { return _defaultColor; }
            set { _defaultColor = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of items to display at once
        /// </summary>
        public int MaxItems
        {
            get { return _maxItems; }
            set { _maxItems = value; }
        }

        /// <summary>
        /// Gets or sets the life of each message in milliseconds
        /// </summary>
        public int MessageLife
        {
            get { return _messageLife; }
            set { _messageLife = value; }
        }

        /// <summary>
        /// Gets or sets the position (bottom-right corner) of the InfoBox.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Adds a message to the InfoBox
        /// </summary>
        /// <param name="message">Message to add</param>
        public void Add(string message)
        {
            Add(message, _defaultColor);
        }

        /// <summary>
        /// Adds a message to the InfoBox
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <param name="color">Color of the message's text</param>
        public void Add(string message, Color color)
        {
            ThreadAsserts.IsMainThread();

            InfoBoxItem newItem = new InfoBoxItem(Environment.TickCount, message, color, _sf);

            // If we are full, remove the old messages until we have room
            while (_items.Count >= _maxItems)
            {
                _items.RemoveAt(0);
            }

            // Add the new item to the list
            _items.Add(newItem);
        }

        /// <summary>
        /// Draws the InfoBox
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            ThreadAsserts.IsMainThread();

            // Remove dead items
            while (_items.Count > 0 && _items[0].CreatedTime + _messageLife < Environment.TickCount)
            {
                _items.RemoveAt(0);
            }

            // Loop through all items
            int i = 0;
            foreach (var item in _items)
            {
                // Set the position
                Vector2 pos = _position;
                pos.Y -= _sf.LineSpacing * (i++ + 1);
                pos.X -= item.Width;

                // Set the color
                int lifeLeft = (item.CreatedTime + _messageLife) - Environment.TickCount;
                byte alpha = (byte)Math.Min(255, lifeLeft);
                Color color = new Color(item.Color.R, item.Color.G, item.Color.B, alpha);

                // Draw
                sb.DrawString(_sf, item.Message, pos.Round(), color);
            }
        }

        /// <summary>
        /// An entry for the InfoBox.
        /// </summary>
        struct InfoBoxItem
        {
            /// <summary>
            /// Color of the item's text.
            /// </summary>
            public readonly Color Color;

            /// <summary>
            /// Time the item was created.
            /// </summary>
            public readonly int CreatedTime;

            /// <summary>
            /// Item's text.
            /// </summary>
            public readonly string Message;

            /// <summary>
            /// Width of the Message.
            /// </summary>
            public readonly float Width;

            /// <summary>
            /// InfoBoxItem constructor.
            /// </summary>
            /// <param name="time">Current time.</param>
            /// <param name="msg">Message to display.</param>
            /// <param name="color">Color of the text.</param>
            /// <param name="sf">SpriteFont that will be used to calculate the Width.</param>
            public InfoBoxItem(int time, string msg, Color color, SpriteFont sf)
            {
                CreatedTime = time;
                Message = msg;
                Color = color;
                Width = sf.MeasureString(msg).X;
            }
        }
    }
}