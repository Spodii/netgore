using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class MainMenuScreen : GameScreen
    {
        public const string ScreenName = "main menu";

        GUIManager _gui;
        SpriteBatch _sb = null;

        public MainMenuScreen()
            : base(ScreenName)
        {
        }

        void cLogin_OnClick(object sender, MouseClickEventArgs e)
        {
            ScreenManager.SetScreen(LoginScreen.ScreenName);
        }

        void cNewAccount_OnClick(object sender, MouseClickEventArgs e)
        {
            // Not implemented...
        }

        void cOptions_OnClick(object sender, MouseClickEventArgs e)
        {
            // Not implemented...
        }

        void cQuit_OnClick(object sender, MouseClickEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void cScreen_OnKeyUp(object sender, KeyboardEventArgs e)
        {
            cLogin_OnClick(this, null);
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
            Button cLogin = new Button("Login", new Vector2(60, 260), new Vector2(250, 45), cScreen);
            Button cNewAccount = new Button("New Account", new Vector2(60, 320), new Vector2(250, 45), cScreen);
            Button cOptions = new Button("Options", new Vector2(60, 380), new Vector2(250, 45), cScreen);
            Button cQuit = new Button("Quit", new Vector2(60, 440), new Vector2(250, 45), cScreen);

            cLogin.OnClick += cLogin_OnClick;
            cQuit.OnClick += cQuit_OnClick;
            cNewAccount.OnClick += cNewAccount_OnClick;
            cOptions.OnClick += cOptions_OnClick;

            cScreen.OnKeyUp += cScreen_OnKeyUp;
        }

        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            int currentTime = (int)gameTime.TotalRealTime.TotalMilliseconds;

            _gui.Update(currentTime);
        }
    }
}