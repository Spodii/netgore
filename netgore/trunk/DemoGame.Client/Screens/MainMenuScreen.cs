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

        GUIManagerBase _gui;
        SpriteBatch _sb = null;

        public MainMenuScreen() : base(ScreenName)
        {
            PlayMusic = false;
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
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            _gui = ScreenManager.CreateGUIManager("Font/Menu");

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);
            Button cLogin = new Button(cScreen, new Vector2(60, 260), new Vector2(250, 45)) { Text = "Login" };
            Button cNewAccount = new Button(cScreen, new Vector2(60, 320), new Vector2(250, 45)) { Text = "New Account" };
            Button cOptions = new Button(cScreen, new Vector2(60, 380), new Vector2(250, 45)) { Text = "Options" };
            Button cQuit = new Button(cScreen, new Vector2(60, 440), new Vector2(250, 45)) { Text = "Quit" };

            cLogin.OnClick += cLogin_OnClick;
            cQuit.OnClick += cQuit_OnClick;
            cNewAccount.OnClick += cNewAccount_OnClick;
            cOptions.OnClick += cOptions_OnClick;

            cScreen.OnKeyUp += cScreen_OnKeyUp;
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;
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