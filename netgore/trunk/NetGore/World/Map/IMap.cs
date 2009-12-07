using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Interface for maps
    /// </summary>
    public interface IMap : IGetTime
    {
        /// <summary>
        /// Gets an IEnumerable of all the Entities on the map.
        /// </summary>
        IEnumerable<Entity> Entities { get; }

        /// <summary>
        /// Gets the height of the map in pixels.
        /// </summary>
        float Height { get; }

        /// <summary>
        /// Gets the size of the map in pixels.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// Gets the width of the map in pixels.
        /// </summary>
        float Width { get; }

        /// <summary>
        /// Checks if an Entity collides with any other entities. For each collision, <paramref name="entity"/>
        /// will call CollideInto(), and the Entity that <paramref name="entity"/> collided into will call
        /// CollideFrom().
        /// </summary>
        /// <param name="entity">Entity to check against other Entities. If the CollisionType is
        /// CollisionType.None, or if null, this will always return 0 and no collision detection
        /// will take place.</param>
        void CheckCollisions(Entity entity);

        /// <summary>
        /// Gets the <see cref="IEntitySpatial"/> for the given type of <see cref="Entity"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="Entity"/>.</param>
        /// <returns>The <see cref="IEntitySpatial"/> that contains the <paramref name="type"/>.</returns>
        IEntitySpatial GetSpatial(Type type);
    }
}