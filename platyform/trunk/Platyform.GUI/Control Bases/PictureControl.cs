using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platyform.Extensions;

namespace Platyform.Graphics.GUI
{
    /// <summary>
    /// A control that focuses around displaying a Sprite
    /// </summary>
    public abstract class SpriteControl : Control
    {
        ISprite _sprite;
        bool _stretch = true;

        /// <summary>
        /// Notifies when the Sprite has changed
        /// </summary>
        public event ControlEventHandler OnChangeSprite;

        /// <summary>
        /// Notifies when the StretchSprite value changes
        /// </summary>
        public event ControlEventHandler OnChangeStretchSprite;

        /// <summary>
        /// Gets or sets the Control's Sprite
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
        /// Gets or sets if the sprite is drawn stretched or not
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
        /// SpriteControl constructor
        /// </summary>
        /// <param name="gui">GUIManager this PictureControl will be part of</param>
        /// <param name="settings">Settings for this PictureControl</param>
        /// <param name="position">Position of the PictureControl reletive to its parent</param>
        /// <param name="sprite">Sprite to display</param>
        /// <param name="size">Size of the PictureControl</param>
        /// <param name="parent">Parent Control of this PictureControl (null for a root Control)</param>
        protected SpriteControl(GUIManagerBase gui, PictureControlSettings settings, Vector2 position, ISprite sprite,
                                Vector2 size, Control parent) : base(gui, settings, position, size, parent)
        {
            _sprite = sprite;

            // Disable dragging by default
            CanDrag = false;
        }

        /// <summary>
        /// SpriteControl constructor
        /// </summary>
        /// <param name="position">Position of the Control reletive to its parent</param>
        /// <param name="settings">Settings for this PictureControl</param>
        /// <param name="sprite">Sprite to display</param>
        /// <param name="parent">Parent Control of this Control (null for a root Control)</param>
        protected SpriteControl(Vector2 position, PictureControlSettings settings, ISprite sprite, Control parent)
            : this(parent.GUIManager, settings, position, sprite, Vector2.Zero, parent)
        {
            if (_sprite == null)
                throw new ArgumentNullException("sprite");

            // Set the size of the control to the size of the sprite
            Size = new Vector2(_sprite.Source.Width, _sprite.Source.Height);

            // Add the size of the border (if there is one)
            if (Border != null)
                Size += new Vector2(Border.Width, Border.Height);
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
            Vector2 min = sp;
            if (Border != null)
                min += new Vector2(Border.LeftWidth, Border.TopHeight);

            // Draw the picture
            if (StretchSprite)
            {
                // Stretched draw
                Vector2 max = sp + Size;
                if (Border != null)
                    max -= new Vector2(Border.LeftWidth + Border.RightWidth, Border.TopHeight - Border.BottomHeight);
                Vector2 s = max - min;
                Rectangle dest = new Rectangle((int)min.X, (int)min.Y, (int)s.X, (int)s.Y);
                _sprite.Draw(spriteBatch, dest, Color.White);
            }
            else
            {
                // Non-stretched draw
                _sprite.Draw(spriteBatch, min, Color.White);
            }
        }
    }
}