using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore.IO;
using NetGore.World;
using log4net;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapGrhPencilTool : MapCursorToolBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        EditorMap _mouseOverMap;
        Vector2 _mousePos;

        /// <summary>
        /// Gets if the given key is used for performing a selection.
        /// </summary>
        public override bool IsSelectKey(Keys key)
        {
            return (key & Keys.Alt) != 0;
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh0
        {
            get { return GlobalState.Instance.HotkeyedGrhs[0]; }
            set { GlobalState.Instance.HotkeyedGrhs[0] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh1
        {
            get { return GlobalState.Instance.HotkeyedGrhs[1]; }
            set { GlobalState.Instance.HotkeyedGrhs[1] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh2
        {
            get { return GlobalState.Instance.HotkeyedGrhs[2]; }
            set { GlobalState.Instance.HotkeyedGrhs[2] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh3
        {
            get { return GlobalState.Instance.HotkeyedGrhs[3]; }
            set { GlobalState.Instance.HotkeyedGrhs[3] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh4
        {
            get { return GlobalState.Instance.HotkeyedGrhs[4]; }
            set { GlobalState.Instance.HotkeyedGrhs[4] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh5
        {
            get { return GlobalState.Instance.HotkeyedGrhs[5]; }
            set { GlobalState.Instance.HotkeyedGrhs[5] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh6
        {
            get { return GlobalState.Instance.HotkeyedGrhs[6]; }
            set { GlobalState.Instance.HotkeyedGrhs[6] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh7
        {
            get { return GlobalState.Instance.HotkeyedGrhs[7]; }
            set { GlobalState.Instance.HotkeyedGrhs[7] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh8
        {
            get { return GlobalState.Instance.HotkeyedGrhs[8]; }
            set { GlobalState.Instance.HotkeyedGrhs[8] = value; }
        }

        [SyncValue, DefaultValue(null)]
        public string HotkeyedGrh9
        {
            get { return GlobalState.Instance.HotkeyedGrhs[9]; }
            set { GlobalState.Instance.HotkeyedGrhs[9] = value; }
        }

        public MapGrhPencilTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsButtonSettings();
            s.ClickToEnable = true;
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
            return false;
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Grh Pencil")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapGrhPencilTool_Disabled,
                EnabledImage = Resources.MapGrhPencilTool_Enabled,
                HelpName = "Map Grh Pencil Cursor",
                HelpWikiPage = "Map grh pencil tool",
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

            if (!IsEnabled)
                return;

            if (map != _mouseOverMap)
                return;

            var grh = GlobalState.Instance.Map.GrhToPlace;
			var worldPos = map.Camera.ToWorld(_mousePos);
            var drawPos = worldPos - map.Camera.Min;
            if (!Input.IsCtrlDown)
                drawPos = (GridAligner.Instance.Align(worldPos, true) - map.Camera.Min).Round();

            if (Input.IsShiftDown)
            {
                // Display tooltip of what would be selected
                var grhToSelect = GetGrhToSelect(map, worldPos);
                DrawMapGrhTooltip(spriteBatch, map, grhToSelect, worldPos);
            }
            else
            {
                // Display mapgrh to draw
                grh.Update(map.GetTime());
                grh.Draw(spriteBatch, drawPos, new Color(255, 255, 255, 180));
            }
        }

        public static bool DrawMapGrhTooltip(ISpriteBatch spriteBatch, IDrawableMap map, MapGrh mapGrh, Vector2 worldPos)
        {
            if (mapGrh == null || mapGrh.Grh == null || mapGrh.Grh.GrhData == null)
                return false;

            Vector2 drawPos = worldPos - map.Camera.Min;
            var font = GlobalState.Instance.DefaultRenderFont;
            string txt = mapGrh.Grh.GrhData.Categorization.ToString();
            Vector2 txtSize = font.MeasureString(txt);
            Vector2 txtPos = drawPos.Max(Vector2.Zero).Min(map.Camera.Size - txtSize);
            spriteBatch.DrawStringShaded(font, txt, txtPos, Color.White, Color.Black);

            return true;
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
        /// Gets the MapGrh to select at the given position.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="worldPos">The world position.</param>
        /// <param name="mustBeOnLayer">If not null, then only return a MapGrh on the specified layer.</param>
        /// <returns>The best MapGrh to be selected, or null if none found.</returns>
        public static MapGrh GetGrhToSelect(IDrawableMap map, Vector2 worldPos, MapRenderLayer? mustBeOnLayer = null)
        {
            // Get all MapGrhs as the position
            var mapGrhs = map.Spatial.GetMany<MapGrh>(worldPos);

            // Filter layer
            if (mustBeOnLayer.HasValue)
                mapGrhs = mapGrhs.Where(x => x.MapRenderLayer == mustBeOnLayer.Value);

            // Prioritize by draw order & distance from origin, then take the first
            mapGrhs = mapGrhs
                .OrderByDescending(x => (int)x.MapRenderLayer)
                .ThenByDescending(x => x.LayerDepth)
                .ThenBy(x => worldPos.QuickDistance(x.Position));

            return mapGrhs.FirstOrDefault();
        }

        /// <summary>
        /// Handles both mouse clicks and moves (does the place/deletes of grhs).
        /// </summary>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        void HandleMouseClickAndMove(EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            // Update some vars
            var cursorPos = e.Position();
            _mouseOverMap = map;
            _mousePos = cursorPos;

            // Don't do any place/deletes while selecting
            if (IsSelecting)
                return;

            Vector2 worldPos = camera.ToWorld(cursorPos);

            // Handle mouse
            if (e.Button == MouseButtons.Left)
            {
                if (!Input.IsShiftDown)
                {
                    // Place grh
                    PlaceMapGrhAtScreenPos(map, camera, cursorPos, !Input.IsCtrlDown);
                }
                else
                {
                    // Select grh under cursor
                    var grhToSelect = GetGrhToSelect(map, worldPos);
                    if (grhToSelect != null)
                    {
                        GlobalState.Instance.Map.SetGrhToPlace(grhToSelect.Grh.GrhData.GrhIndex);
                        GlobalState.Instance.Map.Layer = grhToSelect.MapRenderLayer;
                        GlobalState.Instance.Map.LayerDepth = grhToSelect.LayerDepth;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!Input.IsShiftDown)
                {
                    // Delete from current layer
                    var grhToDelete = GetGrhToSelect(map, worldPos, mustBeOnLayer: GlobalState.Instance.Map.Layer);
                    if (grhToDelete != null)
                        map.RemoveMapGrh(grhToDelete);
                }
                else
                {
                    // Delete from all layers
                    var grhToSelect = GetGrhToSelect(map, worldPos);
                    if (grhToSelect != null)
                        map.RemoveMapGrh(grhToSelect);
                }
            }
        }

        /// <summary>
        /// Handles when a mouse button is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            HandleMouseClickAndMove(map, camera, e);
            base.MapContainer_MouseDown(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse moves over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseMove(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            // Treat as a click unless we're placing grhs not on the grid. Otherwise, that would cause us to place a ton, which is likely not what we want.
            if (!(Input.IsCtrlDown && (e.Button & MouseButtons.Left) != 0))
            {
                HandleMouseClickAndMove(map, camera, e);
            }

            base.MapContainer_MouseMove(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse wheel is moved while over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseWheel(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            // Change the current depth
            int modDepth = 0;
            if (e.Delta < 0)
                modDepth = -1;
            else if (e.Delta > 0)
                modDepth = 1;

            if (modDepth != 0)
            {
                var currDepth = GlobalState.Instance.Map.LayerDepth;
                short newDepth = (short)(currDepth + modDepth).Clamp(short.MinValue, short.MaxValue);
                GlobalState.Instance.Map.LayerDepth = newDepth;
            }

            base.MapContainer_MouseWheel(sender, map, camera, e);
        }

        /// <summary>
        /// Handles what happens when an area of the map is selected.
        /// </summary>
        protected override void MapContainer_AreaSelected(EditorMap map, ICamera2D camera, Rectangle selectionArea, MouseEventArgs e)
        {
            var gridAligner = GridAligner.Instance;

            if ((e.Button & MouseButtons.Left) != 0)
            {
                var grhDataToPlace = GlobalState.Instance.Map.GrhToPlace.GrhData;

                if (grhDataToPlace == null)
                    return;

                // Place sprites over the selection area
                if ((Control.ModifierKeys & Keys.Control) == 0)
                {
                    // Place on grid
                    Vector2 startPos = gridAligner.Align(new Vector2(selectionArea.Left, selectionArea.Top));
                    Vector2 endPos = gridAligner.Align(new Vector2(selectionArea.Right, selectionArea.Bottom));

                    for (int x = (int)startPos.X; x <= endPos.X; x += (int)gridAligner.GridSize.X)
                    {
                        for (int y = (int)startPos.Y; y <= endPos.Y; y += (int)gridAligner.GridSize.Y)
                        {
                            PlaceMapGrhAtWorldPos(map, new Vector2(x, y), true, grhDataToPlace);
                        }
                    }
                }
                else
                {
                    // Place freely
                    Vector2 startPos = new Vector2(selectionArea.Left, selectionArea.Top);
                    Vector2 endPos = new Vector2(selectionArea.Right, selectionArea.Bottom) - grhDataToPlace.Size;

                    for (int x = (int)startPos.X; x <= (int)endPos.X; x += (int)grhDataToPlace.Size.X)
                    {
                        for (int y = (int)startPos.Y; y <= (int)endPos.Y; y += (int)grhDataToPlace.Size.Y)
                        {
                            PlaceMapGrhAtWorldPos(map, new Vector2(x, y), false, grhDataToPlace);
                        }
                    }
                }
            }
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
        /// Handles the KeyDown event of this Cursor tool.  If the Control key is down then it will shift any selected <see cref="ISpatial"/>'s to the direction of the pressed arrow keys.
        /// </summary>
        protected override void MapContainer_KeyDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
            if (!e.Alt && !e.Shift && !e.Control)
            {
                int? num = e.KeyCode.GetNumericKeyAsValue();
                if (num.HasValue && num.Value > 0 && num.Value < 10)
                {
                    GlobalState.Instance.SetGrhFromHotkey(num.Value);
                }
            }

            base.MapContainer_KeyDown(sender, map, camera, e);
        }

        /// <summary>
        /// Places a <see cref="MapGrh"/> on the map.
        /// </summary>
        /// <param name="map">The map to place the <see cref="MapGrh"/> on.</param>
        /// <param name="worldPos">The world position to place the <see cref="MapGrh"/>.</param>
        /// <param name="useTileMode">If TileMode should be used.</param>
        /// <param name="gd">The <see cref="GrhData"/> to place. Set to null to attempt to use the <see cref="GrhData"/> that is
        /// currently selected in the <see cref="GlobalState"/>.</param>
        /// <returns>The <see cref="MapGrh"/> instance that was added, or null if the the <see cref="MapGrh"/> could not be
        /// added for any reason.</returns>
        public static MapGrh PlaceMapGrhAtWorldPos(Map map, Vector2 worldPos, bool useTileMode, GrhData gd = null)
        {
            // Get the GrhData to place from the global state
            if (gd == null)
                gd = GlobalState.Instance.Map.GrhToPlace.GrhData;

            // Ensure we have a GrhData to place
            if (gd == null)
                return null;

            worldPos = GridAligner.Instance.Align(worldPos, useTileMode).Round();

            // Cache some other values
            var selGrhGrhIndex = gd.GrhIndex;
            var layer = GlobalState.Instance.Map.Layer;
            var depth = GlobalState.Instance.Map.LayerDepth;
            var drawPosArea = worldPos.ToRectangle(new Vector2(2), true);

            if (!useTileMode)
            {
                // Make sure the same MapGrh doesn't already exist at that position
                if (map.Spatial.Contains<MapGrh>(drawPosArea, x =>
                    x.Grh.GrhData.GrhIndex == selGrhGrhIndex && x.MapRenderLayer == layer &&
                    Math.Round(x.Position.QuickDistance(worldPos)) <= 1))
                {
                    return null;
                }
            }
            else
            {
                // Make sure the same MapGrh doesn't already exist at that position on the same layer
                if (map.Spatial.Contains<MapGrh>(drawPosArea, x =>
                    x.Grh.GrhData.GrhIndex == selGrhGrhIndex && x.MapRenderLayer == layer &&
                    Math.Round(x.Position.QuickDistance(worldPos)) <= 1))
                {
                    return null;
                }

                // In TileMode, do not allow ANY MapGrh at the same position and layer depth. And if it does exist, instead of aborting,
                // delete the existing one.
                var existingMapGrhs = map.Spatial.GetMany<MapGrh>(drawPosArea, x =>
                    x.LayerDepth == depth && x.MapRenderLayer == layer && Math.Round(x.Position.QuickDistance(worldPos)) <= 1);

                foreach (var toDelete in existingMapGrhs)
                {
                    Debug.Assert(toDelete != null);
                    if (log.IsDebugEnabled)
                        log.DebugFormat("TileMode caused MapGrh `{0}` to be overwritten.", toDelete);

                    map.RemoveMapGrh(toDelete);
                }
            }

            // Create the new MapGrh and add it to the map
            var g = new Grh(gd, AnimType.Loop, map.GetTime());
            var mg = new MapGrh(g, worldPos) { LayerDepth = depth, MapRenderLayer = layer };
            map.AddMapGrh(mg);

            return mg;
        }

        /// <summary>
        /// Places a <see cref="MapGrh"/> on the map.
        /// </summary>
        /// <param name="map">The map to place the <see cref="MapGrh"/> on.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>.</param>
        /// <param name="screenPos">The screen position to place the <see cref="MapGrh"/>.</param>
        /// <param name="useTileMode">If TileMode should be used.</param>
        /// <param name="gd">The <see cref="GrhData"/> to place. Set to null to attempt to use the <see cref="GrhData"/> that is
        /// currently selected in the <see cref="GlobalState"/>.</param>
        /// <returns>The <see cref="MapGrh"/> instance that was added, or null if the the <see cref="MapGrh"/> could not be
        /// added for any reason.</returns>
        public static MapGrh PlaceMapGrhAtScreenPos(Map map, ICamera2D camera, Vector2 screenPos, bool useTileMode, GrhData gd = null)
        {
            // Get the world position to place it
            var worldPos = camera.ToWorld(screenPos);

            return PlaceMapGrhAtWorldPos(map, worldPos, useTileMode, gd);
        }
    }
}