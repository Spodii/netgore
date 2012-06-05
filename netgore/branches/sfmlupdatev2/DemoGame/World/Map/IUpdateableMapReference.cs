using System.Linq;
using NetGore.World;

namespace DemoGame
{
    /// <summary>
    /// Interface for an object that, when added to an <see cref="IMap"/>, the reference of which <see cref="IMap"/>
    /// it is on is updated.
    /// </summary>
    public interface IUpdateableMapReference
    {
        /// <summary>
        /// Gets or sets the map that the <see cref="IUpdateableMapReference"/> is on. This should only be set
        /// by the map that the object was added to.
        /// </summary>
        IMap Map { get; set; }
    }
}