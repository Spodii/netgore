using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using log4net;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapGrhCursorTool : MapCursorToolBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const Keys _placeMapGrhKey = Keys.Control;

        readonly MenuTileMode _mnuTileMode;

        EditorMap _mouseOverMap;
        Vector2 _mousePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapGrhCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.ClickToEnable = true;

            // Build the menu
            _mnuTileMode = new MenuTileMode();

            s.DropDownItems.Add(new MenuDefaultLayer());
            s.DropDownItems.Add(new MenuDefaultDepth());
            s.DropDownItems.Add(_mnuTileMode);
        }

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Gets or sets if tiling mode is active.
        /// </summary>
        [SyncValue]
        public bool TileMode
        {
            get
            {
                if (_mnuTileMode == null)
                {
                    const string errmsg = "_mnuTileMode was null.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    Debug.Fail(errmsg);
                    return false;
                }

                return _mnuTileMode.Checked;
            }
            set
            {
                if (_mnuTileMode == null)
                {
                    const string errmsg = "_mnuTileMode was null.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    Debug.Fail(errmsg);
                    return;
                }

                _mnuTileMode.Checked = value;
            }
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
            if (Input.IsKeyDown(_placeMapGrhKey))
                return false;

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
            grh.Draw(spriteBatch, GridAligner.Instance.Align(_mousePos, TileMode));
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
                    if (map.Spatial.CollectionContains(x))
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
            var wasMapGrhPlaced = false;

            if (e.Button == MouseButtons.Left)
            {
                // Left-click
                if (Input.IsKeyDown(_placeMapGrhKey))
                {
                    PlaceMapGrh(map, camera, e.Position(), TileMode);
                    wasMapGrhPlaced = true;
                }
            }

            base.MapContainer_MouseDown(sender, map, camera, e);

            if (wasMapGrhPlaced)
                SOM.Clear();
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

            // Support dragging operations when using TileMode
            if (TileMode)
            {
                if (Input.IsKeyDown(_placeMapGrhKey))
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        // Drag placement
                        PlaceMapGrh(map, camera, e.Position(), true);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        // Drag delete
                        var worldPos = camera.ToWorld(e.Position());
                        worldPos = GridAligner.Instance.Align(worldPos, true);
                        var worldPosArea = worldPos.ToRectangle(Vector2.One, false);
                        var toDelete = map.Spatial.GetMany<MapGrh>(worldPosArea, x => IsObjectVisible(map, x));

                        foreach (var x in toDelete)
                        {
                            map.RemoveMapGrh(x);
                        }
                    }
                }
            }
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
            if (!map.Spatial.CollectionContains(focusedMapGrh))
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
        public static MapGrh PlaceMapGrh(Map map, ICamera2D camera, Vector2 screenPos, bool useTileMode, GrhData gd = null)
        {
            // Get the GrhData to place from the global state
            if (gd == null)
                gd = GlobalState.Instance.Map.GrhToPlace.GrhData;

            // Ensure we have a GrhData to place
            if (gd == null)
                return null;

            // Get the world position to place it
            var drawPos = camera.ToWorld(screenPos);
            drawPos = GridAligner.Instance.Align(drawPos, useTileMode);

            // Cache some other values
            var selGrhGrhIndex = gd.GrhIndex;
            var isForeground = EditorSettings.Default.MapGrh_DefaultIsForeground;
            var depth = EditorSettings.Default.MapGrh_DefaultDepth;
            var drawPosArea = drawPos.ToRectangle(Vector2.One, false);

            if (!useTileMode)
            {
                // Make sure the same MapGrh doesn't already exist at that position
                if (map.Spatial.Contains<MapGrh>(drawPosArea, x => x.Grh.GrhData.GrhIndex == selGrhGrhIndex))
                    return null;
            }
            else
            {
                // Make sure the same MapGrh doesn't already exist at that position on the same layer
                if (map.Spatial.Contains<MapGrh>(drawPosArea,
                                                 x => x.Grh.GrhData.GrhIndex == selGrhGrhIndex && x.IsForeground == isForeground
                                                 && Math.Round(x.Position.QuickDistance(drawPos)) <= 1))
                    return null;

                // In TileMode, do not allow ANY MapGrh at the same position and layer depth. And if it does exist, instead of aborting,
                // delete the existing one.
                var existingMapGrhs = map.Spatial.GetMany<MapGrh>(drawPosArea, x => x.LayerDepth == depth && x.IsForeground == isForeground
                    && Math.Round(x.Position.QuickDistance(drawPos)) <= 1);
                foreach (var toDelete in existingMapGrhs)
                {
                    Debug.Assert(toDelete != null);
                    if (log.IsDebugEnabled)
                        log.DebugFormat("TileMode caused MapGrh `{0}` to be overwritten.", toDelete);

                    map.RemoveMapGrh(toDelete);
                }

                Debug.Assert(!map.Spatial.Contains<MapGrh>(drawPosArea, x => x.LayerDepth == depth && x.IsForeground == isForeground));
            }

            // Create the new MapGrh and add it to the map
            var g = new Grh(gd, AnimType.Loop, map.GetTime());
            var mg = new MapGrh(g, drawPos, isForeground) { LayerDepth = depth };
            map.AddMapGrh(mg);

            return mg;
        }

        class MenuDefaultDepth : ToolStripMenuItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MenuDefaultDepth"/> class.
            /// </summary>
            public MenuDefaultDepth()
            {
                EditorSettings.Default.PropertyChanged += EditorSettings_PropertyChanged;
                UpdateText();
            }

            /// <summary>
            /// Handles the PropertyChanged event of the EditorSettings control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
            void EditorSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                const string propName = "MapGrh_DefaultDepth";

                EditorSettings.Default.AssertPropertyExists(propName);

                if (!StringComparer.Ordinal.Equals(propName, e.PropertyName))
                    return;

                UpdateText();
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);

                const string title = "New default depth";
                const string msg = @"Enter the new default depth value.
