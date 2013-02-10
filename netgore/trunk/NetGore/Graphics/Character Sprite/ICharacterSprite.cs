using System.Collections.Generic;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a class that controls a Character's sprite.
    /// </summary>
    public interface ICharacterSprite
    {
        /// <summary>
        /// Gets or sets if paperdolling is enabled.
        /// </summary>
        bool Paperdoll { get; set; }

        /// <summary>
        /// Gets the character this <see cref="ICharacterSprite"/> is drawing the sprite for.
        /// </summary>
        Entity Character { get; }

        /// <summary>
        /// Returns the size of the sprite in total
        /// </summary>
        Vector2 SpriteSize { get;  }

        /// <summary>
        /// Adds a sprite body modifier that alters some, but not all, of the body. <see cref="ICharacterSprite"/>s
        /// that do not support dynamic sprites treat this the same as <see cref="ICharacterSprite.SetBody"/>.
        /// </summary>
        /// <param name="bodyModifierName">The name of the sprite body modifier.</param>
        void AddBodyModifier(string bodyModifierName);

        /// <summary>
        /// Draws the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        /// <param name="position">The position to draw the sprite.</param>
        /// <param name="heading">The character's heading.</param>
        /// <param name="color">The color of the sprite.</param>
        void Draw(ISpriteBatch spriteBatch, Vector2 position, Direction heading, Color color);

        /// <summary>
        /// Sets the sprite's body, which describes the components to use to draw a Set.
        /// </summary>
        /// <param name="bodyName">The name of the sprite body.</param>
        void SetBody(string bodyName);

        /// <summary>
        /// Sets the sprite's paper doll layers. This will set all of the layers at once. Layers that are not in the
        /// <paramref name="layers"/> collection should be treated as they are not used and be removed, not be treated
        /// as they are just not updating.
        /// </summary>
        /// <param name="layers">The name of the paper doll layers.</param>
        void SetPaperDollLayers(IEnumerable<string> layers);

        /// <summary>
        /// Sets the Set that describes how the sprite is laid out.
        /// </summary>
        /// <param name="setName">The name of the Set.</param>
        /// <param name="bodySize">The size of the body.</param>
        void SetSet(string setName, Vector2 bodySize);

        /// <summary>
        /// Updates the <see cref="ICharacterSprite"/>.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void Update(TickCount currentTime);
    }
}