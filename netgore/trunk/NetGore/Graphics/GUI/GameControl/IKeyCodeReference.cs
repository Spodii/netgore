using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for an object that provides a reference to a single <see cref="Keyboard.Key"/>.
    /// </summary>
    public interface IKeyCodeReference
    {
        /// <summary>
        /// Gets the referenced <see cref="Keyboard.Key"/>.
        /// </summary>
        Keyboard.Key Key { get; }
    }
}