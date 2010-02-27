using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Audio;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a game screen for the <see cref="ScreenManager"/>.
    /// </summary>
    public interface IGameScreen : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that is used for the GUI of this <see cref="IGameScreen"/>.
        /// </summary>
        IGUIManager GUIManager { get; }

        /// <summary>
        /// Gets the <see cref="MusicManager"/> to use for the music to play on this <see cref="IGameScreen"/>.
        /// </summary>
        MusicManager MusicManager { get; }

        /// <summary>
        /// Gets the unique name of this screen.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets if music is to be played during this screen. If true, the <see cref="IMusic"/> specified
        /// in <see cref="IGameScreen.ScreenMusic"/> will be played. If false, any music will be turned off while this
        /// screen is active.
        /// </summary>
        bool PlayMusic { get; set; }

        /// <summary>
        /// Gets the <see cref="IScreenManager"/> that manages this <see cref="IGameScreen"/>.
        /// </summary>
        IScreenManager ScreenManager { get; }

        /// <summary>
        /// Gets or sets the <see cref="IMusic"/> to play while this screen is active. Only valid if
        /// <see cref="IGameScreen.PlayMusic"/> is set. If null, the track will not be changed, preserving the music
        /// currently playing.
        /// </summary>
        IMusic ScreenMusic { get; set; }

        /// <summary>
        /// Gets the <see cref="SoundManager"/> to use for the sound to play on this <see cref="IGameScreen"/>.
        /// </summary>
        SoundManager SoundManager { get; }

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
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        /// <param name="gameTime">The current game time.</param>
        void Draw(ISpriteBatch spriteBatch, int gameTime);

        /// <summary>
        /// Handles initialization of the GameScreen. This will be invoked after the GameScreen has been
        /// completely and successfully added to the ScreenManager. It is highly recommended that you
        /// use this instead of the constructor. This is invoked only once.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Handles the loading of game content. Any content that is loaded should be placed in here.
        /// This will be invoked once (right after Initialize()), along with an additional time for
        /// every time XNA notifies the ScreenManager that the game content needs to be reloaded.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Handles the unloading of game content. This is raised whenever XNA notifies the ScreenManager
        /// that the content is to be unloaded.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Updates the screen if it is currently the active screen.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        void Update(int gameTime);
    }
}