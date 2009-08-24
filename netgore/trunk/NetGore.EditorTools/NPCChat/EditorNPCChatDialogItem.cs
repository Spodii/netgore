using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public delegate void EditorNPCChatDialogItemEventHandler(EditorNPCChatDialogItem dialogItem);

    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// </summary>
    public class EditorNPCChatDialogItem : NPCChatDialogItemBase
    {
        readonly List<EditorNPCChatResponse> _responses = new List<EditorNPCChatResponse>();
        readonly List<TreeNode> _treeNodes = new List<TreeNode>();
        ushort _index;
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

        public List<TreeNode> TreeNodes
        {
            get { return _treeNodes; }
        }

        public EditorNPCChatDialogItem(IValueReader reader) : base(reader)
        {
        }

        public EditorNPCChatDialogItem(ushort index) : this(index, string.Empty)
        {
        }

        public EditorNPCChatDialogItem(ushort index, string text) : this(index, text, string.Empty)
        {
        }

        public EditorNPCChatDialogItem(ushort index, string text, string title)
        {
            SetText(text);
            SetTitle(title);

            _index = index;
        }

        public void AddResponse(params EditorNPCChatResponse[] responses)
        {
            if (responses == null)
                return;

            foreach (EditorNPCChatResponse response in responses)
            {
                AddResponse(response);
            }
        }

        public void AddResponse(EditorNPCChatResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            _responses.Add(response);
            int index = _responses.IndexOf(response);
            response.SetValue((byte)index);
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected override NPCChatResponseBase CreateResponse(IValueReader reader)
        {
            return new EditorNPCChatResponse(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the index of the next NPCChatDialogItemBase to use from
        /// the given response.
        /// </summary>
        /// <param name="user">The user that is participating in the chatting.</param>
        /// <param name="npc">The NPC chat is participating in the chatting.</param>
        /// <param name="responseIndex">The index of the response used.</param>
        /// <returns>The index of the NPCChatDialogItemBase to go to based off of the given response.</returns>
        public override ushort GetNextPage(object user, object npc, byte responseIndex)
        {
            return ResponseList[responseIndex].Page;
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="responses">The responses.</param>
        protected override void SetReadValues(ushort page, string title, string text, IEnumerable<NPCChatResponseBase> responses)
        {
            _index = page;
            SetTitle(title);
            SetText(text);

            _responses.Clear();
            _responses.AddRange(responses.Cast<EditorNPCChatResponse>());
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