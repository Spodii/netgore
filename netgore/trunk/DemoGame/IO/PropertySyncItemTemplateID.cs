using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing an ItemTemplateID.
    /// </summary>
    [PropertySyncHandler(typeof(ItemTemplateID))]
    public sealed class PropertySyncItemTemplateID : PropertySyncBase<ItemTemplateID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncItemTemplateID"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncItemTemplateID(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override ItemTemplateID Read(string name, IValueReader reader)
        {
            return reader.ReadItemTemplateID(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, ItemTemplateID value)
        {
            value.Write(writer, name);
        }
    }
}