using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client.Properties;
using log4net;
using NetGore;
using NetGore.Audio;
using NetGore.Content;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using NetGore.Network;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;
using Control = NetGore.Graphics.GUI.Control;
using KeyEventArgs = SFML.Window.KeyEventArgs;

namespace DemoGame.Client
{
    /// <summary>
    /// Root object for the Client
    /// </summary>
    public class DemoGame : GameBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ScreenManager _screenManager;
        readonly ClientSockets _sockets;

        IEnumerable<TextureAtlas> _globalAtlases;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoGame"/> class.
        /// </summary>
        public DemoGame() : base(
            new Point((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y),
            new Point((int)GameData.ScreenSize.X, (int)GameData.ScreenSize.Y), GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.GameTitle))
        {



            ThreadAsserts.IsMainThread();

            EngineSettingsInitializer.Initialize();

            // Create the screen manager
            var skinManager = new SkinManager("Default");
            _screenManager = new ScreenManager(this, skinManager, "Font/Arial", 24);

            // Initialize the socket manager
            ClientSockets.Initialize(ScreenManager);
            _sockets = ClientSockets.Instance;

            // Read the GrhInfo
            LoadGrhInfo();

            var lightGD = GrhInfo.GetData("Effect", "light");
            _screenManager.DrawingManager.LightManager.DefaultSprite = new Grh(lightGD);

            // Set up our custom chat bubbles
            ChatBubble.CreateChatBubbleInstance = CreateChatBubbleInstanceHandler;

            // Get our title song
            var titleSong = ScreenManager.AudioManager.MusicManager.MusicInfos.ElementAt(0);

            // Create the screens
            new OptionsScreen(ScreenManager) { ScreenMusic = titleSong }; ;
            new GameplayScreen(ScreenManager);
            new MainMenuScreen(ScreenManager) { ScreenMusic = titleSong };
            new LoginScreen(ScreenManager) { ScreenMusic = titleSong }; ;
            new CharacterSelectionScreen(ScreenManager) { ScreenMusic = titleSong }; ;
            new CreateCharacterScreen(ScreenManager) { ScreenMusic = titleSong }; ;
            new NewAccountScreen(ScreenManager) { ScreenMusic = titleSong }; ;



            ScreenManager.ConsoleScreen = new ConsoleScreen(ScreenManager);
            ScreenManager.SetScreen<MainMenuScreen>();

            ShowMouseCursor = true;

            KeyPressed -= DemoGame_KeyPressed;
            KeyPressed += DemoGame_KeyPressed;

            var clientSettings = ClientSettings.Default;

            // Apply some of the initial settings
            ScreenManager.AudioManager.SoundManager.Volume = clientSettings.Audio_SoundVolume;
            ScreenManager.AudioManager.MusicManager.Volume = clientSettings.Audio_MusicVolume;

            UseVerticalSync = clientSettings.Graphics_VSync;
            IsFullscreen = clientSettings.Graphics_Fullscreen;

            // Listen for changes to the settings
            clientSettings.PropertyChanged -= Default_PropertyChanged;
            clientSettings.PropertyChanged += Default_PropertyChanged;
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
        /// <param name="parent">The parent <see cref="NetGore.Graphics.GUI.Control"/>.</param>
        /// <param name="owner">The <see cref="Entity"/> the chat bubble is for.</param>
        /// <param name="text">The text to display.</param>
        /// <returns>The <see cref="ChatBubble"/> instance.</returns>
        static ChatBubble CreateChatBubbleInstanceHandler(Control parent, Entity owner, string text)
        {
            return new GameChatBubble(parent, owner, text);
        }

        /// <summary>
        /// Gets the system handle to use to display the game on when using windowed mode. If <see cref="IntPtr.Zero"/> is returned,
        /// a system form will be created automatically.
        /// </summary>
        /// <param name="displayContainer">When this method returns a value other than <see cref="IntPtr.Zero"/>, contains the
        /// object that the returned system handle belongs to. This is then later used in other virutal methods to initialize
        /// and dispose the container at the appropriate times.</param>
        /// <returns>
        /// The handle to the custom control to display the game on, or <see cref="IntPtr.Zero"/> to create the window
        /// to display the game on internally.
        /// </returns>
        protected override IntPtr CreateWindowedDisplayHandle(out object displayContainer)
        {
            var frm = new GameForm(this);
            
            // Set form title while we're at it
            frm.Text = GameMessageCollection.CurrentLanguage.GetMessage(GameMessage.GameTitle);

            displayContainer = frm;
            return frm.Handle;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the <see cref="ClientSettings"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string soundVolumeName = "Audio_SoundVolume";
            const string musicVolumeName = "Audio_MusicVolume";
            const string graphicsVSyncName = "Graphics_VSync";
            const string graphicsFullscreenName = "Graphics_Fullscreen";

            ClientSettings.Default.AssertPropertyExists(soundVolumeName);
            ClientSettings.Default.AssertPropertyExists(musicVolumeName);
            ClientSettings.Default.AssertPropertyExists(graphicsVSyncName);
            ClientSettings.Default.AssertPropertyExists(graphicsFullscreenName);

            var sc = StringComparer.Ordinal;

            if (sc.Equals(e.PropertyName, soundVolumeName))
            {
                // Sound volume
                var value = ClientSettings.Default.Audio_SoundVolume;
                ScreenManager.AudioManager.SoundManager.Volume = value;
            }
            else if (sc.Equals(e.PropertyName, musicVolumeName))
            {
                // Music volume
                var value = ClientSettings.Default.Audio_MusicVolume;
                ScreenManager.AudioManager.MusicManager.Volume = value;
            }
            else if (sc.Equals(e.PropertyName, graphicsVSyncName))
            {
                // VSync
                var value = ClientSettings.Default.Graphics_VSync;
                UseVerticalSync = value;
            }
            else if (sc.Equals(e.PropertyName, graphicsFullscreenName))
            {
                // Fullscreen
                var value = ClientSettings.Default.Graphics_Fullscreen;
                IsFullscreen = value;
            }
        }

        /// <summary>
        /// Handles the KeyPressed event of the DemoGame control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void DemoGame_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Tilde)
                _screenManager.ShowConsole = !_screenManager.ShowConsole;

            if (e.Code == Keyboard.Key.Return && e.Alt)
                ClientSettings.Default.Graphics_Fullscreen = !ClientSettings.Default.Graphics_Fullscreen;
        }

