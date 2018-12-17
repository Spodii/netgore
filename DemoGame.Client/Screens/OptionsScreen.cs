using System.Collections.Generic;
using System.Linq;
using DemoGame.Client.Properties;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class OptionsScreen : GameMenuScreenBase
    {
        public const string ScreenName = "options";
        const string _title = "Options";

        IDictionary<string, Control> _menuButtons;
        TextBox _txtMusic;
        TextBox _txtSound;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public OptionsScreen(IScreenManager screenManager) : base(screenManager, ScreenName, _title)
        {
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            _txtSound.Text = ClientSettings.Default.Audio_SoundVolume.ToString();
            _txtMusic.Text = ClientSettings.Default.Audio_MusicVolume.ToString();
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
            _menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Accept", "Cancel");
            _menuButtons["Accept"].Clicked += accept_Clicked;
            _menuButtons["Cancel"].Clicked += cancel_Clicked;

            // Create the options
            var pos = new Vector2(60, 180);
            var lblSound = GameScreenHelper.CreateMenuLabel(cScreen, pos, "Sound (0 to 100):");
            _txtSound = new TextBox(cScreen, pos + new Vector2(lblSound.Size.X + 10, -6), 
                new Vector2(128, lblSound.ClientSize.Y + 4)) { AllowKeysHandler = TextEventArgsFilters.IsDigitFunc };

            pos.Y += _txtSound.Size.Y + 16;
            var lblMusic = GameScreenHelper.CreateMenuLabel(cScreen, pos, "Music (0 to 100):");
            _txtMusic = new TextBox(cScreen, pos + new Vector2(lblMusic.Size.X + 10, -6), 
                new Vector2(128, lblMusic.ClientSize.Y + 4)) { AllowKeysHandler = TextEventArgsFilters.IsDigitFunc };
        }

        void ShowErrorMsgBox(string property)
        {
            const string title = "Invalid value";
            const string msg = "Invalid value entered for the {0} volume. Please enter a value between 0 and 100.";

            foreach (var c in _menuButtons.Values)
            {
                c.IsEnabled = false;
            }

            var mb = new MessageBox(GUIManager, title, string.Format(msg, property), MessageBoxButton.Ok) { Font = GameScreenHelper.DefaultChatFont, DisposeOnSelection = true };
            mb.OptionSelected += delegate
            {
                foreach (var c in _menuButtons.Values)
                {
                    c.IsEnabled = true;
                }
            };
        }

        /// <summary>
        /// Handles the Clicked event of the accept menu button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void accept_Clicked(object sender, MouseButtonEventArgs e)
        {
            // Validate sound
            byte newSound;
            if (!byte.TryParse(_txtSound.Text, out newSound) || newSound < 0 || newSound > 100)
            {
                ShowErrorMsgBox("sound");
                _txtSound.SetFocus();
                return;
            }

            // Validate music
            byte newMusic;
            if (!byte.TryParse(_txtMusic.Text, out newMusic) || newMusic < 0 || newMusic > 100)
            {
                ShowErrorMsgBox("music");
                _txtMusic.SetFocus();
                return;
            }

            ClientSettings.Default.Audio_SoundVolume = newSound;
            ClientSettings.Default.Audio_MusicVolume = newMusic;
            ClientSettings.Default.Save();

            ScreenManager.SetScreen<MainMenuScreen>();
        }

        /// <summary>
        /// Handles the Clicked event of the cancel menu button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void cancel_Clicked(object sender, MouseButtonEventArgs e)
        {
            ScreenManager.SetScreen<MainMenuScreen>();
        }
    }
}