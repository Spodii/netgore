using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ScreenManager : DrawableGameComponent, IScreenManager
    {
        readonly ContentManager _content;
        readonly FrameCounter _fps = new FrameCounter();
        readonly ContentManager _mapContent;
        readonly MusicManager _musicManager;
        readonly Dictionary<string, IGameScreen> _screens = new Dictionary<string, IGameScreen>(StringComparer.OrdinalIgnoreCase);
        readonly ISkinManager _skinManager;
        readonly SoundManager _soundManager;

        IGameScreen _activeScreen;
        IDrawingManager _drawingManager;
        SpriteFont _menuFont;

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
        /// Releases the unmanaged resources used by the DrawableGameComponent and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Dispose all the screens
            foreach (var screen in _screens.Values)
            {
                screen.Dispose();
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
            // Update the FPS
            _fps.Update(gameTime.ElapsedRealTime);

            // Clear the screen
            GraphicsDevice.Clear(DrawingManager.ClearColor);

            // Draw the active screen
            if (_activeScreen != null)
                _activeScreen.Draw(gameTime.ToTotalMS());

            // Draw the console
            if (ShowConsole && ConsoleScreen != null)
                ConsoleScreen.Draw(gameTime.ToTotalMS());
        }

        /// <summary>
        /// Initializes the component. Override this method to load any non-graphics resources and query for any
        /// required services.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _drawingManager = new DrawingManager(GraphicsDevice);
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any
        /// component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            // Read the global content used between screens
            _menuFont = _content.Load<SpriteFont>("Font/Menu");

            // Tell the other screens to load their content, too
            foreach (var screen in _screens.Values)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any component-specific
        /// graphics resources.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload all of the screens
            foreach (var screen in _screens.Values)
            {
                screen.UnloadContent();
            }

            // Unload the content managers
            _content.Unload();
            _mapContent.Unload();
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public override void Update(GameTime gameTime)
        {
            int currentTime = gameTime.ToTotalMS();

            // Update the drawing manager
            DrawingManager.Update(currentTime);

            // Update the active screen
            if (ActiveScreen != null)
                ActiveScreen.Update(currentTime);

            // Update the console screen
            if (ShowConsole && ConsoleScreen != null)
                ConsoleScreen.Update(currentTime);

            // Raise update event
            if (Updated != null)
                Updated(this);
        }

        #region IScreenManager Members

        /// <summary>
        /// Notifies listeners when the <see cref="IScreenManager"/> updates.
        /// </summary>
        public event IScreenManagerEventHandler Updated;

        /// <summary>
        /// Gets or sets the currently active <see cref="IGameScreen"/>.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="value"/> does not belong to this
        /// <see cref="IScreenManager"/>.</exception>
        public IGameScreen ActiveScreen
        {
            get { return _activeScreen; }
            set
            {
                if (value != null && value.ScreenManager != this)
                {
                    const string errmsg = "Game screen `{0}` does not belogn to this IScreenManager.";
                    throw new ArgumentException(string.Format(errmsg, value), "value");
                }

                var lastScreen = _activeScreen;
                _activeScreen = value;

                if (_activeScreen != null)
                    _activeScreen.Activate();

                if (lastScreen != null)
                    lastScreen.Deactivate();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IGameScreen"/> to use to show the console.
        /// </summary>
        public IGameScreen ConsoleScreen { get; set; }

        /// <summary>
        /// Gets the global <see cref="ContentManager"/> shared between all screens.
        /// </summary>
        public ContentManager Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> to use.
        /// </summary>
        public IDrawingManager DrawingManager
        {
            get { return _drawingManager; }
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
        /// Gets the size of the screen in pixels.
        /// </summary>
        public Vector2 ScreenSize
        {
            get { return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
        }

        /// <summary>
        /// Gets or sets if to show the console. If <see cref="IScreenManager.ConsoleScreen"/> is null, the console
        /// will not be shown even if this value is true.
        /// </summary>
        public bool ShowConsole { get; set; }

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
        /// Adds a <see cref="IGameScreen"/> to this manager.
        /// </summary>
        /// <param name="screen">The <see cref="IGameScreen"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screen"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="screen"/>'s <see cref="IScreenManager"/> is not
        /// equal to this <see cref="IScreenManager"/>.</exception>
        public void Add(IGameScreen screen)
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
        /// Creates an <see cref="IGUIManager"/> to be used by the screens managed by this <see cref="IScreenManager"/>.
        /// </summary>
        /// <param name="fontAssetName">The name of the font asset that will be used as the default GUI font.</param>
        /// <returns>
        /// A new <see cref="IGUIManager"/> instance.
        /// </returns>
        public IGUIManager CreateGUIManager(string fontAssetName)
        {
            var font = Content.Load<SpriteFont>(fontAssetName);
            return CreateGUIManager(font);
        }

        /// <summary>
        /// Creates an <see cref="IGUIManager"/> to be used by the screens managed by this <see cref="IScreenManager"/>.
        /// </summary>
        /// <param name="font">The font that will be used as the default GUI font.</param>
        /// <returns>
        /// A new <see cref="GUIManager"/> instance.
        /// </returns>
        public virtual IGUIManager CreateGUIManager(SpriteFont font)
        {
            return new GUIManager(font, SkinManager, ScreenSize);
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
        /// Gets the <see cref="IGameScreen"/> of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IGameScreen"/>.</typeparam>
        /// <returns>
        /// The <see cref="IGameScreen"/> of the given type..
        /// </returns>
        /// <exception cref="ArgumentException">No screen with the given type was found.</exception>
        public T GetScreen<T>()
        {
            var ret = _screens.Values.OfType<T>().FirstOrDefault();
            if (Equals(ret, default(T)))
                throw new ArgumentException(string.Format("No screen of type `{0}` was found.", typeof(T)));

            return ret;
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

        #endregion
    }
}