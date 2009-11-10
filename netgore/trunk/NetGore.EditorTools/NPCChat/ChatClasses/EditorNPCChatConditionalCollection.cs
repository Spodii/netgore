using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.IO;
using NetGore.NPCChat.Conditionals;

namespace NetGore.EditorTools.NPCChat
{
    public class EditorNPCChatConditionalCollection : NPCChatConditionalCollectionBase
    {
        NPCChatConditionalEvaluationType _evaluationType;
        readonly List<EditorNPCChatConditionalCollectionItem> _items = new List<EditorNPCChatConditionalCollectionItem>();

        /// <summary>
        /// Notifies listeners when this EditorNPCChatConditionalCollection changes.
        /// </summary>
        public event EditorNPCChatConditionalCollectionChangeHandler OnChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatConditionalCollection"/> class.
        /// </summary>
        /// <param name="source">The source NPCChatConditionalCollectionBase to copy the values from. If null,
        /// no values are copied.</param>
        public EditorNPCChatConditionalCollection(NPCChatConditionalCollectionBase source)
        {
            if (source == null)
                return;

            var stream = new BitStream(BitStreamMode.Write, 256);

            using (var writer = new BinaryValueWriter(stream))
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
        /// When overridden in the derived class, gets the NPCChatConditionalEvaluationType
        /// used when evaluating this conditional collection.
        /// </summary>
        public override NPCChatConditionalEvaluationType EvaluationType
        {
            get { return _evaluationType; }
        }

        /// <summary>
        /// Copies the values of this NPCChatConditionalCollectionBase to another NPCChatConditionalCollectionBase.
        /// </summary>
        /// <param name="dest">The NPCChatConditionalCollectionBase to copy the values into.</param>
        public void CopyValuesTo(NPCChatConditionalCollectionBase dest)
        {
            var stream = new BitStream(BitStreamMode.Write, 256);

            using (var writer = new BinaryValueWriter(stream))
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
        public override IEnumerator<NPCChatConditionalCollectionItemBase> GetEnumerator()
        {
            foreach (var item in _items)
            {
                if (item == null)
                    continue;

                yield return item;
            }
        }

        /// <summary>
        /// Sets the EvaluationType.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetEvaluationType(NPCChatConditionalEvaluationType value)
        {
            _evaluationType = value;

            if (OnChange != null)
                OnChange(this);
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
            _items.Clear();
            _items.AddRange(items.Cast<EditorNPCChatConditionalCollectionItem>());

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Tries to add a NPCChatConditionalCollectionItemBase to the collection.
        /// </summary>
        /// <param name="item">The NPCChatConditionalCollectionItemBase to add.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added; otherwise false.</returns>
        public bool TryAddItem(NPCChatConditionalCollectionItemBase item)
        {
            if (item == null)
                return false;

            if (item is EditorNPCChatConditionalCollectionItem)
                return TryAddItem((EditorNPCChatConditionalCollectionItem)item);

            var newItem = new EditorNPCChatConditionalCollectionItem(item);
            return TryAddItem(newItem);
        }

        /// <summary>
        /// Tries to add a EditorNPCChatConditionalCollectionItem to the collection.
        /// </summary>
        /// <param name="item">The NPCChatConditionalCollectionItemBase to add.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added; otherwise false.</returns>
        public bool TryAddItem(EditorNPCChatConditionalCollectionItem item)
        {
            if (item == null)
                return false;

            if (_items.Contains(item))
                return false;

            _items.Add(item);

            if (OnChange != null)
                OnChange(this);

            return true;
        }

        /// <summary>
        /// Tries to remove a <see cref="EditorNPCChatConditionalCollectionItem"/> from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the <paramref name="item"/> was successfully removed; otherwise false.</returns>
        public bool TryRemoveItem(EditorNPCChatConditionalCollectionItem item)
        {
            if (item == null)
                return false;

            var success = _items.Remove(item);

            if (success && OnChange != null)
                OnChange(this);

            return success;
        }
    }
}