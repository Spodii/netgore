using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Db;
using NetGore.Editor.EditorTool;
using NetGore.IO;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    public class MapSaveAsTool : Tool
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSaveAsTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapSaveAsTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Saves the currently selected map as a new map";
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

                var newID = MapBase.GetNextFreeIndex(ContentPaths.Dev);

                const string confirmMsg = "Are you sure you wish to save map `{0}` as a new map (with ID `{1}`)?";
                if (MessageBox.Show(string.Format(confirmMsg, map, newID), "Save map as?", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                map.ChangeID(newID);

                // Add the MapGrh-bound walls
                var extraWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(map.MapGrhs);
                foreach (var wall in extraWalls)
                {
                    map.AddEntity(wall);
                }

                // Save the map
                map.Save(ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);

                DbControllerBase.GetInstance().GetQuery<InsertMapQuery>().Execute(map);

                // Pull the MapGrh-bound walls back out
                foreach (var wall in extraWalls)
                {
                    map.RemoveEntity(wall);
                }

                const string savedMsg = "Successfully saved the map `{0}` as a new map!";
                MessageBox.Show(string.Format(savedMsg, map), "Map successfully saved as a new map", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to save map `{0}` as a new map. Exception: {1}";
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
            return new ToolSettings("Map Save As")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapSaveAsTool,
            };
        }
    }
}