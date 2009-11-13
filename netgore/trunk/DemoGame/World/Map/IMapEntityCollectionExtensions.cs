using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="IMapEntityCollection"/> interface.
    /// </summary>
    public static class IMapEntityCollectionExtensions
    {
        /// <summary>
        /// Gets the first <see cref="IUsableEntity"/> that intersects a specified area.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="rect"><see cref="Rectangle"/> of the area to check.</param>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that must be able to use the
        /// <see cref="IUsableEntity"/>.</param>
        /// <returns>First <see cref="IUsableEntity"/> that intersects the specified area that the
        /// <paramref name="charEntity"/> is able to use, or null if none.</returns>
        public static IUsableEntity GetUsable(this IMapEntityCollection c, Rectangle rect, CharacterEntity charEntity)
        {
            // NOTE: This doesn't really belong in here. Maybe make it an extension?
            return c.GetEntities(rect).OfType<IUsableEntity>().Where(x => x.CanUse(charEntity)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first <see cref="IUsableEntity"/> that intersects a specified area.
        /// </summary>
        /// <param name="c">The <see cref="IMapEntityCollection"/>.</param>
        /// <param name="cb"><see cref="CollisionBox"/> of the area to check.</param>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that must be able to use the
        /// <see cref="IUsableEntity"/>.</param>
        /// <returns>First <see cref="IUsableEntity"/> that intersects the specified area that the
        /// <paramref name="charEntity"/> is able to use, or null if none.</returns>
        public static IUsableEntity GetUsable(this IMapEntityCollection c, CollisionBox cb, CharacterEntity charEntity)
        {
            // NOTE: This doesn't really belong in here. Maybe make it an extension?
            return c.GetUsable(cb.ToRectangle(), charEntity);
        }
    }
}