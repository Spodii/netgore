using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Content;
using NetGore.Features.Emoticons;
using NetGore.Graphics;
using NetGore.Graphics.GUI;
using NetGore.IO;
using SFML.Graphics;
using SFML.Window;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// Root object for the Client
    /// </summary>
    public class DemoGame : RenderWindow
    {
        readonly ScreenManager _screenManager;
        readonly ClientSockets _sockets;

        IEnumerable<TextureAtlas> _globalAtlases;

        /// <summary>
        /// Initializes a new instance of the <see cref="DemoGame"/> class.
        /// </summary>
        /// <param name="p">The <see cref="IntPtr"/> to the handle to display the game on
        /// (usually a <see cref="System.Windows.Forms.Control"/>).</param>
        public DemoGame(IntPtr p) : base(p)
        {
            EngineSettingsInitializer.Initialize();

            // Set some globals
            EmoticonDisplayManager.GetDrawPositionHandler = EmoticonDrawPositionHandler;

            // Create the screen manager
            _screenManager = new ScreenManager(this, new SkinManager("Default"), "Font/Arial", 24);

            // Read the GrhInfo
            LoadGrhInfo();
            _screenManager.DrawingManager.LightManager.DefaultSprite = new Grh(GrhInfo.GetData("Effect", "light"));

            // Create the screens
            new GameplayScreen(_screenManager);
            new MainMenuScreen(_screenManager);
            new LoginScreen(_screenManager);
            new CharacterSelectionScreen(_screenManager);
            new CreateCharacterScreen(_screenManager);
            new NewAccountScreen(_screenManager);

            _screenManager.ConsoleScreen = new ConsoleScreen(_screenManager);
            _screenManager.SetScreen(MainMenuScreen.ScreenName);

            // NOTE: Temporary volume reduction
            _screenManager.AudioManager.SoundManager.Volume = 70f;
            _screenManager.AudioManager.MusicManager.Volume = 20f;

            _sockets = ClientSockets.Instance;

            ShowMouseCursor(true);
            UseVerticalSync(true);

            KeyPressed += DemoGame_KeyPressed;
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
        /// Handles the KeyPressed event of the DemoGame control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void DemoGame_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == KeyCode.Tilde)
                _screenManager.ShowConsole = !_screenManager.ShowConsole;
        }

        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy(bool disposing)
        {
            if (_globalAtlases != null)
            {
                foreach (var atlas in _globalAtlases)
                {
                    atlas.Dispose();
                }
            }

            _screenManager.Dispose();

            base.Destroy(disposing);
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        public virtual void Draw()
        {
            _screenManager.Draw(TickCount.Now);
        }

        /// <summary>
        /// The handler for finding the position to draw an emoticon.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/> to get the draw position for.</param>
        /// <returns>The world position to draw the emoticon for the given <paramref name="spatial"/>.</returns>
        static Vector2 EmoticonDrawPositionHandler(ISpatial spatial)
        {
            Vector2 pos;

            // Get the draw offset
            Character character;
            if ((character = spatial as Character) != null)
                pos = character.DrawPosition;
            else
                pos = spatial.Position;

            // Move the emoticon a bit up and to the right
            pos += new Vector2(5, -24);

            return pos;
        }

        /// <summary>
        /// Handles processing and drawing a single frame of the game. This needs to be called continually in a loop to keep a fluent
        /// stream of updates.
        /// </summary>
        public void HandleFrame()
        {
            // Process events
            DispatchEvents();

            // Draw everything
            Draw();

            // Update the sockets
            _sockets.Heartbeat();

            // Update everything else
            Update();

            // Display the window
            Display();
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
        /// Updates the game.
        /// </summary>
        public virtual void Update()
        {
            _screenManager.Update(TickCount.Now);
        }
    }
}