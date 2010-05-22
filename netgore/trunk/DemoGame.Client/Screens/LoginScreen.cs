using System;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

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

            _sockets.Connected += sockets_Connected;
            _sockets.ConnectFailed += sockets_ConnectFailed;
            _sockets.PacketHandler.ReceivedLoginUnsuccessful += sockets_ReceivedLoginUnsuccessful;
            _sockets.PacketHandler.ReceivedLoginSuccessful += sockets_ReceivedLoginSuccessful;

            base.Activate();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in Activate().
        /// </summary>
        public override void Deactivate()
        {
            _sockets.Connected -= sockets_Connected;
            _sockets.ConnectFailed -= sockets_ConnectFailed;
            _sockets.PacketHandler.ReceivedLoginSuccessful -= sockets_ReceivedLoginSuccessful;
            _sockets.PacketHandler.ReceivedLoginUnsuccessful -= sockets_ReceivedLoginUnsuccessful;

            _cError.Text = string.Empty;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the login fields
            new Label(cScreen, new Vector2(60, 260)) { Text = "Name:" };
            _cNameText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = "Spodi" };
            _cNameText.KeyPressed += cNameText_KeyPressed;

            new Label(cScreen, new Vector2(60, 320)) { Text = "Password:" };
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40))
            { IsMultiLine = false, Text = "qwerty123" };
            _cPasswordText.KeyPressed += cPasswordText_KeyPressed;

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Login", "Back");
            _btnLogin = menuButtons["Login"];
            _btnLogin.Clicked += delegate { _sockets.Connect(); };
            menuButtons["Back"].Clicked += delegate { ScreenManager.SetScreen(MainMenuScreen.ScreenName); };

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

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(TickCount gameTime)
        {
            _btnLogin.IsEnabled = !(_sockets.IsConnecting || _sockets.IsConnected);
            base.Update(gameTime);
        }

        /// <summary>
        /// Handles the KeyPressed event of the _cNameText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        void cNameText_KeyPressed(object sender, KeyEventArgs e)
        {
            // Tab to the next control
            if (e.Code == KeyCode.Tab)
            {
                _cPasswordText.SetFocus();
                _cPasswordText.Text = _cPasswordText.Text.Trim();
                _cPasswordText.CursorLinePosition = _cPasswordText.Text.Length;
            }
        }

        /// <summary>
        /// Handles the KeyPressed event of the _cPasswordText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        void cPasswordText_KeyPressed(object sender, KeyEventArgs e)
        {
            // Tab to the next control
            if (e.Code == KeyCode.Tab)
            {
                _cNameText.SetFocus();
                _cNameText.Text = _cNameText.Text.Trim();
                _cNameText.CursorLinePosition = _cNameText.Text.Length;
            }
        }

        void sockets_ConnectFailed(SocketManager sender)
        {
            SetError("Failed to connect to server.");
        }

        void sockets_Connected(SocketManager sender, IIPSocket conn)
        {
            using (var pw = ClientPacket.Login(_cNameText.Text, _cPasswordText.Text))
            {
                _sockets.Send(pw);
            }
        }

        void sockets_ReceivedLoginSuccessful(ClientPacketHandler sender, IIPSocket conn)
        {
            ScreenManager.SetScreen(CharacterSelectionScreen.ScreenName);
        }

        void sockets_ReceivedLoginUnsuccessful(ClientPacketHandler sender, IIPSocket conn, string message)
        {
            SetError(message);
        }
    }
}