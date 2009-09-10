using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    class CharacterSelectionScreen : GameScreen
    {
        public const string ScreenName = "character selection";
        const string _unusedCharacterSlotText = "unused";

        Button[] _characterButtons;
        GameplayScreen _gpScreen = null;
        GUIManager _gui;
        SpriteBatch _sb = null;
        ClientSockets _sockets = null;

        bool _waitingForCharInfos = true;

        public CharacterSelectionScreen() : base(ScreenName)
        {
        }

        public override void Activate()
        {
            _gpScreen = ScreenManager.GetScreen(GameplayScreen.ScreenName) as GameplayScreen;
            if (_gpScreen == null)
                throw new Exception("Failed to find 'game' screen.");

            _sockets = _gpScreen.Socket;
            if (_sockets == null)
                throw new Exception("Failed to reference the ClientSockets.");

            base.Activate();
        }

        void characterButton_OnClick(object sender, MouseClickEventArgs e)
        {
            Button src = (Button)sender;
            byte index = (byte)src.Tag;

            AccountCharacterInfo charInfo;
            if (!_sockets.PacketHandler.AccountCharacterInfos.TryGetInfo(index, out charInfo))
                return;

            using (var pw = ClientPacket.SelectAccountCharacter(index))
            {
                _sockets.Send(pw);
            }
        }

        void cLogOut_OnClick(object sender, MouseClickEventArgs e)
        {
            // TODO: $$ Actually log out (close the socket)
            ScreenManager.SetScreen(LoginScreen.ScreenName);
        }

        public override void Deactivate()
        {
            _gpScreen = null;
            _sockets = null;
            base.Deactivate();
        }

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

        public override void LoadContent()
        {
            _sb = ScreenManager.SpriteBatch;
            base.LoadContent();
        }

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