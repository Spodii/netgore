using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
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
        GameplayScreen _gpScreen = null;
        GUIManager _gui;
        SpriteBatch _sb = null;
        ClientSockets _sockets = null;

        bool _waitingForCharInfos = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSelectionScreen"/> class.
        /// </summary>
        public CharacterSelectionScreen() : base(ScreenName)
        {
            PlayMusic = false;
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on Deactivate().
        /// </summary>
        public override void Activate()
        {
            if (_gpScreen == null)
            {
                _gpScreen = ScreenManager.GetScreen(GameplayScreen.ScreenName) as GameplayScreen;
                if (_gpScreen == null)
                    throw new Exception("Failed to find 'game' screen.");
            }

            if (_sockets == null)
            {
                _sockets = _gpScreen.Socket;
                if (_sockets == null)
                    throw new Exception("Failed to reference the ClientSockets.");
            }

            base.Activate();
        }

        void characterButton_OnClick(object sender, MouseClickEventArgs e)
        {
            Button src = (Button)sender;
            byte index = (byte)src.Tag;

            AccountCharacterInfo charInfo;
            if (!_sockets.PacketHandler.AccountCharacterInfos.TryGetInfo(index, out charInfo))
                return;

            using (PacketWriter pw = ClientPacket.SelectAccountCharacter(index))
            {
                _sockets.Send(pw);
            }
        }

        void cLogOut_OnClick(object sender, MouseClickEventArgs e)
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

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime</param>
        public override void Draw(GameTime gameTime)
        {
            if (_sb == null)
            {
                Debug.Fail("_sb is null.");
                return;
            }

            _sb.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            _gui.Draw(_sb);
            _sb.End();
        }

        void HandleCharInfosUpdated()
        {
            for (int i = 0; i < _characterButtons.Length; i++)
            {
                Button c = _characterButtons[i];
                AccountCharacterInfo charInfo;
                if (_sockets.PacketHandler.AccountCharacterInfos.TryGetInfo((byte)i, out charInfo))
                    c.Text = charInfo.Name;
                else
                    c.Text = _unusedCharacterSlotText;
            }
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public override void Initialize()
        {
            _gui = new GUIManager(ScreenManager.Content.Load<SpriteFont>("Font/Menu"));

            Panel cScreen = new Panel(_gui, Vector2.Zero, ScreenManager.ScreenSize);

            Button cLogOut = new Button("Log out", new Vector2(60, 440), new Vector2(250, 45), cScreen);
            cLogOut.OnClick += cLogOut_OnClick;

            // Create the character controls
            _characterButtons = new Button[GameData.MaxCharactersPerAccount];
            for (int i = 0; i < GameData.MaxCharactersPerAccount; i++)
            {
                Vector2 size = new Vector2(250, 35);
                Vector2 pos = new Vector2((ScreenManager.ScreenWidth / 2f) - (size.X / 2), 10 + (i * (size.Y + 5)));
                Button characterButton = new Button(_unusedCharacterSlotText, pos, size, cScreen) { Tag = (byte)i };
                characterButton.OnClick += characterButton_OnClick;
                _characterButtons[i] = characterButton;
            }

            base.Initialize();
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

            if (_waitingForCharInfos && _sockets != null)
            {
                if (_sockets.PacketHandler.AccountCharacterInfos.IsLoaded)
                {
                    HandleCharInfosUpdated();
                    _waitingForCharInfos = false;
                }
            }

            _gui.Update(currentTime);

            if (_sockets != null)
                _sockets.Heartbeat();
        }
    }
}