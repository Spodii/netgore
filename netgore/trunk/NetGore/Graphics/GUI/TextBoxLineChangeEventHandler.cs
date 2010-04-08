using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles when the <see cref="TextBoxLine"/> has changed.
    /// </summary>
    /// <param name="line">The <see cref="TextBoxLine"/> that was changed.</param>
    /// <param name="difference">The number of characters involved in the change. If greater than zero,
    /// the change involved adding text to the line. If less than zero, the change involved removing
    /// text from the line.</param>
    delegate void TextBoxLineChangeEventHandler(TextBoxLine line, int difference);
}