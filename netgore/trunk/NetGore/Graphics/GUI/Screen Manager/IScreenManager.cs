using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Audio;
using NetGore.Content;
using SFML.Graphics;

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
        event TypedEventHandler<IScreenManager> Updated;

        /// <summary>
        /// Gets the currently active <see cref="IGameScreen"/> that is not the console. Same as
        /// <see cref="IScreenManager.ActiveScreen"/> but wil lnever return the <see cref="IScreenManager.ConsoleScreen"/>.
        /// </summary>
        IGameScreen ActiveNonConsoleScreen { get; }

        /// <summary>
        /// Gets or sets the currently active <see cref="IGameScreen"/>. If <see cref="IScreenManager.ShowConsole"/> is set
        /// and <see cref="IScreenManager.ConsoleScreen"/> is valid, then the console screen will be returned. Otherwise, it will
        /// be the current game screen.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="value"/> does not belong to this
        /// <see cref="IScreenManager"/>.</exception>
        IGameScreen ActiveScreen { get; set; }

        /// <summary>
        /// Gets the <see cref="IAudioManager"/> to be used by all of the <see cref="IGameScreen"/>s in this
        /// <see cref="IScreenManager"/>.
        /// </summary>
        IAudioManager AudioManager { get; }

        /// <summary>
        /// Gets or sets the <see cref="IGameScreen"/> to use to show the console.
        /// </summary>
        IGameScreen ConsoleScreen { get; set; }

        /// <summary>
        /// Gets the global <see cref="IContentManager"/> shared between all screens.
        /// </summary>
        IContentManager Content { get; }

        /// <summary>
        /// Gets the default <see cref="Font"/> to use.
        /// </summary>
        Font DefaultFont { get; }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> to use.
        /// </summary>
        IDrawingManager DrawingManager { get; }

        /// <summary>
        /// Gets the current FPS.
        /// </summary>
        int FPS { get; }

        /// <summary>
        /// Gets the <see cref="IGameContainer"/>.
        /// </summary>
        IGameContainer Game { get; }

        /// <summary>
        /// Determines whether or not the window this screen manager is tied to is focused or not
        /// This can be a window or a render control
        /// </summary>
        bool WindowFocused { get; }

        /// <summary>
        /// Gets if this <see cref="IScreenManager"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        Vector2 ScreenSize { get; }

        /// <summary>
        /// Gets or sets if to show the console. If <see cref="IScreenManager.ConsoleScreen"/> is null, the console
        /// will not be shown even if this value is true.
        /// </summary>
        bool ShowConsole { get; set; }

        /// <summary>
        /// Gets the <see cref="ISkinManager"/> used to manage all the general skinning between all screens.
        /// </summary>
        ISkinManager SkinManager { get; }

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
        IGUIManager CreateGUIManager(Font font);

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
        T GetScreen<T>() where T : IGameScreen;

        /// <summary>
        /// Sets the currently active <see cref="IGameScreen"/> based on the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGameScreen"/>.</param>
        /// <exception cref="ArgumentException">No screen with the given <paramref name="name"/> was found.</exception>
        void SetScreen(string name);

        /// <summary>
        /// Sets the currently active <see cref="IGameScreen"/> based on the type.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IGameScreen"/>.</typeparam>
        /// <exception cref="ArgumentException">No screen of the given type was found.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void SetScreen<T>() where T : IGameScreen;
    }
}