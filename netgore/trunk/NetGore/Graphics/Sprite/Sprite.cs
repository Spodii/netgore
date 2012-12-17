using System;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Defines an image that is either part of or all of an image. This is intended as a very basic, primitive alternative
    /// to a <see cref="Grh"/> and does not support animations or more advanced operations.
    /// </summary>
    public class Sprite : ISprite
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Rectangle _source;
        Texture _texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite()
        {
            _texture = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="texture">The texture used by the Sprite.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c>.</exception>
        public Sprite(Texture texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            _texture = texture;
            var textureSize = _texture.Size;
            _source = new Rectangle(0, 0, textureSize.X, textureSize.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="texture">The texture used by the Sprite.</param>
        /// <param name="source">Source rectangle in the texture for the Sprite.</param>
        /// <exception cref="ArgumentNullException"><paramref name="texture" /> is <c>null</c>.</exception>
        public Sprite(Texture texture, Rectangle source)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            _texture = texture;
            _source = source;
        }

        /// <summary>
        /// Notifies listeners when the sprite's source has changed.
        /// </summary>
        public event EventHandler SourceChanged;

        /// <summary>
        /// Notifies listeners when the texture has changed.
        /// </summary>
        public event EventHandler TextureChanged;

        /// <summary>
        /// Checks if this <see cref="Sprite"/> can be drawn with the given <see cref="ISpriteBatch"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <returns>True if this <see cref="Sprite"/> can be drawn; otherwise false.</returns>
        bool CanDraw(ISpriteBatch sb)
        {
            // Invalid SpriteBatch
            if (sb == null)
            {
                const string errmsg = "Failed to render Sprite `{0}` - SpriteBatch is null!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return false;
            }

            if (sb.IsDisposed)
            {
                const string errmsg = "Failed to render Sprite `{0}` - SpriteBatch is disposed!";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="dest">A rectangle specifying, in screen coordinates, where the sprite will be drawn. 
        /// If this rectangle is not the same size as sourcerectangle the sprite will be scaled to fit.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="effects">Rotations to apply before rendering</param>
        public void Draw(ISpriteBatch sb, Rectangle dest, Color color, float rotation, Vector2 origin, SpriteEffects effects)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, dest, _source, color, rotation, origin, effects);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Float containing separate scalar multiples for both the x and y axis.</param>
        /// <param name="effects">Rotations to apply before rendering.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, Color color, float rotation, Vector2 origin, float scale,
                         SpriteEffects effects)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, position, _source, color, rotation, origin, scale, effects);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="effects">Rotations to apply before rendering.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale,
                         SpriteEffects effects)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, position, _source, color, rotation, origin, scale, effects);
        }

        #region ISprite Members

        /// <summary>
        /// Gets the size of the <see cref="ISprite"/> in pixels.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(Source.Width, Source.Height); }
        }

        /// <summary>
        /// Gets the source rectangle of the sprite on the texture.
        /// </summary>
        public Rectangle Source
        {
            get { return _source; }
            set
            {
                if (_source == value)
                    return;

                _source = value;
                if (SourceChanged != null)
                    SourceChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the texture containing the sprite.
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set
            {
                if (_texture == value)
                    return;

                _texture = value;
                if (TextureChanged != null)
                    TextureChanged.Raise(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="dest">Destination to draw the sprite.</param>
        public void Draw(ISpriteBatch sb, Rectangle dest)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, dest, _source, Color.White);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        public void Draw(ISpriteBatch sb, Vector2 position)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, position, _source, Color.White);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(Texture, dest, Source, color, 0, Vector2.Zero, 1.0f, effect);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        /// <param name="scale">Uniform multiply by which to scale the width and height.</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin,
                         float scale)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, scale, effect);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        /// <param name="scale">Vector2 defining the scale.</param>
        public void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin,
                         Vector2 scale)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, scale, effect);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        public void Draw(ISpriteBatch sb, Rectangle dest, Color color, SpriteEffects effect, float rotation, Vector2 origin)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(Texture, dest, Source, color, rotation, origin, effect);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Position to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        public void Draw(ISpriteBatch sb, Vector2 position, Color color)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, position, _source, color);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest"><see cref="Rectangle"/> to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        public void Draw(ISpriteBatch sb, Rectangle dest, Color color)
        {
            if (!CanDraw(sb))
                return;

            sb.Draw(_texture, dest, _source, color);
        }

        /// <summary>
        /// Updates the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public void Update(TickCount currentTime)
        {
            // Nothing to update... ever
        }

        #endregion
    }
}