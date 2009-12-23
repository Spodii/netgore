using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
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

        /// <summary>
        /// Handles when another Entity collides into us. Not synonymous CollideInto since the
        /// <paramref name="collider"/> Entity is the one who collided into us. For example, if the
        /// two entities in question were a moving Character and a stationary wall, this Entity would be
        /// the Wall and <paramref name="collider"/> would be the Character.
        /// </summary>
        /// <param name="collider">Entity that collided into us.</param>
        /// <param name="displacement">Displacement between the two Entities.</param>
        public override void CollideFrom(Entity collider, Vector2 displacement)
        {
            Character other = collider as Character;
            if (other == null)
                return;

            other.Damage(this, 1);
        }

        /// <summary>
        /// Handles when the Entity collides into another entity. Not synonymous with CollideFrom we
        /// were the ones who collided into the <paramref name="collideWith"/> Entity. For example, if the
        /// two Entities in question were a moving Character and a stationary Wall, this Entity would be
        /// the Character and <paramref name="collideWith"/> would be the Wall.
        /// </summary>
        /// <param name="collideWith">Entity that this Entity collided with.</param>
        /// <param name="displacement">Displacement between the two Entities.</param>
        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
            CollideFrom(collideWith, displacement);
        }
    }
}