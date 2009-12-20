using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A control that focuses around displaying an <see cref="ISprite"/>.
    /// </summary>
    public abstract class SpriteControl : Control
    {
        static readonly object _eventChangeSprite = new object();
        static readonly object _eventChangeStretchSprite = new object();
        ISprite _sprite;
        bool _stretch = true;

        /// <summary>
        /// Notifies listeners when the <see cref="SpriteControl.Sprite"/> has changed.
        /// </summary>
        public event ControlEventHandler OnChangeSprite
        {
            add { Events.AddHandler(_eventChangeSprite, value); }
            remove { Events.RemoveHandler(_eventChangeSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="SpriteControl.StretchSprite"/> value has changed.
        /// </summary>
        public event ControlEventHandler OnChangeStretchSprite
        {
            add { Events.AddHandler(_eventChangeStretchSprite, value); }
            remove { Events.RemoveHandler(_eventChangeStretchSprite, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        protected SpriteControl(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        protected SpriteControl(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Gets or sets the sprite to draw on this <see cref="Control"/>.
        /// </summary>
        public ISprite Sprite
        {
            get { return _sprite; }
            set
            {
                if (_sprite == value)
                    return;

                _sprite = value;

                InvokeChangeSprite();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="SpriteControl.Sprite"/> is drawn stretched when drawn.
        /// </summary>
        public bool StretchSprite
        {
            get { return _stretch; }
            set
            {
                if (_stretch == value)
                    return;

                _stretch = value;

                InvokeChangeStretchSprite();
            }
        }

        /// <summary>
        /// Handles when the <see cref="SpriteControl.Sprite"/> has changed.
        /// This is called immediately before <see cref="SpriteControl.OnChangeSprite"/>.
        /// Override this method instead of using an event hook on <see cref="SpriteControl.OnChangeSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeSprite()
        {
        }

        /// <summary>
        /// Handles when the <see cref="SpriteControl.StretchSprite"/> value has changed.
        /// This is called immediately before <see cref="SpriteControl.OnChangeStretchSprite"/>.
        /// Override this method instead of using an event hook on <see cref="SpriteControl.OnChangeStretchSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeStretchSprite()
        {
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Check for a valid texture
            if (_sprite == null || _sprite.Texture == null)
                return;

            Vector2 sp = ScreenPosition;
            Vector2 min = sp + new Vector2(Border.LeftWidth, Border.TopHeight);

            // Draw the picture
            if (StretchSprite)
            {
                // Stretched draw
                Vector2 drawSize = ClientSize;
                Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)drawSize.X, (int)drawSize.Y);
                _sprite.Draw(spriteBatch, dest, Color.White);
            }
            else
            {
                // Non-stretched draw
                _sprite.Draw(spriteBatch, min, Color.White);
            }
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeSprite()
        {
            ChangeSprite();
            var handler = Events[_eventChangeSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeStretchSprite()
        {
            ChangeStretchSprite();
            var handler = Events[_eventChangeStretchSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            CanDrag = false;
        }

        /// <summary>
        /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(int currentTime)
        {
            base.UpdateControl(currentTime);

            if (Sprite != null)
                Sprite.Update(currentTime);
        }
    }
}