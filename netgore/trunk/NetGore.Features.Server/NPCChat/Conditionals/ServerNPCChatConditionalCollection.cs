using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore.Features.NPCChat.Conditionals
{
    class ServerNPCChatConditionalCollection : NPCChatConditionalCollectionBase
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
        /// Creates a NPCChatConditionalCollectionItemBase instance then reades the values for it from the
        /// given IValueReader.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <returns>A NPCChatConditionalCollectionItemBase instance with values read from
        /// the <paramref name="reader"/>.</returns>
        protected override NPCChatConditionalCollectionItemBase CreateItem(IValueReader reader)
        {
            return new ServerNPCChatConditionalCollectionItem(reader);
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

            Debug.Assert(items.All(x => x != null), "We shouldn't have null items in this array...");
        }
    }
}