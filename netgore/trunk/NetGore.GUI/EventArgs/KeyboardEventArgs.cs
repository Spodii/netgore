using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Delegate for handling events using the KeyboardEventArgs
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Event args</param>
    public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    /// <summary>
    /// Event data for keyboard related events
    /// </summary>
    public class KeyboardEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current state of the keyboard
        /// </summary>
        public readonly KeyboardState KeyboardState;

        /// <summary>
        /// Gets the keys related to the event
        /// </summary>
        public readonly IEnumerable<Keys> Keys;

        /// <summary>
        /// KeyboardEventArgs constructor
        /// </summary>
        /// <param name="keys">Keys related to the event</param>
        /// <param name="keyboardState">Current state of the keyboard</param>
        public KeyboardEventArgs(IEnumerable<Keys> keys, KeyboardState keyboardState)
        {
            Keys = keys;
            KeyboardState = keyboardState;
        }
    }
}