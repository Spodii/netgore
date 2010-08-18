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

        public InputBox(IGUIManager guiManager, string text, string message,
                        MessageBoxButton buttonTypes = MessageBoxButton.OkCancel) : base(guiManager, text, message, buttonTypes)
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
                _input = new TextBox(this, pos, new Vector2(32))
                { Text = "as" };
            }

            InputControl.Size = new Vector2(ClientSize.X - (Padding * 2), _input.Font.GetLineSpacing() + 16);

            yOffset += (int)_input.Size.Y;
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
    }
}