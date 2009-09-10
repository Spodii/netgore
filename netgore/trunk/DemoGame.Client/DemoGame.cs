using System;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;
using NetGore.Graphics.GUI;

namespace DemoGame.Client
{
    /// <summary>
    /// Root object for the Client
    /// </summary>
    public class DemoGame : Game
    {
        readonly GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        /// <summary>
        /// Sets up all the primary components of the game
        /// </summary>
        public DemoGame()
        {
            // Create the graphics manager and device
            graphics = new GraphicsDeviceManager(this);

            // Read the game data
            GameData.Load();

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
        }

        protected override void Initialize()
        {
            Content.RootDirectory = ContentPaths.Build.Root;
            IsMouseVisible = true;

            // Set the graphics settings
            graphics.SynchronizeWithVerticalRetrace = false; // vsync
            graphics.PreferMultiSampling = false;

            // Disable filtering, which makes our 2d art look like crap
            SamplerStateCollection samplerStates = graphics.GraphicsDevice.SamplerStates;
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

            // Build the texture atlases
            TextureAtlas atlasChars = new TextureAtlas();
            TextureAtlas atlasGUI = new TextureAtlas();
            TextureAtlas atlasMisc = new TextureAtlas();

            foreach (GrhData gd in GrhInfo.GrhDatas)
            {
                if (gd.Frames.Length == 1)
                {
                    if (gd.Category.Length > 9 && gd.Category.Substring(0, 9).ToLower() == "character")
                        atlasChars.AtlasItems.Push(gd); // Characters
                    else if (gd.Category.Length > 3 && gd.Category.Substring(0, 3).ToLower() == "gui")
                        atlasGUI.AtlasItems.Push(gd); // GUI
                    else if (!(gd.Category.Length > 3 && gd.Category.Substring(0, 3).ToLower() == "map"))
                        atlasMisc.AtlasItems.Push(gd); // Misc
                }
            }

            // Build the atlases, leaving everything but map Grhs in an atlas
            atlasChars.Build(GraphicsDevice);
            atlasGUI.Build(GraphicsDevice);
            atlasMisc.Build(GraphicsDevice);

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

            using (DemoGame game = new DemoGame())
            {
                game.Run();
            }
        }
    }
}