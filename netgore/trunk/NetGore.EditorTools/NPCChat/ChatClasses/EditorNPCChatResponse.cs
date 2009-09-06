using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace NetGore.EditorTools.NPCChat
{
    public delegate void EditorNPCChatResponseEventHandler(EditorNPCChatResponse response);

    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// </summary>
    public class EditorNPCChatResponse : NPCChatResponseBase
    {
        NPCChatResponseActionBase[] _actions;
        NPCChatConditionalCollectionBase _conditionals;
        ushort _page;
        string _text;
        byte _value;

        /// <summary>
        /// Notifies listeners when the EditorNPCChatResponse has changed.
        /// </summary>
        public event EditorNPCChatResponseEventHandler OnChange;

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
        /// Initializes a new instance of the <see cref="EditorNPCChatResponse"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public EditorNPCChatResponse(string text) : this(EndConversationPage, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatResponse"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public EditorNPCChatResponse(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatResponse"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        public EditorNPCChatResponse(ushort page, string text)
        {
            _page = page;
            _text = text;
        }

        /// <summary>
        /// Clears the Conditionals.
        /// </summary>
        public void ClearConditionals()
        {
            _conditionals = new EditorNPCChatConditionalCollection();

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return new EditorNPCChatConditionalCollection();
        }

        /// <summary>
        /// Sets the Conditionals property.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetConditionals(NPCChatConditionalCollectionBase value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (_conditionals == value)
                return;

            _conditionals = value;

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Sets the Page property.
        /// </summary>
        /// <param name="page">The new page.</param>
        public void SetPage(ushort page)
        {
            if (_page == page)
                return;

            _page = page;

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected override void SetReadValues(byte value, ushort page, string text, NPCChatConditionalCollectionBase conditionals,
                                              NPCChatResponseActionBase[] actions)
        {
            _value = value;
            _page = page;
            _actions = actions;
            SetText(text);

            EditorNPCChatConditionalCollection c = conditionals as EditorNPCChatConditionalCollection;
            _conditionals = c ?? new EditorNPCChatConditionalCollection();
        }

        /// <summary>
        /// Sets the Text property.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetText(string value)
        {
            if (_text == value)
                return;

            _text = value;

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Sets the Value property.
        /// </summary>
        /// <param name="value">The value.</param>
        internal void SetValue(byte value)
        {
            if (_value == value)
                return;

            _value = value;

            if (OnChange != null)
                OnChange(this);
        }
    }
}