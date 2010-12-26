using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Features.NPCChat.Conditionals;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// </summary>
    public class EditorNPCChatResponse : NPCChatResponseBase
    {
        readonly List<NPCChatResponseActionBase> _actions = new List<NPCChatResponseActionBase>();

        NPCChatConditionalCollectionBase _conditionals;
        NPCChatDialogItemID _page;
        string _text;
        byte _value;

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
        public EditorNPCChatResponse(NPCChatDialogItemID page, string text)
        {
            _page = page;
            _text = text;
        }

        /// <summary>
        /// Notifies listeners when the <see cref="EditorNPCChatResponse"/> has changed.
        /// </summary>
        public event TypedEventHandler<EditorNPCChatResponse> Changed;

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
        /// Gets a list of the <see cref="NPCChatResponseActionBase"/>s in this <see cref="EditorNPCChatResponse"/>.
        /// </summary>
        public IList<NPCChatResponseActionBase> ActionsList
        {
            get { return _actions; }
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
        /// Clears the Conditionals.
        /// </summary>
        public void ClearConditionals()
        {
            _conditionals = new EditorNPCChatConditionalCollection();

            if (Changed != null)
                Changed.Raise(this, EventArgs.Empty);
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
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public void SetConditionals(NPCChatConditionalCollectionBase value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (_conditionals == value)
                return;

            _conditionals = value;

            if (Changed != null)
                Changed.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the Page property.
        /// </summary>
        /// <param name="page">The new page.</param>
        public void SetPage(NPCChatDialogItemID page)
        {
            if (_page == page)
                return;

            _page = page;

            if (Changed != null)
                Changed.Raise(this, EventArgs.Empty);
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
            _value = value;
            _page = page;
            SetText(text);

            _actions.Clear();
            _actions.AddRange(actions);

            var c = conditionals as EditorNPCChatConditionalCollection;
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

            if (Changed != null)
                Changed.Raise(this, EventArgs.Empty);
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

            if (Changed != null)
                Changed.Raise(this, EventArgs.Empty);
        }
    }
}