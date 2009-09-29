using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events from the <see cref="GameControl"/>.
    /// </summary>
    /// <param name="gameControl">The <see cref="GameControl"/> the event came from.</param>
    public delegate void GameControlEventHandler(GameControl gameControl);

    /// <summary>
    /// Handles checking for the state needed by a game control.
    /// </summary>
    public class GameControl
    {
        int _delay;
        GameControlKeys _gckeys;
        bool _isEnabled = true;
        int _lastInvokeTime;

        /// <summary>
        /// Notifies listeners that this <see cref="GameControl"/>'s key state requirements match the current key
        /// state and that it is ready to be handled.
        /// </summary>
        public event GameControlEventHandler OnInvoke;

        /// <summary>
        /// Gets or sets a Func containing any additional requirements for this <see cref="GameControl"/> to be invoked.
        /// If value must return true for <see cref="OnInvoke"/> to be raised. If this value is false,
        /// <see cref="OnInvoke"/> will not be raised and the internal delay counter wil not be altered. If this value
        /// is null, it will be treated the same as if it always returned true.
        /// </summary>
        public Func<bool> AdditionalRequirements { get; set; }

        /// <summary>
        /// Gets or sets the minimum delay in milliseconds between invokes of this <see cref="GameControl"/>. If the
        /// delay is greater than 0, then <see cref="OnInvoke"/> will never be raised until the specified amount of
        /// time has elapsed, no matter the key state. This value must be greater than or equal to zero.
        /// </summary>
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
        /// Gets or sets if the <see cref="GameControl"/> is enabled. If false, <see cref="OnInvoke"/> will never
        /// be raised.
        /// </summary>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameControl"/> class.
        /// </summary>
        /// <param name="keys">The GameControlKeys.</param>
        public GameControl(GameControlKeys keys)
        {
            GameControlKeys = keys;
        }

        /// <summary>
        /// Updates the <see cref="GameControl"/>.
        /// </summary>
        /// <param name="gui">The <see cref="GUIManagerBase"/> to get the key states from.</param>
        /// <param name="currentTime">The current time in milliseconds.</param>
        public void Update(GUIManagerBase gui, int currentTime)
        {
            // Ensure the object is enabled and there is an invoke handler before checking the key states
            if (!IsEnabled || OnInvoke == null)
                return;

            // Check that enough time has elapsed since the last invoke
            if (currentTime - _lastInvokeTime < Delay)
                return;

            // Check the key states
            KeyboardState ks = gui.KeyboardState;

            foreach (Keys key in GameControlKeys.KeysUp)
            {
                if (!ks.IsKeyUp(key))
                    return;
            }

            foreach (Keys key in GameControlKeys.KeysDown)
            {
                if (!ks.IsKeyDown(key))
                    return;
            }

            foreach (Keys key in GameControlKeys.NewKeysDown)
            {
                if (!gui.NewKeysDown.Contains(key))
                    return;
            }

            foreach (Keys key in GameControlKeys.NewKeysUp)
            {
                if (!gui.NewKeysUp.Contains(key))
                    return;
            }

            // Check additional requirements
            if (AdditionalRequirements == null || !AdditionalRequirements())
                return;

            // All keys are in the needed state, so invoke the handler
            OnInvoke(this);
            _lastInvokeTime = currentTime;
        }
    }
}