using System;
using System.Linq;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Handles checking for the state needed by a game control.
    /// </summary>
    public class GameControl
    {
        int _delay;
        GameControlKeys _gckeys;
        bool _isEnabled = true;
        TickCount _lastInvokeTime;
        bool _needsReset = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControl"/> class.
        /// </summary>
        /// <param name="keys">The <see cref="GameControlKeys"/>.</param>
        public GameControl(GameControlKeys keys)
        {
            GameControlKeys = keys;
        }

        /// <summary>
        /// Notifies listeners that this <see cref="GameControl"/>'s key state requirements match the current key
        /// state and that it is ready to be handled.
        /// </summary>
        public event TypedEventHandler<GameControl> Invoked;

        /// <summary>
        /// Gets or sets a Func containing any additional requirements for this <see cref="GameControl"/> to be invoked.
        /// If value must return true for <see cref="Invoked"/> to be raised. If this value is false,
        /// <see cref="Invoked"/> will not be raised and the internal delay counter wil not be altered. If this value
        /// is null, it will be treated the same as if it always returned true.
        /// </summary>
        public Func<bool> AdditionalRequirements { get; set; }

        /// <summary>
        /// Gets or sets the minimum delay in milliseconds between invokes of this <see cref="GameControl"/>. If the
        /// delay is greater than 0, then <see cref="Invoked"/> will never be raised until the specified amount of
        /// time has elapsed, no matter the key state. This value must be greater than or equal to zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
        public int Delay
        {
            get { return _delay; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _delay = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GameControlKeys"/> used for this <see cref="GameControl"/>. Cannot be null.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public GameControlKeys GameControlKeys
        {
            get { return _gckeys; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _gckeys = value;
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="GameControl"/> is enabled. If false, <see cref="Invoked"/> will never
        /// be raised.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                _needsReset = false;
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        protected virtual void OnInvoked()
        {
        }

        /// <summary>
        /// Updates the <see cref="GameControl"/>.
        /// </summary>
        /// <param name="guiManager">The <see cref="IGUIManager"/> to get the key states from.</param>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public void Update(IGUIManager guiManager, TickCount currentTime)
        {
            // Ensure the object is enabled and there is an invoke handler before checking the key states
            if (!IsEnabled || Invoked == null)
                return;

            // If there all the NewKeysDown are up, and we need to reset, then reset
            if (_needsReset)
            {
                if (GameControlKeys.NewKeysDown.IsEmpty() ||
                    !GameControlKeys.NewKeysDown.All(x => x.Key == Keyboard.Key.Unknown || guiManager.IsKeyDown(x.Key)))
                {
                    // All of the keys are up, so set the bool to false, allowing us to invoke again
                    _needsReset = false;
                }

                return;
            }

            // Check that enough time has elapsed since the last invoke
            if (currentTime - _lastInvokeTime < Delay)
                return;

            // Check the key states

            // Check if any of the keys required to be up are down
            if (GameControlKeys.KeysUp.Any(x => x.Key != Keyboard.Key.Unknown && guiManager.IsKeyDown(x.Key)))
                return;

            // Check that all of the keys required to be down are down
            if (!GameControlKeys.KeysDown.All(x => x.Key == Keyboard.Key.Unknown || guiManager.IsKeyDown(x.Key)))
                return;

            if (!GameControlKeys.NewKeysDown.All(x => x.Key == Keyboard.Key.Unknown || guiManager.IsKeyDown(x.Key)))
                return;

            // Check additional requirements
            if (AdditionalRequirements == null || !AdditionalRequirements())
                return;

            // All keys are in the needed state, so invoke the handler
            OnInvoked();

            if (Invoked != null)
                Invoked.Raise(this, EventArgs.Empty);

            // Set the timeout
            if (!GameControlKeys.NewKeysDown.IsEmpty())
                _needsReset = true;

            _lastInvokeTime = currentTime;
        }
    }
}