using System;
using System.Collections.Generic;
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
using NetGore.Editor.UI;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapEntityCursorTool : MapCursorToolBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Type _lastCreatedType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEntityCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The tool manager.</param>
        public MapEntityCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
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
            return (obj is Entity) && !(obj is CharacterEntity) && !(obj is WallEntity) && base.CanSelect(map, obj);
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
                ToolBarControlType = ToolBarControlType.Button,
                MapDrawingExtensions = new MapDrawingExtension[] { new MapEntityBoxDrawer() },
                DisabledImage = Resources.MapEntityCursorTool_Disabled,
                EnabledImage = Resources.MapEntityCursorTool_Enabled,
                HelpName = "Map Entity Cursor",
                HelpWikiPage = "Map entity cursor tool",
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
                foreach (var x in SOM.SelectedObjects.OfType<Entity>().ToImmutable())
                {
                    if (map.Spatial.CollectionContains(x))
                    {
                        map.RemoveEntity(x);

                        if (!x.IsDisposed)
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
        protected override void MapContainer_MouseUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera,
                                                     MouseEventArgs e)
        {
            var cursorPos = e.Position();
            var worldPos = camera.ToWorld(cursorPos);

            // Create entity
            if (e.Button == MouseButtons.Right)
            {
                Type createType = null;

                // Create using same type as the last entity, if possible
                if (Input.IsCtrlDown)
                    createType = _lastCreatedType;

                // Display selection dialog
                if (createType == null)
                {
                    using (var frm = new EntityTypeUITypeEditorForm(_lastCreatedType))
                    {
                        if (frm.ShowDialog(sender as IWin32Window) == DialogResult.OK)
                            createType = frm.SelectedItem;
                    }
                }

                // Create the type
                if (createType != null)
                {
                    _lastCreatedType = null;

                    try
                    {
                        // Create the Entity
                        var entity = (Entity)Activator.CreateInstance(createType);
                        map.AddEntity(entity);
                        entity.Size = new Vector2(64);
                        entity.Position = worldPos - (entity.Size / 2f);

                        GridAligner.Instance.Fit(entity);

                        _lastCreatedType = createType;
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to create entity of type `{0}` on map `{1}`. Exception: {2}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, createType, map, ex);
                        Debug.Fail(string.Format(errmsg, createType, map, ex));
                    }
                }
            }

            base.MapContainer_MouseUp(sender, map, camera, e);
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