using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Base class for an Entity which damages those who touch it.
    /// </summary>
    public abstract class DamageTrapEntityBase : DynamicEntity
    {
        int _damage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicEntity"/> class.
        /// </summary>
        /// <param name="position">The initial world position.</param>
        /// <param name="size">The initial size.</param>
        protected DamageTrapEntityBase(Vector2 position, Vector2 size) : base(position, size)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="Entity"/> will collide against
        /// walls. If false, this <see cref="Entity"/> will pass through walls and completely ignore them.
        /// </summary>
        public override bool CollidesAgainstWalls
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the amount of damage that is done to the Entity that touches this DamageTrapEntityBase.
        /// </summary>
        [SyncValue(SkipNetworkSync = true)]
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }
    }
}