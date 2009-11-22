using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Contains the different types of possible collisions for a <see cref="CollisionBox"/>.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// A full, solid box.
        /// </summary>
        Full,

        /// <summary>
        /// An equally split box where the top-right half-triangle is cut out.
        /// </summary>
        TriangleTopRight,

        /// <summary>
        /// An equally split box where the top-left half-triangle is cut out.
        /// </summary>
        TriangleTopLeft,

        /// <summary>
        /// Entity does not support collision detection at all.
        /// </summary>
        None
    }
}