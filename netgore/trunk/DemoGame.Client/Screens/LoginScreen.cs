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
    class LoginScreen : GameScreen
    {
        public const string ScreenName = "login";

        Label _cError;
        TextBox _cNameText;
        TextBox _cPasswordText;
        GameplayScreen _gpScreen = null;
        GUIManager _gui;
        SpriteBatch _sb = null;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginScreen"/> class.
        /// </summary>
        public LoginScreen() : base(ScreenName)
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

        void cBack_OnClick(object sender, MouseClickEventArgs e)
        {
            ScreenManager.SetScreen(MainMenuScreen.ScreenName);
        }

        void cLogin_OnClick(object sender, MouseClickEventArgs e)
        {
            _sockets.Connect();
        }

        void cScreen_OnKeyUp(object sender, KeyboardEventArgs e)
        {
            cLogin_OnClick(this, null);
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
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
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

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            _gui = new GUIManager(ScreenManager.Content.Load<SpriteFont>("Font/Menu"));
            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);

            new Label(cScreen, new Vector2(60, 260)) { Text = "Name:" };
            _cNameText = new TextBox(cScreen, new Vector2(220, 260), new Vector2(200, 40)) { IsMultiLine = false, Text = "Spodi" };

            new Label(cScreen, new Vector2(60, 320)) { Text = "Password:" };
            _cPasswordText = new TextBox(cScreen, new Vector2(220, 320), new Vector2(200, 40))
            { IsMultiLine = false, Text = "qwerty123" };

            Button cLogin = new Button(cScreen, new Vector2(60, 380), new Vector2(250, 45)) { Text = "Login" };
            Button cBack = new Button(cScreen, new Vector2(60, 440), new Vector2(250, 45)) { Text = "Back" };

            _cError = new Label(cScreen, new Vector2(60, 500)) { ForeColor = Color.Red };

            cLogin.OnClick += cLogin_OnClick;
            cBack.OnClick += cBack_OnClick;
            cScreen.OnKeyUp += cScreen_OnKeyUp;

            cScreen.SetFocus();
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
        /// Handles updating of the screen. This will only be called while the screen is the active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Update(GameTime gameTime)
        {
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            _gui.Update(currentTime);

            if (_sockets != null)
                _sockets.Heartbeat();
        }
    }
}