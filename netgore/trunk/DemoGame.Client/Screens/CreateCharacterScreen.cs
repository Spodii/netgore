using System.Linq;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class CreateCharacterScreen : GameMenuScreenBase
    {
        public const string ScreenName = "character creation";
        const string _title = "Create Character";

        Control _btnCreateCharacter;
        TextBox _cStatus;
        ClientSockets _sockets = null;
        TextBox _txtName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public CreateCharacterScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
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

            _btnCreateCharacter.IsEnabled = true;
            _cStatus.IsVisible = false;

            _sockets.PacketHandler.ReceivedCreateAccountCharacter += PacketHandler_ReceivedCreateAccountCharacter;
        }

        void ClickButton_Back(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen<CharacterSelectionScreen>();
        }

        void ClickButton_CreateCharacter(object sender, MouseButtonEventArgs e)
        {
            var name = _txtName.Text;

            if (!GameData.UserName.IsValid(name))
            {
                SetError("Invalid character name.");
                return;
            }

            _btnCreateCharacter.IsEnabled = false;

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

            _txtName = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40))
            { IsMultiLine = false, Text = "", IsEnabled = true };

            var textBoxPos = new Vector2(60, _txtName.Position.Y + _txtName.Size.Y + 20);
            var textBoxSize = new Vector2(cScreen.ClientSize.X - (textBoxPos.X * 2), cScreen.ClientSize.Y - textBoxPos.Y - 60);
            _cStatus = new TextBox(cScreen, textBoxPos, textBoxSize)
            { ForeColor = Color.Red, Border = null, CanFocus = false, IsMultiLine = true, IsEnabled = false };
        }

        void PacketHandler_ReceivedCreateAccountCharacter(IIPSocket sender, CreateAccountEventArgs e)
        {
            _btnCreateCharacter.IsEnabled = true;

            if (e.Successful)
            {
                ScreenManager.SetScreen<CharacterSelectionScreen>();
                return;
            }

            SetError("Error: " + e.ErrorMessage);
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