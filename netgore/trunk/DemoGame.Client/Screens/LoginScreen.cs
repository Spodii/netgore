using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics.GUI;
using NetGore.Network;

namespace DemoGame.Client
{
    class LoginScreen : GameScreen
    {
        public const string ScreenName = "login";

        Button _btnLogin;
        Label _cError;
        TextBox _cNameText;
        TextBox _cPasswordText;
        GameplayScreen _gpScreen = null;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public LoginScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            if (_gpScreen == null)
            {
                _gpScreen = ScreenManager.GetScreen(GameplayScreen.ScreenName) as GameplayScreen;
                if (_gpScreen == null)
                    throw new Exception("Failed to find 'game' screen.");
            }

            if (_sockets == null)
            {
                _sockets = _gpScreen.Socket;
                if (_sockets == null)
                    throw new Exception("Failed to reference the ClientSockets.");
            }

            _sockets.OnConnect += sockets_OnConnect;
            _sockets.OnFailedConnect += sockets_OnFailedConnect;
            _sockets.PacketHandler.OnLoginUnsuccessful += sockets_OnLoginUnsuccessful;
            _sockets.PacketHandler.OnLoginSuccessful += sockets_OnLoginSuccessful;

            base.Activate();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in Activate().
        /// </summary>
        public override void Deactivate()
        {
            _sockets.OnConnect -= sockets_OnConnect;
            _sockets.OnFailedConnect -= sockets_OnFailedConnect;
            _sockets.PacketHandler.OnLoginSuccessful -= sockets_OnLoginSuccessful;
            _sockets.PacketHandler.OnLoginUnsuccessful -= sockets_OnLoginUnsuccessful;

            _cError.Text = string.Empty;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            Panel cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the login fields
            new Label(cScreen, new Vector2(60, 260)) { Text = "Name:" };
            _cNameText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = "Spodi" };

            new Label(cScreen, new Vector2(60, 320)) { Text = "Password:" };
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40))
            { IsMultiLine = false, Text = "qwerty123" };

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Login", "Back");
            _btnLogin = menuButtons["Login"];
            _btnLogin.OnClick += delegate { _sockets.Connect(); };
            menuButtons["Back"].OnClick += delegate { ScreenManager.SetScreen(MainMenuScreen.ScreenName); };

            cScreen.SetFocus();
        }

        /// <summary>
        /// Sets the error message to display
        /// </summary>
        /// <param name="message">Error message</param>
        public void SetError(string message)
        {
            Debug.Assert(_cError != null, "_cError is null.");
            if (_cError == null)
                return;

            _cError.Text = string.Format("Error: {0}", message);
        }

        void sockets_OnConnect(IIPSocket conn)
        {
            using (PacketWriter pw = ClientPacket.Login(_cNameText.Text, _cPasswordText.Text))
            {
                _sockets.Send(pw);
            }
        }

        void sockets_OnFailedConnect(IIPSocket conn)
        {
            SetError("Failed to connect to server.");
        }

        void sockets_OnLoginSuccessful(IIPSocket conn)
        {
            ScreenManager.SetScreen(CharacterSelectionScreen.ScreenName);
        }

        void sockets_OnLoginUnsuccessful(IIPSocket conn, string message)
        {
            SetError(message);
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(int gameTime)
        {
            _btnLogin.IsEnabled = !(_sockets.IsConnecting || _sockets.IsConnected);
            base.Update(gameTime);
        }
    }
}