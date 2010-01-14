using System.Linq;
using System.Reflection;
using NetGore;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace DemoGame
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing a StatType.
    /// </summary>
    [PropertySyncHandler(typeof(StatType))]
    public sealed class PropertySyncStatType : PropertySyncBase<StatType>
    {
        static readonly StatTypeHelper _statTypeHelper = StatTypeHelper.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncStatType"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncStatType(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override StatType Read(string name, IValueReader reader)
        {
            return reader.ReadEnum(_statTypeHelper, name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, StatType value)
        {
            writer.WriteEnum(_statTypeHelper, name, value);
        }
    }
}