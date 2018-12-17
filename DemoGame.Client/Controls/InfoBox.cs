using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Box that displays non-interactive, short-lived messages in a cycling manner.
    /// </summary>
    public class InfoBox
    {
        readonly Font _font;
        readonly List<InfoBoxItem> _items;

        private Color _defaultColor = Color.Black;
        int _maxItems = 20;
        int _messageLife = 5000;
        Vector2 _position;

        /// <summary>
        /// InfoBox constructor
        /// </summary>
        /// <param name="position">Position of the bottom-right corner of the InfoBox</param>
        /// <param name="font"><see cref="Font"/> used to draw the InfoBox text</param>
        public InfoBox(Vector2 position, Font font)
        {
            _items = new List<InfoBoxItem>(_maxItems);
            _position = position;
            _font = font;
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
        /// Adds a message to the InfoBox.
        /// </summary>
        /// <param name="message">Message to add.</param>
        public void Add(string message)
        {
            Add(message, _defaultColor);
        }

        /// <summary>
        /// Adds a message to the InfoBox.
        /// </summary>
        /// <param name="message">Message to add.</param>
        /// <param name="color">Color of the message's text.</param>
        public void Add(string message, Color color)
        {
            ThreadAsserts.IsMainThread();

            var newItem = new InfoBoxItem(TickCount.Now, message, color, _font);

            // If we are full, remove the old messages until we have room
            while (_items.Count >= _maxItems)
            {
                _items.RemoveAt(0);
            }

            // Add the new item to the list
            _items.Add(newItem);
        }

        /// <summary>
        /// Draws the InfoBox.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw with.</param>
        public void Draw(ISpriteBatch sb)
        {
            ThreadAsserts.IsMainThread();

            // Remove dead items
            while (_items.Count > 0 && _items[0].CreatedTime + _messageLife < TickCount.Now)
            {
                _items.RemoveAt(0);
            }

            // Loop through all items
            var i = 0;
            foreach (var item in _items)
            {
                // Set the position
                var pos = _position;
                pos.Y -= _font.GetLineSpacing() * (i++ + 1);
                pos.X -= item.Width;

                // Set the color
                var lifeLeft = (item.CreatedTime + _messageLife) - TickCount.Now;
                var alpha = (byte)Math.Min(255, lifeLeft);
                var color = new Color(item.Color.R, item.Color.G, item.Color.B, alpha);

                // Draw
                sb.DrawString(_font, item.Message, pos, color);
                
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
            public readonly TickCount CreatedTime;

            /// <summary>
            /// Item's text.
            /// </summary>
            public readonly string Message;

            /// <summary>
            /// Width of the Message.
            /// </summary>
            public readonly float Width;

            /// <summary>
            /// Initializes a new instance of the <see cref="InfoBoxItem"/> struct.
            /// </summary>
            /// <param name="time">Current time.</param>
            /// <param name="msg">Message to display.</param>
            /// <param name="color">Color of the text.</param>
            /// <param name="font"><see cref="Font"/> that will be used to calculate the Width.</param>
            public InfoBoxItem(TickCount time, string msg, Color color, Font font)
            {
                CreatedTime = time;
                Message = msg;
                Color = color;
                Width = font.MeasureString(msg).X;
            }
        }
    }
}