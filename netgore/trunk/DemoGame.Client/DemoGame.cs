using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;

namespace DemoGame.Client
{
    /// <summary>
    /// Root object for the Client
    /// </summary>
    public class DemoGame : GameBase
    {
        readonly ScreenManager _screenManager;
        readonly ClientSockets _sockets;

        IEnumerable<TextureAtlas> _globalAtlases;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoGame"/> class.
        /// </summary>
        /// <param name="p">The <see cref="IntPtr"/> to the handle to display the game on
        /// (usually a <see cref="System.Windows.Forms.Control"/>).</param>
        public DemoGame(IntPtr p) : base(p, new Point((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y),
            new Point((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y))
        {
            EngineSettingsInitializer.Initialize();

            // Create the screen manager
            _screenManager = new ScreenManager(this, new SkinManager("Default"), "Font/Arial", 24);

            // Initialize the socket manager
            ClientSockets.Initialize(ScreenManager);
            _sockets = ClientSockets.Instance;

            // Read the GrhInfo
            LoadGrhInfo();
            _screenManager.DrawingManager.LightManager.DefaultSprite = new Grh(GrhInfo.GetData("Effect", "light"));

            // Set up our custom chat bubbles
            ChatBubble.CreateChatBubbleInstance = CreateChatBubbleInstanceHandler;

            // Create the screens
            new GameplayScreen(ScreenManager);
            new MainMenuScreen(ScreenManager);
            new LoginScreen(ScreenManager);
            new CharacterSelectionScreen(ScreenManager);
            new CreateCharacterScreen(ScreenManager);
            new NewAccountScreen(ScreenManager);

            ScreenManager.ConsoleScreen = new ConsoleScreen(ScreenManager);
            ScreenManager.ActiveScreen = ScreenManager.GetScreen<MainMenuScreen>();

            // NOTE: Temporary volume reduction
            ScreenManager.AudioManager.SoundManager.Volume = 70f;
            ScreenManager.AudioManager.MusicManager.Volume = 20f;

            ShowMouseCursor = true;
            UseVerticalSync = true;

            KeyPressed += DemoGame_KeyPressed;
        }

        /// <summary>
        /// Gets the <see cref="IScreenManager"/> instance used to display the screens for the client.
        /// </summary>
        public IScreenManager ScreenManager
        {
            get { return _screenManager; }
        }

        /// <summary>
        /// Gets the <see cref="IClientSocketManager"/> instance used to let the client communicate with the server.
        /// </summary>
        public IClientSocketManager Sockets
        {
            get { return _sockets; }
        }

        /// <summary>
        /// Decides the <see cref="ContentLevel"/> to use for <see cref="GrhData"/>s.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to get the <see cref="ContentLevel"/> for.</param>
        /// <returns>The <see cref="ContentLevel"/> for the <paramref name="grhData"/>.</returns>
        static ContentLevel ContentLevelDecider(GrhData grhData)
        {
            const StringComparison comp = StringComparison.OrdinalIgnoreCase;

            var cat = grhData.Categorization.Category.ToString();

            // For stuff that will be put into a global atlas, use the temporary level
            if (cat.StartsWith("gui", comp))
                return ContentLevel.Temporary;

            // For stuff in the map category, use Map
            if (cat.StartsWith("map", comp))
                return ContentLevel.Map;

            // Everything else, return global
            return ContentLevel.Global;
        }

        /// <summary>
        /// Handles the <see cref="ChatBubble.CreateChatBubbleInstance"/> to create custom <see cref="ChatBubble"/>s.
        /// </summary>
        /// <param name="parent">The parent <see cref="Control"/>.</param>
        /// <param name="owner">The <see cref="Entity"/> the chat bubble is for.</param>
        /// <param name="text">The text to display.</param>
        /// <returns>The <see cref="ChatBubble"/> instance.</returns>
        static ChatBubble CreateChatBubbleInstanceHandler(Control parent, Entity owner, string text)
        {
            return new GameChatBubble(parent, owner, text);
        }

        /// <summary>
        /// Handles the KeyPressed event of the DemoGame control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void DemoGame_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == KeyCode.Tilde)
                _screenManager.ShowConsole = !_screenManager.ShowConsole;

            if (e.Code == KeyCode.Return && e.Alt)
                IsFullscreen = !IsFullscreen;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                if (_globalAtlases != null)
                {
                    foreach (var atlas in _globalAtlases)
                    {
                        atlas.Dispose();
                    }
                }

                _screenManager.Dispose();
            }

            base.Dispose(disposeManaged);
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the game.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            _screenManager.Draw(TickCount.Now);
        }

        /// <summary>
        /// When overridden in the derived class, handles updating the game.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleUpdate(TickCount currentTime)
        {
            // Update the sockets
            _sockets.Heartbeat();

            // Update everything else
            _screenManager.Update(TickCount.Now);
        }

        /// <summary>
        /// Loads the GrhInfo and places them in atlases based on their category
        /// </summary>
        void LoadGrhInfo()
        {
            GrhInfo.ContentLevelDecider = ContentLevelDecider;

            // Read the Grh info, using the MapContent for the ContentManager since all
            // but the map GrhDatas should end up in an atlas. For anything that does not
            // end up in an atlas, this will provide them a way to load still.
            GrhInfo.Load(ContentPaths.Build, _screenManager.Content);

            // Organize the GrhDatas for the atlases
            var gdChars = new List<ITextureAtlasable>();
            var gdGUI = new List<ITextureAtlasable>();
            foreach (var gd in GrhInfo.GrhDatas.SelectMany(x => x.Frames).Distinct())
            {
                var categoryStr = gd.Categorization.Category.ToString();

                if (categoryStr.StartsWith("character", StringComparison.OrdinalIgnoreCase))
                    gdChars.Add(gd);
                else if (categoryStr.StartsWith("gui", StringComparison.OrdinalIgnoreCase))
                    gdGUI.Add(gd);
            }

            // Build the atlases, leaving everything but map GrhDatas in an atlas
            var globalAtlasesList = new List<TextureAtlas>();

            if (gdChars.Count > 0)
                globalAtlasesList.Add(new TextureAtlas(gdChars));

            if (gdGUI.Count > 0)
                globalAtlasesList.Add(new TextureAtlas(gdGUI));

            _globalAtlases = globalAtlasesList.ToArray();

            // Unload all of the textures temporarily loaded into the MapContent
            // from the texture atlasing process
            _screenManager.Content.Unload();
        }
    }
}