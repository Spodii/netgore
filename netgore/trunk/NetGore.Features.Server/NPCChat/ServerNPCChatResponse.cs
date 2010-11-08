using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Features.NPCChat.Conditionals;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// This class is immutable.
    /// </summary>
    public class ServerNPCChatResponse : NPCChatResponseBase
    {
        NPCChatResponseActionBase[] _actions;
        NPCChatConditionalCollectionBase _conditionals;
        NPCChatDialogItemID _page;
        byte _value;

        /// <summary>
        /// NPCChatResponse constructor.
        /// </summary>
        /// <param name="r">IValueReader to read the values from.</param>
        internal ServerNPCChatResponse(IValueReader r) : base(r)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="NPCChatResponseActionBase"/>s that are
        /// executed when selecting this <see cref="NPCChatResponseBase"/>. This value will never be null, but
        /// it can be an empty IEnumerable.
        /// </summary>
        public override IEnumerable<NPCChatResponseActionBase> Actions
        {
            get
            {
                Debug.Assert(_actions != null);
                return _actions;
            }
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

        /// <summary>
        /// When overridden in the derived class, gets if this <see cref="NPCChatResponseBase"/> will load
        /// the <see cref="NPCChatResponseActionBase"/>s. If true, the <see cref="NPCChatResponseActionBase"/>s
        /// will be loaded. If false, <see cref="NPCChatResponseBase.SetReadValues"/> will always contain an empty array for the
        /// actions.
        /// </summary>
        protected override bool LoadActions
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the page of the NPCChatDialogItemBase to go to if this
        /// response is chosen. If this value is equal to the EndConversationPage constant, then the dialog
        /// will end instead of going to a new page.
        /// </summary>
        public override NPCChatDialogItemID Page
        {
            get { return _page; }
        }

        /// <summary>
        /// This property is not supported by the Server's NPCChatResponse, and will always return String.Empty.
        /// </summary>
        public override string Text
        {
            get
            {
                Debug.Fail("This property is not supported by the Server.");
                return string.Empty;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the 0-based response index value for this response.
        /// </summary>
        public override byte Value
        {
            get { return _value; }
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return new ServerNPCChatConditionalCollection();
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        /// <param name="actions">The actions.</param>
        protected override void SetReadValues(byte value, NPCChatDialogItemID page, string text,
                                              NPCChatConditionalCollectionBase conditionals, NPCChatResponseActionBase[] actions)
        {
            Debug.Assert(_value == default(byte) && _page == default(ushort), "Values were already set?");

            _value = value;
            _page = page;
            _conditionals = conditionals;
            _actions = actions;
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