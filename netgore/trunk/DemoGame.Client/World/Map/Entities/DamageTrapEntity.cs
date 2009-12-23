using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Client
{
    public class DamageTrapEntity : DamageTrapEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        public DamageTrapEntity(Vector2 position, Vector2 size) : base(position, size)
        {
        }
    }
}