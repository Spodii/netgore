using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for a container for the game screen.
    /// </summary>
    public abstract class GameBase : IGameContainer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Point _fullscreenRes;
        readonly Point _windowedRes;

        bool _callEnterAppRun = false;
        bool? _changeIsFullscreenValue;
        object _displayContainer;
        bool _isDisposed;
        bool _isFullscreen;
        bool _isHandlingFrame;
        RenderWindow _renderWindow;
        bool _showMouseCursor;
        string _title;
        bool _useVerticalSync;
        bool _usingCustomDisplayContainer = false;
        bool _willDispose;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBase"/> class.
        /// </summary>
        /// <param name="windowedResolution">The windowed resolution.</param>
        /// <param name="fullscreenResolution">The fullscreen resolution.</param>
        /// <param name="title">The initial title value.</param>
        protected GameBase(Point windowedResolution, Point fullscreenResolution, string title = null)
        {
            _windowedRes = windowedResolution;
            _fullscreenRes = fullscreenResolution;

            Title = title;

            SwitchToWindowed(true);
        }

        /// <summary>
        /// Gets or sets the title for the automatically generated windows. Changing this value may not immediately change the actual
        /// title of the window, and only applies to automatically generated windows.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == null)
                    value = string.Empty;

                _title = value;
            }
        }

        /// <summary>
        /// Handles switching to and from fullscreen mode using the <see cref="_changeIsFullscreenValue"/> value.
        /// This lets us ensure we switch to/from fullscreen only when we know it is a valid time to do so.
        /// </summary>
        void ChangeFullscreen()
        {
            // Check for a value
            if (!_changeIsFullscreenValue.HasValue)
                return;

            // Change mode
            if (_changeIsFullscreenValue.Value)
                SwitchToFullscreen();
            else
                SwitchToWindowed();

            // Clear value
            _changeIsFullscreenValue = null;
        }

        /// <summary>
        /// Gets the system handle to use to display the game on when using windowed mode. If <see cref="IntPtr.Zero"/> is returned,
        /// a system form will be created automatically.
        /// </summary>
        /// <param name="displayContainer">When this method returns a value other than <see cref="IntPtr.Zero"/>, contains the
        /// object that the returned system handle belongs to. This is then later used in other virutal methods to initialize
        /// and dispose the container at the appropriate times.</param>
        /// <returns>The handle to the custom control to display the game on, or <see cref="IntPtr.Zero"/> to create the window
        /// to display the game on internally.</returns>
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        protected virtual IntPtr CreateWindowedDisplayHandle(out object displayContainer)
        {
            displayContainer = null;
            return IntPtr.Zero;
        }

        /// <summary>
        /// Provides the ability to dispose of a custom display container. This is only invoked when you use
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/> and when there is an object to clean up.
        /// </summary>
        /// <param name="displayContainer">The display container reference that was used in the last call to
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/>.</param>
        protected virtual void DestroyCustomWindowedDisplayHandle(object displayContainer)
        {
            var asDisposable = displayContainer as IDisposable;
            if (asDisposable != null)
                asDisposable.Dispose();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposeManaged"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
                RenderWindow = null;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="GameBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~GameBase()
        {
            if (IsDisposed)
                return;

            _isDisposed = true;

            Dispose(false);
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the game.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected abstract void HandleDraw(TickCount currentTime);

        /// <summary>
        /// When overridden in the derived class, handles updating the game.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected abstract void HandleUpdate(TickCount currentTime);

        /// <summary>
        /// Handles the <see cref="IGameContainer.RenderWindowChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnRenderWindowChanged(RenderWindow oldValue, RenderWindow newValue)
        {
            if (newValue == null)
                return;

            newValue.SetMouseCursorVisible(ShowMouseCursor);
            newValue.SetVerticalSyncEnabled(UseVerticalSync);
            newValue.SetActive();
            newValue.SetVisible(true);
        }

        /// <summary>
        /// Provides the ability for the derived class to enter a custom loop. This is primarily intended to allow event-driven
        /// objects such as the Windows Forms to run without freezing. If this method blocks, it is up to the implementation
        /// to call <see cref="IGameContainer.HandleFrame"/> manually.
        /// </summary>
        /// <example>
        /// When using WinForms, this should contain a call to Application.Run() where the <paramref name="displayContainer"/>
        /// is the Form that the game exists on. You will have to add additional logic to your form to have it call
        /// <see cref="IGameContainer.HandleFrame"/> whenever the form is idle.
        /// </example>
        /// <param name="displayContainer">The display container reference that was used in the last call to
        /// <see cref="GameBase.CreateWindowedDisplayHandle"/>.</param>
        protected virtual void RunCustomWindowedDisplayHandleLoop(object displayContainer)
        {
        }

        /// <summary>
        /// Sets the event listeners on a <see cref="RenderWindow"/>.
        /// </summary>
        /// <param name="rw">The <see cref="RenderWindow"/> to set the event listeners on.</param>
        /// <param name="remove">If true, the event listeners will be removed. If false, they will be added.</param>
        void SetRenderWindowListeners(RenderWindow rw, bool remove)
        {
            // Check for a valid RenderWindow
            if (rw == null)
            {
                Debug.Fail("rw should not be null.");
                return;
            }

            if (!remove)
            {
                // When setting the events, also make sure they have been removed first to avoid duplicate listeners
                SetRenderWindowListeners(rw, true);

                // Add listeners
                rw.Closed += rw_Closed;
                rw.GainedFocus += rw_GainedFocus;
                rw.JoystickButtonPressed += rw_JoystickButtonPressed;
                rw.JoystickButtonReleased += rw_JoystickButtonReleased;
                rw.JoystickMoved += rw_JoystickMoved;
                rw.KeyPressed += rw_KeyPressed;
                rw.KeyReleased += rw_KeyReleased;
                rw.LostFocus += rw_LostFocus;
                rw.MouseButtonPressed += rw_MouseButtonPressed;
                rw.MouseButtonReleased += rw_MouseButtonReleased;
                rw.MouseEntered += rw_MouseEntered;
                rw.MouseLeft += rw_MouseLeft;
                rw.MouseMoved += rw_MouseMoved;
                rw.MouseWheelMoved += rw_MouseWheelMoved;
                rw.Resized += rw_Resized;
                rw.TextEntered += rw_TextEntered;
            }
            else
            {
                // Remove listeners
                rw.Closed -= rw_Closed;
                rw.GainedFocus -= rw_GainedFocus;
                rw.JoystickButtonPressed -= rw_JoystickButtonPressed;
                rw.JoystickButtonReleased -= rw_JoystickButtonReleased;
                rw.JoystickMoved -= rw_JoystickMoved;
                rw.KeyPressed -= rw_KeyPressed;
                rw.KeyReleased -= rw_KeyReleased;
                rw.LostFocus -= rw_LostFocus;
                rw.MouseButtonPressed -= rw_MouseButtonPressed;
                rw.MouseButtonReleased -= rw_MouseButtonReleased;
                rw.MouseEntered -= rw_MouseEntered;
                rw.MouseLeft -= rw_MouseLeft;
                rw.MouseMoved -= rw_MouseMoved;
                rw.MouseWheelMoved -= rw_MouseWheelMoved;
                rw.Resized -= rw_Resized;
                rw.TextEntered -= rw_TextEntered;
            }
        }

        /// <summary>
        /// Switches to fullscreen mode.
        /// </summary>
        /// <param name="force">If true, the <see cref="RenderWindow"/> will be recreated even if already in fullscreen mode.</param>
        void SwitchToFullscreen(bool force = false)
        {
            if (!force && _isFullscreen)
                return;

            if (log.IsInfoEnabled)
                log.Info("Changing to fullscreen mode.");

            RenderWindow = null;

            var videoMode = new VideoMode((uint)FullscreenResolution.X, (uint)FullscreenResolution.Y);
            var newRW = new RenderWindow(videoMode, Title, Styles.Fullscreen);

            _isFullscreen = true;
            _usingCustomDisplayContainer = false;
            _displayContainer = null;

            RenderWindow = newRW;
        }

        /// <summary>
        /// Switches to windowed mode.
        /// </summary>
        /// <param name="force">If true, the <see cref="RenderWindow"/> will be recreated even if already in windowed mode.</param>
        void SwitchToWindowed(bool force = false)
        {
            if (!force && !_isFullscreen)
                return;

            if (log.IsInfoEnabled)
                log.Info("Changing to windowed mode.");

            RenderWindow = null;

            RenderWindow newRW;

            // Get the new display container
            var displayHandle = CreateWindowedDisplayHandle(out _displayContainer);

            if (displayHandle == IntPtr.Zero)
            {
                // Not using custom handle
                _usingCustomDisplayContainer = false;
                _displayContainer = null;
                var videoMode = new VideoMode((uint)WindowedResolution.X, (uint)WindowedResolution.Y);
                newRW = new RenderWindow(videoMode, Title, Styles.Titlebar | Styles.Close);
                _callEnterAppRun = false;
            }
            else
            {
                // Using custom handle
                _usingCustomDisplayContainer = true;
                newRW = new RenderWindow(displayHandle);
                _callEnterAppRun = true;
            }

            _isFullscreen = false;

            RenderWindow = newRW;
        }

        /// <summary>
        /// Handles the Closed event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rw_Closed(object sender, EventArgs e)
        {
            if (Closed != null)
                Closed.Raise(this, e);
        }

        /// <summary>
        /// Handles the GainedFocus event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rw_GainedFocus(object sender, EventArgs e)
        {
            if (GainedFocus != null)
                GainedFocus.Raise(this, e);
        }

        /// <summary>
        /// Handles the JoyButtonPressed event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.JoystickButtonEventArgs"/> instance containing the event data.</param>
        void rw_JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            if (JoyButtonPressed != null)
                JoyButtonPressed.Raise(this, e);
        }

        /// <summary>
        /// Handles the JoyButtonReleased event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.JoystickButtonEventArgs"/> instance containing the event data.</param>
        void rw_JoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            if (JoyButtonReleased != null)
                JoyButtonReleased.Raise(this, e);
        }

        /// <summary>
        /// Handles the JoyMoved event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.JoystickMoveEventArgs"/> instance containing the event data.</param>
        void rw_JoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            if (JoyMoved != null)
                JoyMoved.Raise(this, e);
        }

        /// <summary>
        /// Handles the KeyPressed event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void rw_KeyPressed(object sender, KeyEventArgs e)
        {
            if (KeyPressed != null)
                KeyPressed.Raise(this, e);
        }

        /// <summary>
        /// Handles the KeyReleased event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.KeyEventArgs"/> instance containing the event data.</param>
        void rw_KeyReleased(object sender, KeyEventArgs e)
        {
            if (KeyReleased != null)
                KeyReleased.Raise(this, e);
        }

        /// <summary>
        /// Handles the LostFocus event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rw_LostFocus(object sender, EventArgs e)
        {
            if (LostFocus != null)
                LostFocus.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseButtonPressed event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void rw_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonPressed != null)
                MouseButtonPressed.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseButtonReleased event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        void rw_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonReleased != null)
                MouseButtonReleased.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseEntered event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rw_MouseEntered(object sender, EventArgs e)
        {
            if (MouseEntered != null)
                MouseEntered.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseLeft event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void rw_MouseLeft(object sender, EventArgs e)
        {
            if (MouseLeft != null)
                MouseLeft.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseMoved event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseMoveEventArgs"/> instance containing the event data.</param>
        void rw_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (MouseMoved != null)
                MouseMoved.Raise(this, e);
        }

        /// <summary>
        /// Handles the MouseWheelMoved event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseWheelEventArgs"/> instance containing the event data.</param>
        void rw_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            if (MouseWheelMoved != null)
                MouseWheelMoved.Raise(this, e);
        }

        /// <summary>
        /// Handles the Resized event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.SizeEventArgs"/> instance containing the event data.</param>
        void rw_Resized(object sender, SizeEventArgs e)
        {
            if (Resized != null)
                Resized.Raise(this, e);
        }

        /// <summary>
        /// Handles the TextEntered event of the <see cref="GameBase.RenderWindow"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.TextEventArgs"/> instance containing the event data.</param>
        void rw_TextEntered(object sender, TextEventArgs e)
        {
            if (TextEntered != null)
                TextEntered.Raise(this, e);
        }

        #region IGameContainer Members

        /// <summary>
        /// Event handler for the Closed event.
        /// </summary>
        public event TypedEventHandler<IGameContainer> Closed = null;

        /// <summary>
        /// Event handler for the GainedFocus event.
        /// </summary>
        public event TypedEventHandler<IGameContainer> GainedFocus = null;

        /// <summary>
        /// Event handler for the JoyButtonPressed event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, JoystickButtonEventArgs> JoyButtonPressed = null;

        /// <summary>
        /// Event handler for the JoyButtonReleased event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, JoystickButtonEventArgs> JoyButtonReleased = null;

        /// <summary>
        /// Event handler for the JoyMoved event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, JoystickMoveEventArgs> JoyMoved = null;

        /// <summary>
        /// Event handler for the KeyPressed event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, KeyEventArgs> KeyPressed = null;

        /// <summary>
        /// Event handler for the KeyReleased event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, KeyEventArgs> KeyReleased = null;

        /// <summary>
        /// Event handler for the LostFocus event.
        /// </summary>
        public event TypedEventHandler<IGameContainer> LostFocus = null;

        /// <summary>
        /// Event handler for the MouseButtonPressed event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, MouseButtonEventArgs> MouseButtonPressed = null;

        /// <summary>
        /// Event handler for the MouseButtonReleased event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, MouseButtonEventArgs> MouseButtonReleased = null;

        /// <summary>
        /// Event handler for the MouseEntered event.
        /// </summary>
        public event TypedEventHandler<IGameContainer> MouseEntered = null;

        /// <summary>
        /// Event handler for the MouseLeft event.
        /// </summary>
        public event TypedEventHandler<IGameContainer> MouseLeft = null;

        /// <summary>
        /// Event handler for the MouseMoved event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, MouseMoveEventArgs> MouseMoved = null;

        /// <summary>
        /// Event handler for the MouseWheelMoved event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, MouseWheelEventArgs> MouseWheelMoved = null;

        /// <summary>
        /// Notifies listeners when the <see cref="IGameContainer.RenderWindow"/> has changed.
        /// </summary>
        public event TypedEventHandler<IGameContainer, ValueChangedEventArgs<RenderWindow>> RenderWindowChanged;

        /// <summary>
        /// Event handler for the Resized event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, SizeEventArgs> Resized = null;

        /// <summary>
        /// Event handler for the TextEntered event.
        /// </summary>
        public event TypedEventHandler<IGameContainer, TextEventArgs> TextEntered = null;

        /// <summary>
        /// Gets the resolution to use while in fullscreen mode.
        /// </summary>
        public Point FullscreenResolution
        {
            get { return _fullscreenRes; }
        }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets or sets if fullscreen mode is enabled.
        /// </summary>
        public bool IsFullscreen
        {
            get { return _isFullscreen; }
            set { _changeIsFullscreenValue = value; }
        }

        /// <summary>
        /// Gets the current <see cref="IGameContainer.RenderWindow"/>. Can be null.
        /// </summary>
        public RenderWindow RenderWindow
        {
            get { return _renderWindow; }
            private set
            {
                // Check that the value is actually changing
                if (RenderWindow == value)
                    return;

                // Store old RenderWindow
                var oldRW = RenderWindow;

                // Set new RenderWindow
                _renderWindow = value;

                // Remove the event listeners from the old RenderWindow, and add to the new
                if (oldRW != null)
                {
                    SetRenderWindowListeners(oldRW, true);

                    // Destroy old custom display container
                    if (_usingCustomDisplayContainer)
                    {
                        try
                        {
                            DestroyCustomWindowedDisplayHandle(_displayContainer);
                        }
                        catch (Exception ex)
                        {
                            const string errmsg = "Failed to dispose of custom window display handle for `{0}`. Exception: {1}";
                            if (log.IsErrorEnabled)
                                log.ErrorFormat(errmsg, _displayContainer, ex);
                            Debug.Fail(string.Format(errmsg, _displayContainer, ex));
                        }

                        _displayContainer = null;
                    }
                }

                if (RenderWindow != null)
                    SetRenderWindowListeners(RenderWindow, false);

                // Raise events
                OnRenderWindowChanged(oldRW, RenderWindow);
                if (RenderWindowChanged != null)
                    RenderWindowChanged.Raise(this, ValueChangedEventArgs.Create(oldRW, RenderWindow));

                // Dispose old RenderWindow
                if (oldRW != null && !oldRW.IsDisposed)
                    oldRW.Dispose();

                // Finally, ensure we are showing the new RenderWindow
                if (RenderWindow != null)
                {
                    RenderWindow.SetVisible(true);
                }
            }
        }

        /// <summary>
        /// Gets the size of the screen in pixels.
        /// </summary>
        public Vector2 ScreenSize
        {
            get
            {
                if (IsFullscreen)
                    return new Vector2(FullscreenResolution.X, FullscreenResolution.Y);
                else
                    return new Vector2(WindowedResolution.X, WindowedResolution.Y);
            }
        }

        /// <summary>
        /// Gets or sets if the mouse cursor is to be displayed.
        /// </summary>
        public bool ShowMouseCursor
        {
            get { return _showMouseCursor; }
            set
            {
                _showMouseCursor = value;

                if (RenderWindow != null)
                    RenderWindow.SetMouseCursorVisible(ShowMouseCursor);
            }
        }

        /// <summary>
        /// Gets or sets if vertical sync is to be used.
        /// </summary>
        public bool UseVerticalSync
        {
            get { return _useVerticalSync; }
            set
            {
                _useVerticalSync = value;

                if (RenderWindow != null)
                    RenderWindow.SetVerticalSyncEnabled(UseVerticalSync);
            }
        }

        /// <summary>
        /// Gets the resolution to use while in windowed mode.
        /// </summary>
        public Point WindowedResolution
        {
            get { return _windowedRes; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Make sure we are not already disposed
            if (IsDisposed)
                return;

            // If handling a frame (which is common since dispose events frequently come from input events), then we will wait until
            // the frame is over before disposing
            if (_isHandlingFrame)
            {
                _willDispose = true;
                return;
            }

            // Dispose
            _isDisposed = true;

            GC.SuppressFinalize(this);

            Dispose(true);
        }

        /// <summary>
        /// Handles processing and drawing a single frame of the game. This needs to be called continually in a loop to keep a fluent
        /// stream of updates.
        /// </summary>
        public void HandleFrame()
        {
            if (IsDisposed)
                return;

            _isHandlingFrame = false;

            // Check if we need to dispose
            if (_willDispose)
            {
                Dispose();
                return;
            }

            // Begin handling frame
            _isHandlingFrame = true;

            try
            {
                // Check if we need to toggle fullscreen mode
                ChangeFullscreen();

                // Get the current time
                var currentTime = TickCount.Now;

                // Dispatch the events
                var rw = RenderWindow;
                if (rw != null && !rw.IsDisposed)
                    rw.DispatchEvents();

                // Update
                HandleUpdate(currentTime);

                // Draw
                rw = RenderWindow;
                if (rw != null && !rw.IsDisposed)
                    HandleDraw(currentTime);

                // Display
                rw = RenderWindow;
                if (rw != null && !rw.IsDisposed)
                    rw.Display();
            }
            finally
            {
                // End handling frame
                _isHandlingFrame = false;
            }

            // Check if we need to dispose
            if (_willDispose)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Starts running the game loop for this <see cref="IGameContainer"/>. This will create a blocking loop that will
        /// continuously call <see cref="HandleFrame"/> until the game is terminated.
        /// </summary>
        public void Run()
        {
            while (!IsDisposed)
            {
                if (_callEnterAppRun)
                {
                    _callEnterAppRun = false;
                    RunCustomWindowedDisplayHandleLoop(_displayContainer);
                }

                HandleFrame();
            }
        }

        #endregion
    }
}