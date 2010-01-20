using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Audio;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Manages a collection of screens, allowing for selecting which screen(s) is/are
    /// active and updating and drawing them as needed. Also allows for interaction
    /// between the screens and common objects and properties for screens.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        /// <summary>
        /// Delegate for handling an event from the <see cref="ScreenManager"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="ScreenManager"/> the event came from.</param>
        public delegate void ScreenManagerEventHandler(ScreenManager screenManager);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ContentManager _content;
        readonly FrameCounter _fps = new FrameCounter();
        readonly ContentManager _mapContent;
        readonly MusicManager _musicManager;
        readonly Dictionary<string, IGameScreen> _screens = new Dictionary<string, IGameScreen>(StringComparer.OrdinalIgnoreCase);
        readonly ISkinManager _skinManager;
        readonly SoundManager _soundManager;

        IGameScreen _activeScreen;
        SpriteFont _menuFont;
        SpriteBatch _sb;

        /// <summary>
        /// Notifies listeners when the <see cref="ScreenManager"/> updates.
        /// </summary>
        public event ScreenManagerEventHandler OnUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="game">The Game that the game component should be attached to.</param>
        /// <param name="skinManager">The <see cref="ISkinManager"/> used to manage all the general skinning
        /// between all screens.</param>
        /// <param name="rootContentDirectory">The default root content directory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="game"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="skinManager"/> is null.</exception>
        public ScreenManager(Game game, ISkinManager skinManager, string rootContentDirectory) : base(game)
        {
            if (game == null)
                throw new ArgumentNullException("game");
            if (skinManager == null)
                throw new ArgumentNullException("skinManager");

            if (rootContentDirectory == null)
                rootContentDirectory = string.Empty;

            _skinManager = skinManager;

            _content = new ContentManager(game.Services, rootContentDirectory);
            _mapContent = new ContentManager(game.Services, rootContentDirectory);

            _soundManager = SoundManager.GetInstance(_content);
            _musicManager = MusicManager.GetInstance(_content);
        }

        /// <summary>
        /// Gets or sets the currently active <see cref="IGameScreen"/>.
        /// </summary>
        public IGameScreen ActiveScreen
        {
            get { return _activeScreen; }
            set
            {
                if (_activeScreen == value)
                    return;

                var lastScreen = _activeScreen;
                _activeScreen = value;

                if (_activeScreen != null)
                    _activeScreen.Activate();

                if (lastScreen != null)
                    lastScreen.Deactivate();
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
        /// Gets the <see cref="MusicManager"/> managed by this <see cref="ScreenManager"/>.
        /// </summary>
        public MusicManager MusicManager
        {
            get { return _musicManager; }
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
        /// Gets the <see cref="ISkinManager"/> used to manage all the general skinning between all screens.
        /// </summary>
        public ISkinManager SkinManager
        {
            get { return _skinManager; }
        }

        /// <summary>
        /// Gets the <see cref="SoundManager"/> managed by this <see cref="ScreenManager"/>.
        /// </summary>
        public SoundManager SoundManager
        {
            get { return _soundManager; }
        }

        /// <summary>
        /// Gets the generic <see cref="SpriteBatch"/> to use.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _sb; }
        }

        /// <summary>
        /// Adds a <see cref="GameScreen"/> to this manager.
        /// </summary>
        /// <param name="screen">The <see cref="GameScreen"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screen"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="screen"/>'s ScreenManager is not equal to this
        /// ScreenManager.</exception>
        public void Add(GameScreen screen)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");
            if (screen.ScreenManager != this)
                throw new ArgumentException("The screen's ScreenManager must be set to this ScreenManager.", "screen");

            _screens.Add(screen.Name, screen);

            screen.Initialize();
            screen.LoadContent();
        }

        /// <summary>
        /// Creates an <see cref="IGUIManager"/>.
        /// </summary>
        /// <param name="fontAssetName">The name of the font asset that will be used as the default GUI font.</param>
        /// <returns>A new <see cref="IGUIManager"/> instance.</returns>
        public IGUIManager CreateGUIManager(string fontAssetName)
        {
            var font = Content.Load<SpriteFont>(fontAssetName);
            return CreateGUIManager(font);
        }

        /// <summary>
        /// Creates a <see cref="GUIManager"/>.
        /// </summary>
        /// <param name="font">The font that will be used as the default GUI font.</param>
        /// <returns>A new <see cref="GUIManager"/> instance.</returns>
        public virtual GUIManager CreateGUIManager(SpriteFont font)
        {
            return new GUIManager(font, SkinManager);
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
        /// Gets the <see cref="IGameScreen"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGameScreen"/> to get.</param>
        /// <returns>The <see cref="IGameScreen"/> with the given <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentException">No screen with the given <paramref name="name"/> was found.</exception>
        public IGameScreen GetScreen(string name)
        {
            IGameScreen ret;
            if (!_screens.TryGetValue(name, out ret))
                throw new ArgumentException(string.Format("No screen with the given name `{0}` was found.", name), "name");

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
        /// <exception cref="ArgumentException">No screen with the given <paramref name="name"/> was found.</exception>
        public void SetScreen(string name)
        {
            ActiveScreen = GetScreen(name);
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
            if (OnUpdate != null)
                OnUpdate(this);

            if (_activeScreen != null)
                _activeScreen.Update(gameTime);
        }
    }
}