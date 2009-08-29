using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat.Conditionals
{
    class NPCChatConditionalCollection<TUser, TNPC> : NPCChatConditionalCollectionBase<TUser, TNPC>
        where TUser : class
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
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        protected override void SetReadValues(NPCChatConditionalEvaluationType evaluationType, NPCChatConditionalCollectionItemBase<TUser, TNPC>[] items)
        {
            _evaluationType = evaluationType;
            _items = items;
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionItemBase instance.
        /// </summary>
        /// <returns>A NPCChatConditionalCollectionItemBase instance.</returns>
        protected override NPCChatConditionalCollectionItemBase<TUser, TNPC> CreateItem()
        {
            return new NPCChatConditionalCollectionItem<TUser, TNPC>();
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
                yield return item;
        }
    }
}
