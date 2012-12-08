using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace NetGore.Audio
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing a <see cref="SoundID"/>.
    /// </summary>
    [PropertySyncHandler(typeof(SoundID))]
    public sealed class PropertySyncSoundID : PropertySyncBase<SoundID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncSoundID"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncSoundID(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override SoundID Read(string name, IValueReader reader)
        {
            return reader.ReadSoundID(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, SoundID value)
        {
            value.Write(writer, name);
        }
    }
}