using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.ProgressBar"/>.
    /// </summary>
    public interface IToolBarProgressBarSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets if the <see cref="Tool"/>'s <see cref="NetGore.Editor.EditorTool.Tool.IsEnabled"/> state
        /// will be toggle by clicking this control.
        /// </summary>
        bool ClickToEnable { set; get; }

        /// <summary>
        /// Gets or sets a value representing the delay between each
        /// <see cref="System.Windows.Forms.ProgressBarStyle.Marquee"/> display update, in milliseconds.
        /// </summary>
        int MarqueeAnimationSpeed { set; get; }

        /// <summary>
        /// Gets or sets the upper bound of the range that is defined for this control.
        /// </summary>
        int Maximum { set; get; }

        /// <summary>
        /// Gets or sets the lower bound of the range that is defined for this control.
        /// </summary>
        int Minimum { set; get; }

        /// <summary>
        /// Gets the <see cref="System.Windows.Forms.ProgressBar"/>.
        /// </summary>
        ProgressBar ProgressBar { get; }

        /// <summary>
        /// Gets or sets the amount by which to increment the current value of the progress bar each time
        /// <see cref="IToolBarProgressBarSettings.PerformStep"/> is called.
        /// </summary>
        int Step { set; get; }

        /// <summary>
        /// Gets or sets the style of the progress bar.
        /// </summary>
        ProgressBarStyle Style { set; get; }

        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        int Value { set; get; }

        /// <summary>
        /// Advances the current position of the progress bar by the specified amount.
        /// </summary>
        /// <param name="value">The amount by which to increment the progress bar's current position.</param>
        void Increment(int value);

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the
        /// <see cref="IToolBarProgressBarSettings.Step"/> property.
        /// </summary>
        void PerformStep();
    }
}