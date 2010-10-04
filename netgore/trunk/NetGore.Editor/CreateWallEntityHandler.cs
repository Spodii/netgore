using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor
{
    /// <summary>
    /// Handler delegate for creating a <see cref="WallEntityBase"/>.
    /// </summary>
    /// <param name="position">The position to give the <see cref="WallEntityBase"/>.</param>
    /// <param name="size">The size to give the <see cref="WallEntityBase"/>.</param>
    /// <returns>A <see cref="WallEntityBase"/> created with the specified parameters.</returns>
    public delegate WallEntityBase CreateWallEntityHandler(Vector2 position, Vector2 size);
}