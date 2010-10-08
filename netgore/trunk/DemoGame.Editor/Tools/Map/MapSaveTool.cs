using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using log4net;
using NetGore.Editor.EditorTool;
using NetGore.IO;
using SFML.Graphics;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    public class MapSaveTool : Tool
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            Map map = null;

            try
            {
                var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
                if (tb == null)
                    return;

                map = tb.DisplayObject as Map;
                if (map == null)
                    return;

                const string confirmMsg = "Are you sure you wish to save the changes to map `{0}`?";
                if (MessageBox.Show(string.Format(confirmMsg, map), "Save map?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                // Add the MapGrh-bound walls
                var extraWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(map.MapGrhs);
                foreach (var wall in extraWalls)
                {
                    map.AddEntity(wall);
                }

                // Save the map
                map.Save(ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);

                // Pull the MapGrh-bound walls back out
                foreach (var wall in extraWalls)
                {
                    map.RemoveEntity(wall);
                }

                const string savedMsg = "Successfully saved the changes to map `{0}`!";
                MessageBox.Show(string.Format(savedMsg, map), "Map saved", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to save map `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, map, ex);
                Debug.Fail(string.Format(errmsg, map, ex));
            }
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Grid Drawer")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapSaveTool,
            };
        }
    }
}