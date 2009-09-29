using System;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for retreiving the text to display in a <see cref="Tooltip"/> for a <see cref="Control"/>.
    /// </summary>
    /// <param name="sender">The <see cref="Control"/> to get the <see cref="Tooltip"/> text for.</param>
    /// <param name="args">The arguments for the display the tooltip text. These values can be altered
    /// to change the behavior of the <see cref="Tooltip"/> for this display of the tooltip. All changes
    /// to these values will be restored to the default values before each time this delegate is invoked.</param>
    /// <returns>An array of the <see cref="StyledText"/> to display in the <see cref="Tooltip"/>. If the
    /// collection is empty, the <see cref="Tooltip"/> will not be displayed. If the collection is null,
    /// no tooltip will be displayed, but a request to get the tooltip text will continue to be made
    /// periodically until a non-null value is returned. The text will be used in ascending order,
    /// so the first element in the array will be drawn first. Line breaks are defined by
    /// <see cref="Environment.NewLine"/>.</returns>
    public delegate StyledText[] TooltipHandler(Control sender, TooltipArgs args);
}