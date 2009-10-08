using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the <see cref="KeyboardEventArgs"/>.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);
}