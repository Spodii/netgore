using System;
using System.Linq;
using NetGore.Audio;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a game screen for the <see cref="ScreenManager"/>.
    /// </summary>
    public interface IGameScreen : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> that is used to help draw on this screen.
        /// </summary>
        IDrawingManager DrawingManager { get; }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that is used for the GUI of this <see cref="IGameScreen"/>.
        /// </summary>
        IGUIManager GUIManager { get; }

        /// <summary>
        /// Gets the <see cref="IMusicManager"/> to use for the music to play on this <see cref="IGameScreen"/>.
        /// </summary>
        IMusicManager MusicManager { get; }

        /// <summary>
        /// Gets the unique name of this screen.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets if music is to be played during this screen. If true, the <see cref="IMusicInfo"/> specified
        /// in <see cref="IGameScreen.ScreenMusic"/> will be played. If false, any music will be turned off while this
        /// screen is active.
        /// </summary>
        bool PlayMusic { get; set; }

        /// <summary>
        /// Gets the <see cref="IScreenManager"/> that manages this <see cref="IGameScreen"/>.
        /// </summary>
        IScreenManager ScreenManager { get; }

        /// <summary>
        /// Gets or sets the <see cref="IMusicInfo"/> of the music to play while this screen is active. Only valid if
        /// <see cref="IGameScreen.PlayMusic"/> is set. If null, the track will not be changed, preserving the music
        /// currently playing.
        /// </summary>
        IMusicInfo ScreenMusic { get; set; }

        /// <summary>
        /// Gets the <see cref="ISoundManager"/> to use for the sound to play on this <see cref="IGameScreen"/>.
        /// </summary>
        ISoundManager SoundManager { get; }

        /// <summary>
        /// Handles screen activation, which occurs every time the screen becomes the current
        /// active screen. Objects in here often will want to be destroyed on <see cref="IGameScreen.Deactivate"/>().
        /// </summary>
        void Activate();

        /// <summary>
        /// Handles screen deactivation, which occurs every time the screen changes from being
        /// the current active screen. Good place to clean up any objects created in <see cref="IGameScreen.Activate"/>().
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Handles drawing of the screen. The ScreenManager already provides a GraphicsDevice.Clear() so
        /// there is often no need to clear the screen. This will only be called while the screen is the 
        /// active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Draw(TickCount gameTime);

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with any time the content needs
        /// to be reloaded for whatever reason.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Handles the unloading of game content. This is raised whenever the content for this screen
        /// needs to be unloaded. All content loaded in <see cref="IGameScreen.LoadContent"/> should
        /// be unloaded here to ensure complete and proper disposal.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(TickCount gameTime);
    }
}