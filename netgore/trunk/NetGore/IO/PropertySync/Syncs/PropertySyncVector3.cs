using System.Linq;
using NetGore;
using SFML.Graphics;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing a <see cref="Vector3"/>.
    /// </summary>
    [PropertySyncHandler(typeof(Vector3))]
    public sealed class PropertySyncVector3 : PropertySyncBase<Vector3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncVector3"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncVector3(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override Vector3 Read(string name, IValueReader reader)
        {
            return reader.ReadVector3(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, Vector3 value)
        {
            IValueWriterExtensions.Write(writer, name, value);
        }
    }
}