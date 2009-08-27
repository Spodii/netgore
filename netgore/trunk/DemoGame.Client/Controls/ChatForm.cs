using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    delegate void ChatFormSayHandler(ChatForm sender, string text);

    class ChatForm : Form, IRestorableSettings
    {
        readonly TextBoxSingleLine _input;
        readonly TextBoxMultiLineLocked _output;

        /// <summary>
        /// Notifies listeners that a message is trying to be sent from the ChatForm's input box.
        /// </summary>
        public event ChatFormSayHandler OnSay;

        public ChatForm(Control parent, Vector2 pos) : base(parent.GUIManager, "Chat", pos, new Vector2(300, 150), parent)
        {
            // Add the input and output boxes
            float fontHeight = Font.LineSpacing;

            Vector2 inputSize = new Vector2(ClientSize.X, fontHeight);
            Vector2 outputSize = new Vector2(ClientSize.X, ClientSize.Y - inputSize.Y);

            Vector2 inputPos = new Vector2(0, outputSize.Y);
            Vector2 outputPos = Vector2.Zero;

            _input = new TextBoxSingleLine(string.Empty, inputPos, inputSize, this);
            _output = new TextBoxMultiLineLocked(string.Empty, outputPos, outputSize, this);

            _input.OnKeyDown += Input_OnKeyDown;
        }

        /// <summary>
        /// Appends a set of styled text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        public void AppendToOutput(string text)
        {
            _output.Append(text);
        }

        /// <summary>
        /// Appends a string of text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        /// <param name="color">Color of the text to append.</param>
        public void AppendToOutput(string text, Color color)
        {
            _output.Append(text, color);
        }

        /// <summary>
        /// Appends a set of styled text to the output TextBox.
        /// </summary>
        /// <param name="text">Text to append to the output TextBox.</param>
        public void AppendToOutput(List<StyledText> text)
        {
            _output.Append(text);
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
                        _output.BufferOffset += bufferScrollRate;
                        break;

                    case Keys.PageDown:
                        _output.BufferOffset -= bufferScrollRate;
                        break;
                }
            }
        }

        #region IRestorableSettings Members

        public void Load(IDictionary<string, string> items)
        {
            Position = new Vector2(float.Parse(items["X"]), float.Parse(items["Y"]));
            IsVisible = bool.Parse(items["IsVisible"]);
        }

        public IEnumerable<NodeItem> Save()
        {
            return new NodeItem[]
            { new NodeItem("X", Position.X), new NodeItem("Y", Position.Y), new NodeItem("IsVisible", IsVisible) };
        }

        #endregion
    }
}