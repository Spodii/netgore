using System.Linq;
using NetGore.IO;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Handler delegate for creating a <see cref="WallEntityBase"/>.
    /// </summary>
    /// <param name="reader"><see cref="IValueReader"/> to read the creation values from.</param>
    /// <returns>A <see cref="WallEntityBase"/> created with the specified parameters.</returns>
    public delegate WallEntityBase CreateWallEntityFromReaderHandler(IValueReader reader);
}