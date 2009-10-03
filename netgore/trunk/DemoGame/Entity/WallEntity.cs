using System.Linq;
using DemoGame;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public class WallEntity : WallEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntity"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        public WallEntity(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntity"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="collisionType">Type of the collision.</param>
        public WallEntity(Vector2 position, Vector2 size, CollisionType collisionType) : base(position, size, collisionType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallEntity"/> class.
        /// </summary>
        /// <param name="r">The r.</param>
        public WallEntity(IValueReader r) : base(r)
        {
        }
    }
}