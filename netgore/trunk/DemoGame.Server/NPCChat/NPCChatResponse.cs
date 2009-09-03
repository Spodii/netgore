using System;
using System.Diagnostics;
using System.Linq;
using DemoGame.Server.NPCChat.Conditionals;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace DemoGame.Server.NPCChat
{
    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// This class is immutable and intended for use in the Server only.
    /// </summary>
    public class NPCChatResponse : NPCChatResponseBase
    {
        ushort _page;
        byte _value;

        /// <summary>
        /// When overridden in the derived class, gets the page of the NPCChatDialogItemBase to go to if this
        /// response is chosen. If this value is equal to the EndConversationPage constant, then the dialog
        /// will end instead of going to a new page.
        /// </summary>
        public override ushort Page
        {
            get { return _page; }
        }

        /// <summary>
        /// This property is not supported by the Server's NPCChatDialog.
        /// </summary>
        public override string Text
        {
            get { throw new NotSupportedException("This property is not supported by the Server's NPCChatDialog."); }
        }

        /// <summary>
        /// When overridden in the derived class, gets the 0-based response index value for this response.
        /// </summary>
        public override byte Value
        {
            get { return _value; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalCollectionBase that contains the
        /// conditionals used to evaluate if this NPCChatResponseBase may be used. If this value is null, it
        /// is assumed that there are no conditionals attached to this NPCChatResponseBase, and should be treated
        /// the same way as if the conditionals evaluated to true.
        /// </summary>
        public override NPCChatConditionalCollectionBase Conditionals
        {
            get { return _conditionals; }
        }

        NPCChatConditionalCollectionBase _conditionals;

        /// <summary>
        /// NPCChatResponse constructor.
        /// </summary>
        /// <param name="r">IValueReader to read the values from.</param>
        internal NPCChatResponse(IValueReader r) : base(r)
        {
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected override void SetReadValues(byte value, ushort page, string text, NPCChatConditionalCollectionBase conditionals)
        {
            Debug.Assert(_value == default(byte) && _page == default(ushort), "Values were already set?");

            _value = value;
            _page = page;
            _conditionals = conditionals;
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return new NPCChatConditionalCollection();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Value: {1}]", GetType().Name, Value));
        }
    }
}