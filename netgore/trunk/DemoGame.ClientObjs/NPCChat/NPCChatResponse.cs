using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace DemoGame.Client.NPCChat
{
    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// This class is immutable and intended for use in the Client only.
    /// </summary>
    public class NPCChatResponse : NPCChatResponseBase
    {
        ushort _page;
        string _text;
        byte _value;

        /// <summary>
        /// Not used by the Client, and will always return an empty collection.
        /// </summary>
        public override IEnumerable<NPCChatResponseActionBase> Actions
        {
            get { return NPCChatResponseActionBase.EmptyActions; }
        }

        /// <summary>
        /// Not used by the Client, and will always return null.
        /// </summary>
        public override NPCChatConditionalCollectionBase Conditionals
        {
            get { return null; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="NPCChatResponseBase"/> will load
        /// the <see cref="NPCChatResponseActionBase"/>s. If true, the <see cref="NPCChatResponseActionBase"/>s
        /// will be loaded. If false, <see cref="NPCChatResponseBase.SetReadValues"/> will always contain an empty array for the
        /// actions.
        /// </summary>
        protected override bool LoadActions
        {
            get { return false; }
        }

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
        /// When overridden in the derived class, gets the text to display for this response.
        /// </summary>
        public override string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the 0-based response index value for this response.
        /// </summary>
        public override byte Value
        {
            get { return _value; }
        }

        /// <summary>
        /// NPCChatResponse constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal NPCChatResponse(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        /// <param name="actions">The actions.</param>
        protected override void SetReadValues(byte value, ushort page, string text, NPCChatConditionalCollectionBase conditionals,
                                              NPCChatResponseActionBase[] actions)
        {
            Debug.Assert(_value == default(byte) && _page == default(ushort) && _text == default(string),
                         "Values were already set?");

            _value = value;
            _page = page;
            _text = text;
        }
    }
}