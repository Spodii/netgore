using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.GUI
{
    public class MessageBox : Form
    {
        const int _padding = 4;
        static readonly object _eventOptionSelected = new object();

        /// <summary>
        /// The valid <see cref="MessageBoxButton"/> types for creating buttons.
        /// </summary>
        static readonly IEnumerable<MessageBoxButton> _validButtonCreationTypes;

        /// <summary>
        /// Initializes the <see cref="MessageBox"/> class.
        /// </summary>
        static MessageBox()
        {
            MaxWidth = 500;

            // Only allow creating buttons where the enum's value is a power of 2, since non-power-of-2 values
            // means that it is a concatenation of flags
            _validButtonCreationTypes = EnumHelper<MessageBoxButton>.Values.Where(x => BitOps.IsPowerOf2((int)x)).ToCompact();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="buttonTypes">The <see cref="MessageBoxButton"/>s to display.</param>
        public MessageBox(IGUIManager guiManager, string text, MessageBoxButton buttonTypes)
            : base(guiManager, Vector2.Zero, new Vector2(32))
        {
            ResizeToChildren = true;

            // Create the text
            var lines = StyledText.ToMultiline(new StyledText[] { new StyledText(text) }, true, Font, MaxWidth - (_padding * 2) - Border.Width);
            int yOffset = _padding;
            var labels = new List<Label>();
            foreach (var line in lines)
            {
                var concatLine = StyledText.ToString(line);
                var lbl = new Label(this, new Vector2(_padding, yOffset)) { Text = concatLine };
                labels.Add(lbl);
                yOffset += Font.LineSpacing;
            }

            yOffset += _padding;

            // Create the buttons
            var buttons = CreateButtons(buttonTypes);

            // Expand the form if needed to fit the buttons
            float neededButtonWidth = buttons.Sum(x => x.Size.X) + ((buttons.Count() + 1) * _padding);
            if (ClientSize.X < neededButtonWidth)
                ClientSize = new Vector2(neededButtonWidth, ClientSize.Y);

            // Arrange the buttons
            float xOffset = Math.Max(_padding, (ClientSize.X - neededButtonWidth) / 2f);
            foreach (var button in buttons)
            {
                button.Position = new Vector2(xOffset, yOffset);
                xOffset += button.Size.X + _padding;
            }

            // Center to the screen
            Position = (GUIManager.ScreenSize / 2f) - (Size / 2f);
        }

        /// <summary>
        /// Notifies listeners when the <see cref="MessageBox"/> has been closed from an option button being clicked.
        /// </summary>
        public event ControlEventHandler<MessageBoxButton> OptionSelected
        {
            add { Events.AddHandler(_eventOptionSelected, value); }
            remove { Events.RemoveHandler(_eventOptionSelected, value); }
        }

        /// <summary>
        /// Gets or sets the maximum width of a <see cref="MessageBox"/>.
        /// </summary>
        public static int MaxWidth { get; set; }

        /// <summary>
        /// Handles the Clicked event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.GUI.MouseClickEventArgs"/> instance containing the event data.</param>
        void Button_Clicked(object sender, MouseClickEventArgs e)
        {
            var type = (MessageBoxButton)((Control)sender).Tag;
            if (!EnumHelper<MessageBoxButton>.IsDefined(type))
                type = MessageBoxButton.None;

            InvokeClosed(type);

            Dispose();
        }

        /// <summary>
        /// Creates the buttons.
        /// </summary>
        /// <param name="types">The types of the buttons to create.</param>
        /// <returns>The created buttons.</returns>
        IEnumerable<Button> CreateButtons(MessageBoxButton types)
        {
            List<Button> ret = new List<Button>();

            // Create the buttons
            var typesInt = (int)types;
            foreach (var possibleType in _validButtonCreationTypes)
            {
                // Check if the flag for the value is set
                if (((int)possibleType & typesInt) == 0)
                    continue;

                string text = possibleType.ToString();
                var fontSize = Font.MeasureString(text);
                var btn = new Button(this, Vector2.Zero, fontSize + new Vector2(6)) { Text = text, Tag = possibleType };
                btn.Clicked += Button_Clicked;
                ret.Add(btn);
            }

            // Resize all buttons to equal the size of the largest button (looks better when consistently sized)
            Vector2 greatestSize = new Vector2(ret.Max(x => x.ClientSize.X), ret.Max(x => x.ClientSize.Y));
            foreach (var btn in ret)
            {
                btn.ClientSize = greatestSize;
            }

            return ret;
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="button">The button that was used to close the <see cref="MessageBox"/>.</param>
        void InvokeClosed(MessageBoxButton button)
        {
            OnOptionSelected(button);
            var handler = Events[_eventOptionSelected] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Handles when the <see cref="MessageBox"/> has been closed from an option button being clicked.
        /// This is called immediately before <see cref="CheckBox.TickedOverSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.TickedOverSpriteChanged"/> when possible.
        /// </summary>
        /// <param name="button">The button that was used to close the <see cref="MessageBox"/>.</param>
        protected virtual void OnOptionSelected(MessageBoxButton button)
        {
        }
    }
}