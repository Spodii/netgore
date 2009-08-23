using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public delegate void EditorNPCChatResponseEventHandler(EditorNPCChatResponse response);

    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// </summary>
    public class EditorNPCChatResponse : NPCChatResponseBase
    {
        readonly List<TreeNode> _treeNodes = new List<TreeNode>(1);
        ushort _page;
        string _text;

        public event EditorNPCChatResponseEventHandler OnChange;

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

        byte _value;

        /// <summary>
        /// When overridden in the derived class, gets the 0-based response index value for this response.
        /// </summary>
        public override byte Value
        {
            get { return _value; }
        }

        internal void SetValue(byte value)
        {
            if (_value == value)
                return;

            _value = value;

            if (OnChange != null)
                OnChange(this);
        }

        public List<TreeNode> TreeNodes
        {
            get { return _treeNodes; }
        }

        public EditorNPCChatResponse(string text)
            : this(EndConversationPage, text)
        {
        }

        public EditorNPCChatResponse(IValueReader reader)
            : base(reader)
        {
        }

        public EditorNPCChatResponse(ushort page, string text)
        {
            _page = page;
            _text = text;
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        protected override void SetReadValues(byte value, ushort page, string text)
        {
            _value = value;
            _page = page;
            SetText(text);
        }

        public void SetText(string value)
        {
            if (_text == value)
                return;

            _text = value;

            if (OnChange != null)
                OnChange(this);
        }
    }
}