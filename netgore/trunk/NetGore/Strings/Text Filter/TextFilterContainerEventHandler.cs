using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Delegate for handling events from the <see cref="TextFilterContainer"/>.
    /// </summary>
    /// <param name="sender">The <see cref="TextFilterContainer"/> that the event came from.</param>
    public delegate void TextFilterContainerEventHandler(TextFilterContainer sender);
}