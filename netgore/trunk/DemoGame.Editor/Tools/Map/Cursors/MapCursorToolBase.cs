using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor.Tools
{
    public abstract class MapCursorToolBase : MapToolBase
    {
        /// <summary>
        /// Gets if the given key is used for performing a selection.
        /// Default is true only for Shift.
        /// </summary>
        public virtual bool IsSelectKey(Keys key)
        {
            return (key & Keys.Shift) != 0;
        }

        /// <summary>
        /// Gets if the given mouse button is used for performing a selection.
        /// Default is true only for Left.
        /// </summary>
        public virtual bool IsSelectMouseButton(MouseButtons button)
        {
            return (button & MouseButtons.Left) != 0;
        }

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

        /// <summary>
        /// The map the cursor is currently or was last over. Once set, doesn't get unset, so don't rely on it telling
        /// you that no map is under the cursor.
        /// </summary>
        EditorMap _mouseOverMap = null;

        Vector2 _selectionEnd = Vector2.Zero;
        Vector2 _selectionStart = Vector2.Zero;
        string _toolTip = string.Empty;
        object _toolTipObject = null;
        Vector2 _toolTipPos;

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
        /// Gets or sets if a tooltip for the object currently under the cursor will be shown.
        /// The default value is true.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowObjectToolTip { get; set; }

        /// <summary>
        /// Gets the <see cref="Font"/> to use to draw the tooltips.
        /// </summary>
        public Font ToolTipFont
        {
            get { return GlobalState.Instance.DefaultRenderFont; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this cursor can select the given object.
        /// </summary>
        /// <param name="map">The map containing the object to be selected.</param>
        /// <param name="obj">The object to try to select.</param>
        /// <returns>True if the <paramref name="obj"/> can be selected and handled by this cursor; otherwise false.</returns>
        protected virtual bool CanSelect(EditorMap map, object obj)
        {
            return IsObjectVisible(map, obj);
        }

        /// <summary>
        /// Gets the map objects to select in the given region.
        /// </summary>
        /// <param name="map">The <see cref="EditorMap"/>.</param>
        /// <param name="selectionArea">The selection box area.</param>
        /// <returns>The objects to select.</returns>
        protected virtual IEnumerable<object> CursorSelectObjects(EditorMap map, Rectangle selectionArea)
        {
            return map.Spatial.GetMany<object>(selectionArea, x => CanSelect(map, x));
        }

        /// <summary>
        /// Gets the selectable object currently under the cursor.
        /// </summary>
        /// <param name="map">The <see cref="EditorMap"/>.</param>
        /// <param name="worldPos">The world position.</param>
        /// <returns>The selectable object currently under the cursor, or null if none.</returns>
        protected virtual object GetObjUnderCursor(EditorMap map, Vector2 worldPos)
        {
            // By default, this will only get anything that implements ISpatial since its much faster that way and most
            // map cursors will be working with types that implement ISpatial
            return map.Spatial.Get(worldPos, x => CanSelect(map, x));
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
        /// Handles the KeyDown event of this Cursor tool.  If the Control key is down then it will shift any selected <see cref="ISpatial"/>'s to the direction of the pressed arrow keys.
        /// </summary>
        protected override void MapContainer_KeyDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
            var som = GlobalState.Instance.Map.SelectedObjsManager;

            if (!e.Control) 
                return;


            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        var left = new Vector2(-1, 0);
                        foreach (var entity in som.SelectedObjects.OfType<ISpatial>())
                        {
                            entity.TryMove(entity.Position + left);
                        }
                    }
                    break;
 
                case Keys.Right:
                    {
                        var right = new Vector2(1, 0);
                        foreach (var entity in som.SelectedObjects.OfType<ISpatial>())
                        {
                            entity.TryMove(entity.Position + right);
                        }
                    }
                    break;

                case Keys.Up:
                    {
                        var up = new Vector2(0, -1);
                        foreach (var entity in som.SelectedObjects.OfType<ISpatial>())
                        {
                            entity.TryMove(entity.Position + up);
                        }
                    }
                    break;

                case Keys.Down:
                    {
                        var down = new Vector2(0, 1);
                        foreach (var entity in som.SelectedObjects.OfType<ISpatial>())
                        {
                            entity.TryMove(entity.Position + down);
                        }
                    }
                    break;
            }
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

            var map = imap as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            // Only draw for the map under the cursor
            if (map != _mouseOverMap)
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
                var font = ToolTipFont;
                if (_toolTipObject != null && _toolTip != null && font != null)
                    spriteBatch.DrawStringShaded(font, _toolTip, _toolTipPos, Color.White, Color.Black);
            }
        }

        /// <summary>
        /// Gets if the given object is visible according to the drawing filter currently being used.
        /// Objects that are never drawn or are never filtered always return true, even if they are not actually visible.
        /// </summary>
        /// <param name="map">The current map.</param>
        /// <param name="obj">The object to check if visible.</param>
        /// <returns>True if the <paramref name="obj"/> is visible or not applicable to the map drawing filter; otherwise false.</returns>
        protected virtual bool IsObjectVisible(EditorMap map, object obj)
        {
            var drawable = obj as IDrawable;
            if (drawable == null)
                return true;

            if (map == null)
                return true;

            if (map.DrawFilter == null)
                return true;

            return map.DrawFilter(drawable);
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
            // Terminate any current area selection when any mouse button is pressed
            _selectionStart = Vector2.Zero;
            _selectionEnd = Vector2.Zero;
            _isSelecting = false;

            var worldPos = camera.ToWorld(e.Position());
            var underCursor = GetObjUnderCursor(map, worldPos);

            GlobalState.Instance.Map.SelectedObjsManager.SetSelected(underCursor);

            if (IsSelectMouseButton(e.Button) && IsSelectKey(Control.ModifierKeys))
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
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseMove(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            _mouseOverMap = map;

            var worldPos = camera.ToWorld(e.Position());

            if (IsSelecting)
            {
                // Expand selection area
                _selectionEnd = worldPos;
            }
            else
            {
                // Create the tooltip
                var font = ToolTipFont;
                if (ShowObjectToolTip && font != null)
                {
                    var hoverEntity = GetObjUnderCursor(map, worldPos);

                    if (hoverEntity == null)
                    {
                        // Nothing under the cursor that we are allowed to select
                        _toolTip = string.Empty;
                        _toolTipObject = null;
                    }
                    else if (_toolTipObject != hoverEntity)
                    {
                        // Something found under the cursor
                        _toolTipObject = hoverEntity;
                        _toolTipPos = e.Position();
                        _toolTip = GetObjectToolTip(hoverEntity) ?? hoverEntity.ToString();

                        // Make sure the text stays in the view area
                        const int toolTipPadding = 4;
                        var toolTipSize = font.MeasureString(_toolTip);

                        if (_toolTipPos.X < toolTipPadding)
                            _toolTipPos.X = toolTipPadding;
                        else if (_toolTipPos.X + toolTipSize.X + toolTipPadding > camera.Size.X)
                            _toolTipPos.X = camera.Size.X - toolTipSize.X - toolTipPadding;

                        if (_toolTipPos.Y < toolTipPadding)
                            _toolTipPos.Y = toolTipPadding;
                        else if (_toolTipPos.Y + toolTipSize.Y + toolTipPadding > camera.Size.Y)
                            _toolTipPos.Y = camera.Size.Y - toolTipSize.Y - toolTipPadding;
                    }
                }
            }

            base.MapContainer_MouseMove(sender, map, camera, e);
        }

        /// <summary>
        /// Handles when the mouse button is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected override void MapContainer_MouseUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
            if (IsSelecting && IsSelectMouseButton(e.Button))
            {
                // End the mass selection
                _selectionEnd = camera.ToWorld(e.Position());

                var area = Rectangle.FromPoints(_selectionStart, _selectionEnd);
                MapContainer_AreaSelected(map, camera, area, e);

                _isSelecting = false;
                _selectionStart = Vector2.Zero;
                _selectionEnd = Vector2.Zero;
            }

            base.MapContainer_MouseUp(sender, map, camera, e);
        }

        /// <summary>
        /// Handles what happens when an area of the map is selected.
        /// Default behavior is to place all selectable objects in the SelectedObjectsManager.
        /// </summary>
        protected virtual void MapContainer_AreaSelected(EditorMap map, ICamera2D camera, Rectangle selectionArea, MouseEventArgs e)
        {
            var selected = CursorSelectObjects(map, selectionArea);
            GlobalState.Instance.Map.SelectedObjsManager.SetManySelected(selected);
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