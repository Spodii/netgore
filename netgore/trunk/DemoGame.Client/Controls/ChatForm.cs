using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    delegate void ChatFormSayHandler(ChatForm sender, string text);

    class ChatForm : Form, IRestorableSettings
    {
        readonly TextBox _input;
        readonly TextBox _output;

        int _bufferOffset = 0;

        /// <summary>
        /// Notifies listeners that a message is trying to be sent from the ChatForm's input box.
        /// </summary>
        public event ChatFormSayHandler OnSay;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatForm"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="pos">The pos.</param>
        public ChatForm(Control parent, Vector2 pos) : base(parent.GUIManager, "Chat", pos, new Vector2(300, 150), parent)
        {
            // Add the input and output boxes
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            float fontHeight = Font.LineSpacing;
// ReSharper restore DoNotCallOverridableMethodsInConstructor

            int borderHeight = GUIManager.TextBoxSettings.Border != null ? GUIManager.TextBoxSettings.Border.Height : 0;

            Vector2 inputSize = new Vector2(ClientSize.X, fontHeight + borderHeight);
            Vector2 outputSize = new Vector2(ClientSize.X, ClientSize.Y - inputSize.Y);

            Vector2 inputPos = new Vector2(0, outputSize.Y);
            Vector2 outputPos = Vector2.Zero;

            _input = new TextBox(inputPos, inputSize, this) { IsMultiLine = false, IsEnabled = true };
            _output = new TextBox(outputPos, outputSize, this) { IsMultiLine = true, IsEnabled = false };

            _input.OnKeyDown += Input_OnKeyDown;
        }

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

        void Input_OnKeyDown(object sender, KeyboardEventArgs e)
        {
            const int bufferScrollRate = 3;

            foreach (Keys key in e.Keys)
            {
                switch (key)
                {
                    case Keys.Enter:
                        if (OnSay != null && !string.IsNullOrEmpty(_input.Text))
                        {
                            string text = _input.Text;
                            _input.Text = string.Empty;
                            OnSay(this, text);
                        }
                        break;

                    case Keys.PageUp:
                        _bufferOffset += bufferScrollRate;
                        break;

                    case Keys.PageDown:
                        _bufferOffset -= bufferScrollRate;
                        break;
                }
            }
        }

        protected override void UpdateControl(int currentTime)
        {
            UpdateBufferOffset();
            base.UpdateControl(currentTime);
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

        #region IRestorableSettings Members

        /// <summary>
        /// Loads the values supplied by the <paramref name="items"/> to reconstruct the settings.
        /// </summary>
        /// <param name="items">NodeItems containing the values to restore.</param>
        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(items.AsFloat("X", Position.X), items.AsFloat("Y", Position.Y));
            IsVisible = items.AsBool("IsVisible", IsVisible);
        }

        /// <summary>
        /// Returns the key and value pairs needed to restore the settings.
        /// </summary>
        /// <returns>The key and value pairs needed to restore the settings.</returns>
        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion
    }
}