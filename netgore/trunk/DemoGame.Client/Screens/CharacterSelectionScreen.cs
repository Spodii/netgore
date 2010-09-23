using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Graphics.GUI;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    class CharacterSelectionScreen : GameScreen
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public const string ScreenName = "character selection";
        const string _unusedCharacterSlotText = "unused";

        Button[] _characterButtons;
        ClientSockets _sockets = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSelectionScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        public CharacterSelectionScreen(IScreenManager screenManager) : base(screenManager, ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Gets the <see cref="Font"/> to use as the default font for the <see cref="IGUIManager"/> for this
        /// <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for this screen.</param>
        /// <returns>The <see cref="Font"/> to use for this <see cref="GameScreen"/>. If null, the
        /// <see cref="IScreenManager.DefaultFont"/> for this <see cref="GameScreen"/> will be used instead.</returns>
        protected override Font GetScreenManagerFont(IScreenManager screenManager)
        {
            return GameScreenHelper.GetScreenDefaultFont(screenManager);
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            if (_sockets == null)
                _sockets = ClientSockets.Instance;

            _sockets.PacketHandler.AccountCharacterInfos.AccountCharactersLoaded += HandleCharInfosUpdated;
        }

        void ClickButton_CharacterSelection(object sender, MouseButtonEventArgs e)
        {
            var src = (Button)sender;
            var index = (byte)src.Tag;

            AccountCharacterInfo charInfo;
            if (!_sockets.PacketHandler.AccountCharacterInfos.TryGetInfo(index, out charInfo))
                ScreenManager.SetScreen(CreateCharacterScreen.ScreenName);
            else
            {
                using (var pw = ClientPacket.SelectAccountCharacter(index))
                {
                    _sockets.Send(pw, ClientMessageType.System);
                }
            }
        }

        void ClickButton_LogOut(object sender, MouseButtonEventArgs e)
        {
            // Change screens
            ScreenManager.SetScreen(LoginScreen.ScreenName);

            // Disconnect the socket so we actually "log out"
            if (_sockets != null)
            {
                try
                {
                    _sockets.Disconnect();
                }
                catch (Exception ex)
                {
                    // Ignore errors in disconnecting
                    Debug.Fail("Disconnect failed: " + ex);
                    if (log.IsErrorEnabled)
                        log.ErrorFormat("Failed to disconnect client socket ({0}). Exception: {1}", _sockets, ex);
                }
            }
        }

        void HandleCharInfosUpdated(AccountCharacterInfos sender)
        {
            for (var i = 0; i < _characterButtons.Length; i++)
            {
                AccountCharacterInfo charInfo;
                _sockets.PacketHandler.AccountCharacterInfos.TryGetInfo((byte)i, out charInfo);
                _characterButtons[i].Text = charInfo != null ? charInfo.Name : _unusedCharacterSlotText;
            }
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            var cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(ScreenManager, cScreen, "Log out");
            menuButtons["Log out"].Clicked += ClickButton_LogOut;

            // Create the character controls
            _characterButtons = new Button[GameData.MaxCharactersPerAccount];
            for (var i = 0; i < GameData.MaxCharactersPerAccount; i++)
            {
                var size = new Vector2(250, 35);
                var pos = new Vector2((ScreenManager.ScreenSize.X / 2f) - (size.X / 2), 10 + (i * (size.Y + 10)));
                var characterButton = new Button(cScreen, pos, size) { Text = _unusedCharacterSlotText, Tag = (byte)i };
                characterButton.Clicked += ClickButton_CharacterSelection;
                _characterButtons[i] = characterButton;
            }

            base.Initialize();
        }
    }
}