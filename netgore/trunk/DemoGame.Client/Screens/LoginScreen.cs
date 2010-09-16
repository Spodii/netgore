using System.Diagnostics;
using System.Linq;
using Lidgren.Network;
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
        MaskedTextBox _cPasswordText;
        ClientSockets _sockets;

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
            // No reason to be connected while at the login screen
            if (_sockets.IsConnected)
                _sockets.Disconnect();

            SetError(null);

            base.Activate();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in Activate().
        /// </summary>
        public override void Deactivate()
        {
            SetError(null);
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
            _cNameText.TextChanged += cNameText_TextChanged;

            new Label(cScreen, new Vector2(60, 320)) { Text = "Password:" };
            _cPasswordText = new MaskedTextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40))
            { IsMultiLine = false, Text = "qwerty123" };
            _cPasswordText.KeyPressed += cPasswordText_KeyPressed;
            _cPasswordText.TextChanged += cPasswordText_TextChanged;

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Login", "Back");
            _btnLogin = menuButtons["Login"];
            _btnLogin.Clicked += delegate { _sockets.Connect(); };
            menuButtons["Back"].Clicked += delegate { ScreenManager.SetScreen(MainMenuScreen.ScreenName); };

            cScreen.SetFocus();

            // Set up the networking stuff for this screen
            _sockets = ClientSockets.Instance;
            _sockets.StatusChanged += _sockets_StatusChanged;
            _sockets.PacketHandler.ReceivedLoginSuccessful += PacketHandler_ReceivedLoginSuccessful;
            _sockets.PacketHandler.ReceivedLoginUnsuccessful += PacketHandler_ReceivedLoginUnsuccessful;
        }

        void PacketHandler_ReceivedLoginUnsuccessful(ClientPacketHandler sender, IIPSocket conn, string e)
        {
            // Show the login screen
            ScreenManager.ActiveScreen = this;

            // Display the message
            SetError(e);
        }

        void PacketHandler_ReceivedLoginSuccessful(ClientPacketHandler sender, IIPSocket conn)
        {
            // Show the character selection screen
            ScreenManager.ActiveScreen = ScreenManager.GetScreen<CharacterSelectionScreen>();
        }

        /// <summary>
        /// Sets the error message to display.
        /// </summary>
        /// <param name="message">The error message. Use null to clear.</param>
        public void SetError(string message)
        {
            if (_cError == null)
            {
                Debug.Fail("_cError is null.");
                return;
            }

            if (message == null)
                _cError.Text = string.Empty;
            else
                _cError.Text = "Error: " + message;
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(TickCount gameTime)
        {
            var sock = _sockets.RemoteSocket;
            _btnLogin.IsEnabled = (sock == null ||
                                   !(sock.Status == NetConnectionStatus.Connected || sock.Status == NetConnectionStatus.Connecting || sock.Status == NetConnectionStatus.None));

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
                _cPasswordText.CursorLinePosition = _cPasswordText.Text.Length;
            }
        }

        /// <summary>
        /// Handles the <see cref="TextBox.TextChanged"/> event of the <see cref="_cNameText"/> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void cNameText_TextChanged(Control sender)
        {
            var c = sender as TextBox;
            if (c == null)
                return;

            var clp = _cNameText.CursorLinePosition;
            _cNameText.Text = GameData.AccountName.AllowedChars.GetValidCharsOnly(_cNameText.Text);
            _cNameText.CursorLinePosition = clp;
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
                _cNameText.CursorLinePosition = _cNameText.Text.Length;
            }
        }

        /// <summary>
        /// Handles the <see cref="TextBox.TextChanged"/> event of the <see cref="_cPasswordText"/> control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void cPasswordText_TextChanged(Control sender)
        {
            var c = sender as TextBox;
            if (c == null)
                return;

            var clp = _cPasswordText.CursorLinePosition;
            _cPasswordText.Text = GameData.AccountPassword.AllowedChars.GetValidCharsOnly(_cPasswordText.Text);
            _cPasswordText.CursorLinePosition = clp;
        }

        void _sockets_StatusChanged(IClientSocketManager sender, NetConnectionStatus newStatus, string reason)
        {
            switch (newStatus)
            {
                case NetConnectionStatus.Connected:

                    // When the status has changed to Connected, and this screen is active, send the login info
                    if (ScreenManager.ActiveNonConsoleScreen == this)
                    {
                        var name = _cNameText.Text;
                        var pass = _cPasswordText.Text;
                        using (var pw = ClientPacket.Login(name, pass))
                        {
                            _sockets.Send(pw, ClientMessageType.System);
                        }
                    }
                    break;

                case NetConnectionStatus.Disconnected:

                    // If any screen other than this screen or the new account screen is the active screen when we
                    // receive a disconnect, set this screen as active
                    if (!(ScreenManager.ActiveNonConsoleScreen is LoginScreen || ScreenManager.ActiveNonConsoleScreen is NewAccountScreen))
                        ScreenManager.ActiveScreen = this;

                    // If this screen is the active screen, set the error text
                    if (ScreenManager.ActiveNonConsoleScreen == this)
                    {
                        // If we were the ones to disconnect, clear the error
                        if (sender.ClientDisconnected)
                        {
                            SetError(null);
                            break;
                        }

                        // If no reason specified, use generic one
                        if (string.IsNullOrEmpty(reason))
                            reason = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);

                        SetError(reason);
                    }

                    break;
            }
        }
    }
}