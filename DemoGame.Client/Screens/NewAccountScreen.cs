using System.Linq;
using Lidgren.Network;
using NetGore;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class NewAccountScreen : GameMenuScreenBase
    {
        public const string ScreenName = "new account";
        const string _title = "New Account";

        TextBox _cEmailText;
        TextBox _cNameText;
        TextBox _cPasswordText;
        TextBox _cStatus;
        Control _createAccountButton;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewAccountScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public NewAccountScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            SetMessage(null);

            // If we are already connected to the server for some reason, disconnect
            if (_sockets.IsConnected)
                _sockets.Disconnect();
        }

        /// <summary>
        /// Handles when the CreateAccount button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void ClickButton_CreateAccount(object sender, MouseButtonEventArgs e)
        {
            // Manually check for invalid values before checking with the server
            if (!GameData.AccountName.IsValid(_cNameText.Text))
            {
                SetError(GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CreateAccountInvalidName));
                return;
            }

            if (!GameData.AccountPassword.IsValid(_cPasswordText.Text))
            {
                SetError(GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CreateAccountInvalidPassword));
                return;
            }

            if (!GameData.AccountEmail.IsValid(_cEmailText.Text))
            {
                SetError(GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.CreateAccountInvalidEmail));
                return;
            }

            // Disconnect if already connected
            if (_sockets.IsConnected)
                _sockets.Disconnect();

            // Start connecting to the server
            SetMessage("Connecting to server...");

            _sockets.Connect();
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public override void Deactivate()
        {
            if (_sockets.IsConnected)
                _sockets.Disconnect();

            SetMessage(null);

            base.Deactivate();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            _sockets.PacketHandler.ReceivedCreateAccount -= PacketHandler_ReceivedCreateAccount;

            base.Dispose();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _sockets = ClientSockets.Instance;

            _sockets.PacketHandler.ReceivedCreateAccount -= PacketHandler_ReceivedCreateAccount;
            _sockets.PacketHandler.ReceivedCreateAccount += PacketHandler_ReceivedCreateAccount;

            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the new account fields. Use a different font since our default TextBox font doesn't support some characters
            // needed for email, and it'd look funny to only change it for the email textbox.
            var textBoxFont = GameScreenHelper.DefaultChatFont;

            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 180), "Name:");
            _cNameText = new TextBox(cScreen, new Vector2(220, 180), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty, Font = textBoxFont };

            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 260), "Password:");
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty, Font = textBoxFont };

            GameScreenHelper.CreateMenuLabel(cScreen, new Vector2(60, 320), "Email:");
            _cEmailText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40)) { IsMultiLine = false, Text = string.Empty, Font = textBoxFont };

            var textBoxPos = new Vector2(60, _cEmailText.Position.Y + _cEmailText.Size.Y + 20);
            var textBoxSize = new Vector2(cScreen.ClientSize.X - (textBoxPos.X * 2), cScreen.ClientSize.Y - textBoxPos.Y - 60);
            _cStatus = new TextBox(cScreen, textBoxPos, textBoxSize) { ForeColor = Color.Red, Border = null, CanFocus = false, IsMultiLine = true, IsEnabled = false };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Create Account", "Back");
            menuButtons["Create Account"].Clicked += ClickButton_CreateAccount;
            menuButtons["Back"].Clicked += delegate { ScreenManager.SetScreen<MainMenuScreen>(); };

            _createAccountButton = menuButtons["Create Account"];

            _sockets.StatusChanged -= _sockets_StatusChanged;
            _sockets.StatusChanged += _sockets_StatusChanged;
        }

        /// <summary>
        /// Handles the ReceiveCreateAccount event from the <see cref="ClientPacketHandler"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CreateAccountEventArgs"/> instance containing the event data.</param>
        void PacketHandler_ReceivedCreateAccount(IIPSocket sender, CreateAccountEventArgs e)
        {
            if (!e.Successful)
            {
                // Unsuccessful - display reason for failure
                var s = "Failed to create account: ";
                if (!string.IsNullOrEmpty(s))
                    s += e.ErrorMessage;
                else
                    s += "Unspecified error returned from server.";

                SetError(s);
            }
            else
            {
                // Successful
                SetMessage("Account successfully created!");
            }

            _sockets.Disconnect();
        }

        /// <summary>
        /// Sets the message text for displaying an error.
        /// </summary>
        /// <param name="errorMsg">The error message to display. Use null to clear.</param>
        void SetError(string errorMsg)
        {
            if (errorMsg == null)
            {
                _cStatus.Text = string.Empty;
                return;
            }

            _cStatus.Text = errorMsg;
            _cStatus.ForeColor = Color.Red;
        }

        /// <summary>
        /// Sets the message text.
        /// </summary>
        /// <param name="message">The message to display. Use null to clear.</param>
        void SetMessage(string message)
        {
            if (message == null)
            {
                _cStatus.Text = string.Empty;
                return;
            }

            _cStatus.Text = message;
            _cStatus.ForeColor = Color.Green;
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(TickCount gameTime)
        {
            base.Update(gameTime);

            if (_sockets.RemoteSocket == null || _sockets.RemoteSocket.Status == NetConnectionStatus.None ||
                _sockets.RemoteSocket.Status == NetConnectionStatus.Disconnected)
                _createAccountButton.IsEnabled = true;
            else
                _createAccountButton.IsEnabled = false;
        }

        /// <summary>
        /// Handles when the status of the <see cref="IClientSocketManager"/> changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClientSocketManagerStatusChangedEventArgs"/> instance containing the event data.</param>
        void _sockets_StatusChanged(IClientSocketManager sender, ClientSocketManagerStatusChangedEventArgs e)
        {
            // Make sure we are the active screen
            if (ScreenManager.ActiveNonConsoleScreen != this)
                return;

            switch (e.NewStatus)
            {
                case NetConnectionStatus.Connected:
                    SetMessage("Connected to server. Sending new account request...");

                    // When the status has changed to Connected, send the account info
                    var name = _cNameText.Text;
                    var pass = _cPasswordText.Text;
                    var email = _cEmailText.Text;
                    using (var pw = ClientPacket.CreateNewAccount(name, pass, email))
                    {
                        _sockets.Send(pw, ClientMessageType.System);
                    }
                    break;

                case NetConnectionStatus.Disconnected:

                    // If we were the ones to disconnect, do not change the display text
                    if (sender.ClientDisconnected)
                        break;

                    // If no reason specified, use generic one
                    var reason = e.Reason;
                    if (string.IsNullOrEmpty(reason))
                        reason = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.DisconnectNoReasonSpecified);

                    SetError(reason);

                    break;
            }
        }
    }
}