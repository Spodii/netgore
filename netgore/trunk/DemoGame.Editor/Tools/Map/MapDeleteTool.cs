using System;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that deletes the current map.
    /// </summary>
    public class MapDeleteTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDeleteTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapDeleteTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Delete the currently selected map";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        /// <summary>
        /// Handles the Click event of the ControlSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void ControlSettings_Click(object sender, EventArgs e)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            MapHelper.DeleteMap(tb.DisplayObject as EditorMap);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Delete")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapDeleteTool,
                HelpName = "Map Delete Tool",
                HelpWikiPage = "Map delete tool",
            };
        }
    }
}