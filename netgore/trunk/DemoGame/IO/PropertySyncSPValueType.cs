using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing a SPValueType.
    /// </summary>
    [PropertySyncHandler(typeof(SPValueType))]
    public sealed class PropertySyncSPValueType : PropertySyncBase<SPValueType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncSPValueType"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncSPValueType(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override SPValueType Read(string name, IValueReader reader)
        {
            var v = reader.ReadSPValueType(name);
            return v;
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, SPValueType value)
        {
            value.Write(writer, name);
        }
    }
}