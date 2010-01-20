using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// Root object for the Client
    /// </summary>
    public class DemoGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        IEnumerable<TextureAtlas> _globalAtlases;
        ScreenManager _screenManager;
        ClientSockets _sockets;

        /// <summary>
        /// Sets up all the primary components of the game
        /// </summary>
        public DemoGame()
        {
            // Create the graphics manager and device
            graphics = new GraphicsDeviceManager(this);
        }

        /// <summary>
        /// Called after all components are initialized but before the first update in the game loop.
        /// </summary>
        protected override void BeginRun()
        {
            base.BeginRun();

            // Create the screen manager
            _screenManager = new ScreenManager(this, new SkinManager("Default"), "Content");
            Components.Add(_screenManager);

            // Read the GrhInfo
            LoadGrhInfo();
            
            // Create the screens
            new GameplayScreen(_screenManager);
            new MainMenuScreen(_screenManager);
            new LoginScreen(_screenManager);
            new CharacterSelectionScreen(_screenManager);
            new CreateCharacterScreen(_screenManager);
            new NewAccountScreen(_screenManager);
            _screenManager.SetScreen(MainMenuScreen.ScreenName);

            // NOTE: Temporary volume reduction
            // We use the thread pool due to the potentially long time it can take to load the audio engine
            ThreadPool.QueueUserWorkItem(delegate
                                         {
                                             _screenManager.SoundManager.Volume = 0.7f;
                                             _screenManager.MusicManager.Volume = 0.2f;
                                         });

            _sockets = ClientSockets.Instance;
            _screenManager.OnUpdate += screenManager_OnUpdate;
        }

        /// <summary>
        /// Releases all resources used by the Game class.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_globalAtlases != null)
            {
                foreach (var atlas in _globalAtlases)
                {
                    atlas.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called after the Game and GraphicsDevice are created, but before LoadContent.  Reference page contains code sample.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = ContentPaths.Build.Root;
            IsMouseVisible = true;

            // Try to go for 75 FPS for the update rate
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / 75);
            IsFixedTimeStep = true;

            // Set the graphics settings
            graphics.SynchronizeWithVerticalRetrace = false; // vsync
            graphics.PreferMultiSampling = false;

            // Disable filtering, which makes our 2d art look like crap
            var samplerStates = graphics.GraphicsDevice.SamplerStates;
            samplerStates[0].MinFilter = TextureFilter.None;
            samplerStates[0].MipFilter = TextureFilter.None;
            samplerStates[0].MagFilter = TextureFilter.None;
            samplerStates[0].MaxMipLevel = 0;

            // Screen size
            graphics.PreferredBackBufferWidth = (int)GameData.ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)GameData.ScreenSize.Y;

            // Apply the changes
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// Loads the GrhInfo and places them in atlases based on their category
        /// </summary>
        void LoadGrhInfo()
        {
            // Read the Grh info, using the MapContent for the ContentManager since all
            // but the map GrhDatas should end up in an atlas. For anything that does not
            // end up in an atlas, this will provide them a way to load still.
            GrhInfo.Load(ContentPaths.Build, _screenManager.MapContent);

            // Organize the GrhDatas for the atlases
            var gdChars = new List<ITextureAtlasable>();
            var gdGUI = new List<ITextureAtlasable>();
            var gdNonMap = new List<ITextureAtlasable>();
            foreach (var gd in GrhInfo.GrhDatas.SelectMany(x => x.Frames).Distinct())
            {
                var categoryStr = gd.Categorization.Category.ToString();

                if (categoryStr.StartsWith("character", StringComparison.OrdinalIgnoreCase))
                    gdChars.Add(gd);
                else if (categoryStr.StartsWith("gui", StringComparison.OrdinalIgnoreCase))
                    gdGUI.Add(gd);
                else if (!categoryStr.StartsWith("map", StringComparison.OrdinalIgnoreCase))
                    gdNonMap.Add(gd);
            }

            // Build the atlases, leaving everything but map GrhDatas in an atlas
            var atlasChars = new TextureAtlas(GraphicsDevice, gdChars);
            var atlasGUI = new TextureAtlas(GraphicsDevice, gdGUI);
            var atlasMisc = new TextureAtlas(GraphicsDevice, gdNonMap);
            _globalAtlases = new TextureAtlas[] { atlasChars, atlasGUI, atlasMisc };

            // Unload all of the textures temporarily loaded into the MapContent
            // from the texture atlasing process
            _screenManager.MapContent.Unload();
        }

        void screenManager_OnUpdate(ScreenManager screenManager)
        {
            // Update the sockets
            _sockets.Heartbeat();
        }
    }
}