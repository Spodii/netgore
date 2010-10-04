using System;
using System.Linq;

namespace NetGore.Editor.EditorTool
{
    /// <summary>
    /// An implementation of <see cref="Tool"/> that displays a button on the <see cref="ToolBar"/>,
    /// and is enabled or disabled by clicking the <see cref="ToolBar"/> button.
    /// Will always use <see cref="ToolBarControlType.Button"/>.
    /// </summary>
    public abstract class ToggledButtonTool : Tool
    {
        /// <summary>
        /// Processes the <see cref="ToolSettings"/> passed to this class's object constructor, modifies it, and returns
        /// the modified <see cref="ToolSettings"/>. This way, it can force certain settings.
        /// </summary>
        /// <param name="settings">The <see cref="ToolSettings"/>.</param>
        /// <returns>The modified <see cref="ToolSettings"/>.</returns>
        static ToolSettings ProcessToolSettings(ToolSettings settings)
        {
            settings.ToolBarControlType = ToolBarControlType.Button;
            return settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggledButtonTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="settings">The <see cref="ToolSettings"/> to use to create this <see cref="Tool"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected ToggledButtonTool(ToolManager toolManager, ToolSettings settings)
            : base(toolManager, ProcessToolSettings(settings))
        {
            var btn = ToolBarControl.ControlSettings.AsButtonSettings();
            btn.CheckOnClick = true;
            btn.Checked = IsEnabled;
            btn.CheckedChanged += ToolBarButton_CheckedChanged;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="Tool.IsEnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected override void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            base.OnIsEnabledChanged(oldValue, newValue);

            // Ensure the Checked value on the control is the same as the IsEnabled value
            ToolBarControl.ControlSettings.AsButtonSettings().Checked = IsEnabled;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the <see cref="Tool.ToolBarControl"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void ToolBarButton_CheckedChanged(object sender, EventArgs e)
        {
            IsEnabled = ToolBarControl.ControlSettings.AsButtonSettings().Checked;
        }
    }
}