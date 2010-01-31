using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using NetGore.Graphics.GUI;
using NetGore.Network;

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
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            if (_sockets == null)
                _sockets = ClientSockets.Instance;

            _sockets.PacketHandler.AccountCharacterInfos.AccountCharactersLoaded += HandleCharInfosUpdated;

            base.Activate();
        }

        void ClickButton_CharacterSelection(object sender, MouseClickEventArgs e)
        {
            Button src = (Button)sender;
            byte index = (byte)src.Tag;

            AccountCharacterInfo charInfo;
            if (!_sockets.PacketHandler.AccountCharacterInfos.TryGetInfo(index, out charInfo))
                ScreenManager.SetScreen(CreateCharacterScreen.ScreenName);
            else
            {
                using (PacketWriter pw = ClientPacket.SelectAccountCharacter(index))
                {
                    _sockets.Send(pw);
                }
            }
        }

        void ClickButton_LogOut(object sender, MouseClickEventArgs e)
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
            for (int i = 0; i < _characterButtons.Length; i++)
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
            Panel cScreen = new Panel(GUIManager, Vector2.Zero, ScreenManager.ScreenSize);

            // Create the menu buttons
            var menuButtons = GameScreenHelper.CreateMenuButtons(cScreen, "Log out");
            menuButtons["Log out"].Clicked += ClickButton_LogOut;

            // Create the character controls
            _characterButtons = new Button[GameData.MaxCharactersPerAccount];
            for (int i = 0; i < GameData.MaxCharactersPerAccount; i++)
            {
                Vector2 size = new Vector2(250, 35);
                Vector2 pos = new Vector2((ScreenManager.ScreenSize.X / 2f) - (size.X / 2), 10 + (i * (size.Y + 10)));
                Button characterButton = new Button(cScreen, pos, size) { Text = _unusedCharacterSlotText, Tag = (byte)i };
                characterButton.Clicked += ClickButton_CharacterSelection;
                _characterButtons[i] = characterButton;
            }

            base.Initialize();
        }
    }
}