using System.Linq;
using NetGore.Audio;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    class MainMenuScreen : GameMenuScreenBase
    {
        public const string ScreenName = "main menu";
        private static string _title = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.GameTitle);

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public MainMenuScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
            PlayMusic = true;
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Login", "New Account", "Options", "Quit");

            menuButtons["Login"].Clicked += delegate { ScreenManager.SetScreen<LoginScreen>(); };
            menuButtons["New Account"].Clicked += delegate { ScreenManager.SetScreen<NewAccountScreen>(); };
            menuButtons["Options"].Clicked += delegate { ScreenManager.SetScreen<OptionsScreen>(); };
            menuButtons["Quit"].Clicked += delegate { ScreenManager.Game.Dispose(); };

        }

      


    }
}