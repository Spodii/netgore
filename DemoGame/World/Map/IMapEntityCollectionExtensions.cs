using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="ISpatialCollection"/> interface.
    /// </summary>
    public static class IEntitySpatialExtensions
    {
        /// <summary>
        /// Gets the first <see cref="IUsableEntity"/> that intersects a specified area.
        /// </summary>
        /// <param name="c">The <see cref="ISpatialCollection"/>.</param>
        /// <param name="rect"><see cref="Rectangle"/> of the area to check.</param>
        /// <param name="charEntity"><see cref="CharacterEntity"/> that must be able to use the
        /// <see cref="IUsableEntity"/>.</param>
        /// <returns>First <see cref="IUsableEntity"/> that intersects the specified area that the
        /// <paramref name="charEntity"/> is able to use, or null if none.</returns>
        public static IUsableEntity GetUsable(this ISpatialCollection c, Rectangle rect, CharacterEntity charEntity)
        {
            return c.Get<IUsableEntity>(rect, x => x.CanUse(charEntity));
        }
    }
}