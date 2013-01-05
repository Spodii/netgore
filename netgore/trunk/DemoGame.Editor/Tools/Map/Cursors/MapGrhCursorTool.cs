using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapGrhCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsButtonSettings();
            s.ClickToEnable = true;
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
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapGrhCursorTool_Disabled,
                EnabledImage = Resources.MapGrhCursorTool_Enabled,
                HelpName = "Map Grh Cursor",
                HelpWikiPage = "Map grh cursor tool",
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
            // Handle deletes
            if (e.KeyCode == Keys.Delete)
            {
                // Only delete when it is an Entity that is on this map
                var removed = new List<object>();
                foreach (var x in SOM.SelectedObjects.OfType<MapGrh>().ToImmutable())
                {
                    if (map.Spatial.CollectionContains(x))
                    {
                        map.RemoveMapGrh(x);
                        removed.Add(x);
                    }
                }

                SOM.SetManySelected(SOM.SelectedObjects.Except(removed).ToImmutable());
            }

            base.MapContainer_KeyUp(sender, map, camera, e);
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
            int modDepth = 0;
            if (e.Delta < 0)
                modDepth = -1;
            else if (e.Delta > 0)
                modDepth = 1;

            if (modDepth != 0)
            {
                // Change layer depth, making sure it is clamped in the needed range
                foreach (var mapGrh in GlobalState.Instance.Map.SelectedObjsManager.SelectedObjects.OfType<MapGrh>())
                {
                    if (map.Spatial.CollectionContains(mapGrh))
                    {
                        mapGrh.LayerDepth = (short)(mapGrh.LayerDepth + modDepth).Clamp(short.MinValue, short.MaxValue);
                    }
                }
                GlobalState.Instance.Map.SelectedObjsManager.UpdateFocused();
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

    }
}