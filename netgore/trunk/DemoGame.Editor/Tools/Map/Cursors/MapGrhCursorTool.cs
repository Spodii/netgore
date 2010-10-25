using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
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
            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.ClickToEnable = true;

            // Build the menu
            s.DropDownItems.Add(new MenuDefaultLayer());
            s.DropDownItems.Add(new MenuDefaultDepth());
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

                    var isForeground = EditorSettings.Default.MapGrh_DefaultIsForeground;
                    var depth = EditorSettings.Default.MapGrh_DefaultDepth;

                    var g = new Grh(gd, AnimType.Loop, map.GetTime());
                    var mg = new MapGrh(g, drawPos, isForeground) { LayerDepth = depth };
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
            void EditorSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (!StringComparer.Ordinal.Equals("MapGrh_DefaultDepth", e.PropertyName))
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
                UpdateText();
            }

            /// <summary>
            /// Handles the PropertyChanged event of the EditorSettings control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
            void EditorSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (!StringComparer.Ordinal.Equals("MapGrh_DefaultIsForeground", e.PropertyName))
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

                EditorSettings.Default.MapGrh_DefaultIsForeground = !EditorSettings.Default.MapGrh_DefaultIsForeground;
            }

            void UpdateText()
            {
                Text = "Default in BG: " + EditorSettings.Default.MapGrh_DefaultIsForeground;
            }
        }
    }
}