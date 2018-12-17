using System;
using System.Diagnostics;
using System.Linq;
using DemoGame.Client.Properties;
using Lidgren.Network;
using NetGore;
using NetGore.Audio;
using NetGore.Cryptography;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class LoginScreen : GameMenuScreenBase
    {
        public const string ScreenName = "login";
        const string _title = "Account Login";

        Control _btnLogin;
        TextBox _cNameText;
        MaskedTextBox _cPasswordText;
        CheckBox _cRememberPassword;
        TextBox _cStatus;
        ClientSockets _sockets;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public LoginScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            // No reason to be connected while at the login screen
            if (_sockets.IsConnected)
                _sockets.Disconnect();

            SetError(null);
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in Activate().
        /// </summary>
        public override void Deactivate()
        {
            SaveScreenSettings();

            SetError(null);

            base.Deactivate();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            var decodedPass = MachineCrypto.ValidatedDecode(ClientSettings.Default.UI_EnteredPassword) ?? string.Empty;

            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the login fields
            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 260), "Name:");
            _cNameText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = ClientSettings.Default.UI_EnteredUserName };
            _cNameText.KeyPressed += cNameText_KeyPressed;
            _cNameText.TextChanged += cNameText_TextChanged;

            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 320), "Password:");
            _cPasswordText = new MaskedTextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40)) { IsMultiLine = false, Text = decodedPass };
            _cPasswordText.KeyPressed += cPasswordText_KeyPressed;
            _cPasswordText.TextChanged += cPasswordText_TextChanged;

            var textBoxPos = new Vector2(60, _cPasswordText.Position.Y + _cPasswordText.Size.Y + 20);
            var textBoxSize = new Vector2(cScreen.ClientSize.X - (textBoxPos.X * 2), cScreen.ClientSize.Y - textBoxPos.Y - 60);
            _cStatus = new TextBox(cScreen, textBoxPos, textBoxSize) 
            { 
                ForeColor = Color.Red, 
                Border = null, 
                CanFocus = false, 
                IsMultiLine = true, 
                IsEnabled = false 
            };

            _cRememberPassword = new CheckBox(cScreen, _cPasswordText.Position + new Vector2(50, _cPasswordText.Size.Y + 8))
            {
                Text = "Remember Password",
                Value = ClientSettings.Default.UI_RememberPassword,
                ForeColor = Color.White,
                Font = GameScreenHelper.DefaultChatFont
            };
            _cRememberPassword.ValueChanged += _cRememberPassword_ValueChanged;

            // Display the admin account info
            const string adminAccountInfo = "Use the following account for the default admin account:" + "\n    Username: Spodi" + "\n    Password: qwerty123";
            new Label(cScreen, _cRememberPassword.Position + new Vector2(-32, _cRememberPassword.Size.Y + 32)) 
            { 
                Text = adminAccountInfo, 
                ForeColor = Color.Green, 
                Font = GameScreenHelper.DefaultChatFont 
            };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Login", "Back");
            _btnLogin = menuButtons["Login"];
            _btnLogin.Clicked += _btnLogin_Clicked;
            menuButtons["Back"].Clicked += LoginScreen_Clicked;

            cScreen.SetFocus();

            // Set up the networking stuff for this screen
            _sockets = ClientSockets.Instance;

            _sockets.StatusChanged -= _sockets_StatusChanged;
            _sockets.StatusChanged += _sockets_StatusChanged;

            _sockets.PacketHandler.ReceivedLoginSuccessful -= PacketHandler_ReceivedLoginSuccessful;
            _sockets.PacketHandler.ReceivedLoginSuccessful += PacketHandler_ReceivedLoginSuccessful;

            _sockets.PacketHandler.ReceivedLoginUnsuccessful -= PacketHandler_ReceivedLoginUnsuccessful;
            _sockets.PacketHandler.ReceivedLoginUnsuccessful += PacketHandler_ReceivedLoginUnsuccessful;


        }

        /// <summary>
        /// Handles the Clicked event of the back button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void LoginScreen_Clicked(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen<MainMenuScreen>();
        }

        void PacketHandler_ReceivedLoginSuccessful(ClientPacketHandler sender, ClientPacketHandlerEventArgs e)
        {
            // Show the character selection screen
            ScreenManager.SetScreen<CharacterSelectionScreen>();
        }

        void PacketHandler_ReceivedLoginUnsuccessful(ClientPacketHandler sender, ClientPacketHandlerEventArgs<string> e)
        {
            // Show the login screen
            ScreenManager.ActiveScreen = this;

            // Display the message
            SetError(e.Args);
        }

        /// <summary>
        /// Saves the <see cref="ClientSettings"/> related to this screen.
        /// </summary>
        void SaveScreenSettings()
        {
            ClientSettings.Default.UI_EnteredUserName = _cNameText.Text;

            if (ClientSettings.Default.UI_RememberPassword)
                ClientSettings.Default.UI_EnteredPassword = MachineCrypto.Encode(_cPasswordText.Text);
            else
                ClientSettings.Default.UI_EnteredPassword = string.Empty;

            ClientSettings.Default.Save();
        }

        /// <summary>
        /// Sets the error message to display.
        /// </summary>
        /// <param name="message">The error message. Use null to clear.</param>
        public void SetError(string message)
        {
            if (_cStatus == null)
            {
                Debug.Fail("_cError is null.");
                return;
            }

            if (message == null)
                _cStatus.Text = string.Empty;
            else
                _cStatus.Text = "Error: " + message;
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(TickCount gameTime)
        {
            var sock = _sockets.RemoteSocket;
            _btnLogin.IsEnabled = (sock == null ||
                                   !(sock.Status == NetConnectionStatus.Connected ||
                                     sock.Status == NetConnectionStatus.InitiatedConnect ||
                                     sock.Status == NetConnectionStatus.RespondedConnect ||
                                     sock.Status == NetConnectionStatus.None));

            base.Update(gameTime);
        }

        /// <summary>
        /// Handles the Clicked event of the login button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _btnLogin_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!_sockets.Connect())
                SetError("Already attempting to connect or error occured while creating socket.");
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void _cRememberPassword_ValueChanged(Control sender, EventArgs e)
        {
            ClientSettings.Default.UI_RememberPassword = _cRememberPassword.Value;
            SaveScreenSettings();
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClientSocketManagerStatusChangedEventArgs"/> instance containing the event data.</param>
        void _sockets_StatusChanged(IClientSocketManager sender, ClientSocketManagerStatusChangedEventArgs e)
        {
            switch (e.NewStatus)
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
                    if (
                        !(ScreenManager.ActiveNonConsoleScreen is LoginScreen ||
                          ScreenManager.ActiveNonConsoleScreen is NewAccountScreen))
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
                        var reason = e.Reason;
                        if (string.IsNullOrEmpty(reason))
                            reason = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);

                        SetError(reason);
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the KeyPressed event of the _cNameText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        void cNameText_KeyPressed(object sender, KeyEventArgs e)
        {
            // Tab to the next control
            if (e.Code == Keyboard.Key.Tab)
            {
                _cPasswordText.SetFocus();
                _cPasswordText.CursorLinePosition = _cPasswordText.Text.Length;
            }
        }

        /// <summary>
        /// Handles the <see cref="TextBox.TextChanged"/> event of the <see cref="_cNameText"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void cNameText_TextChanged(Control sender, EventArgs e)
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
            if (e.Code == Keyboard.Key.Tab)
            {
                _cNameText.SetFocus();
                _cNameText.CursorLinePosition = _cNameText.Text.Length;
            }
        }

        /// <summary>
        /// Handles the <see cref="TextBox.TextChanged"/> event of the <see cref="_cPasswordText"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void cPasswordText_TextChanged(Control sender, EventArgs e)
        {
            var c = sender as TextBox;
            if (c == null)
                return;

            var clp = _cPasswordText.CursorLinePosition;
            _cPasswordText.Text = GameData.AccountPassword.AllowedChars.GetValidCharsOnly(_cPasswordText.Text);
            _cPasswordText.CursorLinePosition = clp;
        }
    }
}