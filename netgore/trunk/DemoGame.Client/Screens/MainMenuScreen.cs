using System.Linq;
using NetGore.Graphics.GUI;
using SFML.Graphics;

namespace DemoGame.Client
{
    class OptionsScreen : GameMenuScreenBase
    {
        public const string ScreenName = "options";
        const string _title = "Options";

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public OptionsScreen(IScreenManager screenManager)
            : base(screenManager, ScreenName, _title)
        {
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

            menuButtons["Accept"].Clicked += accept_Clicked;
            menuButtons["Cancel"].Clicked += cancel_Clicked;
        }

        void cancel_Clicked(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen<MainMenuScreen>();
        }

        void accept_Clicked(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            // TODO: !!

            ScreenManager.SetScreen<MainMenuScreen>();
        }
    }

    class MainMenuScreen : GameMenuScreenBase
    {
        public const string ScreenName = "main menu";
        const string _title = "NetGore";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public MainMenuScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
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

            menuButtons["Login"].Clicked += delegate { ScreenManager.SetScreen < LoginScreen>(); };
            menuButtons["New Account"].Clicked += delegate { ScreenManager.SetScreen<NewAccountScreen>(); };
            menuButtons["Options"].Clicked += delegate
            {
                // TODO: Add options
            };
            menuButtons["Quit"].Clicked += delegate { ScreenManager.Game.Dispose(); };
        }
    }
}