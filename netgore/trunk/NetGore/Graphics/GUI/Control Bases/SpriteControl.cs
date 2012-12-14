using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A control that focuses around displaying an <see cref="ISprite"/>.
    /// </summary>
    public abstract class SpriteControl : Control
    {
        static readonly object _eventSpriteChanged = new object();
        static readonly object _eventStretchSpriteChanged = new object();

        ISprite _sprite;
        bool _stretch = true;

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
        /// Notifies listeners when the <see cref="SpriteControl.Sprite"/> has changed.
        /// </summary>
        public event TypedEventHandler<Control> SpriteChanged
        {
            add { Events.AddHandler(_eventSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="SpriteControl.StretchSprite"/> value has changed.
        /// </summary>
        public event TypedEventHandler<Control> StretchSpriteChanged
        {
            add { Events.AddHandler(_eventStretchSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventStretchSpriteChanged, value); }
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

                InvokeSpriteChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="SpriteControl.Sprite"/> is drawn stretched when drawn.
        /// </summary>
        [SyncValue]
        public bool StretchSprite
        {
            get { return _stretch; }
            set
            {
                if (_stretch == value)
                    return;

                _stretch = value;

                InvokeStretchSpriteChanged();
            }
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Check for a valid sprite
            if (Sprite == null || Sprite.Texture == null)
                return;

            // Update the sprite before drawing it
            Sprite.Update(TickCount.Now);

            var sp = ScreenPosition;
            var min = sp + new Vector2(Border.LeftWidth, Border.TopHeight);

            // Draw the picture
            if (StretchSprite)
            {
                // Stretched draw
                var drawSize = ClientSize;
                var dest = new Rectangle(min.X, min.Y, drawSize.X, drawSize.Y);
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
        void InvokeSpriteChanged()
        {
            OnSpriteChanged();
            var handler = Events[_eventSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeStretchSpriteChanged()
        {
            OnStretchSpriteChanged();
            var handler = Events[_eventStretchSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles when the <see cref="SpriteControl.Sprite"/> has changed.
        /// This is called immediately before <see cref="SpriteControl.SpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="SpriteControl.SpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnSpriteChanged()
        {
        }

        /// <summary>
        /// Handles when the <see cref="SpriteControl.StretchSprite"/> value has changed.
        /// This is called immediately before <see cref="SpriteControl.StretchSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="SpriteControl.StretchSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnStretchSpriteChanged()
        {
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
    }
}