using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A simple pop-up message box that displays a message, one or more response options, and a field to input text into.
    /// </summary>
    public class InputBox : MessageBox
    {
        TextBox _input;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="text">The message box's title text.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="buttonTypes">The <see cref="MessageBoxButton"/>s to display. Default is <see cref="MessageBoxButton.OkCancel"/></param>
        /// <param name="maxWidth">The maximum width of the created <see cref="MessageBox"/>. If less than or equal to 0, then
        /// the <see cref="MessageBox.DefaultMaxWidth"/> will be used instead. Default is 0.</param>
        public InputBox(IGUIManager guiManager, string text, string message,
                        MessageBoxButton buttonTypes = MessageBoxButton.OkCancel, int maxWidth = 0)
            : base(guiManager, text, message, buttonTypes, maxWidth)
        {
        }

        /// <summary>
        /// Gets the <see cref="TextBox"/> used to receive the input text.
        /// </summary>
        public TextBox InputControl
        {
            get { return _input; }
        }

        /// <summary>
        /// Gets or sets the text entered into the <see cref="InputControl"/>.
        /// </summary>
        public string InputText
        {
            get
            {
                if (InputControl == null)
                    return string.Empty;

                return _input.Text;
            }
            set
            {
                if (InputControl == null)
                    return;

                InputControl.Text = value;
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for something to be created and placed on the <see cref="MessageBox"/>
        /// before the buttons.
        /// </summary>
        /// <param name="yOffset">The current y-axis offset. If controls are added, this offset should be used, then updated afterwards
        /// to offset the <see cref="Control"/>s that will come after it.</param>
        protected override void BeforeCreateButtons(ref int yOffset)
        {
            base.BeforeCreateButtons(ref yOffset);

            var pos = new Vector2(Padding, yOffset);

            if (_input == null)
            {
                _input = CreateTextBoxControl(pos, new Vector2(32, 4));
                _input.SetFocus();
            }

            var height = Math.Max(_input.Size.Y, _input.Font.GetLineSpacing(_input.Font.DefaultSize) + _input.Border.Height);
            InputControl.Size = new Vector2(ClientSize.X - (Padding * 2), height);

            yOffset += (int)_input.Size.Y;
        }

        /// <summary>
        /// Creates a <see cref="InputBox"/> that only accepts numeric values.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="text">The message box's title text.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="buttonTypes">The <see cref="MessageBoxButton"/>s to display. Default is <see cref="MessageBoxButton.OkCancel"/></param>
        /// <param name="maxWidth">The maximum width of the created <see cref="MessageBox"/>. If less than or equal to 0, then
        /// the <see cref="MessageBox.DefaultMaxWidth"/> will be used instead. Default is 0.</param>
        /// <returns>An <see cref="InputBox"/> that only accepts numeric values.</returns>
        public static InputBox CreateNumericInputBox(IGUIManager guiManager, string text, string message,
                                                     MessageBoxButton buttonTypes = MessageBoxButton.OkCancel, int maxWidth = 0)
        {
            var ret = new InputBox(guiManager, text, message, buttonTypes, maxWidth);
            ret.InputControl.AllowKeysHandler = TextEventArgsFilters.IsDigitFunc;
            ret.InputText = "0";
            return ret;
        }

        /// <summary>
        /// Creates the <see cref="TextBox"/> instance to use for the input.
        /// </summary>
        /// <param name="position">The position of the <see cref="TextBox"/>.</param>
        /// <param name="size">The suggested size of the <see cref="TextBox"/>.</param>
        /// <returns>The <see cref="TextBox"/> instance to use for the input.</returns>
        protected virtual TextBox CreateTextBoxControl(Vector2 position, Vector2 size)
        {
            var ret = new InputTextBox(this, position, size) { IsMultiLine = false };
            return ret;
        }

        /// <summary>
        /// Gets if this <see cref="Control"/> should always be on top. This method will be invoked during the construction
        /// of the root level <see cref="Control"/>, so values set during the construction of the derived class will not
        /// be set before this method is called. It is highly recommended you only return a constant True or False value.
        /// This is only called when the <see cref="Control"/> is a root-level <see cref="Control"/>.
        /// </summary>
        /// <returns>If this <see cref="Control"/> will always be on top.</returns>
        protected override bool GetIsAlwaysOnTop()
        {
            return true;
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            if (InputControl != null)
                InputControl.Size = new Vector2(ClientSize.X - (Padding * 2), _input.Size.Y);
        }

        /// <summary>
        /// The input <see cref="TextBox"/> for the <see cref="InputBox"/>.
        /// </summary>
        sealed class InputTextBox : TextBox
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InputTextBox"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="position">Position of the Control reletive to its parent.</param>
            /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            public InputTextBox(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
            {
            }
        }
    }
}