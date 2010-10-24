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
    public class MapGrhCursorTool : MapCursorToolBase
    {
        const Keys _placeMapGrhKey = Keys.Control;

        EditorMap _mouseOverMap;
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
        /// When overridden in the derived class, gets if this cursor can select the given object.
        /// </summary>
        /// <param name="map">The map containing the object to be selected.</param>
        /// <param name="obj">The object to try to select.</param>
        /// <returns>
        /// True if the <paramref name="obj"/> can be selected and handled by this cursor; otherwise false.
        /// </returns>
        protected override bool CanSelect(EditorMap map, object obj)
        {
            return (obj is MapGrh) && base.CanSelect(map, obj);
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
            grh.Draw(spriteBatch, GridAligner.Instance.Align(_mousePos));
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
        /// Handles when a key is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_KeyUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
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

            base.MapContainer_KeyUp(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when a mouse button is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera,
                                                       MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Left-click

                if ((Control.ModifierKeys & _placeMapGrhKey) != 0)
                {
                    // Place MapGrh

                    var gd = GlobalState.Instance.Map.GrhToPlace.GrhData;
                    if (gd == null)
                        return;

                    var drawPos = camera.ToWorld(e.Position());
                    drawPos = GridAligner.Instance.Align(drawPos);
                    var selGrhGrhIndex = gd.GrhIndex;

                    // Make sure the same GrhData doesn't already exist at that position
                    if (map.MapGrhs.Any(x => x.Position == drawPos && x.Grh.GrhData.GrhIndex == selGrhGrhIndex))
                        return;

                    var g = new Grh(gd, AnimType.Loop, map.GetTime());
                    var mg = new MapGrh(g, drawPos, false);
                    map.AddMapGrh(mg);
                }
            }

            base.MapContainer_MouseDown(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse moves over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseMove(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera,
                                                       MouseEventArgs e)
        {
            base.MapContainer_MouseMove(sender, map, camera, e);

            var cursorPos = e.Position();

            _mouseOverMap = map;
            _mousePos = cursorPos;
        }

        /// <summary>
        /// Handles when the mouse wheel is moved while over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseWheel(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera,
                                                        MouseEventArgs e)
        {
            if (e.Delta == 0)
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

            base.MapContainer_MouseWheel(sender, map, camera, e);
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