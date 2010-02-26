using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles events from the <see cref="Control"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Control"/> that the event came from.</param>
    public delegate void ControlEventHandler(Control sender);

    /// <summary>
    /// Handles events from the <see cref="Control"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Control"/> that the event came from.</param>
    /// <param name="args">The argument related to the event.</param>
    /// <typeparam name="T">The type of argument.</typeparam>
    public delegate void ControlEventHandler<T>(Control sender, T args);
}