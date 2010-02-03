using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a class that provides a <see cref="ICamera2D"/>.
    /// </summary>
    public interface ICamera2DProvider
    {
        /// <summary>
        /// Gets the <see cref="ICamera2D"/> instance.
        /// </summary>
        ICamera2D Camera { get; }
    }
}