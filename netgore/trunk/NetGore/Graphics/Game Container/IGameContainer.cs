using System;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for a container for the game window.
    /// </summary>
    public interface IGameContainer : IDisposable
    {
        /// <summary>
        /// Event handler for the Closed event.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Event handler for the GainedFocus event.
        /// </summary>
        event EventHandler GainedFocus;

        /// <summary>
        /// Event handler for the JoyButtonPressed event.
        /// </summary>
        event EventHandler<JoyButtonEventArgs> JoyButtonPressed;

        /// <summary>
        /// Event handler for the JoyButtonReleased event.
        /// </summary>
        event EventHandler<JoyButtonEventArgs> JoyButtonReleased;

        /// <summary>
        /// Event handler for the JoyMoved event.
        /// </summary>
        event EventHandler<JoyMoveEventArgs> JoyMoved;

        /// <summary>
        /// Event handler for the KeyPressed event.
        /// </summary>
        event EventHandler<KeyEventArgs> KeyPressed;

        /// <summary>
        /// Event handler for the KeyReleased event.
        /// </summary>
        event EventHandler<KeyEventArgs> KeyReleased;

        /// <summary>
        /// Event handler for the LostFocus event.
        /// </summary>
        event EventHandler LostFocus;

        /// <summary>
        /// Event handler for the MouseButtonPressed event.
        /// </summary>
        event EventHandler<MouseButtonEventArgs> MouseButtonPressed;

        /// <summary>
        /// Event handler for the MouseButtonReleased event.
        /// </summary>
        event EventHandler<MouseButtonEventArgs> MouseButtonReleased;

        /// <summary>
        /// Event handler for the MouseEntered event.
        /// </summary>
        event EventHandler MouseEntered;

        /// <summary>
        /// Event handler for the MouseLeft event.
        /// </summary>
        event EventHandler MouseLeft;

        /// <summary>
        /// Event handler for the MouseMoved event.
        /// </summary>
        event EventHandler<MouseMoveEventArgs> MouseMoved;

        /// <summary>
        /// Event handler for the MouseWheelMoved event.
        /// </summary>
        event EventHandler<MouseWheelEventArgs> MouseWheelMoved;

        /// <summary>
        /// Notifies listeners when the <see cref="IGameContainer.RenderWindow"/> has changed.
        /// </summary>
        event TypedEventHandler<IGameContainer, ValueChangedEventArgs<RenderWindow>> RenderWindowChanged;

        /// <summary>
        /// Event handler for the Resized event.
        /// </summary>
        event EventHandler<SizeEventArgs> Resized;

        /// <summary>
        /// Event handler for the TextEntered event.
        /// </summary>
        event EventHandler<TextEventArgs> TextEntered;

        /// <summary>
        /// Gets the resolution to use while in fullscreen mode.
        /// </summary>
        Point FullscreenResolution { get; }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets or sets if fullscreen mode is enabled.
        /// </summary>
        bool IsFullscreen { get; set; }

        /// <summary>
        /// Gets the current <see cref="RenderWindow"/>. Can be null.
        /// </summary>
        RenderWindow RenderWindow { get; }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        Vector2 ScreenSize { get; }

        /// <summary>
        /// Gets or sets if the mouse cursor is to be displayed.
        /// </summary>
        bool ShowMouseCursor { get; set; }

        /// <summary>
        /// Gets or sets if vertical sync is to be used.
        /// </summary>
        bool UseVerticalSync { get; set; }

        /// <summary>
        /// Gets the resolution to use while in windowed mode.
        /// </summary>
        Point WindowedResolution { get; }

        /// <summary>
        /// Handles processing and drawing a single frame of the game. This needs to be called continually in a loop to keep a fluent
        /// stream of updates.
        /// </summary>
        void HandleFrame();

        /// <summary>
        /// Starts running the game loop for this <see cref="IGameContainer"/>. This will create a blocking loop that will
        /// continuously call <see cref="HandleFrame"/> until the game is terminated.
        /// </summary>
        void Run();
    }
}