using System;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor.Tools
{
    public class MapSaveAsTool : MapToolBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapSaveAsTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapSaveAsTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Saves the currently selected map as a new map (Ctrl+Shift+S)";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
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
        /// Handles when a key is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_KeyDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
            if (map != null)
            {
                // Save (Ctrl + Shift + S)
                if (e.KeyCode == Keys.S && e.Control && e.Shift)
                {
                    MapHelper.SaveMapAs(map, false);
                    return;
                }
            }

            base.MapContainer_KeyDown(sender, map, camera, e);
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