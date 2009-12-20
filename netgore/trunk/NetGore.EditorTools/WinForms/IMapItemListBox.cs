using System.Linq;
using System.Windows.Forms;
using NetGore.Graphics;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Interface for a <see cref="ListBox"/> that contains a collection of the items on a map.
    /// </summary>
    public interface IMapItemListBox
    {
        /// <summary>
        /// Gets or sets the <see cref="ICamera2D"/> used to view the Map.
        /// </summary>
        ICamera2D Camera { get; set; }

        /// <summary>
        /// Gets or sets the IMap containing the objects being handled.
        /// </summary>
        IMap IMap { get; set; }
    }
}