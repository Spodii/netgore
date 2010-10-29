using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for an object that provides a reference to a single <see cref="KeyCode"/>.
    /// </summary>
    public interface IKeyCodeReference
    {
        /// <summary>
        /// Gets the referenced <see cref="KeyCode"/>.
        /// </summary>
        KeyCode Key { get; }
    }
}