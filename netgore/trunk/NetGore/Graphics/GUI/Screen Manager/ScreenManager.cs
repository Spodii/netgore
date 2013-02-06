using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Audio;
using NetGore.Content;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Manages a collection of screens, allowing for selecting which screen(s) is/are
    /// active and updating and drawing them as needed. Also allows for interaction
    /// between the screens and common objects and properties for screens.
    /// </summary>
    public class ScreenManager : IScreenManager
    {
        readonly IAudioManager _audioManager;
        readonly IContentManager _content;
        readonly Font _defaultFont;
        readonly IDrawingManager _drawingManager;
        readonly FrameCounter _fps = new FrameCounter();
        readonly IGameContainer _game;
        readonly Dictionary<string, IGameScreen> _screens = new Dictionary<string, IGameScreen>(StringComparer.OrdinalIgnoreCase);
        readonly ISkinManager _skinManager;

        // This is changed when the focus on the window changes
        private bool _updateGameControls = true;

        /// <summary>
        /// Determines whether or not the screen manager window is focused or not
        /// </summary>
        public bool WindowFocused
        {
            get { return _updateGameControls; }
        }

        IGameScreen _activeScreen;
        bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenManager"/> class.
        /// </summary>
        /// <param name="game">The <see cref="IGameContainer"/>.</param>
        /// <param name="skinManager">The <see cref="ISkinManager"/> used to manage all the general skinning
        /// between all screens.</param>
        /// <param name="defaultFontName">The asset name of the default font.</param>
        /// <param name="defaultFontSize">The size of the default font.</param>
        /// <exception cref="ArgumentNullException"><paramref name="game"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="skinManager"/> is null.</exception>
        public ScreenManager(IGameContainer game, ISkinManager skinManager, string defaultFontName, int defaultFontSize)
        {
            if (game == null)
                throw new ArgumentNullException("game");
            if (skinManager == null)
                throw new ArgumentNullException("skinManager");

            _game = game;

            _game.RenderWindowChanged -= _game_RenderWindowChanged;
            _game.RenderWindowChanged += _game_RenderWindowChanged;

            _skinManager = skinManager;

            _content = ContentManager.Create();
            _drawingManager = new DrawingManager(_game.RenderWindow);

            _audioManager = Audio.AudioManager.GetInstance(_content);

            // Add event listeners to the input-related events
            _game.KeyPressed -= _game_KeyPressed;
            _game.KeyPressed += _game_KeyPressed;

            _game.KeyReleased -= _game_KeyReleased;
            _game.KeyReleased += _game_KeyReleased;

            _game.TextEntered -= _game_TextEntered;
            _game.TextEntered += _game_TextEntered;

            _game.MouseButtonPressed -= _game_MouseButtonPressed;
            _game.MouseButtonPressed += _game_MouseButtonPressed;

            _game.MouseButtonReleased -= _game_MouseButtonReleased;
            _game.MouseButtonReleased += _game_MouseButtonReleased;

            _game.MouseMoved -= _game_MouseMoved;
            _game.MouseMoved += _game_MouseMoved;

            _game.GainedFocus -= _game_GainedFocus;
            _game.GainedFocus += _game_GainedFocus;

            _game.LostFocus -= _game_LostFocus;
            _game.LostFocus += _game_LostFocus;

            // Load the global content used between screens
            _defaultFont = _content.LoadFont(defaultFontName, defaultFontSize, ContentLevel.Global);

            // Tell the other screens to load their content, too
            foreach (var screen in _screens.Values)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Fires when the game window loses focus
        /// </summary>
        /// <param name="sender">The sender of this event</param>
        /// <param name="e">The event arguments of the game window</param>
        void _game_LostFocus(IGameContainer sender, EventArgs e)
        {
            _updateGameControls = false;
        }

        /// <summary>
        /// Fires when the game window gains focus
        /// </summary>
        /// <param name="sender">The sender of this event</param>
        /// <param name="e">The event arguments of the game window</param>
        void _game_GainedFocus(IGameContainer sender, EventArgs e)
        {
            _updateGameControls = true;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the DrawableGameComponent and optionally
        /// releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            UnloadContent();
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with
        /// component-specific drawing code.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public virtual void Draw(TickCount gameTime)
        {
            // Update the FPS
            _fps.Update(gameTime);

            // Clear the screen
            _game.RenderWindow.Clear(DrawingManager.BackgroundColor);

            // Draw the active non-console screen
            var ancs = ActiveNonConsoleScreen;
            if (ancs != null)
                ancs.Draw(gameTime);

            // Draw the console
            var cs = ConsoleScreen;
            if (ShowConsole && cs != null)
                cs.Draw(gameTime);

            // Draw the console
            if (ShowConsole && ConsoleScreen != null)
                ConsoleScreen.Draw(gameTime);
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any component-specific
        /// graphics resources.
        /// </summary>
        protected virtual void UnloadContent()
        {
            // Unload all of the screens
            foreach (var screen in _screens.Values)
            {
                screen.UnloadContent();
            }

            // Unload the content manager
            _content.Unload();
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public virtual void Update(TickCount gameTime)
        {
            var currentTime = gameTime;

            // Update the audio
            AudioManager.Update();

            // Update the drawing manager
            DrawingManager.Update(currentTime);

            // Update the active screen
            var ancs = ActiveNonConsoleScreen;
            if (ancs != null)
                ancs.Update(currentTime);

            // Update the console screen
            var cs = ConsoleScreen;
            if (ShowConsole && cs != null)
                cs.Update(currentTime);

            // Raise update event
            if (Updated != null)
                Updated.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the KeyPressed event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void _game_KeyPressed(object sender, KeyEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventKeyPressed(e);
        }

        /// <summary>
        /// Handles the KeyReleased event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void _game_KeyReleased(object sender, KeyEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventKeyReleased(e);
        }

        /// <summary>
        /// Handles the MouseButtonPressed event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _game_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventMouseButtonPressed(e);
        }

        /// <summary>
        /// Handles the MouseButtonReleased event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void _game_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventMouseButtonReleased(e);
        }

        /// <summary>
        /// Handles the MouseMoved event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseMoveEventArgs"/> instance containing the event data.</param>
        void _game_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventMouseMoved(e);
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs{T}"/> instance containing the event data.</param>
        void _game_RenderWindowChanged(IGameContainer sender, ValueChangedEventArgs<RenderWindow> e)
        {
            _drawingManager.RenderWindow = e.NewValue;

            foreach (var gs in _screens.Values)
            {
                var guiMan = gs.GUIManager;
                if (guiMan != null)
                    guiMan.Window = e.NewValue;
            }
        }

        /// <summary>
        /// Handles the TextEntered event of the _game control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.TextEventArgs"/> instance containing the event data.</param>
        void _game_TextEntered(object sender, TextEventArgs e)
        {
            var a = ActiveScreen;
            if (a != null)
                a.GUIManager.SendEventTextEntered(e);
        }

        #region IScreenManager Members

        /// <summary>
        /// Notifies listeners when the <see cref="IScreenManager"/> updates.
        /// </summary>
        public event TypedEventHandler<IScreenManager> Updated;

        /// <summary>
        /// Gets the currently active <see cref="IGameScreen"/> that is not the console. Same as
        /// <see cref="IScreenManager.ActiveScreen"/> but wil lnever return the <see cref="IScreenManager.ConsoleScreen"/>.
        /// </summary>
        public IGameScreen ActiveNonConsoleScreen
        {
            get { return _activeScreen; }
        }

        /// <summary>
        /// Gets or sets the currently active <see cref="IGameScreen"/>. If <see cref="IScreenManager.ShowConsole"/> is set
        /// and <see cref="IScreenManager.ConsoleScreen"/> is valid, then the console screen will be returned. Otherwise, it will
        /// be the current game screen.
        /// </summary>
        /// <value></value>
        /// <exception cref="ArgumentException"><paramref name="value"/> does not belong to this
        /// <see cref="IScreenManager"/>.</exception>
        public IGameScreen ActiveScreen
        {
            get
            {
                if (ShowConsole && ConsoleScreen != null)
                    return ConsoleScreen;
                else
                    return _activeScreen;
            }
            set
            {
                if (value != null && value.ScreenManager != this)
                {
                    const string errmsg = "Game screen `{0}` does not belogn to this IScreenManager.";
                    throw new ArgumentException(string.Format(errmsg, value), "value");
                }

                if (value == ConsoleScreen && ConsoleScreen != null)
                {
                    ShowConsole = true;
                    return;
                }

                if (value == ActiveNonConsoleScreen)
                    return;

                var lastScreen = _activeScreen;
                _activeScreen = value;

                Content.Unload(ContentLevel.GameScreen);

                if (_activeScreen != null)
                    _activeScreen.Activate();

                if (lastScreen != null)
                    lastScreen.Deactivate();
            }
        }

        /// <summary>
        /// Gets the <see cref="IAudioManager"/> to be used by all of the <see cref="IGameScreen"/>s in this
        /// <see cref="IScreenManager"/>.
        /// </summary>
        public IAudioManager AudioManager
        {
            get { return _audioManager; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IGameScreen"/> to use to show the console.
        /// </summary>
        public IGameScreen ConsoleScreen { get; set; }

        /// <summary>
        /// Gets the global <see cref="IContentManager"/> shared between all screens.
        /// </summary>
        public IContentManager Content
        {
            get { return _content; }
        }

        /// <summary>
        /// Gets the default <see cref="Font"/> to use.
        /// </summary>
        public Font DefaultFont
        {
            get { return _defaultFont; }
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
        /// Gets the <see cref="IGameContainer"/>.
        /// </summary>
        public IGameContainer Game
        {
            get { return _game; }
        }

        /// <summary>
        /// Gets if this <see cref="IScreenManager"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        public Vector2 ScreenSize
        {
            get { return _game.ScreenSize; }
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
            var font = Content.LoadFont(fontAssetName, 28, ContentLevel.Global);
            return CreateGUIManager(font);
        }

        /// <summary>
        /// Creates an <see cref="IGUIManager"/> to be used by the screens managed by this <see cref="IScreenManager"/>.
        /// </summary>
        /// <param name="font">The font that will be used as the default GUI font.</param>
        /// <returns>
        /// A new <see cref="GUIManager"/> instance.
        /// </returns>
        public virtual IGUIManager CreateGUIManager(Font font)
        {
            return new GUIManager(_game.RenderWindow, font, SkinManager, ScreenSize);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            Dispose(true);
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
        public T GetScreen<T>() where T : IGameScreen
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

        /// <summary>
        /// Sets the currently active <see cref="IGameScreen"/> based on the type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IGameScreen"/>.</typeparam>
        /// <exception cref="ArgumentException">No screen of the given type was found.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void SetScreen<T>() where T : IGameScreen
        {
            ActiveScreen = GetScreen<T>();
        }

        #endregion
    }
}