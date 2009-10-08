using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Event data for keyboard related events.
    /// </summary>
    public class KeyboardEventArgs : EventArgs
    {
        /// <summary>
        /// The current state of the keyboard.
        /// </summary>
        public readonly KeyboardState KeyboardState;

        /// <summary>
        /// The keys related to the event.
        /// </summary>
        public readonly IEnumerable<Keys> Keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardEventArgs"/> class.
        /// </summary>
        /// <param name="keys">Keys related to the event.</param>
        /// <param name="keyboardState">Current state of the keyboard.</param>
        public KeyboardEventArgs(IEnumerable<Keys> keys, KeyboardState keyboardState)
        {
            Keys = keys;
            KeyboardState = keyboardState;
        }
    }
}