using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
        readonly List<TreeNode> _treeNodes = new List<TreeNode>(1);
        NPCChatConditionalCollectionBase _conditionals;
        ushort _page;
        string _text;
        byte _value;

        /// <summary>
        /// Notifies listeners when the EditorNPCChatResponse has changed.
        /// </summary>
        public event EditorNPCChatResponseEventHandler OnChange;

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

        public List<TreeNode> TreeNodes
        {
            get { return _treeNodes; }
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
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return new EditorNPCChatConditionalCollection();
        }

        public void SetConditionals(NPCChatConditionalCollectionBase value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _conditionals = value;
        }

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
        protected override void SetReadValues(byte value, ushort page, string text, NPCChatConditionalCollectionBase conditionals)
        {
            _value = value;
            _page = page;
            SetText(text);

            EditorNPCChatConditionalCollection c = conditionals as EditorNPCChatConditionalCollection;
            _conditionals = c ?? new EditorNPCChatConditionalCollection();
        }

        public void SetText(string value)
        {
            if (_text == value)
                return;

            _text = value;

            if (OnChange != null)
                OnChange(this);
        }

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