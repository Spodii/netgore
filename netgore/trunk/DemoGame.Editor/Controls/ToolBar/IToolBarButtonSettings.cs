namespace DemoGame.Editor
{
    /// <summary>
    /// Contains the settings specific to the <see cref="ToolBarControlType.Button"/>.
    /// </summary>
    public interface IToolBarButtonSettings : IToolBarControlSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control should automatically appear pressed in and not
        /// pressed in when clicked.
        /// </summary>
        bool CheckOnClick { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is in the pressed or not pressed state by default,
        /// or is in an indeterminate state.
        /// </summary>
        System.Windows.Forms.CheckState CheckState { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control is pressed or not pressed.
        /// </summary>
        bool Checked { set; get; }

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarButtonSettings.CheckState"/> property changes.
        /// </summary>
        event System.EventHandler CheckStateChanged;

        /// <summary>
        /// Notifies listeners when the <see cref="IToolBarButtonSettings.Checked"/> property changes.
        /// </summary>
        event System.EventHandler CheckedChanged;
    }
}