using System;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor.Tools
{
    public class MapSaveAsTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSaveAsTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapSaveAsTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Saves the currently selected map as a new map";
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        static void ControlSettings_Click(object sender, EventArgs e)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            MapHelper.SaveMapAs(tb.DisplayObject as EditorMap);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Save As")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapSaveAsTool,
                HelpName = "Map Save As Tool",
                HelpWikiPage = "Map save as tool",
            };
        }
    }
}