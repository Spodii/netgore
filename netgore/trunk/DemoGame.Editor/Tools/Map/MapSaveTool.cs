using System;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor.Tools
{
    public class MapSaveTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSaveTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapSaveTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Saves the currently selected map";
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        static void ControlSettings_Click(object sender, EventArgs e)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            MapHelper.SaveMap(tb.DisplayObject as EditorMap);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Save")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapSaveTool,
                HelpName = "Map Save Tool",
                HelpWikiPage = "Map save tool",
            };
        }
    }
}