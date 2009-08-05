using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Manages a collection of screens, allowing for selecting which screen(s) is/are
    /// active and updating and drawing them as needed. Also allows for interaction
    /// between the screens and common objects and properties for screens.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        readonly ContentManager _content;

        readonly FrameCounter _fps = new FrameCounter();
        readonly ContentManager _mapContent;

        readonly Dictionary<string, GameScreen> _screens = new Dictionary<string, GameScreen>(8);

        GameScreen _activeScreen;

        SpriteFont _menuFont;
        SpriteBatch _sb;

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

        public ContentManager Content
        {
            get { return _content; }
        }

        public int FPS
        {
            get { return _fps.FrameRate; }
        }

        public ContentManager MapContent
        {
            get { return _mapContent; }
        }

        public SpriteFont MenuFont
        {
            get { return _menuFont; }
        }

        public int ScreenHeight
        {
            get { return GraphicsDevice.Viewport.Height; }
        }

        public Vector2 ScreenSize
        {
            get { return new Vector2(ScreenWidth, ScreenHeight); }
        }

        public int ScreenWidth
        {
            get { return GraphicsDevice.Viewport.Width; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _sb; }
        }

        public ScreenManager(Game game) : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            _content = new ContentManager(game.Services, "Content");
            _mapContent = new ContentManager(game.Services, "Content");
        }

        public void Add(GameScreen screen)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");

            screen.ScreenManager = this;
            _screens.Add(screen.Name, screen);

            screen.Initialize();
            screen.LoadContent();
        }

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

        public override void Draw(GameTime gameTime)
        {
            _fps.Update(gameTime.ElapsedRealTime);

            if (_activeScreen == null)
                return;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeScreen.Draw(gameTime);
        }

        public GameScreen GetScreen(string name)
        {
            GameScreen ret;
            if (!_screens.TryGetValue(name, out ret))
                return null;
            return ret;
        }

        protected override void LoadContent()
        {
            // Load the global content used between screens
            _sb = new SpriteBatch(GraphicsDevice);
            _menuFont = _content.Load<SpriteFont>("Font/Menu");

            // Tell the other screens to load their content, too
            foreach (GameScreen screen in _screens.Values)
            {
                screen.LoadContent();
            }
        }

        public void SetScreen(string name)
        {
            GameScreen gs = GetScreen(name);
            if (gs == null)
                throw new Exception(string.Format("Unknown screen with name '{0}'", name));

            ActiveScreen = gs;
        }

        protected override void UnloadContent()
        {
            _content.Unload();

            foreach (GameScreen screen in _screens.Values)
            {
                screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_activeScreen == null)
                return;

            _activeScreen.Update(gameTime);
        }
    }
}