namespace NetGore.IO
{
    /// <summary>
    /// Enum of the different formats that can be used with the <see cref="GenericValueReader"/> and <see cref="GenericValueWriter"/>.
    /// </summary>
    public enum GenericValueIOFormat
    {
        /// <summary>
        /// Uses Xml format through the <see cref="XmlValueReader"/> and <see cref="XmlValueWriter"/>.
        /// Files of this type end with the .xml suffix.
        /// </summary>
        Xml,

        /// <summary>
        /// Uses binary format through the <see cref="BinaryValueReader"/> and <see cref="BinaryValueWriter"/>.
        /// Files of this type end with the .bin suffix.
        /// </summary>
        Binary
    }
}