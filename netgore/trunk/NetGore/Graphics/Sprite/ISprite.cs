using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a 2D game sprite with basic drawing and updating abilities.
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Gets the size of the <see cref="ISprite"/> in pixels.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// Gets the source rectangle of the sprite on the texture.
        /// </summary>
        Rectangle Source { get; }

        /// <summary>
        /// Gets the texture containing the sprite.
        /// </summary>
        Texture Texture { get; }

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Position to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        void Draw(ISpriteBatch spriteBatch, Vector2 position, Color color);

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest"><see cref="Rectangle"/> to draw to.</param>
        /// <param name="color"><see cref="Color"/> to draw with.</param>
        void Draw(ISpriteBatch spriteBatch, Rectangle dest, Color color);

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        void Draw(ISpriteBatch sb, Vector2 dest);

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="dest">Destination to draw the sprite.</param>
        void Draw(ISpriteBatch sb, Rectangle dest);

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Top-left corner pixel of the destination.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect);

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
        void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin, float scale);

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
        void Draw(ISpriteBatch sb, Vector2 dest, Color color, SpriteEffects effect, float rotation, Vector2 origin, Vector2 scale);

        /// <summary>
        /// Draws the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to add the draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the sprite (default Color.White).</param>
        /// <param name="effect">Sprite effect to use (default SpriteEffects.None).</param>
        /// <param name="rotation">The angle, in radians, to rotate the sprite around the origin (default 0).</param>
        /// <param name="origin">The origin of the sprite to rotate around (default Vector2.Zero).</param>
        void Draw(ISpriteBatch sb, Rectangle dest, Color color, SpriteEffects effect, float rotation, Vector2 origin);

        /// <summary>
        /// Updates the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        void Update(TickCount currentTime);
    }
}