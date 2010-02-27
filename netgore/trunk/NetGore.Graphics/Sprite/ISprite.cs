using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a 2D game sprite with basic drawing and updating abilities.
    /// </summary>
    public interface ISprite
    {
        /// <summary>
        /// Gets the source rectangle of the sprite on the texture.
        /// </summary>
        Rectangle Source { get; }

        /// <summary>
        /// Gets the texture containing the sprite.
        /// </summary>
        Texture2D Texture { get; }

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
        /// Updates the <see cref="ISprite"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        void Update(int currentTime);
    }
}