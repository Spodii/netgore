using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Interface for a tooltip that displays pop-up text for <see cref="Control"/>s after they have been hovered
    /// over by the cursor for the appropriate amount of time.
    /// </summary>
    public interface ITooltip
    {
        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the background of this <see cref="ITooltip"/>. This value
        /// is only valid for when the <see cref="ITooltip.Border"/> is null.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ControlBorder"/> for this <see cref="ITooltip"/>. Can be null.
        /// </summary>
        ControlBorder Border { get; set; }

        /// <summary>
        /// Gets or sets number of pixels the border is padded on each side, where the X value corresponds to the
        /// number of pixels padded on the left and right side, and the Y value corresponds to the top and bottom.
        /// </summary>
        Vector2 BorderPadding { get; set; }

        /// <summary>
        /// Gets or sets the delay in milliseconds for the tooltip to be shown once a <see cref="Control"/> is
        /// hovered over. If this value is zero, the tooltip will be displayed immediately. If this value is set
        /// to less than zero, zero will be used instead.
        /// </summary>
        int Delay { get; set; }

        /// <summary>
        /// Gets or sets the offset from the cursor position to draw the <see cref="ITooltip"/>.
        /// </summary>
        Vector2 DrawOffset { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Font"/> used by this <see cref="ITooltip"/>. By default, this value will
        /// be equal to the <see cref="IGUIManager"/>'s Font. Cannot be null.
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> to draw the text when no color is specified.
        /// </summary>
        Color FontColor { get; set; }

        /// <summary>
        /// Gets the <see cref="IGUIManager"/> that this <see cref="ITooltip"/> is for.
        /// </summary>
        IGUIManager GUIManager { get; }

        /// <summary>
        /// Gets if the <see cref="ITooltip"/> is currently being displayed.
        /// </summary>
        bool IsDisplayed { get; }

        /// <summary>
        /// Gets or sets if the <see cref="ITooltip"/> is currently visible. If false, the <see cref="ITooltip"/> will
        /// not make requests for the tooltip text from <see cref="Control"/>s and will not be drawn.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the <see cref="ITooltip"/>.
        /// </summary>
        int MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the delay in milliseconds between retry attempts in getting the tooltip text. This
        /// only applies to when the <see cref="Control"/> returns a null collection for the tooltip text.
        /// </summary>
        int RetryGetTooltipDelay { get; set; }

        /// <summary>
        /// Gets or sets the timeout in milliseconds for the tooltip. After the tooltip has been displayed
        /// for the given amount of time, it will hide. If this value is zero, the tooltip will be displayed
        /// indefinitely. If the value is set to less than zero, zero will be used instead.
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Draws the <see cref="ITooltip"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw the <see cref="ITooltip"/> with.</param>
        void Draw(ISpriteBatch sb);

        /// <summary>
        /// Updates the <see cref="ITooltip"/>.
        /// </summary>
        /// <param name="currentTime">The current time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}