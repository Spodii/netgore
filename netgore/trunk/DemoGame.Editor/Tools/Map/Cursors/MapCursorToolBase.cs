using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public abstract class MapCursorToolBase : MapToolBase
    {
        /// <summary>
        /// The key used to perform an area selection;
        /// </summary>
        public const Keys SelectKey = Keys.Shift;

        /// <summary>
        /// The mouse button used to perform an area selection;
        /// </summary>
        public const MouseButtons SelectMouseButton = MouseButtons.Left;

        const string _enabledToolsGroup = "Map Cursors";

        /// <summary>
        /// The minimum size the selection area must be before it is drawn.
        /// </summary>
        const int _minSelectionAreaDrawSize = 4;

        /// <summary>
        /// The <see cref="Color"/> of the inner area of the selection rectangle.
        /// </summary>
        static readonly Color _selectionAreaColorInner = new Color(0, 255, 0, 100);

        /// <summary>
        /// The <see cref="Color"/> of the outer area of the selection rectangle.
        /// </summary>
        static readonly Color _selectionAreaColorOuter = new Color(0, 150, 0, 200);

        bool _isSelecting = false;
        Vector2 _selectionEnd = Vector2.Zero;
        Vector2 _selectionStart = Vector2.Zero;
        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

        /// <summary>
        /// Gets or sets if a tooltip for the object currently under the cursor will be shown.
        /// The default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowObjectToolTip
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCursorToolBase"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="settings">The <see cref="ToolSettings"/> to use to create this <see cref="Tool"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected MapCursorToolBase(ToolManager toolManager, ToolSettings settings)
            : base(toolManager, ModifyToolSettings(settings))
        {
            ShowObjectToolTip = true;
        }

        /// <summary>
        /// Gets if this <see cref="MapCursorToolBase"/> is currently performing an area selection.
        /// </summary>
        public bool IsSelecting
        {
            get { return _isSelecting; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this cursor can select the given object.
        /// </summary>
        /// <param name="obj">The object to try to select.</param>
        /// <returns>True if the <paramref name="obj"/> can be selected and handled by this cursor; otherwise false.</returns>
        protected abstract bool CanSelect(object obj);

        /// <summary>
        /// Gets the map objects to select in the given region.
        /// </summary>
        /// <param name="map">The <see cref="Map"/>.</param>
        /// <param name="selectionArea">The selection box area.</param>
        /// <returns>The objects to select.</returns>
        protected virtual IEnumerable<object> CursorSelectObjects(Map map, Rectangle selectionArea)
        {
            return map.Spatial.GetMany<object>(selectionArea, CanSelect);
        }

        /// <summary>
        /// Gets the selectable object currently under the cursor.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="worldPos">The world position.</param>
        /// <returns>The selectable object currently under the cursor, or null if none.</returns>
        protected virtual object GetObjUnderCursor(IMap map, Vector2 worldPos)
        {
            // By default, this will only get anything that implements ISpatial since its much faster that way and most
            // map cursors will be working with types that implement ISpatial
            return map.Spatial.Get(worldPos, CanSelect);
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="imap">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleBeforeDrawMapGUI(ISpriteBatch spriteBatch, IDrawableMap imap)
        {
            base.HandleBeforeDrawMapGUI(spriteBatch, imap);

            var map = imap as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            if (IsSelecting)
            {
                // Draw the selection area
                var a = camera.ToScreen(_selectionStart);
                var b = camera.ToScreen(_selectionEnd);

                if (a.QuickDistance(b) > _minSelectionAreaDrawSize)
                {
                    var rect = Rectangle.FromPoints(a, b);
                    RenderRectangle.Draw(spriteBatch, rect, _selectionAreaColorInner, _selectionAreaColorOuter);
                }
            }
            else
            {
                // Draw the tooltip
                if (_toolTipObject != null && _toolTip != null)
                {
                    var font = GlobalState.Instance.DefaultRenderFont;
                    spriteBatch.DrawStringShaded(font, _toolTip, _toolTipPos, Color.White, Color.Black);
                }
            }
        }

        /// <summary>
        /// Handles when a mouse button is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="Map"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseDown(IToolTargetMapContainer sender, Map map, ICamera2D camera, MouseEventArgs e)
        {
            // Terminate any current area selection when any mouse button is pressed
            _selectionStart = Vector2.Zero;
            _selectionEnd = Vector2.Zero;
            _isSelecting = false;
            
            var worldPos = camera.ToWorld(e.Position());
            var underCursor = GetObjUnderCursor(map, worldPos);

            GlobalState.Instance.Map.SelectedObjsManager.SetSelected(underCursor);

            if (e.Button == SelectMouseButton && ((Control.ModifierKeys & SelectKey) != 0))
            {
                // Start area selection
                _selectionStart = worldPos;
                _selectionEnd = _selectionStart;
                _isSelecting = true;
            }

            base.MapContainer_MouseDown(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse moves over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="Map"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseMove(IToolTargetMapContainer sender, Map map, ICamera2D camera, MouseEventArgs e)
        {
            var worldPos = camera.ToWorld(e.Position());

            if (IsSelecting)
            {
                // Expand selection area
                _selectionEnd = worldPos;
            }
            else
            {
                // Show the tooltip

                if (ShowObjectToolTip)
                {
                    var hoverEntity = GetObjUnderCursor(map, worldPos);

                    if (hoverEntity == null)
                    {
                        _toolTip = string.Empty;
                        _toolTipObject = null;
                    }
                    else if (_toolTipObject != hoverEntity)
                    {
                        _toolTipObject = hoverEntity;
                        _toolTipPos = e.Position();
                        _toolTip = GetObjectToolTip(hoverEntity) ?? hoverEntity.ToString();
                    }
                }
            }

            base.MapContainer_MouseMove(sender, map, camera, e);
        }

        /// <summary>
        /// Gets the tooltip to display for an object.
        /// </summary>
        /// <param name="obj">The object to display the tooltip for.</param>
        /// <returns>The tooltip to display for the <paramref name="obj"/>.</returns>
        protected virtual string GetObjectToolTip(object obj)
        {
            if (obj is MapGrh)
            {
                // MapGrh
                const string format = "{0}\n{1} ({2}x{3})";
                var o = (MapGrh)obj;
                var gd = o.Grh != null ? o.Grh.GrhData : null;
                var cat = gd != null ? gd.Categorization : null;
                var catStr = cat != null ? cat.ToString() : "[GrhData not set]";

                return string.Format(format, catStr, o.Position, o.Size.X, o.Size.Y);
            }
            else if (obj is ISpatial)
            {
                // ISpatial
                const string format = "{0}\n{1} ({2}x{3})";
                var o = (ISpatial)obj;
                return string.Format(format, o, o.Position, o.Size.X, o.Size.Y);
            }
            else
            {
                // No custom support provided - just use ToString
                return obj.ToString();
            }
        }

        /// <summary>
        /// Handles when the mouse button is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="Map"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseUp(IToolTargetMapContainer sender, Map map, ICamera2D camera, MouseEventArgs e)
        {
            if (IsSelecting && e.Button == SelectMouseButton)
            {
                // End the mass selection

                _selectionEnd = camera.ToWorld(e.Position());

                var area = Rectangle.FromPoints(_selectionStart, _selectionEnd);

                var selected = CursorSelectObjects(map, area);
                GlobalState.Instance.Map.SelectedObjsManager.SetManySelected(selected);

                _isSelecting = false;
                _selectionStart = Vector2.Zero;
                _selectionEnd = Vector2.Zero;
            }

            base.MapContainer_MouseUp(sender, map, camera, e);
        }

        /// <summary>
        /// Modifies the <see cref="ToolSettings"/> as it is passed to the base class constructor.
        /// </summary>
        /// <param name="settings">The <see cref="ToolSettings"/>.</param>
        /// <returns>The <see cref="ToolSettings"/></returns>
        static ToolSettings ModifyToolSettings(ToolSettings settings)
        {
            settings.ToolBarVisibility = ToolBarVisibility.Map;
            settings.EnabledToolsGroup = _enabledToolsGroup;
            settings.EnabledByDefault = false;
            return settings;
        }
    }
}