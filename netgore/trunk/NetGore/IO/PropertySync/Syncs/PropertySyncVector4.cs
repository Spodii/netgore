using System.Linq;
using NetGore;
using SFML.Graphics;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing a <see cref="Vector4"/>.
    /// </summary>
    [PropertySyncHandler(typeof(Vector4))]
    public sealed class PropertySyncVector4 : PropertySyncBase<Vector4>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncVector4"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncVector4(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override Vector4 Read(string name, IValueReader reader)
        {
            return reader.ReadVector4(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, Vector4 value)
        {
            IValueWriterExtensions.Write(writer, name, value);
        }
    }
}