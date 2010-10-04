using System;
using System.Linq;

namespace NetGore.EditorTools
{
    /// <summary>
    /// An implementation of <see cref="Tool"/> that displays a button on the <see cref="ToolBar"/>,
    /// and is enabled or disabled by clicking the <see cref="ToolBar"/> button.
    /// Will always use <see cref="ToolBarControlType.Button"/>.
    /// </summary>
    public abstract class ToggledButtonTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggledButtonTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="name">The name of the tool.</param>
        /// <param name="toolBarVisibility">The visibility of this <see cref="Tool"/> in a <see cref="ToolBar"/>.</param>
        protected ToggledButtonTool(ToolManager toolManager, string name, ToolBarVisibility toolBarVisibility)
            : base(toolManager, name, ToolBarControlType.Button, toolBarVisibility)
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
            ToolBarControl.ControlSettings.AsButtonSettings().Checked = newValue;
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