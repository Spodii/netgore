using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
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
        IEnumerable<TextureAtlas> _globalAtlases;
        readonly GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        /// <summary>
        /// Sets up all the primary components of the game
        /// </summary>
        public DemoGame()
        {
            // Create the graphics manager and device
            graphics = new GraphicsDeviceManager(this);

            // No need to use a time step since we use delta time in our updating
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1000 / 100);
            IsFixedTimeStep = true;
        }

        protected override void BeginRun()
        {
            base.BeginRun();

            // Create the screen manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // Read the GrhInfo
            LoadGrhInfo();

            // Create the screens
            screenManager.Add(new MainMenuScreen());
            screenManager.Add(new LoginScreen());
            screenManager.Add(new CharacterSelectionScreen());
            screenManager.Add(new GameplayScreen());
            screenManager.SetScreen(MainMenuScreen.ScreenName);

            // NOTE: Temporary volume reduction
            // We use the thread pool due to the potentially long time it can take to load the audio engine
            ThreadPool.QueueUserWorkItem(delegate
                                         {
                                             screenManager.SoundManager.Volume = 0.7f;
                                             screenManager.MusicManager.Volume = 0.2f;
                                         });
        }

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

        protected override void Initialize()
        {
            Content.RootDirectory = ContentPaths.Build.Root;
            IsMouseVisible = true;

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
            GrhInfo.Load(ContentPaths.Build, screenManager.MapContent);

            // Organize the GrhDatas for the atlases
            var gdChars = new List<ITextureAtlasable>();
            var gdGUI = new List<ITextureAtlasable>();
            var gdNonMap = new List<ITextureAtlasable>();
            foreach (var gd in GrhInfo.GrhDatas.Where(x => !x.IsAnimated))
            {
                if (gd.Category.StartsWith("character", StringComparison.OrdinalIgnoreCase))
                    gdChars.Add(gd);
                else if (gd.Category.StartsWith("gui", StringComparison.OrdinalIgnoreCase))
                    gdGUI.Add(gd);
                else if (!gd.Category.StartsWith("map", StringComparison.OrdinalIgnoreCase))
                    gdNonMap.Add(gd);
            }

            // Build the atlases, leaving everything but map GrhDatas in an atlas
            var atlasChars = new TextureAtlas(GraphicsDevice, gdChars);
            var atlasGUI = new TextureAtlas(GraphicsDevice, gdGUI);
            var atlasMisc = new TextureAtlas(GraphicsDevice, gdNonMap);
            _globalAtlases = new TextureAtlas[] { atlasChars, atlasGUI, atlasMisc };

            // Unload all of the textures temporarily loaded into the MapContent
            // from the texture atlasing process
            screenManager.MapContent.Unload();
        }
    }

    /// <summary>
    /// Main application entry point - does nothing more than start the primary class
    /// </summary>
    static class Program
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            log.Info("Starting client...");

            ThreadAsserts.IsMainThread();

            using (var game = new DemoGame())
            {
                game.Run();
            }
        }
    }
}