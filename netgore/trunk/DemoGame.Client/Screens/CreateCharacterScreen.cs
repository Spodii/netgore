using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    class CreateCharacterScreen : GameScreen
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const string ScreenName = "character creation";
        Button _btnCreateCharacter;
        Label _cError;
        IGUIManager _gui;

        SpriteBatch _sb = null;
        ClientSockets _sockets = null;
        TextBox _txtName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCharacterScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public CreateCharacterScreen(IScreenManager screenManager)
            : base(screenManager, ScreenName)
        {
            PlayMusic = false;
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

            _sockets.PacketHandler.OnCreateAccountCharacter += PacketHandler_OnCreateAccountCharacter;

            base.Activate();
        }

        void ClickButton_Back(object sender, MouseClickEventArgs e)
        {
            ScreenManager.SetScreen(CharacterSelectionScreen.ScreenName);
        }

        void ClickButton_CreateCharacter(object sender, MouseClickEventArgs e)
        {
            var name = _txtName.Text;

            if (!GameData.CharacterName.IsValid(name))
            {
                SetError("Invalid character name.");
                return;
            }

            _btnCreateCharacter.IsEnabled = false;

            using (var pw = ClientPacket.CreateNewAccountCharacter(name))
            {
                _sockets.Send(pw);
            }
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the 
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            if (_sb == null)
            {
                Debug.Fail("_sb is null.");
                return;
            }

            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            _gui.Draw(_sb);
            _sb.End();
        }

        public override void Initialize()
        {
            _gui = ScreenManager.CreateGUIManager("Font/Menu");

            base.Initialize();

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Create character", "Back");
            _btnCreateCharacter = menuButtons["Create character"];
            _btnCreateCharacter.OnClick += ClickButton_CreateCharacter;
            menuButtons["Back"].OnClick += ClickButton_Back;

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            new Label(cScreen, new Vector2(60, 260)) { Text = "Name:" };
            _txtName = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = "", IsEnabled = true };
        }

        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;
            base.LoadContent();
        }

        void PacketHandler_OnCreateAccountCharacter(IIPSocket sender, bool successful, string errorMessage)
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

        /// <summary>
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            _gui.Update(currentTime);
        }
    }
}