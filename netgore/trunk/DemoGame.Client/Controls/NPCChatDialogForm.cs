using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Client
{
    delegate void ChatDialogSelectResponseHandler(NPCChatDialogForm sender, NPCChatResponseBase response);

    delegate void ChatDialogRequestEndDialogHandler(NPCChatDialogForm sender);

    class NPCChatDialogForm : Form, IRestorableSettings
    {
        const int _numDisplayedResponses = 4;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly TextBoxMultiLineLocked _dialogTextControl;
        readonly ResponseText[] _responseTextControls = new ResponseText[_numDisplayedResponses];
        NPCChatDialogBase _dialog;
        NPCChatDialogItemBase _page;

        byte _responseOffset = 0;

        NPCChatResponseBase[] _responses;

        /// <summary>
        /// Notifies listeners when this NPCChatDialogForm wants to end the chat dialog.
        /// </summary>
        public event ChatDialogRequestEndDialogHandler OnRequestEndDialog;

        /// <summary>
        /// Notifies listeners when a dialog response was chosen.
        /// </summary>
        public event ChatDialogSelectResponseHandler OnSelectResponse;

        /// <summary>
        /// Gets if a dialog is currently open.
        /// </summary>
        public bool IsChatting
        {
            get { return _dialog != null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogForm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public NPCChatDialogForm(Vector2 position, Control parent)
            : base(parent.GUIManager, "NPC Chat", position, new Vector2(600, 500), parent)
        {
            IsVisible = false;

            OnKeyPress += NPCChatDialogForm_OnKeyPress;

            // NOTE: We want to use a scrollable textbox here... when we finally make one

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            float spacing = Font.LineSpacing;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            float responseStartY = ClientSize.Y - (_numDisplayedResponses * spacing);
            Vector2 textboxSize = ClientSize - new Vector2(0, ClientSize.Y - responseStartY);
            _dialogTextControl = new TextBoxMultiLineLocked(string.Empty, Vector2.Zero, textboxSize, this);
            _dialogTextControl.OnKeyPress += NPCChatDialogForm_OnKeyPress;

            for (byte i = 0; i < _numDisplayedResponses; i++)
            {
                ResponseText r = new ResponseText(new Vector2(0, responseStartY + (spacing * i)), this)
                {
                    IsVisible = true
                };
                r.OnClick += ResponseText_OnClick;
                _responseTextControls[i] = r;
            }
        }

        public void EndDialog()
        {
            IsVisible = false;
            _dialog = null;
        }

        void NPCChatDialogForm_OnKeyPress(object sender, KeyboardEventArgs e)
        {
            if (e.Keys.Contains(Keys.Escape))
            {
                if (OnRequestEndDialog != null)
                    OnRequestEndDialog(this);
            }
        }

        void ResponseText_OnClick(object sender, MouseClickEventArgs e)
        {
            ResponseText src = (ResponseText)sender;
            NPCChatResponseBase response = src.Response;
            if (response != null)
            {
                if (OnSelectResponse != null)
                    OnSelectResponse(this, response);
            }
        }

        public void SetPageIndex(ushort pageIndex, IEnumerable<byte> responsesToSkip)
        {
            _page = _dialog.GetDialogItem(pageIndex);

            if (_page == null)
            {
                const string errmsg =
                    "Page `{0}` in dialog `{1}` is null. The Client should never be trying to set an invalid page.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, pageIndex, _dialog);
                Debug.Fail(string.Format(errmsg, pageIndex, _dialog));
                EndDialog();
                return;
            }

            _responses = _page.Responses.Where(x => !responsesToSkip.Contains(x.Value)).OrderBy(x => x.Value).ToArray();

            _dialogTextControl.Text = _page.Text;
            SetResponseOffset(0);
            IsVisible = true;
        }

        public void SetResponseOffset(int newOffset)
        {
            if (newOffset > _responses.Length - _numDisplayedResponses)
                newOffset = _responses.Length - _numDisplayedResponses;
            if (newOffset < 0)
                newOffset = 0;

            Debug.Assert(_responseOffset >= 0);

            _responseOffset = (byte)newOffset;

            for (int i = 0; i < _numDisplayedResponses; i++)
            {
                int responseIndex = _responseOffset + i;

                if (responseIndex >= 0 && responseIndex < _responses.Length)
                    _responseTextControls[i].SetResponse(_responses[responseIndex], responseIndex);
                else
                    _responseTextControls[i].UnsetResponse();
            }
        }

        public void StartDialog(NPCChatDialogBase dialog)
        {
            if (dialog == null)
            {
                Debug.Fail("Dialog is null.");
                return;
            }

            _dialog = dialog;
        }

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            {
                new NodeItem("X", Position.X), new NodeItem("Y", Position.Y)
            };
        }

        #endregion

        class ResponseText : Label
        {
            NPCChatResponseBase _response;

            public NPCChatResponseBase Response
            {
                get { return _response; }
            }

            public ResponseText(Vector2 position, Control parent) : base(string.Empty, position, parent)
            {
            }

            public void SetResponse(NPCChatResponseBase response, int index)
            {
                if (response == null)
                {
                    Debug.Fail("Parameter 'response' should never be null. Use UnsetResponse() instead.");
                    UnsetResponse();
                    return;
                }

                _response = response;
                Text = index + ": " + Response.Text;
            }

            public void UnsetResponse()
            {
                _response = null;
                Text = string.Empty;
            }
        }
    }
}