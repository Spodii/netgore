using System.Linq;
using System.Reflection;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing a BodyIndex.
    /// </summary>
    [PropertySyncHandler(typeof(BodyIndex))]
    public sealed class PropertySyncBodyIndex : PropertySyncBase<BodyIndex>
    {
        public PropertySyncBodyIndex(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override BodyIndex Read(string name, IValueReader reader)
        {
            var v = reader.ReadBodyIndex(name);
            return v;
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, BodyIndex value)
        {
            writer.Write(name, value);
        }
    }
}