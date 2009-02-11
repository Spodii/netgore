using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Contains the different ways a TextBox can handle automatic scrolling when text is appended or removed.
    /// </summary>
    public enum TextBoxAutoScrollMode
    {
        /// <summary>
        /// Makes the TextBox to stay at the current line of text.
        /// </summary>
        None,

        /// <summary>
        /// Makes the TextBox always scroll to view the last line of text when text is appended or removed.
        /// </summary>
        AlwaysScroll,

        /// <summary>
        /// Makes the TextBox scroll to view the last line of text when text is appended or removed only if
        /// the TextBox is currently already at the last line of text.
        /// </summary>
        ScrollIfCurrent
    }
}