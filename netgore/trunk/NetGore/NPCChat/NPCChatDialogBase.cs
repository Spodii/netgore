using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public abstract class NPCChatDialogBase
    {
        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public abstract ushort Index { get; }

        /// <summary>
        /// NPCChatDialogBase constructor.
        /// </summary>
        protected NPCChatDialogBase()
        {
        }

        /// <summary>
        /// NPCChatDialogBase constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected NPCChatDialogBase(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// When overridden in the derived class, creates an NPCChatDialogItemBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>An NPCChatDialogItemBase created using the given IValueReader.</returns>
        protected abstract NPCChatDialogItemBase CreateDialogItem(IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="page">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="page"/>, or null if no valid
        /// NPCChatDialogItemBase existed for the given <paramref name="page"/>.</returns>
        public abstract NPCChatDialogItemBase GetDialogItem(ushort page);

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the NPCChatDialogItemBases in this
        /// NPCChatDialogBase.
        /// </summary>
        /// <returns>An IEnumerable of the NPCChatDialogItemBases in this NPCChatDialogBase.</returns>
        protected abstract IEnumerable<NPCChatDialogItemBase> GetDialogItems();

        /// <summary>
        /// When overridden in the derived class, gets the initial NPCChatDialogItemBase that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial NPCChatDialogItemBase that is used at the start of a conversation.</returns>
        public abstract NPCChatDialogItemBase GetInitialDialogItem();

        /// <summary>
        /// Reads the values for this NPCChatDialogBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public void Read(IValueReader reader)
        {
            ushort index = reader.ReadUShort("Index");
            ushort itemCount = reader.ReadUShort("ItemCount");
            var itemReaders = reader.ReadNodes("Item", itemCount);

            var items = new NPCChatDialogItemBase[itemCount];
            int i = 0;
            foreach (IValueReader r in itemReaders)
            {
                items[i] = CreateDialogItem(r);
                i++;
            }

            SetReadValues(index, items);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="items">The dialog items.</param>
        protected abstract void SetReadValues(ushort index, IEnumerable<NPCChatDialogItemBase> items);

        /// <summary>
        /// Writes the NPCChatDialogBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            var items = GetDialogItems();

            writer.Write("Index", Index);
            writer.Write("ItemCount", (ushort)items.Count());

            foreach (NPCChatDialogItemBase item in items)
            {
                writer.WriteStartNode("Item");
                item.Write(writer);
                writer.WriteEndNode("Item");
            }
        }
    }
}