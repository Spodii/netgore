using System.Collections.Generic;
using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    delegate void ChatFormSayHandler(ChatForm sender, string text);

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
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            // Create the input and output TextBoxes
            _input = new TextBox(this, Vector2.Zero, new Vector2(32, 32))
            { IsMultiLine = false, IsEnabled = true, Font = Font, MaxInputTextLength = GameData.MaxClientSayLength };
            _input.KeyPressed += Input_KeyPressed;

            _output = new TextBox(this, Vector2.Zero, new Vector2(32, 32)) { IsMultiLine = true, IsEnabled = false, Font = Font };

            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            // Force the initial repositioning
            RepositionTextBoxes();
        }

        /// <summary>
        /// Notifies listeners that a message is trying to be sent from the ChatForm's input box.
        /// </summary>
        public event ChatFormSayHandler Say;

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
        public void AppendToOutput(List<StyledText> text)
        {
            _output.AppendLine(text);
        }

        void Input_KeyPressed(object sender, KeyEventArgs e)
        {
            const int bufferScrollRate = 3;

            switch (e.Code)
            {
                case KeyCode.Return:
                    if (Say != null && !string.IsNullOrEmpty(_input.Text))
                    {
                        string text = _input.Text;
                        _input.Text = string.Empty;
                        Say(this, text);
                    }
                    break;

                case KeyCode.PageUp:
                    _bufferOffset += bufferScrollRate;
                    break;

                case KeyCode.PageDown:
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
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
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

            Vector2 inputSize = new Vector2(ClientSize.X, Font.CharacterSize + _input.Border.Height);
            Vector2 outputSize = new Vector2(ClientSize.X, ClientSize.Y - inputSize.Y);

            Vector2 inputPos = new Vector2(0, outputSize.Y);
            Vector2 outputPos = Vector2.Zero;

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
        }

        void UpdateBufferOffset()
        {
            if (_bufferOffset > _output.LineCount - _output.MaxVisibleLines)
                _bufferOffset = _output.LineCount - _output.MaxVisibleLines;

            if (_bufferOffset < 0)
                _bufferOffset = 0;

            int pos = _output.LineCount - _bufferOffset - _output.MaxVisibleLines - 1;
            if (pos < 0)
                pos = 0;
            else if (pos >= _output.LineCount)
                pos = _output.LineCount - 1;

            _output.LineBufferOffset = pos;
        }

        /// <summary>
        /// Updates the <see cref="Control"/> for anything other than the mouse or keyboard.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        protected override void UpdateControl(int currentTime)
        {
            UpdateBufferOffset();
            base.UpdateControl(currentTime);
        }
    }
}