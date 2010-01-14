using System.Linq;
using System.Reflection;
using NetGore.IO;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Implementation of a PropertySyncBase that handles synchronizing a Alignment.
    /// </summary>
    [PropertySyncHandler(typeof(Alignment))]
    public sealed class PropertySyncAlignment : PropertySyncBase<Alignment>
    {
        static readonly AlignmentHelper _alignmentHelper = AlignmentHelper.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncAlignment"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncAlignment(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override Alignment Read(string name, IValueReader reader)
        {
            return reader.ReadEnum(_alignmentHelper, name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, Alignment value)
        {
            writer.WriteEnum(_alignmentHelper, name, value);
        }
    }
}