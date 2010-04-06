using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Contains the arguments used for altering the behavior of the <see cref="ITooltip"/>.
    /// </summary>
    public class TooltipArgs
    {
        int _refreshRate;
        int _timeout;

        /// <summary>
        /// Gets or sets the <see cref="Color"/> of the background. Only used if the <see cref="Border"/> is not set.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> to use for drawing the backdrop of the <see cref="ITooltip"/>.
        /// </summary>
        public ControlBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> of the text for when no color is explicitly given for the text.
        /// </summary>
        public Color FontColor { get; set; }

        /// <summary>
        /// Gets or sets the rate in milliseconds at which the text will be refreshed. A refresh rate of zero will
        /// result in the text never being refreshed. Any value less than zero will use zero instead.
        /// </summary>
        public int RefreshRate
        {
            get { return _refreshRate; }
            set { _refreshRate = Math.Max(0, value); }
        }

        /// <summary>
        /// Gets or sets the timeout time for the <see cref="ITooltip"/>.
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = Math.Max(0, value); }
        }

        /// <summary>
        /// Restores the default values.
        /// </summary>
        /// <param name="tooltip">The <see cref="ITooltip"/>.</param>
        public void RestoreDefaults(ITooltip tooltip)
        {
            FontColor = tooltip.FontColor;
            BackgroundColor = tooltip.BackgroundColor;
            Border = tooltip.Border;
            Timeout = tooltip.Timeout;
            RefreshRate = 0;
        }
    }
}