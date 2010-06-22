using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Enum of the different formats that can be used with the <see cref="GenericValueReader"/> and <see cref="GenericValueWriter"/>.
    /// </summary>
    public enum GenericValueIOFormat
    {
        /// <summary>
        /// Uses Xml format through the <see cref="XmlValueReader"/> and <see cref="XmlValueWriter"/>.
        /// Xml is very verbose and bulky, but is very stable, human-editable, and can recover from
        /// corruption much more easily.
        /// </summary>
        Xml,

        /// <summary>
        /// Uses binary format through the <see cref="BinaryValueReader"/> and <see cref="BinaryValueWriter"/>.
        /// Binary is very compact and fast but is incredibly difficult to read or edit.
        /// </summary>
        Binary
    }
}