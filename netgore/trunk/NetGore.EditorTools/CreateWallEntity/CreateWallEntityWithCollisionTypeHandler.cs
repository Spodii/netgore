using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handler delegate for creating a <see cref="WallEntityBase"/>.
    /// </summary>
    /// <param name="position">The position to give the <see cref="WallEntityBase"/>.</param>
    /// <param name="size">The size to give the <see cref="WallEntityBase"/>.</param>
    /// <param name="collisionType">The <see cref="CollisionType"/> to give the <see cref="WallEntityBase"/>.</param>
    /// <returns>A <see cref="WallEntityBase"/> created with the specified parameters.</returns>
    public delegate WallEntityBase CreateWallEntityWithCollisionTypeHandler(
        Vector2 position, Vector2 size, CollisionType collisionType);
}