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

        IGUIManager _gui;
        SpriteBatch _sb = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public MainMenuScreen(IScreenManager screenManager)
            : base(screenManager, ScreenName)
        {
            PlayMusic = false;
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

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Login", "New Account", "Options", "Quit");
            menuButtons["Login"].OnClick += delegate { ScreenManager.SetScreen(LoginScreen.ScreenName); };
            menuButtons["New Account"].OnClick += delegate { ScreenManager.SetScreen(NewAccountScreen.ScreenName); };
            menuButtons["Options"].OnClick += delegate
                                              {
                                                  /* Not implemented */
                                              };
            menuButtons["Quit"].OnClick += delegate { ScreenManager.Game.Exit(); };
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