using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the <see cref="MouseButtonEventArgs"/>.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    public delegate void MouseClickEventHandler(object sender, MouseButtonEventArgs e);
}