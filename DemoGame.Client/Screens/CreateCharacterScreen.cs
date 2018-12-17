using System;
using System.Linq;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;
using NetGore;

namespace DemoGame.Client
{
    class CreateCharacterScreen : GameMenuScreenBase
    {
        public const string ScreenName = "character creation";
        const string _title = "Create Character";

        const string _invalidCharacterNameMessage = "Invalid character name.";

        Control _btnCreateCharacter;
        TextBox _cStatus;
        ClientSockets _sockets = null;
        TextBox _txtName;
        Label _lblValidNameMarker;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public CreateCharacterScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
            PlayMusic = true;
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            if (_sockets == null)
                _sockets = ClientSockets.Instance;

            // Set event hooks (remove first to ensure only one listener is set)
            _sockets.PacketHandler.ReceivedCreateAccountCharacter -= PacketHandler_ReceivedCreateAccountCharacter;
            _sockets.PacketHandler.ReceivedCreateAccountCharacter += PacketHandler_ReceivedCreateAccountCharacter;

            // Reset the control states
            SetError(null);
            _btnCreateCharacter.IsEnabled = true;
            _txtName.Text = string.Empty;
        }

        public override void Deactivate()
        {
            base.Deactivate();

            // Remove event hooks
            if (_sockets != null)
            {
                _sockets.PacketHandler.ReceivedCreateAccountCharacter -= PacketHandler_ReceivedCreateAccountCharacter;
            }
        }

        void ClickButton_Back(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen<CharacterSelectionScreen>();
        }

        void ClickButton_CreateCharacter(object sender, MouseButtonEventArgs e)
        {
            var name = _txtName.Text;

            // Validate the name client-side before talking to the server
            if (!GameData.UserName.IsValid(name))
            {
                SetError(_invalidCharacterNameMessage);
                return;
            }

            // Disable the button while we wait for a response
            _btnCreateCharacter.IsEnabled = false;

            // Send request to server
            using (var pw = ClientPacket.CreateNewAccountCharacter(name))
            {
                _sockets.Send(pw, ClientMessageType.System);
            }
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Create character", "Back");
            _btnCreateCharacter = menuButtons["Create character"];
            _btnCreateCharacter.Clicked += ClickButton_CreateCharacter;
            menuButtons["Back"].Clicked += ClickButton_Back;

            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 260), "Name:");

            _txtName = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = "", IsEnabled = true };
            _txtName.TextChanged += _txtName_TextChanged;

            _lblValidNameMarker = new Label(cScreen, _txtName.Position + new Vector2(_txtName.Size.X + 10, 0)) { Text = "*", IsVisible = false, ForeColor = Color.Red };

            var textBoxPos = new Vector2(60, _txtName.Position.Y + _txtName.Size.Y + 20);
            var textBoxSize = new Vector2(cScreen.ClientSize.X - (textBoxPos.X * 2), cScreen.ClientSize.Y - textBoxPos.Y - 60);

            _cStatus = new TextBox(cScreen, textBoxPos, textBoxSize) { ForeColor = Color.Red, Border = null, CanFocus = false, IsMultiLine = true, IsEnabled = false };
        }

        void _txtName_TextChanged(Control sender, EventArgs e)
        {
            bool isNameValid = GameData.UserName.IsValid(_txtName.Text);

            // Update the validation marker next to the name
            _lblValidNameMarker.IsVisible = !isNameValid;

            // Remove error message when name changes
            SetError(null);
        }

        void PacketHandler_ReceivedCreateAccountCharacter(IIPSocket sender, CreateAccountEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.GetHashCode());
            // Handles when we receive the CreateAccountCharacter message from the server
            _btnCreateCharacter.IsEnabled = true; // Re-enable the button now that we got a response

            if (e.Successful)
            {
                // Character created - move back to selection screen
                ScreenManager.SetScreen<CharacterSelectionScreen>();
                return;
            }

            // Character creation failed, display error
            SetError("Error: " + e.ErrorMessage);
        }

        /// <summary>
        /// Gets if the error is set to the given message.
        /// </summary>
        bool IsErrorSet(string message)
        {
            return _cStatus.IsVisible && _cStatus.Text.Contains(message, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Sets the error message to display
        /// </summary>
        /// <param name="message">Error message</param>
        void SetError(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                _cStatus.Text = string.Empty;
                _cStatus.IsVisible = false;
            }
            else
            {
                _cStatus.Text = "Error: " + message;
                _cStatus.IsVisible = true;
            }
        }
    }
}