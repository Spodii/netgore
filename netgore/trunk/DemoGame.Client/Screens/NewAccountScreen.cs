using System;
using System.Linq;
using Lidgren.Network;
using NetGore.Graphics.GUI;
using NetGore.Network;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class NewAccountScreen : GameScreen
    {
        public const string ScreenName = "new account";

        TextBox _cEmailText;
        TextBox _cNameText;
        TextBox _cPasswordText;
        Button _createAccountButton;
        Label _errorLabel;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewAccountScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public NewAccountScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
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

            SetSocketHooks(true);

            _createAccountButton.IsEnabled = true;
            _errorLabel.IsVisible = false;

            base.Activate();
        }

        void ClickButton_CreateAccount(object sender, MouseButtonEventArgs e)
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
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public override void Deactivate()
        {
            SetSocketHooks(false);

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
            _sockets = ClientSockets.Instance;

            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            _errorLabel = new Label(cScreen, new Vector2(410, 80)) { IsVisible = false };

            // Create the new account fields
            new Label(cScreen, new Vector2(60, 180)) { Text = "Name:" };
            _cNameText = new TextBox(cScreen, new Vector2(220, 180), new Vector2(200, 40))
            { IsMultiLine = false, Text = string.Empty };

            new Label(cScreen, new Vector2(60, 260)) { Text = "Password:" };
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40))
            { IsMultiLine = false, Text = string.Empty };

            new Label(cScreen, new Vector2(60, 320)) { Text = "Email:" };
            _cEmailText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40))
            { IsMultiLine = false, Text = string.Empty };

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Create Account", "Back");
            menuButtons["Create Account"].Clicked += ClickButton_CreateAccount;
            menuButtons["Back"].Clicked += delegate { ScreenManager.SetScreen(MainMenuScreen.ScreenName); };

            _createAccountButton = menuButtons["Create Account"];

            _sockets.StatusChanged += _sockets_StatusChanged;
        }

        void PacketHandler_ReceivedCreateAccount(IIPSocket conn, bool successful, string errorMessage)
        {
            if (!successful)
            {
                var s = "Failed to create account: ";
                if (!string.IsNullOrEmpty(s))
                    s += errorMessage;
                else
                    s += "Unspecified error returned from server.";

                ShowError(s);
            }
            else
                ShowMessage("Account successfully created!");

            _sockets.Disconnect();
        }

        void SetSocketHooks(bool add)
        {
            if (add)
            {
                _sockets.PacketHandler.ReceivedCreateAccount += PacketHandler_ReceivedCreateAccount;
            }
            else
            {
                _sockets.PacketHandler.ReceivedCreateAccount -= PacketHandler_ReceivedCreateAccount;
            }
        }

        void ShowError(string errorMsg)
        {
            _errorLabel.Text = errorMsg;
            _errorLabel.IsVisible = true;
            _errorLabel.ForeColor = Color.Red;
        }

        void ShowMessage(string message)
        {
            ShowError(message);
            _errorLabel.ForeColor = Color.Green;
        }

        void _sockets_StatusChanged(IClientSocketManager sender, NetConnectionStatus newStatus, string reason)
        {
            // Make sure we are the active screen
            if (ScreenManager.ActiveNonConsoleScreen != this)
                return;

            switch (newStatus)
            {
                case NetConnectionStatus.Connected:
                    ShowMessage("Connected to server. Sending new account request...");

                    // When the status has changed to Connected, send the account info
                    var name = _cNameText.Text;
                    var pass = _cPasswordText.Text;
                    var email = _cEmailText.Text;
                    using (var pw = ClientPacket.CreateNewAccount(name, pass, email))
                    {
                        _sockets.Send(pw);
                    }
                    break;

                case NetConnectionStatus.Disconnected:
                    string s = "Failed to connect to the server";
                    if (!string.IsNullOrEmpty(reason))
                        s += ": " + reason;
                    ShowError(s);
                    break;
            }
        }
    }
}