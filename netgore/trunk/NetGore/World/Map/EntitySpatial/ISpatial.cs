using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Interface for an object that occupies space in the world.
    /// </summary>
    public interface ISpatial
    {
        /// <summary>
        /// Gets the <see cref="CollisionBox"/> used to determine the location of the object in the world.
        /// </summary>
        CollisionBox CB { get; }
    }
}
