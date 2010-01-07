using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    class NewAccountScreen : GameScreen
    {
        public const string ScreenName = "new account";

        TextBox _cNameText;
        TextBox _cPasswordText;
        TextBox _cEmailText;
        Label _errorLabel;
        IGUIManager _gui;
        Button _createAccountButton;
        SpriteBatch _sb = null;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewAccountScreen"/> class.
        /// </summary>
        public NewAccountScreen() : base(ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public override void Activate()
        {
            if (_sockets == null)
            {
                _sockets = ((GameplayScreen)ScreenManager.GetScreen(GameplayScreen.ScreenName)).Socket;
                if (_sockets == null)
                    throw new Exception("Failed to reference the ClientSockets.");
            }

            _sockets.OnConnect += sockets_OnConnect;
            _sockets.OnDisconnect += _sockets_OnDisconnect;
            _sockets.OnFailedConnect += sockets_OnFailedConnect;
            _sockets.PacketHandler.OnCreateAccount += PacketHandler_OnCreateAccount;

            _createAccountButton.IsEnabled = true;
            _errorLabel.IsVisible = false;

            base.Activate();
        }

        void _sockets_OnDisconnect(IIPSocket conn)
        {
            _createAccountButton.IsEnabled = true;
        }

        void PacketHandler_OnCreateAccount(IIPSocket conn, bool successful, string errorMessage)
        {
            if (!successful)
            {
                string s = "Failed to create account: ";
                if (!string.IsNullOrEmpty(s))
                    s += errorMessage;
                else
                    s += "Unspecified error returned from server.";

                ShowError(s);
            }
            else
            {
                ShowMessage("Account successfully created!");
            }

            _sockets.Disconnect();
        }

        void ClickButton_CreateAccount(object sender, MouseClickEventArgs e)
        {
            if (!GameData.AccountName.IsValid(_cNameText.Text))
            {
                ShowError("Invalid account name.");
                return;
            }

            if (!GameData.AccountPassword.IsValid(_cPasswordText.Text))
            {
                ShowError("Invalid account password.");
                return;
            }

            if (!GameData.AccountEmail.IsValid(_cEmailText.Text))
            {
                ShowError("Invalid email address.");
                return;
            }

            if (_sockets.IsConnected)
                _sockets.Disconnect();

            ShowMessage("Connecting to server...");

            _sockets.Connect();

            _createAccountButton.IsEnabled = false;
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            Debug.Assert(_sb != null, "_sb is null.");
            if (_sb == null)
                return;

            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            _gui.Draw(_sb);
            _sb.End();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public override void Deactivate()
        {
            if (_sockets.IsConnected)
                _sockets.Disconnect();

            base.Deactivate();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            _gui = ScreenManager.CreateGUIManager("Font/Menu");

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);

            _errorLabel = new Label(cScreen, new Vector2(410, 80)) { IsVisible = false };

            // Create the new account fields
            new Label(cScreen, new Vector2(60, 180)) { Text = "Name:" };
            _cNameText = new TextBox(cScreen, new Vector2(220, 180), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty };

            new Label(cScreen, new Vector2(60, 260)) { Text = "Password:" };
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty };

            new Label(cScreen, new Vector2(60, 320)) { Text = "Email:" };
            _cEmailText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Create Account", "Back");
            menuButtons["Create Account"].OnClick += ClickButton_CreateAccount;
            menuButtons["Back"].OnClick += delegate { ScreenManager.SetScreen(MainMenuScreen.ScreenName); };

            _createAccountButton = menuButtons["Create Account"];
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;

            base.LoadContent();
        }

        void ShowMessage(string message)
        {
            ShowError(message);
            _errorLabel.ForeColor = Color.Green;
        }

        void ShowError(string errorMsg)
        {
            _errorLabel.Text = errorMsg;
            _errorLabel.IsVisible = true;
            _errorLabel.ForeColor = Color.Red;
        }

        void sockets_OnConnect(IIPSocket conn)
        {
            ShowMessage("Connected to server. Sending new account request...");

            using (var pw = ClientPacket.CreateNewAccount(_cNameText.Text, _cPasswordText.Text, _cEmailText.Text))
                _sockets.Send(pw);
        }

        void sockets_OnFailedConnect(IIPSocket conn)
        {
            ShowError("Failed to connect to the server.");
        }

        /// <summary>
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Update(GameTime gameTime)
        {
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            _gui.Update(currentTime);
        }
    }
}