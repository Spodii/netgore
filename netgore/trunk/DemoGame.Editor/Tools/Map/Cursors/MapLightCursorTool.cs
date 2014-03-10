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
    public class MapLightCursorTool : MapCursorToolBase
    {
        const Keys _placeLightKey = Keys.Control;

        EditorMap _mouseOverMap;
        Vector2 _mousePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLightCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapLightCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
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
            return (obj is ILight) && base.CanSelect(map, obj);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Light Cursor")
            {
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapLightCursorTool_Disabled,
                EnabledImage = Resources.MapLightCursorTool_Enabled,
                HelpName = "Map Light Cursor",
                HelpWikiPage = "Map light cursor tool",
            };
        }

        /// <summary>
        /// Gets the map objects to select in the given region.
        /// </summary>
        /// <param name="map">The <see cref="EditorMap"/>.</param>
        /// <param name="selectionArea">The selection box area.</param>
        /// <returns>The objects to select.</returns>
        protected override IEnumerable<object> CursorSelectObjects(EditorMap map, Rectangle selectionArea)
        {
            return map.Lights.Where(x => x.Intersects(selectionArea)).Cast<object>();
        }

        /// <summary>
        /// Gets the selectable object currently under the cursor.
        /// </summary>
        /// <param name="map">The <see cref="EditorMap"/>.</param>
        /// <param name="worldPos">The world position.</param>
        /// <returns>The selectable object currently under the cursor, or null if none.</returns>
        protected override object GetObjUnderCursor(EditorMap map, Vector2 worldPos)
        {
            var closestLight = map.Lights.MinElementOrDefault(x => worldPos.QuickDistance(x.Center));
            if (closestLight == null)
                return null;

            if (worldPos.QuickDistance(closestLight.Center) > 10)
                return null;

            return closestLight;
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="imap">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleBeforeDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap imap)
        {
            base.HandleBeforeDrawMapGUI(spriteBatch, imap);

            if (!IsEnabled)
                return;

            if (IsSelecting)
                return;

            if ((Control.ModifierKeys & _placeLightKey) != 0)
            {
                if (imap == _mouseOverMap)
                {
                    var lightSprite = SystemSprites.Lightblub;
                    var pos = _mousePos - (lightSprite.Size / 2f);
                    pos = GridAligner.Instance.Align(pos);
                    lightSprite.Draw(spriteBatch, pos);
                }
            }
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
                foreach (var x in SOM.SelectedObjects.OfType<ILight>().ToImmutable())
                {
                    if (map.Lights.Contains(x))
                    {
                        map.RemoveLight(x);
                        removed.Add(x);

                        // Remove the graphic and effect from the map
                        var msc = MapScreenControl.TryFindInstance(map);
                        if (msc != null)
                        {
                            var dm = msc.DrawingManager;
                            if (dm != null)
                            {
                                dm.LightManager.Remove(x);
                            }
                        }
                    }
                }

                SOM.SetManySelected(SOM.SelectedObjects.Except(removed).ToImmutable());
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
            base.MapContainer_MouseDown(sender, map, camera, e);

            if (IsSelecting)
                return;

            // Left-click
            if (e.Button == MouseButtons.Left)
            {
                // Place light
                if (Input.IsKeyDown(_placeLightKey))
                {
                    var msc = MapScreenControl.TryFindInstance(map);
                    if (msc != null)
                    {
                        var dm = msc.DrawingManager;
                        if (dm != null)
                        {
                            var pos = camera.ToWorld(e.Position());
                            pos = GridAligner.Instance.Align(pos);

                            var light = new Light { Center = pos, IsEnabled = true, Tag = map };

                            map.AddLight(light);
                            dm.LightManager.Add(light);
                        }
                    }
                }
            }
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
    }
}