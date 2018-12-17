using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Features.NPCChat;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that is used to interact with NPCs.
    /// </summary>
    public class NPCChatDialogForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const int _numDisplayedResponses = 4;

        readonly TextBox _dialogTextControl;
        readonly ResponseText[] _responseTextControls = new ResponseText[_numDisplayedResponses];

        NPCChatDialogBase _dialog;
        NPCChatDialogItemBase _page;
        byte _responseOffset = 0;
        NPCChatResponseBase[] _responses;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogForm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="parent">The parent.</param>
        public NPCChatDialogForm(Vector2 position, Control parent) : base(parent, position, new Vector2(400, 300))
        {
            IsVisible = false;

            // NOTE: We want to use a scrollable textbox here... when we finally make one

            float spacing = Font.GetLineSpacing();

            var responseStartY = ClientSize.Y - (_numDisplayedResponses * spacing);
            var textboxSize = ClientSize - new Vector2(0, ClientSize.Y - responseStartY);
            _dialogTextControl = new TextBox(this, Vector2.Zero, textboxSize)
            { IsEnabled = false, CanFocus = false, IsMultiLine = true, Font = GameScreenHelper.DefaultChatFont };
            _dialogTextControl.ClientSize -= _dialogTextControl.Border.Size;

            for (byte i = 0; i < _numDisplayedResponses; i++)
            {
                var r = new ResponseText(this, new Vector2(5, responseStartY + (spacing * i))) { IsVisible = true, Font = GameScreenHelper.DefaultChatFont };
                r.Clicked += ResponseText_Clicked;
                _responseTextControls[i] = r;
            }
        }

        /// <summary>
        /// Notifies listeners when this NPCChatDialogForm wants to end the chat dialog.
        /// </summary>
        public event TypedEventHandler<NPCChatDialogForm> RequestEndDialog;

        /// <summary>
        /// Notifies listeners when a dialog response was chosen.
        /// </summary>
        public event TypedEventHandler<NPCChatDialogForm, EventArgs<NPCChatResponseBase>> SelectResponse;

        /// <summary>
        /// Gets if a dialog is currently open.
        /// </summary>
        public bool IsChatting
        {
            get { return _dialog != null; }
        }

        /// <summary>
        /// Handles when the Close button on the form is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected override void CloseButtonClicked(object sender, MouseButtonEventArgs e)
        {
            // Since we have to let the server know what our chat state is, we can't just close the window. Instead,
            // make a request to the server that we want to end the chat dialog. If the server allows it, then it
            // will eventually close.
            if (RequestEndDialog != null)
                RequestEndDialog.Raise(this, EventArgs.Empty);
        }

        public void EndDialog()
        {
            IsVisible = false;
            _dialog = null;
        }

        /// <summary>
        /// Handles when a key is being pressed while the <see cref="Control"/> has focus.
        /// This is called immediately before <see cref="Control.KeyPressed"/>.
        /// Override this method instead of using an event hook on <see cref="Control.KeyPressed"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnKeyPressed(KeyEventArgs e)
        {
            base.OnKeyPressed(e);

            if (e.Code == Keyboard.Key.Escape)
            {
                if (RequestEndDialog != null)
                    RequestEndDialog.Raise(this, EventArgs.Empty);
            }
            else
            {
                var asNumeric = e.Code.GetNumericKeyAsValue();
                if (!asNumeric.HasValue)
                    return;

                var value = asNumeric.Value - 1;
                if (value < 0)
                    value = 10;

                if (value < _responses.Length)
                {
                    if (SelectResponse != null)
                        SelectResponse.Raise(this, EventArgsHelper.Create(_responses[value]));
                }
            }
        }

        void ResponseText_Clicked(object sender, MouseButtonEventArgs e)
        {
            var src = (ResponseText)sender;
            var response = src.Response;
            if (response != null)
            {
                if (SelectResponse != null)
                    SelectResponse.Raise(this, EventArgsHelper.Create(response));
            }
        }

        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "NPC Chat";
        }

        public void SetPageIndex(NPCChatDialogItemID pageID, IEnumerable<byte> responsesToSkip)
        {
            _page = _dialog.GetDialogItem(pageID);

            if (_page == null)
            {
                const string errmsg =
                    "Page `{0}` in dialog `{1}` is null. The Client should never be trying to set an invalid page.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, pageID, _dialog);
                Debug.Fail(string.Format(errmsg, pageID, _dialog));
                EndDialog();
                return;
            }

            if (responsesToSkip != null)
                _responses = _page.Responses.Where(x => !responsesToSkip.Contains(x.Value)).OrderBy(x => x.Value).ToArray();
            else
                _responses = _page.Responses.OrderBy(x => x.Value).ToArray();

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

            for (var i = 0; i < _numDisplayedResponses; i++)
            {
                var responseIndex = _responseOffset + i;

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
            SetFocus();
        }

        class ResponseText : Label
        {
            NPCChatResponseBase _response;

            public ResponseText(Control parent, Vector2 position) : base(parent, position)
            {
            }

            public NPCChatResponseBase Response
            {
                get { return _response; }
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
                Text = (index + 1) + ": " + Response.Text;
            }

            public void UnsetResponse()
            {
                _response = null;
                Text = string.Empty;
            }
        }
    }
}