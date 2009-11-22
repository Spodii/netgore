using System.Linq;
using System.Reflection;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing an unsigned 8-bit integer.
    /// </summary>
    [PropertySyncHandler(typeof(byte))]
    public sealed class PropertySyncByte : PropertySyncBase<byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncByte"/> class.
        /// </summary>
        /// <param name="bindObject">Object that this property is to be bound to.</param>
        /// <param name="p">PropertyInfo for the property to bind to.</param>
        public PropertySyncByte(object bindObject, PropertyInfo p) : base(bindObject, p)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override byte Read(string name, IValueReader reader)
        {
            return reader.ReadByte(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, byte value)
        {
            writer.Write(name, value);
        }
    }
}