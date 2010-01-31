using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class MainMenuScreen : GameScreen
    {
        public const string ScreenName = "main menu";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public MainMenuScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            Panel cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Login", "New Account", "Options", "Quit");
            menuButtons["Login"].Clicked += delegate { ScreenManager.SetScreen(LoginScreen.ScreenName); };
            menuButtons["New Account"].Clicked += delegate { ScreenManager.SetScreen(NewAccountScreen.ScreenName); };
            menuButtons["Options"].Clicked += delegate
                                              {
                                                  /* Not implemented */
                                              };
            menuButtons["Quit"].Clicked += delegate { ScreenManager.Game.Exit(); };
        }
    }
}