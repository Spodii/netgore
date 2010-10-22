using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using log4net;
using NetGore.Editor.EditorTool;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor.Tools
{
    public class MapDeleteTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDeleteTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapDeleteTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Delete the currently selected map";
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

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
            };
        }
    }
}