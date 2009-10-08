using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the <see cref="MouseEventArgs"/>.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
}