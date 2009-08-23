using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public delegate void EditorNPCChatDialogItemEventHandler(EditorNPCChatDialogItem dialogItem);

    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// </summary>
    public class EditorNPCChatDialogItem : NPCChatDialogItemBase
    {
        ushort _index;
        readonly List<TreeNode> _treeNodes = new List<TreeNode>();
        List<EditorNPCChatResponse> _responses = new List<EditorNPCChatResponse>();
        string _text;
        string _title;

        public event EditorNPCChatDialogItemEventHandler OnChange;

        /// <summary>
        /// When overridden in the derived class, gets the page index of this NPCChatDialogItemBase in the
        /// NPCChatDialogBase. This value is unique to each NPCChatDialogItemBase in the NPCChatDialogBase.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        public List<EditorNPCChatResponse> ResponseList
        {
            get { return _responses; }
            set { _responses = value; }
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the EditorNPCChatResponses available
        /// for this page of dialog.
        /// </summary>
        public override IEnumerable<NPCChatResponseBase> Responses
        {
            get { return ResponseList.Cast<NPCChatResponseBase>(); }
        }

        /// <summary>
        /// When overridden in the derived class, gets the main dialog text in this page of dialog.
        /// </summary>
        public override string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the title for this page of dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public override string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        protected override void SetReadValues(ushort page, string title, string text)
        {
            _index = page;
            SetTitle(title);
            SetText(text);
        }

        public List<TreeNode> TreeNodes
        {
            get { return _treeNodes; }
        }

        public EditorNPCChatDialogItem(ushort index)
            : this(index, string.Empty)
        {
        }

        public EditorNPCChatDialogItem(ushort index, string text)
            : this(index, text, string.Empty)
        {
        }

        public EditorNPCChatDialogItem(ushort index, string text, string title)
        {
            SetText(text);
            SetTitle(title);

            _index = index;
        }

        public override ushort GetNextPage(object user, object npc, byte responseIndex)
        {
            return ResponseList[responseIndex].Page;
        }

        public void SetText(string value)
        {
            if (_text == value)
                return;

            _text = value;

            if (OnChange != null)
                OnChange(this);
        }

        public void SetTitle(string value)
        {
            if (_title == value)
                return;

            _title = value;

            if (OnChange != null)
                OnChange(this);
        }
    }
}
