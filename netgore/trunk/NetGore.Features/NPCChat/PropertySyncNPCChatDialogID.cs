using System.Linq;
using NetGore.IO;
using NetGore.IO.PropertySync;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Implementation of a <see cref="PropertySyncBase{T}"/> that handles synchronizing a <see cref="NPCChatDialogID"/>.
    /// </summary>
    [PropertySyncHandler(typeof(NPCChatDialogID))]
    public sealed class PropertySyncNPCChatDialogID : PropertySyncBase<NPCChatDialogID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySyncNPCChatDialogID"/> class.
        /// </summary>
        /// <param name="syncValueAttributeInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        public PropertySyncNPCChatDialogID(SyncValueAttributeInfo syncValueAttributeInfo) : base(syncValueAttributeInfo)
        {
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected override NPCChatDialogID Read(string name, IValueReader reader)
        {
            return reader.ReadNPCChatDialogID(name);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected override void Write(string name, IValueWriter writer, NPCChatDialogID value)
        {
            value.Write(writer, name);
        }
    }
}