Must be value between {0} and {1}.";

                while (true)
                {
                    var defaultValue = EditorSettings.Default.MapGrh_DefaultDepth.ToString();
                    var result = InputBox.Show(title, string.Format(msg, short.MinValue, short.MaxValue), defaultValue);

                    if (string.IsNullOrEmpty(result))
                        return;

                    short newValue;
                    if (!short.TryParse(result, out newValue))
                    {
                        const string errTitle = "Invalid value";
                        const string errMsg = "You entered an invalid value. Please enter a numeric value in the required range.";
                        MessageBox.Show(errMsg, errTitle, MessageBoxButtons.OK);
                    }
                    else
                    {
                        EditorSettings.Default.MapGrh_DefaultDepth = newValue;
                        break;
                    }
                }
            }

            void UpdateText()
            {
                Text = "Default depth: " + EditorSettings.Default.MapGrh_DefaultDepth;
            }
        }

        class MenuDefaultLayer : ToolStripMenuItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MenuDefaultLayer"/> class.
            /// </summary>
            public MenuDefaultLayer()
            {
                EditorSettings.Default.PropertyChanged += EditorSettings_PropertyChanged;
                Text = "Add to foreground";
                Checked = EditorSettings.Default.MapGrh_DefaultIsForeground;
            }

            /// <summary>
            /// Handles the PropertyChanged event of the EditorSettings control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
            void EditorSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (!StringComparer.Ordinal.Equals("MapGrh_DefaultIsForeground", e.PropertyName))
                    return;

                Checked = EditorSettings.Default.MapGrh_DefaultIsForeground;
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click"/> event.
            /// </summary>
            /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);

                EditorSettings.Default.MapGrh_DefaultIsForeground = !EditorSettings.Default.MapGrh_DefaultIsForeground;
            }
        }

        class MenuTileMode : ToolStripMenuItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MenuTileMode"/> class.
            /// </summary>
            public MenuTileMode()
            {
                CheckOnClick = true;
                Text = "Tile mode";
            }
        }
    }
}