        /// <summary>
        /// Provides the ability to dispose of a custom display container. This is only invoked when you use
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/> and when there is an object to clean up.
        /// </summary>
        /// <param name="displayContainer">The display container reference that was used in the last call to
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/>.</param>
        protected override void DestroyCustomWindowedDisplayHandle(object displayContainer)
        {
            var frm = GetDisplayContainerAsGameForm(displayContainer);
            if (frm == null)
                return;

            frm.DisposeGameOnClose = false;

            if (!frm.IsDisposed)
                frm.Dispose();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposeManaged)
        {
            ClientSettings.Default.Save();

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
        /// Gets a displayContainer parameter as a <see cref="GameForm"/>, and makes sure it is a <see cref="GameForm"/> like
        /// we expect.
        /// </summary>
        /// <param name="displayContainer">The display container.</param>
        /// <returns>The <see cref="GameForm"/>, or null if <paramref name="displayContainer"/> is not a <see cref="GameForm"/>.</returns>
        static GameForm GetDisplayContainerAsGameForm(object displayContainer)
        {
            var frm = displayContainer as GameForm;
            if (frm == null)
            {
                const string errmsg = "Was expecting `{0}` to be type GameForm, but was `{1}` instead.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, displayContainer,
                        displayContainer != null ? displayContainer.GetType().ToString() : "[NULL]");
                Debug.Fail(string.Format(errmsg, displayContainer,
                    displayContainer != null ? displayContainer.GetType().ToString() : "[NULL]"));
                return null;
            }

            return frm;
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

        /// <summary>
        /// Provides the ability for the derived class to enter a custom loop. This is primarily intended to allow event-driven
        /// objects such as the Windows Forms to run without freezing. If this method blocks, it is up to the implementation
        /// to call <see cref="IGameContainer.HandleFrame"/> manually.
        /// </summary>
        /// <param name="displayContainer">The display container reference that was used in the last call to
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/>.</param>
        /// <example>
        /// When using WinForms, this should contain a call to Application.Run() where the <paramref name="displayContainer"/>
        /// is the Form that the game exists on. You will have to add additional logic to your form to have it call
        /// <see cref="IGameContainer.HandleFrame"/> whenever the form is idle.
        /// </example>
        protected override void RunCustomWindowedDisplayHandleLoop(object displayContainer)
        {
            var frm = GetDisplayContainerAsGameForm(displayContainer);
            if (frm == null)
                return;

            Application.Run(frm);
        }
    }
}