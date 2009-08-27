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
        Label _cError;
        TextBoxSingleLine _cNameText;
        TextBoxSingleLine _cPasswordText;
        GameplayScreen _gpScreen = null;
        GUIManager _gui;
        SpriteBatch _sb = null;
        ClientSockets _sockets = null;

        public LoginScreen(string name) : base(name)
        {
        }

        public override void Activate()
        {
            _gpScreen = ScreenManager.GetScreen("game") as GameplayScreen;
            if (_gpScreen == null)
                throw new Exception("Failed to find 'game' screen.");

            _sockets = _gpScreen.Socket;
            if (_sockets == null)
                throw new Exception("Failed to reference the ClientSockets.");

            _sockets.OnConnect += sockets_OnConnect;
            _sockets.OnFailedConnect += sockets_OnFailedConnect;
            _sockets.PacketHandler.OnLoginUnsuccessful += sockets_OnLoginUnsuccessful;
            _sockets.PacketHandler.OnLoginSuccessful += sockets_OnLoginSuccessful;
        }

        void cBack_OnClick(object sender, MouseClickEventArgs e)
        {
            ScreenManager.SetScreen("main menu");
        }

        void cLogin_OnClick(object sender, MouseClickEventArgs e)
        {
            _sockets.Connect();
        }

        void cScreen_OnKeyUp(object sender, KeyboardEventArgs e)
        {
            cLogin_OnClick(this, null);
        }

        public override void Deactivate()
        {
            if (_sockets != null)
            {
                _sockets.OnConnect -= sockets_OnConnect;
                _sockets.OnFailedConnect -= sockets_OnFailedConnect;
                _sockets.PacketHandler.OnLoginSuccessful -= sockets_OnLoginSuccessful;
                _sockets.PacketHandler.OnLoginUnsuccessful -= sockets_OnLoginUnsuccessful;
            }

            _gpScreen = null;
            _sockets = null;
            _cError.Text = string.Empty;
        }

        public override void Draw(GameTime gameTime)
        {
            Debug.Assert(_sb != null, "_sb is null.");
            if (_sb == null)
                return;

            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            _gui.Draw(_sb);
            _sb.End();
        }

        public override void Initialize()
        {
            _gui = new GUIManager(ScreenManager.Content.Load<SpriteFont>("Font/Menu"));
            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);

            new Label("Name:", new Vector2(60, 260), cScreen);
            _cNameText = new TextBoxSingleLine("Spodi", new Vector2(220, 260), new Vector2(200, 40), cScreen);

            new Label("Password:", new Vector2(60, 320), cScreen);
            _cPasswordText = new TextBoxSingleLine("asdf", new Vector2(220, 320), new Vector2(200, 40), cScreen);

            Button cLogin = new Button("Login", new Vector2(60, 380), new Vector2(250, 45), cScreen);
            Button cBack = new Button("Back", new Vector2(60, 440), new Vector2(250, 45), cScreen);

            _cError = new Label(string.Empty, new Vector2(60, 500), cScreen)
            {
                ForeColor = Color.Red
            };

            cLogin.OnClick += cLogin_OnClick;
            cBack.OnClick += cBack_OnClick;
            cScreen.OnKeyUp += cScreen_OnKeyUp;

            cScreen.SetFocus();
        }

        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;
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
            ScreenManager.SetScreen("game");
        }

        void sockets_OnLoginUnsuccessful(IIPSocket conn, string message)
        {
            SetError(message);
        }

        public override void Update(GameTime gameTime)
        {
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            _gui.Update(currentTime);

            if (_sockets != null)
                _sockets.Heartbeat();
        }
    }
}