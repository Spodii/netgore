using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapWallCursorTool : MapCursorToolBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapWallCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapWallCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.AsButtonSettings().ClickToEnable = true;
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this cursor can select the given object.
        /// </summary>
        /// <param name="map">The map containing the object to be selected.</param>
        /// <param name="obj">The object to try to select.</param>
        /// <returns>
        /// True if the <paramref name="obj"/> can be selected and handled by this cursor; otherwise false.
        /// </returns>
        protected override bool CanSelect(EditorMap map, object obj)
        {
            return (obj is WallEntityBase) && base.CanSelect(map, obj);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Wall Cursor")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapWallCursorTool_Disabled,
                EnabledImage = Resources.MapWallCursorTool_Enabled,
                HelpName = "Map Wall Cursor",
                HelpWikiPage = "Map wall cursor tool",
            };
        }

        /// <summary>
        /// Handles when a key is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_KeyUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
            // Handle delete
            if (e.KeyCode == Keys.Delete)
            {
                // Only delete when it is an Entity that is on this map
                var removed = new List<object>();
                foreach (var x in SOM.SelectedObjects.OfType<WallEntityBase>().ToImmutable())
                {
                    if (map.Entities.Contains(x))
                    {
                        x.Dispose();
                        removed.Add(x);
                    }
                }

                SOM.SetManySelected(SOM.SelectedObjects.Except(removed).ToImmutable());
            }

            base.MapContainer_KeyUp(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse button is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            var cursorPos = e.Position();
            var worldPos = camera.ToWorld(cursorPos);

            if (e.Button == MouseButtons.Right)
            {
                var ga = GridAligner.Instance;

                var entity = new WallEntity(worldPos, ga.GridSize);

                ga.Fit(entity);

                map.AddEntity(entity);
                SOM.SetSelected(entity);
            }

            base.MapContainer_MouseDown(sender, map, camera, e);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="Tool.IsEnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected override void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            base.OnIsEnabledChanged(oldValue, newValue);

            HandleResetState();
        }
    }
}