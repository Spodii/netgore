using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.IO;
using NetGore.Editor.EditorTool;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that deletes the current map.
    /// </summary>
    public class MapPreviewTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapPreviewTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Saves a preview image of the map to file";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        void ControlSettings_Click(object sender, EventArgs e)
        {
            SaveMapPreview();
        }

        /// <summary>
        /// Automatically saves an image of the map in it's mini-map format.
        /// </summary>
        void SaveMapPreview()
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            var map = tb.DisplayObject as Map;
            if (map == null)
                return;

            var filePath = ContentPaths.Dev.Grhs.Join("MiniMap\\" + map.ID + EngineSettings.ImageFileSuffix);

            var mp = new MapPreviewer();
            mp.CreatePreview(map, ToolManager.MapDrawingExtensions, filePath);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Preview")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapPreviewTool,
                HelpName = "Map Preview Tool",
                HelpWikiPage = "Map preview tool",
            };
        }
    }
}