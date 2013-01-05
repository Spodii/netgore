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
    public class MapGrhFillTool : MapCursorToolBase
    {
        EditorMap _mouseOverMap;
        Vector2 _mousePos;

        public MapGrhFillTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsButtonSettings();
            s.ClickToEnable = true;
        }

        /// <summary>
        /// Gets if the given key is used for performing a selection.
        /// Default is true only for Shift.
        /// </summary>
        public override bool IsSelectKey(Keys key)
        {
            return false;
        }

        /// <summary>
        /// Gets if the given mouse button is used for performing a selection.
        /// Default is true only for Left.
        /// </summary>
        public override bool IsSelectMouseButton(MouseButtons button)
        {
            return false;
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
            return new ToolSettings("Map Grh Fill")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapGrhFillTool_Disabled,
                EnabledImage = Resources.MapGrhFillTool_Enabled,
                HelpName = "Map Grh Fill Tool",
                HelpWikiPage = "Map grh fill tool",
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
                drawPos = GridAligner.Instance.Align(drawPos, true).Round();

            if (Input.IsShiftDown)
            {
                // Display tooltip of what would be selected
                var grhToSelect = MapGrhPencilTool.GetGrhToSelect(map, worldPos);
                MapGrhPencilTool.DrawMapGrhTooltip(spriteBatch, map, grhToSelect, worldPos);
            }
            else
            {
                grh.Update(map.GetTime());
                grh.Draw(spriteBatch, drawPos, new Color(255, 255, 255, 180));
            }
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
        /// Handles both mouse clicks and moves.
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

            var globalState = GlobalState.Instance;
            var currentGrhData = globalState.Map.GrhToPlace.GrhData;

            Vector2 worldPos = camera.ToWorld(cursorPos);

            // Handle mouse
            if (e.Button == MouseButtons.Left)
            {
                if (!Input.IsShiftDown)
                {
                    // Fill
                    if (currentGrhData != null)
                    {
                        var mapGrhsToReplace = GetFillMapGrhs(map, worldPos);
                        foreach (var mapGrh in mapGrhsToReplace)
                        {
                            mapGrh.Grh.SetGrh(currentGrhData);
                        }
                    }
                }
                else
                {
                    // Select grh under cursor
                    var grhToSelect = MapGrhPencilTool.GetGrhToSelect(map, worldPos);
                    if (grhToSelect != null)
                    {
                        globalState.Map.SetGrhToPlace(grhToSelect.Grh.GrhData.GrhIndex);
                        globalState.Map.Layer = grhToSelect.MapRenderLayer;
                        globalState.Map.LayerDepth = grhToSelect.LayerDepth;
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                // Fill-delete
                if (currentGrhData != null)
                {
                    var mapGrhsToReplace = GetFillMapGrhs(map, worldPos);
                    foreach (var mapGrh in mapGrhsToReplace)
                    {
                        map.RemoveMapGrh(mapGrh);
                    }
                }
            }
        }

        IEnumerable<MapGrh> GetFillMapGrhs(EditorMap map, Vector2 worldPos)
        {
            MapGrh mapGrh = MapGrhPencilTool.GetGrhToSelect(map, worldPos);
            HashSet<MapGrh> ret = new HashSet<MapGrh>();
            GetFillMapGrhs(map, mapGrh, ret);
            return ret.ToArray();
        }

        void GetFillMapGrhs(EditorMap map, MapGrh mapGrh, HashSet<MapGrh> ret)
        {
            if (mapGrh == null || !ret.Add(mapGrh))
                return;

            Rectangle rect = new Rectangle(mapGrh.Position.X - 1, mapGrh.Position.Y - 1, mapGrh.Size.X + 2, mapGrh.Size.Y + 2);
            foreach (var mg in map.Spatial.GetMany<MapGrh>(rect, x => x.Grh != null && x.Grh.GrhData == mapGrh.Grh.GrhData))
            {
                GetFillMapGrhs(map, mg, ret);
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
            HandleMouseClickAndMove(map, camera, e);
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
    }
}