using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// A <see cref="Form"/> that displays the chat messages and allows the user to enter chat text.
    /// </summary>
    class ChatForm : Form
    {
        readonly TextBox _input;
        readonly TextBox _output;

        int _bufferOffset = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="pos">The pos.</param>
        public ChatForm(Control parent, Vector2 pos) : base(parent, pos, new Vector2(300, 150))
        {
            // Create the input and output TextBoxes
            _input = new TextBox(this, Vector2.Zero, new Vector2(32, 32))
            {
                IsMultiLine = false,
                IsEnabled = true,
                Font = GameScreenHelper.DefaultChatFont,
                MaxInputTextLength = GameData.MaxClientSayLength,
                BorderColor = new Color(255, 255, 255, 100)
            };

            _output = new TextBox(this, Vector2.Zero, new Vector2(32, 32))
            {
                IsMultiLine = true,
                IsEnabled = false,
                Font = GameScreenHelper.DefaultChatFont,
                BorderColor = new Color(255, 255, 255, 100)
            };

            _input.KeyPressed -= Input_KeyPressed;
            _input.KeyPressed += Input_KeyPressed;
           
            // Force the initial repositioning
            RepositionTextBoxes();
        }

        /// <summary>
        /// Notifies listeners that a message is trying to be sent from the ChatForm's input box.
        /// </summary>
        public event TypedEventHandler<ChatForm, EventArgs<string>> Say;

        /// <summary>
        /// Appends a set of styled text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        public void AppendToOutput(string text)
        {
            _output.AppendLine(text);
        }

        /// <summary>
        /// Appends a string of text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        /// <param name="color">Color of the text to append.</param>
        public void AppendToOutput(string text, Color color)
        {
            _output.AppendLine(new StyledText(text, color));
        }

        /// <summary>
        /// Appends a set of styled text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        public void AppendToOutput(IEnumerable<StyledText> text)
        {
            _output.AppendLine(text);
        }

        /// <summary>
        /// Clears the input text.
        /// </summary>
        public void ClearInput()
        {
            _input.Clear();
        }

        /// <summary>
        /// Clears the output text.
        /// </summary>
        public void ClearOutput()
        {
            _output.Clear();
        }

        void Input_KeyPressed(object sender, KeyEventArgs e)
        {
            const int bufferScrollRate = 3;

            switch (e.Code)
            {
                case Keyboard.Key.Return:
                    if (Say != null)
                    {
                        var text = _input.Text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            _input.Text = string.Empty;

                            Say.Raise(this, EventArgsHelper.Create(text));
                        }
                    }
                    break;

                case Keyboard.Key.PageUp:
                    _bufferOffset += bufferScrollRate;
                    break;

                case Keyboard.Key.PageDown:
                    _bufferOffset -= bufferScrollRate;
                    break;
            }
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.BorderChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderChanged"/> when possible.
        /// </summary>
        protected override void OnBorderChanged()
        {
            base.OnBorderChanged();

            RepositionTextBoxes();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            if (_input == null || _output == null)
                return;

            _input.Font = Font;
            _output.Font = Font;

            RepositionTextBoxes();
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            RepositionTextBoxes();
        }

        /// <summary>
        /// Repositions the input and output TextBoxes on the form.
        /// </summary>
        void RepositionTextBoxes()
        {
            if (_input == null || _output == null)
                return;

            var inputSize = new Vector2(ClientSize.X, Font.GetLineSpacing() + _input.Border.Height);
            var outputSize = new Vector2(ClientSize.X, ClientSize.Y - inputSize.Y);

            var inputPos = new Vector2(0, outputSize.Y);
            var outputPos = Vector2.Zero;

            _input.Size = inputSize;
            _input.Position = inputPos;

            _output.Size = outputSize;
            _output.Position = outputPos;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            Text = "Chat";
            IsCloseButtonVisible = false;
        }

        void UpdateBufferOffset()
        {
            if (_bufferOffset > _output.LineCount - _output.MaxVisibleLines)
                _bufferOffset = _output.LineCount - _output.MaxVisibleLines;

            if (_bufferOffset < 0)
                _bufferOffset = 0;

            var pos = _output.LineCount - _bufferOffset - _output.MaxVisibleLines - 1;
            if (pos < 0)
                pos = 0;
            else if (pos >= _output.LineCount)
                pos = _output.LineCount - 1;

            _output.LineBufferOffset = pos;
        }

        /// <summary>
        /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
        /// not visible.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(TickCount currentTime)
        {
            UpdateBufferOffset();
            base.UpdateControl(currentTime);
        }

        /// <summary>
        /// Gets if the input text box has focus.
        /// </summary>
        public bool HasInputFocus()
        {
            return _input.HasFocus;
        }
    }
}