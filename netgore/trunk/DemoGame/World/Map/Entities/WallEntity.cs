using System.Linq;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

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
        /// <param name="r">The r.</param>
        public WallEntity(IValueReader r) : base(r)
        {
        }
    }
}