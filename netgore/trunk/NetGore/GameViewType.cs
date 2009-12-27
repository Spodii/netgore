using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Describes the different types of supported view perspectives for games.
    /// </summary>
    public enum GameViewType
    {
        /// <summary>
        /// A side view on the world. Gravity pulls entities down, and movement is done to the left and right.
        /// Also known as a platformer.
        /// </summary>
        Sidescroller,

        /// <summary>
        /// A top-down view on the world. No gravity exists, and entities can move in any direction. This is the
        /// typical 2d RPG view.
        /// </summary>
        TopDown,
    }
}
