using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A simple pop-up message box that displays a message and one or more response options.
    /// </summary>
    public class MessageBox : Form
    {
        /// <summary>
        /// The amount of padding between the child controls in the <see cref="MessageBox"/>.
        /// </summary>
        public const int Padding = 4;

        static readonly object _eventButtonTypesChanged = new object();
        static readonly object _eventMessageChanged = new object();
        static readonly object _eventOptionSelected = new object();

        /// <summary>
        /// The valid <see cref="MessageBoxButton"/> types for creating buttons.
        /// </summary>
        static readonly IEnumerable<MessageBoxButton> _validButtonCreationTypes;

        /// <summary>
        /// The maximum width.
        /// </summary>
        readonly int _maxWidth;

        /// <summary>
        /// List of the child controls created by the <see cref="MessageBox"/>.
        /// </summary>
        readonly List<Control> _msgBoxChildren = new List<Control>();

        /// <summary>
        /// If true, the child controls will not be updated.
        /// </summary>
        readonly bool _suspendCreateChildControls = true;

        MessageBoxButton _buttonTypes;
        string _message;

        /// <summary>
        /// Initializes the <see cref="MessageBox"/> class.
        /// </summary>
        static MessageBox()
        {
            DefaultMaxWidth = 500;

            // Only allow creating buttons where the enum's value is a power of 2, since non-power-of-2 values
            // means that it is a concatenation of flags
            _validButtonCreationTypes = EnumHelper<MessageBoxButton>.Values.Where(x => BitOps.IsPowerOf2((int)x)).ToCompact();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="text">The message box's title text.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="buttonTypes">The <see cref="MessageBoxButton"/>s to display.</param>
        /// <param name="maxWidth">The maximum width of the created <see cref="MessageBox"/>. If less than or equal to 0, then
        /// the <see cref="DefaultMaxWidth"/> will be used instead. Default is 0.</param>
        public MessageBox(IGUIManager guiManager, string text, string message, MessageBoxButton buttonTypes, int maxWidth = 0)
            : base(guiManager, Vector2.Zero, new Vector2(32))
        {
            if (maxWidth <= 0)
                maxWidth = DefaultMaxWidth;

            _maxWidth = maxWidth;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Text = text;
            Message = message;
            ButtonTypes = buttonTypes;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _suspendCreateChildControls = false;

            CreateChildControls();
        }

        /// <summary>
        /// Notifies listeners when the <see cref="MessageBox.ButtonTypes"/> has changed.
        /// </summary>
        public event TypedEventHandler<Control> ButtonTypesChanged
        {
            add { Events.AddHandler(_eventButtonTypesChanged, value); }
            remove { Events.RemoveHandler(_eventButtonTypesChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="MessageBox.Message"/> has changed.
        /// </summary>
        public event TypedEventHandler<Control> MessageChanged
        {
            add { Events.AddHandler(_eventMessageChanged, value); }
            remove { Events.RemoveHandler(_eventMessageChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="MessageBox"/> has been closed from an option button being clicked.
        /// </summary>
        public event TypedEventHandler<Control, EventArgs<MessageBoxButton>> OptionSelected
        {
            add { Events.AddHandler(_eventOptionSelected, value); }
            remove { Events.RemoveHandler(_eventOptionSelected, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxButton"/>s that are included in this <see cref="MessageBox"/>.
        /// The default value is <see cref="MessageBoxButton.OkCancel"/>.
        /// </summary>
        [DefaultValue(MessageBoxButton.OkCancel)]
        public MessageBoxButton ButtonTypes
        {
            get { return _buttonTypes; }
            set
            {
                if (_buttonTypes == value)
                    return;

                _buttonTypes = value;

                InvokeButtonTypesChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default maximum width of a <see cref="MessageBox"/>. Default value is 500.
        /// </summary>
        [DefaultValue(500)]
        public static int DefaultMaxWidth { get; set; }

        /// <summary>
        /// Gets or sets if this <see cref="MessageBox"/> will automatically be disposed when any of the options
        /// have been selected. Disposing will happen after the <see cref="MessageBox.OptionSelected"/> event is
        /// raised, so it is possible to effectively change this value through the <see cref="MessageBox.OptionSelected"/>
        /// event handlers.
        /// The default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool DisposeOnSelection { get; set; }

        /// <summary>
        /// Gets or sets the message displayed in the <see cref="MessageBox"/>.
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set
            {
                if (_message == value)
                    return;

                _message = value;

                InvokeMessageChanged();
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for something to be created and placed on the <see cref="MessageBox"/>
        /// after the buttons.
        /// </summary>
        /// <param name="yOffset">The current y-axis offset. If controls are added, this offset should be used, then updated afterwards
        /// to offset the <see cref="Control"/>s that will come after it.</param>
        protected virtual void AfterCreateButtons(ref int yOffset)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for something to be created and placed on the <see cref="MessageBox"/>
        /// before the buttons.
        /// </summary>
        /// <param name="yOffset">The current y-axis offset. If controls are added, this offset should be used, then updated afterwards
        /// to offset the <see cref="Control"/>s that will come after it.</param>
        protected virtual void BeforeCreateButtons(ref int yOffset)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for something to be created and placed on the <see cref="MessageBox"/>
        /// before the message.
        /// </summary>
        /// <param name="yOffset">The current y-axis offset. If controls are added, this offset should be used, then updated afterwards
        /// to offset the <see cref="Control"/>s that will come after it.</param>
        protected virtual void BeforeCreateMessage(ref int yOffset)
        {
        }

        /// <summary>
        /// Handles the Clicked event of the Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        void Button_Clicked(object sender, MouseButtonEventArgs e)
        {
            var type = (MessageBoxButton)((Control)sender).Tag;
            if (!EnumHelper<MessageBoxButton>.IsDefined(type))
                type = MessageBoxButton.None;

            InvokeOptionSelected(type);

            if (DisposeOnSelection)
                Dispose();
        }

        /// <summary>
        /// Creates the buttons.
        /// </summary>
        /// <param name="types">The types of the buttons to create.</param>
        /// <returns>The created buttons.</returns>
        IEnumerable<Button> CreateButtons(MessageBoxButton types)
        {
            var ret = new List<Button>();

            // Create the buttons
            var typesInt = (int)types;
            foreach (var possibleType in _validButtonCreationTypes)
            {
                // Check if the flag for the value is set
                if (((int)possibleType & typesInt) == 0)
                    continue;

                var text = possibleType.ToString();
                var fontSize = Font.MeasureString(text);
                var btn = new Button(this, Vector2.Zero, fontSize + new Vector2(6)) { Text = text, Tag = possibleType, Font = Font };
                btn.Clicked += Button_Clicked;
                ret.Add(btn);
            }

            // Resize all buttons to equal the size of the largest button (looks better when consistently sized)
            var greatestSize = new Vector2(ret.Max(x => x.ClientSize.X), ret.Max(x => x.ClientSize.Y));
            foreach (var btn in ret)
            {
                btn.ClientSize = greatestSize;
            }

            return ret;
        }

        /// <summary>
        /// Creates the child controls for the <see cref="MessageBox"/>.
        /// </summary>
        void CreateChildControls()
        {
            if (_suspendCreateChildControls)
                return;

            // Clear out the old controls
            if (_msgBoxChildren.Count > 0)
            {
                foreach (var c in _msgBoxChildren)
                {
                    c.Dispose();
                }

                _msgBoxChildren.Clear();
            }

            var yOffset = Padding;

            BeforeCreateMessage(ref yOffset);

            // Create the text
            var lines = StyledText.ToMultiline(new StyledText[] { new StyledText(Message) }, true, Font,
                _maxWidth - (Padding * 2) - Border.Width);

            foreach (var line in lines)
            {
                var concatLine = StyledText.ToString(line);
                var lbl = new Label(this, new Vector2(Padding, yOffset)) { Text = concatLine, Font = Font };
                _msgBoxChildren.Add(lbl);
                yOffset += Font.GetLineSpacing();
            }

            yOffset += Padding;

            BeforeCreateButtons(ref yOffset);

            // Create the buttons
            var buttons = CreateButtons(ButtonTypes);
            _msgBoxChildren.AddRange(buttons);

            // Expand the form if needed to fit the buttons
            var neededButtonWidth = buttons.Sum(x => x.Size.X) + ((buttons.Count() + 1) * Padding);
            if (ClientSize.X < neededButtonWidth)
                ClientSize = new Vector2(neededButtonWidth, ClientSize.Y);

            // Arrange the buttons
            var xOffset = Math.Max(Padding, (ClientSize.X - neededButtonWidth) / 2f);
            foreach (var button in buttons)
            {
                button.Position = new Vector2(xOffset, yOffset);
                xOffset += button.Size.X + Padding;
            }

            AfterCreateButtons(ref yOffset);

            // Center to the screen
            Position = (GUIManager.ScreenSize / 2f) - (Size / 2f);
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
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeButtonTypesChanged()
        {
            OnButtonTypesChanged();
            var handler = Events[_eventButtonTypesChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeMessageChanged()
        {
            OnMessageChanged();
            var handler = Events[_eventMessageChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        /// <param name="button">The button that was used to close the <see cref="MessageBox"/>.</param>
        void InvokeOptionSelected(MessageBoxButton button)
        {
            OnOptionSelected(button);
            var handler = Events[_eventOptionSelected] as TypedEventHandler<Control, EventArgs<MessageBoxButton>>;
            if (handler != null)
                handler(this, EventArgsHelper.Create(button));
        }

        /// <summary>
        /// Handles when the <see cref="MessageBox.ButtonTypes"/> has changed.
        /// This is called immediately before <see cref="MessageBox.ButtonTypesChanged"/>.
        /// Override this method instead of using an event hook on <see cref="MessageBox.ButtonTypesChanged"/> when possible.
        /// </summary>
        protected virtual void OnButtonTypesChanged()
        {
            CreateChildControls();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            CreateChildControls();
        }

        /// <summary>
        /// Handles when the <see cref="MessageBox.Message"/> has changed.
        /// This is called immediately before <see cref="MessageBox.MessageChanged"/>.
        /// Override this method instead of using an event hook on <see cref="MessageBox.MessageChanged"/> when possible.
        /// </summary>
        protected virtual void OnMessageChanged()
        {
            CreateChildControls();
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

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            CreateChildControls();
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            DisposeOnSelection = true;
            ResizeToChildren = true;
        }
    }
}