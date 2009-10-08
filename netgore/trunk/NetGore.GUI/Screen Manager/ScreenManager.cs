using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Manages a collection of screens, allowing for selecting which screen(s) is/are
    /// active and updating and drawing them as needed. Also allows for interaction
    /// between the screens and common objects and properties for screens.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ContentManager _content;
        readonly FrameCounter _fps = new FrameCounter();
        readonly ContentManager _mapContent;
        readonly Dictionary<string, GameScreen> _screens = new Dictionary<string, GameScreen>(8, StringComparer.OrdinalIgnoreCase);

        GameScreen _activeScreen;
        SpriteFont _menuFont;
        SpriteBatch _sb;

        /// <summary>
        /// Gets or sets the currently active <see cref="GameScreen"/>.
        /// </summary>
        public GameScreen ActiveScreen
        {
            get { return _activeScreen; }
            set
            {
                if (_activeScreen != value)
                {
                    GameScreen lastScreen = _activeScreen;
                    _activeScreen = value;
                    if (_activeScreen != null)
                        _activeScreen.Activate();
                    if (lastScreen != null)
                        lastScreen.Deactivate();
                }
            }
        }

        /// <summary>
        /// Gets the global <see cref="ContentManager"/>.
        /// </summary>
        public ContentManager Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Gets the current FPS.
        /// </summary>
        public int FPS
        {
            get { return _fps.FrameRate; }
        }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> intended for loading content for the current map only.
        /// </summary>
        public ContentManager MapContent
        {
            get { return _mapContent; }
        }

        /// <summary>
        /// Gets the <see cref="SpriteFont"/> to use for the menus.
        /// </summary>
        public SpriteFont MenuFont
        {
            get { return _menuFont; }
        }

        /// <summary>
        /// Gets the height of the screen in pixels.
        /// </summary>
        public int ScreenHeight
        {
            get { return GraphicsDevice.Viewport.Height; }
        }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        public Vector2 ScreenSize
        {
            get { return new Vector2(ScreenWidth, ScreenHeight); }
        }

        /// <summary>
        /// Gets the width of the screen in pixels.
        /// </summary>
        public int ScreenWidth
        {
            get { return GraphicsDevice.Viewport.Width; }
        }

        /// <summary>
        /// Gets the generic <see cref="SpriteBatch"/> to use.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _sb; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="game">The Game that the game component should be attached to.</param>
        public ScreenManager(Game game) : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            _content = new ContentManager(game.Services, "Content");
            _mapContent = new ContentManager(game.Services, "Content");
        }

        /// <summary>
        /// Adds a <see cref="GameScreen"/> to this manager.
        /// </summary>
        /// <param name="screen">The <see cref="GameScreen"/> to add.</param>
        public void Add(GameScreen screen)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");

            screen.ScreenManager = this;
            _screens.Add(screen.Name, screen);

            screen.Initialize();
            screen.LoadContent();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the DrawableGameComponent and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Call Dispose on screens that implement it
            foreach (GameScreen screen in _screens.Values)
            {
                IDisposable disposable = screen as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with
        /// component-specific drawing code.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            _fps.Update(gameTime.ElapsedRealTime);

            if (_activeScreen == null)
                return;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeScreen.Draw(gameTime);
        }

        /// <summary>
        /// Gets the <see cref="GameScreen"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="GameScreen"/> to get.</param>
        /// <returns>The <see cref="GameScreen"/> with the given <paramref name="name"/>, or null if the
        /// <paramref name="name"/> was invalid or there is no screen was found.</returns>
        public GameScreen GetScreen(string name)
        {
            GameScreen ret;
            if (!_screens.TryGetValue(name, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any
        /// component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            // Read the global content used between screens
            _sb = new SpriteBatch(GraphicsDevice);
            _menuFont = _content.Load<SpriteFont>("Font/Menu");

            // Tell the other screens to load their content, too
            foreach (GameScreen screen in _screens.Values)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Sets the currently active <see cref="GameScreen"/> based on the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="GameScreen"/>.</param>
        public void SetScreen(string name)
        {
            GameScreen gs = GetScreen(name);
            if (gs == null)
            {
                const string errmsg = "Unknown screen with name `{0}`.";
                string err = string.Format(errmsg, name);
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                throw new Exception(err);
            }

            ActiveScreen = gs;
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any component-specific
        /// graphics resources.
        /// </summary>
        protected override void UnloadContent()
        {
            _content.Unload();

            foreach (GameScreen screen in _screens.Values)
            {
                screen.UnloadContent();
            }
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public override void Update(GameTime gameTime)
        {
            if (_activeScreen == null)
                return;

            _activeScreen.Update(gameTime);
        }
    }
}