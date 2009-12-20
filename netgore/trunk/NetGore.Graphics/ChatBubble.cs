using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Collections;
using NetGore.Graphics.GUI;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for a manager for <see cref="ChatBubble"/>s.
    /// </summary>
    public abstract class ChatBubbleManagerBase
    {
        readonly Dictionary<Entity, ChatBubble> _chatBubbles = new Dictionary<Entity, ChatBubble>();

        SpriteFont _font;
        ControlBorder _border;

        /// <summary>
        /// Gets or sets the color to draw the text for a <see cref="ChatBubble"/>.
        /// </summary>
        public Color FontColor { get; set; }

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
        /// Gets or sets how long in milliseconds a <see cref="ChatBubble"/> lives.
        /// </summary>
        public int Lifespan { get; set; }

        /// <summary>
        /// Queue of the <see cref="ChatBubble"/>s to remove. Only use in <see cref="ChatBubbleManagerBase.Update"/>.
        /// </summary>
        readonly Stack<Entity> _bubblesToRemove = new Stack<Entity>();

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
                    Debug.Assert(!_bubblesToRemove.Contains(kvp.Key), "This entity is already in the remove queue... how did this happen?");
                    _bubblesToRemove.Push(kvp.Key);
                }
            }

            // Remove dead bubbles
            while (_bubblesToRemove.Count > 0)
            {
                Entity toRemove = _bubblesToRemove.Pop();
                Debug.Assert(_chatBubbles[toRemove].IsExpired, "Why are we removing a non-expired bubble with the remove stack...?");
                _chatBubbles.Remove(toRemove);
            }

            Debug.Assert(_bubblesToRemove.Count == 0, "The remove queue should have been empty by now...");
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
    }

    /// <summary>
    /// A bubble of text that appears near an <see cref="Entity"/> and is used to denote chatting text.
    /// </summary>
    public class ChatBubble
    {
        readonly Entity _owner;
        readonly int _deathTime;
        readonly string _text;

        /// <summary>
        /// Gets if the <see cref="ChatBubble"/> has expired.
        /// </summary>
        public bool IsExpired { get; private set; }

        /// <summary>
        /// Gets the <see cref="Entity"/> that this <see cref="ChatBubble"/> is attached to.
        /// </summary>
        public Entity Owner { get { return _owner; } }

        /// <summary>
        /// Gets the text that is displayed in this <see cref="ChatBubble"/>.
        /// </summary>
        public string Text { get { return _text; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBubble"/> class.
        /// </summary>
        public ChatBubble()
        {
        }

        /// <summary>
        /// Forces the <see cref="ChatBubble"/> to be destroyed immediately instead of waiting for it to expire.
        /// </summary>
        public void Destroy()
        {
            IsExpired = true;
        }

        /// <summary>
        /// Updates the <see cref="ChatBubble"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(int currentTime)
        {
            if (IsExpired)
                return;

            if (_deathTime <= currentTime)
            {
                Destroy();
                return;
            }
        }

        readonly Vector2 _textSize;

        /// <summary>
        /// Gets the size of the <see cref="ChatBubble"/>.
        /// </summary>
        public Vector2 Size { get { return _textSize + _manager.Border.Size; } }

        /// <summary>
        /// Calculates the size of the <see cref="ChatBubble"/>'s text (Border not included).
        /// </summary>
        /// <returns>The size of the <see cref="ChatBubble"/>'s text (Border not included).</returns>
        Vector2 CalculateSize()
        {
            return _manager.Font.MeasureString(Text);
        }

        /// <summary>
        /// Draws the <see cref="ChatBubble"/>.
        /// </summary>
        /// <param name="sb">The <see cref="SpriteBatch"/> to draw to.</param>
        public void Draw(SpriteBatch sb)
        {
            if (IsExpired)
                return;

            Vector2 size = Size;
            Vector2 drawPos = _manager.GetDrawOffset(Owner, size);

            // Draw the border
            Rectangle borderArea = new Rectangle((int)drawPos.X, (int)drawPos.Y, (int)size.X, (int)size.Y);
            _manager.Border.Draw(sb, borderArea);

            // Draw the text
            Vector2 textPos = drawPos + new Vector2(_manager.Border.LeftWidth, _manager.Border.TopHeight);
            sb.DrawString(_manager.Font, Text, textPos, _manager.FontColor);
        }

        readonly ChatBubbleManagerBase _manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBubble"/> class.
        /// </summary>
        /// <param name="manager">The <see cref="ChatBubble"/> manager.</param>
        /// <param name="owner">The <see cref="Entity"/> the <see cref="ChatBubble"/> is attached to.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="currentTime">The current game time.</param>
        public ChatBubble(ChatBubbleManagerBase manager, Entity owner, string text, int currentTime)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            _manager = manager;
            _owner = owner;
            _text = text;
            _deathTime = currentTime + manager.Lifespan;

            _textSize = CalculateSize();

            IsExpired = false;
        }
    }
}
