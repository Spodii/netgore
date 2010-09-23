using System.Diagnostics;
using System.Linq;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class CreateCharacterScreen : GameScreen
    {
        public const string ScreenName = "character creation";

        Control _btnCreateCharacter;
        Label _cError;
        ClientSockets _sockets = null;
        TextBox _txtName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public CreateCharacterScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Gets the <see cref="Font"/> to use as the default font for the <see cref="IGUIManager"/> for this
        /// <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for this screen.</param>
        /// <returns>The <see cref="Font"/> to use for this <see cref="GameScreen"/>. If null, the
        /// <see cref="IScreenManager.DefaultFont"/> for this <see cref="GameScreen"/> will be used instead.</returns>
        protected override Font GetScreenManagerFont(IScreenManager screenManager)
        {
            return GameScreenHelper.GetScreenDefaultFont(screenManager);
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            if (_sockets == null)
                _sockets = ClientSockets.Instance;

            _btnCreateCharacter.IsEnabled = true;
            _cError.IsVisible = false;

            _sockets.PacketHandler.ReceivedCreateAccountCharacter += PacketHandler_ReceivedCreateAccountCharacter;

            base.Activate();
        }

        void ClickButton_Back(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen(CharacterSelectionScreen.ScreenName);
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
            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Create character", "Back");
            _btnCreateCharacter = menuButtons["Create character"];
            _btnCreateCharacter.Clicked += ClickButton_CreateCharacter;
            menuButtons["Back"].Clicked += ClickButton_Back;

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            new Label(cScreen, new Vector2(60, 260)) { Text = "Name:" };
            _txtName = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40))
            { IsMultiLine = false, Text = "", IsEnabled = true };

            base.Initialize();
        }

        void PacketHandler_ReceivedCreateAccountCharacter(IIPSocket sender, bool successful, string errorMessage)
        {
            _btnCreateCharacter.IsEnabled = true;

            if (successful)
            {
                ScreenManager.SetScreen(CharacterSelectionScreen.ScreenName);
                return;
            }

            SetError("Error: " + errorMessage);
        }

        /// <summary>
        /// Sets the error message to display
        /// </summary>
        /// <param name="message">Error message</param>
        void SetError(string message)
        {
            Debug.Assert(_cError != null, "_cError is null.");
            if (_cError == null)
                return;

            _cError.Text = string.Format("Error: {0}", message);
            _cError.IsVisible = true;
        }
    }
}