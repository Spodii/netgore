using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for a manager for <see cref="ChatBubble"/>s.
    /// </summary>
    public abstract class ChatBubbleManagerBase
    {
        /// <summary>
        /// Queue of the <see cref="ChatBubble"/>s to remove. Only use in <see cref="ChatBubbleManagerBase.Update"/>.
        /// </summary>
        readonly Stack<Entity> _bubblesToRemove = new Stack<Entity>();

        readonly Dictionary<Entity, ChatBubble> _chatBubbles = new Dictionary<Entity, ChatBubble>();

        ControlBorder _border;
        SpriteFont _font;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBubbleManagerBase"/> class.
        /// </summary>
        /// <param name="border">The <see cref="ControlBorder"/> to use for the <see cref="ChatBubble"/>s when
        /// drawing.</param>
        /// <param name="font">The <see cref="SpriteFont"/> to use to write the bubble's text.</param>
        protected ChatBubbleManagerBase(ControlBorder border, SpriteFont font)
        {
            Lifespan = 5000;
            FontColor = Color.Black;
            BorderColor = Color.White;

            _border = border;
            _font = font;
        }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> to use for drawing the border of the bubbles.
        /// Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ControlBorder Border
        {
            get { return _border; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _border = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to draw the border for a <see cref="ChatBubble"/>.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SpriteFont"/> used to draw the chat bubble text.
        /// Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _font = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to draw the text for a <see cref="ChatBubble"/>.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Gets or sets how long in milliseconds a <see cref="ChatBubble"/> lives.
        /// </summary>
        public int Lifespan { get; set; }

        /// <summary>
        /// Adds a <see cref="ChatBubble"/> to this <see cref="ChatBubbleManagerBase"/>.
        /// </summary>
        /// <param name="owner">The <see cref="Entity"/> to attach the <see cref="ChatBubble"/> to.</param>
        /// <param name="text">The text to display in the bubble.</param>
        /// <param name="currentTime">The current game time.</param>
        public void Add(Entity owner, string text, int currentTime)
        {
            // Create the new chat bubble
            var bubble = new ChatBubble(this, owner, text, currentTime);

            // Remove the old bubble for the owner if they already had a bubble
            if (_chatBubbles.ContainsKey(owner))
            {
                _chatBubbles[owner].Destroy();
                _chatBubbles[owner] = bubble;
            }
            else
                _chatBubbles.Add(owner, bubble);
        }

        /// <summary>
        /// Draws all of the <see cref="ChatBubble"/>s in this <see cref="ChatBubbleManagerBase"/>.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw to.</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (var bubble in _chatBubbles.Values)
            {
                bubble.Draw(sb);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the
        /// </summary>
        /// <param name="target">The <see cref="Entity"/> that the <see cref="ChatBubble"/> will be draw at.</param>
        /// <param name="bubbleSize">The size of the bubble.</param>
        /// <returns>The coordinates of the top-left corner to draw the <see cref="ChatBubble"/> at.</returns>
        public abstract Vector2 GetDrawOffset(Entity target, Vector2 bubbleSize);

        /// <summary>
        /// Updates the <see cref="ChatBubble"/>s in this <see cref="ChatBubbleManagerBase"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(int currentTime)
        {
            Debug.Assert(_bubblesToRemove.Count == 0, "The remove queue should NOT be used outside of Update()!");

            // Update the bubbles
            foreach (var kvp in _chatBubbles)
            {
                // Update the bubble
                kvp.Value.Update(currentTime);

                // If it expired, queue it for removal
                if (kvp.Value.IsExpired)
                {
                    Debug.Assert(!_bubblesToRemove.Contains(kvp.Key),
                                 "This entity is already in the remove queue... how did this happen?");
                    _bubblesToRemove.Push(kvp.Key);
                }
            }

            // Remove dead bubbles
            while (_bubblesToRemove.Count > 0)
            {
                Entity toRemove = _bubblesToRemove.Pop();
                Debug.Assert(_chatBubbles[toRemove].IsExpired,
                             "Why are we removing a non-expired bubble with the remove stack...?");
                _chatBubbles.Remove(toRemove);
            }

            Debug.Assert(_bubblesToRemove.Count == 0, "The remove queue should have been empty by now...");
        }
    }
}