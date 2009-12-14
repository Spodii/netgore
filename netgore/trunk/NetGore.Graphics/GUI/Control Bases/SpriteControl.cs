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
        ISprite _sprite;
        bool _stretch = true;

        /// <summary>
        /// Notifies listeners when the Sprite has changed.
        /// </summary>
        public event ControlEventHandler OnChangeSprite;

        /// <summary>
        /// Notifies listeners when the StretchSprite value changes.
        /// </summary>
        public event ControlEventHandler OnChangeStretchSprite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteControl"/> class.
        /// </summary>
        /// <param name="gui">GUIManager this PictureControl will be part of.</param>
        /// <param name="settings">Settings for this PictureControl.</param>
        /// <param name="position">Position of the PictureControl reletive to its parent.</param>
        /// <param name="sprite">Sprite to display.</param>
        /// <param name="size">Size of the PictureControl</param>
        /// <param name="parent">Parent Control of this PictureControl (null for a root Control).</param>
        protected SpriteControl(GUIManagerBase gui, SpriteControlSettings settings, Vector2 position, ISprite sprite, Vector2 size,
                                Control parent) : base(gui, settings, position, size, parent)
        {
            _sprite = sprite;

            // Disable dragging by default
            CanDrag = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteControl"/> class.
        /// </summary>
        /// <param name="settings">Settings for this PictureControl</param>
        /// <param name="position">Position of the Control reletive to its parent</param>
        /// <param name="sprite">Sprite to display</param>
        /// <param name="parent">Parent Control of this Control (null for a root Control)</param>
        protected SpriteControl(SpriteControlSettings settings, Vector2 position, ISprite sprite, Control parent)
            : this(parent.GUIManager, settings, position, sprite, Vector2.Zero, parent)
        {
            if (_sprite == null)
                throw new ArgumentNullException("sprite");

            // Set the size of the control to the size of the sprite plus the size of the border
            Size = new Vector2(_sprite.Source.Width, _sprite.Source.Height) + Border.Size;
        }

        /// <summary>
        /// Gets or sets the Control's <see cref="ISprite"/>.
        /// </summary>
        public ISprite Sprite
        {
            get { return _sprite; }
            set
            {
                if (_sprite != value)
                {
                    _sprite = value;
                    if (OnChangeSprite != null)
                        OnChangeSprite(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets if the sprite is drawn stretched or not.
        /// </summary>
        public bool StretchSprite
        {
            get { return _stretch; }
            set
            {
                if (_stretch != value)
                {
                    _stretch = value;
                    if (OnChangeStretchSprite != null)
                        OnChangeStretchSprite(this);
                }
            }
        }

        /// <summary>
        /// Draws the control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
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
                Vector2 drawSize = Size - Border.Size;
                Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)drawSize.X, (int)drawSize.Y);
                _sprite.Draw(spriteBatch, dest, Color.White);
            }
            else
            {
                // Non-stretched draw
                _sprite.Draw(spriteBatch, min, Color.White);
            }
        }

        protected override void UpdateControl(int currentTime)
        {
            base.UpdateControl(currentTime);

            if (Sprite != null)
                Sprite.Update(currentTime);
        }
    }
}