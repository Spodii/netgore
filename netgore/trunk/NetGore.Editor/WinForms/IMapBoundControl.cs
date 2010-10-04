using System.Linq;
using System.Windows.Forms;
using NetGore.World;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Interface for a <see cref="Control"/> that needs to know what the current <see cref="IMap"/> is.
    /// </summary>
    public interface IMapBoundControl
    {
        /// <summary>
        /// Gets or sets the current <see cref="IMap"/>.
        /// </summary>
        IMap IMap { get; set; }
    }
}