using System;
using System.Linq;
using NetGore.Audio;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Base of a game screen for the <see cref="ScreenManager"/>. Each screen should ideally be independent of
    /// one another.
    /// </summary>
    public abstract class GameScreen : IGameScreen
    {
        readonly IGUIManager _guiManager;
        readonly string _name;
        readonly IScreenManager _screenManager;

        bool _playMusic = true;
        IMusicInfo _screenMusic;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameScreen"/> class.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> to add this <see cref="GameScreen"/> to.</param>
        /// <param name="name">Unique name of the screen that can be used to identify and
        /// call it from other screens</param>
        /// <exception cref="ArgumentNullException"><paramref name="screenManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null or empty.</exception>
        protected GameScreen(IScreenManager screenManager, string name)
        {
            if (screenManager == null)
                throw new ArgumentNullException("screenManager");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _screenManager = screenManager;
            _name = name;

            var font = GetScreenManagerFont(ScreenManager) ?? screenManager.DefaultFont;
            _guiManager = ScreenManager.CreateGUIManager(font);

            screenManager.Add(this);
        }

        /// <summary>
        /// Gets the <see cref="Font"/> to use as the default font for the <see cref="IGUIManager"/> for this
        /// <see cref="GameScreen"/>.
        /// </summary>
        /// <param name="screenManager">The <see cref="IScreenManager"/> for this screen.</param>
        /// <returns>The <see cref="Font"/> to use for this <see cref="GameScreen"/>. If null, the
        /// <see cref="IScreenManager.DefaultFont"/> for this <see cref="GameScreen"/> will be used instead.</returns>
        protected virtual Font GetScreenManagerFont(IScreenManager screenManager)
        {
            return screenManager.DefaultFont;
        }

        /// <summary>
        /// Updates the currently playing music based off the values defined by <see cref="GameScreen.PlayMusic"/>
        /// and <see cref="GameScreen.ScreenMusic"/>.
        /// </summary>
        void UpdateMusic()
        {
            // Make sure we have the screen enabled
            if (ScreenManager == null || ScreenManager.ActiveNonConsoleScreen != this)
                return;

            // Turn off music
            if (!PlayMusic)
            {
                MusicManager.Stop();
                return;
            }

            // Change track
            if (ScreenMusic != null)
                ScreenManager.AudioManager.MusicManager.Play(ScreenMusic);
        }

        #region IGameScreen Members

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> that is used to help draw on this screen.
        /// </summary>
        public IDrawingManager DrawingManager
        {
            get { return ScreenManager.DrawingManager; }
        }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that is used for the GUI of this <see cref="IGameScreen"/>.
        /// </summary>
        public IGUIManager GUIManager
        {
            get { return _guiManager; }
        }

        /// <summary>
        /// Gets the <see cref="IMusicManager"/> to use for the music to play on this <see cref="IGameScreen"/>.
        /// </summary>
        public IMusicManager MusicManager
        {
            get { return ScreenManager.AudioManager.MusicManager; }
        }

        /// <summary>
        /// Gets the unique name of this screen.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets or sets if music is to be played during this screen. If true, the <see cref="IMusicInfo"/> specified
        /// in <see cref="IGameScreen.ScreenMusic"/> will be played. If false, any music will be turned off while this
        /// screen is active.
        /// </summary>
        public bool PlayMusic
        {
            get { return _playMusic; }
            set
            {
                // Value changed?
                if (_playMusic == value)
                    return;

                // Update value and update music
                _playMusic = value;
                UpdateMusic();
            }
        }

        /// <summary>
        /// Gets the <see cref="IScreenManager"/> that manages this <see cref="IGameScreen"/>.
        /// </summary>
        public IScreenManager ScreenManager
        {
            get { return _screenManager; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IMusicInfo"/> of the music to play while this screen is active. Only valid if
        /// <see cref="IGameScreen.PlayMusic"/> is set. If null, the track will not be changed, preserving the music
        /// currently playing.
        /// </summary>
        public IMusicInfo ScreenMusic
        {
            get { return _screenMusic; }
            set
            {
                // Value changed?
                if (_screenMusic == value)
                    return;

                // Update value and update music
                _screenMusic = value;
                UpdateMusic();
            }
        }

        /// <summary>
        /// Gets the <see cref="ISoundManager"/> to use for the sound to play on this <see cref="IGameScreen"/>.
        /// </summary>
        public ISoundManager SoundManager
        {
            get { return ScreenManager.AudioManager.SoundManager; }
        }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="GameScreen.Deactivate"/>().
        /// </summary>
        public virtual void Activate()
        {
            UpdateMusic();

            GUIManager.IsEnabled = true;
        }

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="GameScreen.Activate"/>().
        /// </summary>
        public virtual void Deactivate()
        {
            GUIManager.IsEnabled = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public virtual void Draw(TickCount gameTime)
        {
            var spriteBatch = DrawingManager.BeginDrawGUI();
            if (spriteBatch == null)
                return;

            GUIManager.Draw(spriteBatch);
            DrawingManager.EndDrawGUI();
        }

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with any time the content needs
        /// to be reloaded for whatever reason.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Handles the unloading of game content. This is raised whenever the content for this screen
        /// needs to be unloaded. All content loaded in <see cref="IGameScreen.LoadContent"/> should
        /// be unloaded here to ensure complete and proper disposal.
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public virtual void Update(TickCount gameTime)
        {
            GUIManager.Update(gameTime);
        }

        #endregion
    }
}