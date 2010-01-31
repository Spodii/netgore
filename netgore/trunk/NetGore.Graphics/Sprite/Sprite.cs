using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Defines an image that is either part of or all of a <see cref="Texture2D"/>. This is intended as a very
    /// basic, primitive alternative to a <see cref="Grh"/>.
    /// </summary>
    public class Sprite : ISprite
    {
        Rectangle _source;
        Texture2D _texture;

        /// <summary>
        /// Notifies listeners when the sprite's source has changed.
        /// </summary>
        public event EventHandler SourceChanged;

        /// <summary>
        /// Notifies listeners when the texture has changed.
        /// </summary>
        public event EventHandler TextureChanged;

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
        /// <param name="source">Source rectangle in the texture for the Sprite.</param>
        public Sprite(Texture2D texture, Rectangle source)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");
            _texture = texture;
            _source = source;
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="dest">A rectangle specifying, in screen coordinates, where the sprite will be drawn. 
        /// If this rectangle is not the same size as sourcerectangle the sprite will be scaled to fit.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle dest)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, dest, _source, Color.White);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, position, _source, Color.White);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="dest">A rectangle specifying, in screen coordinates, where the sprite will be drawn. 
        /// If this rectangle is not the same size as sourcerectangle the sprite will be scaled to fit.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="effects">Rotations to apply before rendering</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify either 
        /// SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for this parameter to affect sprite drawing.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle dest, Color color, float rotation, Vector2 origin,
                         SpriteEffects effects, float layerDepth)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, dest, _source, color, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Float containing separate scalar multiples for both the x and y axis.</param>
        /// <param name="effects">Rotations to apply before rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify either 
        /// SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for this parameter to affect sprite drawing.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale,
                         SpriteEffects effects, float layerDepth)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, position, _source, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws the Sprite.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="position">The location, in screen coordinates, where the sprite will be drawn.</param>
        /// <param name="color">The color channel modulation to use. Use Color.White for full color with no tinting.</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin.</param>
        /// <param name="origin">The origin of the sprite. Specify (0,0) for the upper-left corner.</param>
        /// <param name="scale">Vector containing separate scalar multiples for the x- and y-axes of the sprite.</param>
        /// <param name="effects">Rotations to apply before rendering.</param>
        /// <param name="layerDepth">The sorting depth of the sprite, between 0 (front) and 1 (back). You must specify either 
        /// SpriteSortMode.FrontToBack or SpriteSortMode.BackToFront for this parameter to affect sprite drawing.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale,
                         SpriteEffects effects, float layerDepth)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, position, _source, color, rotation, origin, scale, effects, layerDepth);
        }

        #region ISprite Members

        /// <summary>
        /// Gets the source rectangle of the sprite on the texture.
        /// </summary>
        public Rectangle Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    if (SourceChanged != null)
                        SourceChanged(this, null);
                }
            }
        }

        /// <summary>
        /// Gets the texture containing the sprite.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                if (_texture != value)
                {
                    _texture = value;
                    if (TextureChanged != null)
                        TextureChanged(this, null);
                }
            }
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw to.</param>
        /// <param name="position">Position to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, position, _source, color);
        }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw to.</param>
        /// <param name="dest"><see cref="Rectangle"/> to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle dest, Color color)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");
            spriteBatch.Draw(_texture, dest, _source, color);
        }

        /// <summary>
        /// Updates the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public void Update(int currentTime)
        {
            // Nothing to update... ever
        }

        #endregion
    }
}