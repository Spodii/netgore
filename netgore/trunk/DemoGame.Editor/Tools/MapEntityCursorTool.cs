using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class MapEntityCursorTool : MapCursorToolBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEntityCursorTool"/> class.
        /// </summary>
        /// <param name="toolManager">The tool manager.</param>
        public MapEntityCursorTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.AsSplitButtonSettings().ClickToEnable = true;
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
                ToolBarControlType = ToolBarControlType.SplitButton,
                DisabledImage = Resources.MapEntityCursorTool_Disabled,
                EnabledImage = Resources.MapEntityCursorTool_Enabled,
            };
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

            mapContainer.KeyUp += mapContainer_KeyUp;
            mapContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(mapContainer_MouseMove);
        }

        Entity GetEntityUnderCursor(IMap map, Vector2 worldPos)
        {
            return map.Spatial.Get<Entity>(worldPos, GetEntityUnderCursorFilter);
        }

        bool GetEntityUnderCursorFilter(Entity entity)
        {
            if (entity is CharacterEntity)
                return false;

            if (entity is WallEntityBase)
                return false;

            return true;
        }

        /// <summary>
        /// Handles the MouseMove event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map as Map;
            if (map == null)
                return;

            var cursorPos = e.Position();
            var worldPos = map.Camera.ToWorld(cursorPos);

            // Ensure in the map bounds
            if (!map.IsInMapBoundaries(worldPos))
                return;

            var focusedEntity = SOM.Focused as Entity;

            if (focusedEntity != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Input.IsCtrlDown)
                    {
                        // Resize the entity
                        var size = worldPos - focusedEntity.Position;
                        if (size.X < 4)
                            size.X = 4;
                        if (size.Y < 4)
                            size.Y = 4;
                        map.SafeResizeEntity(focusedEntity, size);
                    }
                    else
                    {
                        // Move the entity
                        map.SafeTeleportEntity(focusedEntity, worldPos - _selectionOffset);
                    }
                }
            }
            else
            {
                // Set the tooltip to the entity under the cursor
                var hoverEntity = GetEntityUnderCursor(map, worldPos);

                if (hoverEntity == null)
                {
                    _toolTip = string.Empty;
                    _toolTipObject = null;
                }
                else if (_toolTipObject != hoverEntity)
                {
                    _toolTipObject = hoverEntity;
                    _toolTip = string.Format("{0}\n{1} ({2}x{3})", hoverEntity, hoverEntity.Position, hoverEntity.Size.X,
                                             hoverEntity.Size.Y);
                    _toolTipPos = GetToolTipPos(GlobalState.Instance.DefaultRenderFont, _toolTip, hoverEntity);
                }
            }
        }

        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="map">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleBeforeDrawMapGUI(NetGore.Graphics.ISpriteBatch spriteBatch, NetGore.Graphics.IDrawableMap map)
        {
            base.HandleBeforeDrawMapGUI(spriteBatch, map);

            if (!string.IsNullOrEmpty(_toolTip))
                spriteBatch.DrawStringShaded(GlobalState.Instance.DefaultRenderFont, _toolTip, _toolTipPos, Color.White,
                                             Color.Black);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling the <see cref="Tool.IsEnabledChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old (previous) value.</param>
        /// <param name="newValue">The new (current) value.</param>
        protected override void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            base.OnIsEnabledChanged(oldValue, newValue);

           // HandleResetState();
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling resetting the state of this <see cref="Tool"/>.
        /// For simplicity, all default values should be constant, no matter the current state.
        /// </summary>
        protected override void HandleResetState()
        {
            _toolTip = string.Empty;
            _selectionOffset = Vector2.Zero;
            _toolTipObject = null;
            _toolTipPos = Vector2.Zero;

            base.HandleResetState();
        }

        /// <summary>
        /// Gets the position to display the tooltip text.
        /// </summary>
        /// <param name="font">The font to use.</param>
        /// <param name="text">The tooltip text.</param>
        /// <param name="entity">The entity the tooltip is for.</param>
        /// <returns>The position to display the tooltip text.</returns>
        public static Vector2 GetToolTipPos(Font font, string text, ISpatial entity)
        {
            var pos = new Vector2(entity.Max.X, entity.Position.Y);
            pos -= new Vector2(5, (font.GetLineSpacing() * text.Split('\n').Length) + 5);
            return pos;
        }

        Vector2 _selectionOffset;

        /// <summary>
        /// Property to access the <see cref="SelectedObjectsManager{T}"/>. Provided purely for convenience.
        /// </summary>
        static SelectedObjectsManager<object> SOM
        {
            get { return GlobalState.Instance.Map.SelectedObjsManager; }
        }

        /// <summary>
        /// Handles the KeyUp event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void mapContainer_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var c = sender as IToolTargetMapContainer;
            if (c == null)
                return;

            var map = c.Map;
            if (map == null)
                return;

            // Handle deletes
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                // Only delete when it is an Entity that is on this map
                List<object> removed = new List<object>();
                foreach (var x in SOM.SelectedObjects.OfType<Entity>())
                {
                    if (map.Entities.Contains(x))
                    {
                        x.Dispose();
                        removed.Add(x);
                    }
                }

                SOM.SetManySelected(SOM.SelectedObjects.Except(removed));
            }
        }
    }
}