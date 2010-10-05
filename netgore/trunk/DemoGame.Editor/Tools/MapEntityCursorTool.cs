using System.Collections.Generic;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.World;

namespace DemoGame.Editor
{
    public class MapEntityCursorTool : MapCursorToolBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEntityCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The tool manager.</param>
        public MapEntityCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Entity Cursor")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.SplitButton,
                DisabledImage = Resources.MapEntityCursorTool_Disabled,
                EnabledImage = Resources.MapEntityCursorTool_Enabled,
            };
        }

        /// <summary>
        /// When overridden in the derived class, handles setting up event listeners for a <see cref="IToolTargetContainer"/>.
        /// This will be invoked once for every <see cref="Tool"/> instance for every <see cref="IToolTargetContainer"/> available.
        /// When the <see cref="Tool"/> is newly added to the <see cref="ToolManager"/>, all existing <see cref="IToolTargetContainer"/>s
        /// will be sent through this method. As new ones are added while this <see cref="Tool"/> exists, those new
        /// <see cref="IToolTargetContainer"/>s will also be passed through. What events to listen to and on what instances is
        /// purely up to the derived <see cref="Tool"/>.
        /// Make sure that all attached event listeners are also removed in the <see cref="Tool.ToolTargetContainerRemoved"/> method.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerAdded(IToolTargetContainer c)
        {
            base.ToolTargetContainerAdded(c);

            var mapContainer = c.AsMapContainer();
            if (mapContainer == null)
                return;

            mapContainer.KeyUp += mapContainer_KeyUp;
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Handles the KeyUp event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void mapContainer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map;
            if (map == null)
                return;

            // Handle deletes
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                // Only delete when it is an Entity that is on this map
                List<object> removed = new List<object>();
                foreach (var x in SOM.SelectedObjects.OfType<Entity>())
                {
                    if (map.Entities.Contains(x))
                    {
                        x.Dispose();
                        removed.Add(x);
                    }
                }

                SOM.SetManySelected(SOM.SelectedObjects.Except(removed));
            }
        }
    }
}