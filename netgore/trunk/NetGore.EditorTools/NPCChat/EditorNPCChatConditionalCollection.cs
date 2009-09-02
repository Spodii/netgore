using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools.NPCChat
{
    public class EditorNPCChatConditionalCollection : NPCChatConditionalCollectionBase
    {
        NPCChatConditionalEvaluationType _evaluationType;
        NPCChatConditionalCollectionItemBase[] _items;

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalEvaluationType
        /// used when evaluating this conditional collection.
        /// </summary>
        public override NPCChatConditionalEvaluationType EvaluationType
        {
            get { return _evaluationType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollection"/> class.
        /// </summary>
        /// <param name="source">The source INPCChatConditionalCollection to copy the values from.</param>
        public EditorNPCChatConditionalCollection(NPCChatConditionalCollectionBase source)
        {
            BitStream stream = new BitStream(BitStreamMode.Write, 256);

            using (BinaryValueWriter writer = new BinaryValueWriter(stream))
            {
                source.Write(writer);
            }

            stream.Mode = BitStreamMode.Read;

            IValueReader reader = new BinaryValueReader(stream);

            Read(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollection"/> class.
        /// </summary>
        public EditorNPCChatConditionalCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollection"/> class.
        /// </summary>
        /// <param name="reader">The IValueReader to read the values from.</param>
        public EditorNPCChatConditionalCollection(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// Copies the values of this INPCChatConditionalCollection to another INPCChatConditionalCollection.
        /// </summary>
        /// <param name="dest">The INPCChatConditionalCollection to copy the values into.</param>
        public void CopyValuesTo(NPCChatConditionalCollectionBase dest)
        {
            BitStream stream = new BitStream(BitStreamMode.Write, 256);

            using (BinaryValueWriter writer = new BinaryValueWriter(stream))
            {
                Write(writer);
            }

            stream.Mode = BitStreamMode.Read;

            IValueReader reader = new BinaryValueReader(stream);

            dest.Read(reader);
        }

        /// <summary>
        /// Creates a NPCChatConditionalCollectionItemBase instance then reades the values for it from the
        /// given IValueReader.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <returns>A NPCChatConditionalCollectionItemBase instance with values read from
        /// the <paramref name="reader"/>.</returns>
        protected override NPCChatConditionalCollectionItemBase CreateItem(IValueReader reader)
        {
            return new EditorNPCChatConditionalCollectionItem(reader);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<NPCChatConditionalCollectionItemBase> GetEnumerator()
        {
            foreach (NPCChatConditionalCollectionItemBase item in _items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="evaluationType">The NPCChatConditionalEvaluationType.</param>
        /// <param name="items">The NPCChatConditionalCollectionItemBases.</param>
        protected override void SetReadValues(NPCChatConditionalEvaluationType evaluationType,
                                              NPCChatConditionalCollectionItemBase[] items)
        {
            _evaluationType = evaluationType;
            _items = items;
        }
    }
}