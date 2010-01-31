using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Audio;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for an object that manages a collection of screens, allowing for selecting which screen(s) is/are
    /// active and updating and drawing them as needed. Also allows for interaction
    /// between the screens and common objects and properties for screens.
    /// </summary>
    public interface IScreenManager : IDisposable
    {
        /// <summary>
        /// Notifies listeners when the <see cref="IScreenManager"/> updates.
        /// </summary>
        event IScreenManagerEventHandler Updated;

        /// <summary>
        /// Gets or sets the currently active <see cref="IGameScreen"/>.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="value"/> does not belong to this
        /// <see cref="IScreenManager"/>.</exception>
        IGameScreen ActiveScreen { get; set; }

        /// <summary>
        /// Gets the global <see cref="ContentManager"/> shared between all screens.
        /// </summary>
        ContentManager Content { get; }

        /// <summary>
        /// Gets the current FPS.
        /// </summary>
        int FPS { get; }

        /// <summary>
        /// Gets the <see cref="Game"/>.
        /// </summary>
        Game Game { get; }

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/>.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Gets the <see cref="ContentManager"/> intended for loading content for the current map only.
        /// </summary>
        ContentManager MapContent { get; }

        /// <summary>
        /// Gets the <see cref="SpriteFont"/> to use for the menus.
        /// </summary>
        SpriteFont MenuFont { get; }

        /// <summary>
        /// Gets the <see cref="MusicManager"/> managed by this <see cref="ScreenManager"/>.
        /// </summary>
        MusicManager MusicManager { get; }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        Vector2 ScreenSize { get; }

        /// <summary>
        /// Gets the <see cref="ISkinManager"/> used to manage all the general skinning between all screens.
        /// </summary>
        ISkinManager SkinManager { get; }

        /// <summary>
        /// Gets the <see cref="SoundManager"/> managed by this <see cref="ScreenManager"/>.
        /// </summary>
        SoundManager SoundManager { get; }

        /// <summary>
        /// Gets a general-purpose <see cref="SpriteBatch"/> to use for drawing the screens.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Adds a <see cref="IGameScreen"/> to this manager.
        /// </summary>
        /// <param name="screen">The <see cref="IGameScreen"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="screen"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="screen"/>'s <see cref="IScreenManager"/> is not
        /// equal to this <see cref="IScreenManager"/>.</exception>
        void Add(IGameScreen screen);

        /// <summary>
        /// Creates an <see cref="IGUIManager"/> to be used by the screens managed by this <see cref="IScreenManager"/>.
        /// </summary>
        /// <param name="fontAssetName">The name of the font asset that will be used as the default GUI font.</param>
        /// <returns>A new <see cref="IGUIManager"/> instance.</returns>
        IGUIManager CreateGUIManager(string fontAssetName);

        /// <summary>
        /// Creates an <see cref="IGUIManager"/> to be used by the screens managed by this <see cref="IScreenManager"/>.
        /// </summary>
        /// <param name="font">The font that will be used as the default GUI font.</param>
        /// <returns>A new <see cref="GUIManager"/> instance.</returns>
        IGUIManager CreateGUIManager(SpriteFont font);

        /// <summary>
        /// Gets the <see cref="IGameScreen"/> with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGameScreen"/> to get.</param>
        /// <returns>The <see cref="IGameScreen"/> with the given <paramref name="name"/>.</returns>
        /// <exception cref="ArgumentException">No screen with the given <paramref name="name"/> was found.</exception>
        IGameScreen GetScreen(string name);

        /// <summary>
        /// Gets the <see cref="IGameScreen"/> of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IGameScreen"/>.</typeparam>
        /// <returns>
        /// The <see cref="IGameScreen"/> of the given type..
        /// </returns>
        /// <exception cref="ArgumentException">No screen with the given type was found.</exception>
        T GetScreen<T>();

        /// <summary>
        /// Sets the currently active <see cref="IGameScreen"/> based on the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGameScreen"/>.</param>
        /// <exception cref="ArgumentException">No screen with the given <paramref name="name"/> was found.</exception>
        void SetScreen(string name);
    }
}