using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class MapGrhCursorTool : MapCursorToolBase
    {
        const Keys _placeMapGrhKey = Keys.Shift;

        Map _mouseOverMap;
        Vector2 _mousePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapGrhCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.AsSplitButtonSettings().ClickToEnable = true;
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Grh Cursor")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.SplitButton,
                DisabledImage = Resources.MapGrhCursorTool_Disabled,
                EnabledImage = Resources.MapGrhCursorTool_Enabled,
            };
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing after the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleAfterDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap map)
        {
            base.HandleAfterDrawMapGUI(spriteBatch, map);

            if (map != _mouseOverMap)
                return;

            if ((Control.ModifierKeys & _placeMapGrhKey) == 0)
                return;

            var grh = GlobalState.Instance.Map.GrhToPlace;
            if (grh != null)
                grh.Draw(spriteBatch, _mousePos);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling resetting the state of this <see cref="Tool"/>.
        /// For simplicity, all default values should be constant, no matter the current state.
        /// </summary>
        protected override void HandleResetState()
        {
            base.HandleResetState();

            _mouseOverMap = null;
            _mousePos = Vector2.Zero;
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

            mapContainer.MouseDown += mapContainer_MouseDown;
            mapContainer.MouseMove += mapContainer_MouseMove;
            mapContainer.MouseWheel += mapContainer_MouseWheel;
            mapContainer.KeyUp += mapContainer_KeyUp;
        }

        void mapContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            var grh = GlobalState.Instance.Map.GrhToPlace;
            if (grh == null || grh.GrhData == null)
                return;

            var drawPos = camera.ToWorld(e.Position());
            var selGrhGrhIndex = grh.GrhData.GrhIndex;

            if (map.MapGrhs.Any(x => x.Position == drawPos && x.Grh.GrhData.GrhIndex == selGrhGrhIndex))
                return;

            var g = new Grh(grh.GrhData, AnimType.Loop, map.GetTime());
            var mg = new MapGrh(g, drawPos, false);
            map.AddMapGrh(mg);
        }

        /// <summary>
        /// When overridden in the derived class, handles tearing down event listeners for a <see cref="IToolTargetContainer"/>.
        /// Any event listeners set up in <see cref="Tool.ToolTargetContainerAdded"/> should be torn down here.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerRemoved(IToolTargetContainer c)
        {
            base.ToolTargetContainerRemoved(c);

            var mapContainer = c.AsMapContainer();
            if (mapContainer == null)
                return;

            mapContainer.MouseDown -= mapContainer_MouseDown;
            mapContainer.MouseMove -= mapContainer_MouseMove;
            mapContainer.MouseWheel -= mapContainer_MouseWheel;
            mapContainer.KeyUp -= mapContainer_KeyUp;
        }

        void mapContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (!IsEnabled)
                return;

            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map as Map;
            if (map == null)
                return;

            // Handle deletes
            if (e.KeyCode == Keys.Delete)
            {
                // Only delete when it is an Entity that is on this map
                foreach (var x in SOM.SelectedObjects.OfType<MapGrh>())
                {
                    if (map.Spatial.Contains(x))
                        map.RemoveMapGrh(x);
                }
            }
        }

        void mapContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map as Map;
            if (map == null)
                return;

            var cursorPos = e.Position();

            _mouseOverMap = map;
            _mousePos = cursorPos;
        }

        void mapContainer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (e.Delta == 0)
                return;

            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map;
            if (map == null)
                return;

            // Only change depth on selected MapGrh if only one is selected
            var focusedMapGrh = GlobalState.Instance.Map.SelectedObjsManager.SelectedObjects.FirstOrDefault() as MapGrh;
            if (focusedMapGrh == null)
                return;

            // Require the MapGrh to be on the map the scroll event took place on
            if (!map.Spatial.Contains(focusedMapGrh))
                return;

            // Change layer depth, making sure it is clamped in the needed range
            focusedMapGrh.LayerDepth = (short)(focusedMapGrh.LayerDepth + e.Delta).Clamp(short.MinValue, short.MaxValue);
        }
    }
}