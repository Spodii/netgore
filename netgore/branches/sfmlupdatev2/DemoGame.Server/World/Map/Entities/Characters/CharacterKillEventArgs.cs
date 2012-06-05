using System;
using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a <see cref="Character"/> attacks.
    /// </summary>
    public class CharacterKillEventArgs : EventArgs
    {
        readonly Character _killer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKillEventArgs"/> class.
        /// </summary>
        /// <param name="killer">The <see cref="Character"/> that killed the sender.</param>
        public CharacterKillEventArgs(Character killer)
        {
            _killer = killer;
        }

        /// <summary>
        /// Gets the <see cref="Character"/> that killed the sender.
        /// </summary>
        public Character Killer
        {
            get { return _killer; }
        }
    }
}