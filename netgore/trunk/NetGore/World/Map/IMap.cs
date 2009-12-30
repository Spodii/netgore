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
        /// Gets the <see cref="ISpatialCollection"/> for all the spatial objects on the map.
        /// </summary>
        ISpatialCollection Spatial { get; }

        /// <summary>
        /// Finds the <see cref="WallEntityBase"/> that the <paramref name="stander"/> is standing on.
        /// </summary>
        /// <param name="stander">The <see cref="ISpatial"/> to check for standing on a <see cref="WallEntityBase"/>.</param>
        /// <returns>The best-fit <see cref="WallEntityBase"/> that the <paramref name="stander"/> is standing on, or
        /// null if they are not standing on any walls.</returns>
        WallEntityBase FindStandingOn(ISpatial stander);
    }
}