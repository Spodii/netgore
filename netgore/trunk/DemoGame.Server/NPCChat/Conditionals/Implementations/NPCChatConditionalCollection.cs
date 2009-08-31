using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat.Conditionals
{
    class NPCChatConditionalCollection<TUser, TNPC> : NPCChatConditionalCollectionBase<TUser, TNPC> where TUser : class
                                                                                                    where TNPC : class
    {
        NPCChatConditionalEvaluationType _evaluationType;
        NPCChatConditionalCollectionItemBase<TUser, TNPC>[] _items;

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
        protected override NPCChatConditionalCollectionItemBase<TUser, TNPC> CreateItem(IValueReader reader)
        {
            return new NPCChatConditionalCollectionItem<TUser, TNPC>(reader);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<NPCChatConditionalCollectionItemBase<TUser, TNPC>> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        protected override void SetReadValues(NPCChatConditionalEvaluationType evaluationType,
                                              NPCChatConditionalCollectionItemBase<TUser, TNPC>[] items)
        {
            _evaluationType = evaluationType;
            _items = items;
        }
    }
}