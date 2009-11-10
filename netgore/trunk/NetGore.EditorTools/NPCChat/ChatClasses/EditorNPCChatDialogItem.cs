using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.IO;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace NetGore.EditorTools.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// </summary>
    public class EditorNPCChatDialogItem : NPCChatDialogItemBase
    {
        NPCChatConditionalCollectionBase _conditionals;
        ushort _index;
        bool _isBranch;
        readonly List<EditorNPCChatResponse> _responses = new List<EditorNPCChatResponse>();
        string _text;
        string _title;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Notifies listeners when any of the object's property values have changed.
        /// </summary>
        public event EditorNPCChatDialogItemEventHandler OnChange;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialogItem"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public EditorNPCChatDialogItem(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialogItem"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public EditorNPCChatDialogItem(ushort index) : this(index, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialogItem"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        public EditorNPCChatDialogItem(ushort index, string text) : this(index, text, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialogItem"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        /// <param name="title">The title.</param>
        public EditorNPCChatDialogItem(ushort index, string text, string title)
        {
            SetText(text);
            SetTitle(title);

            _index = index;
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalCollectionBase that contains the
        /// conditionals used to evaluate if this NPCChatDialogItemBase may be used. If this value is null, it
        /// is assumed that there are no conditionals attached to this NPCChatDialogItemBase, and should be treated
        /// the same way as if the conditionals evaluated to true.
        /// </summary>
        public override NPCChatConditionalCollectionBase Conditionals
        {
            get { return _conditionals; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the page index of this NPCChatDialogItemBase in the
        /// NPCChatDialogBase. This value is unique to each NPCChatDialogItemBase in the NPCChatDialogBase.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this NPCChatDialogItemBase is a branch dialog or not. If
        /// true, the dialog should be automatically progressed by using EvaluateBranch() instead of waiting for
        /// and accepting input from the user for a response.
        /// </summary>
        public override bool IsBranch
        {
            get { return _isBranch; }
        }

        /// <summary>
        /// Gets the response list.
        /// </summary>
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

        /// <summary>
        /// Adds multiple EditorNPCChatResponses.
        /// </summary>
        /// <param name="responses">The EditorNPCChatResponses to add.</param>
        public void AddResponse(params EditorNPCChatResponse[] responses)
        {
            if (responses == null)
                return;

            foreach (var response in responses)
            {
                AddResponse(response);
            }
        }

        /// <summary>
        /// Adds a EditorNPCChatResponse.
        /// </summary>
        /// <param name="response">The EditorNPCChatResponse to add.</param>
        public void AddResponse(EditorNPCChatResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            _responses.Add(response);
            var index = _responses.IndexOf(response);
            response.SetValue((byte)index);

            EnsureResponseValuesAreValid();
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
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected override NPCChatResponseBase CreateResponse(IValueReader reader)
        {
            return new EditorNPCChatResponse(reader);
        }

        /// <summary>
        /// Ensures that the <see cref="EditorNPCChatResponse.Value"/> for each <see cref="EditorNPCChatResponse"/> is valid.
        /// </summary>
        void EnsureResponseValuesAreValid()
        {
            for (var i = 0; i < _responses.Count; i++)
            {
                if (_responses[i].Value != i)
                    _responses[i].SetValue((byte)i);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatResponseBase of the response with the given
        /// <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>The NPCChatResponseBase for the response at index <paramref name="responseIndex"/>, or null
        /// if the response is invalid or ends the chat dialog.</returns>
        public override NPCChatResponseBase GetResponse(byte responseIndex)
        {
            if (responseIndex >= _responses.Count)
            {
                const string errmsg = "Invalid response index `{0}` for page `{1}`. Max response index is `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, responseIndex, Index, _responses.Count - 1);
                return null;
            }

            return _responses[responseIndex];
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
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="isBranch">The IsBranch value.</param>
        /// <param name="responses">The responses.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected override void SetReadValues(ushort page, string title, string text, bool isBranch,
                                              IEnumerable<NPCChatResponseBase> responses,
                                              NPCChatConditionalCollectionBase conditionals)
        {
            _index = page;
            _isBranch = isBranch;
            SetTitle(title);
            SetText(text);

            _responses.Clear();
            _responses.AddRange(responses.Cast<EditorNPCChatResponse>().OrderBy(x => x.Value));
            EnsureResponseValuesAreValid();

            var c = conditionals as EditorNPCChatConditionalCollection;
            _conditionals = c ?? new EditorNPCChatConditionalCollection();
        }

        /// <summary>
        /// Sets the Text.
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
        /// Sets the Title.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetTitle(string value)
        {
            if (_title == value)
                return;

            _title = value;

            if (OnChange != null)
                OnChange(this);
        }

        /// <summary>
        /// Tries to set the EditorNPCChatDialogItem as a branch dialog.
        /// </summary>
        /// <param name="error">Contains the message for the error if there was one, or an empty string
        /// if there was no error.</param>
        /// <returns>True if the EditorNPCChatDialogItem was successfully set as a branch; otherwise false.</returns>
        public bool TrySetAsBranch(out string error)
        {
            if (IsBranch)
            {
                error = "Already set as a branch dialog item.";
                return false;
            }

            if (ResponseList.Count > 2)
            {
                error = "Cannot change to a branch dialog item when there are more than 2 responses.";
                return false;
            }

            // Add responses until we have exactly 2
            var responsesNeeded = 2 - ResponseList.Count;
            for (var i = 0; i < responsesNeeded; i++)
            {
                AddResponse(new EditorNPCChatResponse("New response"));
            }

            Debug.Assert(ResponseList.Count == 2);

            // Set up the responses
            _responses[0].SetText("False");
            _responses[0].ClearConditionals();
            _responses[1].SetText("True");
            _responses[1].ClearConditionals();

            if (_conditionals == null)
                _conditionals = new EditorNPCChatConditionalCollection();

            _isBranch = true;

            error = string.Empty;
            return true;
        }

        /// <summary>
        /// Tries to set the EditorNPCChatDialogItem as a normal (non-branch) dialog.
        /// </summary>
        /// <param name="error">Contains the message for the error if there was one, or an empty string
        /// if there was no error.</param>
        /// <returns>True if the EditorNPCChatDialogItem was successfully set as a non-branch; otherwise false.</returns>
        public bool TrySetAsNonBranch(out string error)
        {
            if (!IsBranch)
            {
                error = "Already set as a non-branch dialog item.";
                return false;
            }

            // Clear the conditionals
            _conditionals = new EditorNPCChatConditionalCollection();

            _isBranch = false;
            error = string.Empty;
            return true;
        }
    }
}