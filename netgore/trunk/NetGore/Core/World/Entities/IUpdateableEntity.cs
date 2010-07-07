using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Interface for an <see cref="Entity"/> that needs to be updated.
    /// </summary>
    public interface IUpdateableEntity
    {
        /// <summary>
        /// Updates the <see cref="IUpdateableEntity"/>.
        /// </summary>
        /// <param name="imap">The map that this <see cref="IUpdateableEntity"/> is on.</param>
        /// <param name="deltaTime">Time elapsed (in milliseconds) since the last update.</param>
        void Update(IMap imap, int deltaTime);
    }
}