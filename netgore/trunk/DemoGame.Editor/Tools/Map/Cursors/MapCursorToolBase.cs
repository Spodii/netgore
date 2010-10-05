using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public abstract class MapCursorToolBase : Tool
    {
        const string _enabledToolsGroup = "Map Cursors";
        const MouseButtons _selectButton = MouseButtons.Left;

        readonly bool _canSelect;
        readonly bool _canSelectArea;

        bool _isSelecting = false;
        Vector2 _selectionEnd = Vector2.Zero;
        Vector2 _selectionStart = Vector2.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCursorToolBase"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="settings">The <see cref="ToolSettings"/> to use to create this <see cref="Tool"/>.</param>
        /// <param name="canSelect">If the cursor supports selecting objects on the map.</param>
        /// <param name="canSelectArea">If the cursor supports selecting a group of objects on the map.</param>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected MapCursorToolBase(ToolManager toolManager, ToolSettings settings, bool canSelect = true,
                                    bool canSelectArea = true) : base(toolManager, ModifyToolSettings(settings))
        {
            _canSelect = canSelect;
            _canSelectArea = canSelectArea;
        }

        /// <summary>
        /// Gets if the cursor supports selecting objects on the map.
        /// </summary>
        public bool CanSelect
        {
            get { return _canSelect; }
        }

        /// <summary>
        /// Gets if the cursor supports selecting groups of objects on the map.
        /// </summary>
        public bool CanSelectArea
        {
            get { return _canSelectArea; }
        }

        /// <summary>
        /// Gets the map objects to select in the given region.
        /// </summary>
        /// <param name="map">The <see cref="Map"/>.</param>
        /// <param name="selectionArea">The selection box area.</param>
        /// <returns>The objects to select.</returns>
        protected virtual IEnumerable<object> CursorSelectObjects(Map map, Rectangle selectionArea)
        {
            return map.Spatial.GetMany<object>(selectionArea, CursorSelectObjectsFilter);
        }

        /// <summary>
        /// Filter used by <see cref="MapCursorToolBase.CursorSelectObjects"/> to determine if an object should be selected.
        /// </summary>
        /// <param name="obj">The object to check if should be selected.</param>
        /// <returns>True if the object should be selected; otherwise false.</returns>
        protected virtual bool CursorSelectObjectsFilter(object obj)
        {
            if (obj == null)
                return false;

            return true;
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
            return settings;
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

            if (!CanSelect && !CanSelectArea)
                return;

            var mapContainer = c.AsMapContainer();
            if (mapContainer == null)
                return;

            mapContainer.MouseDown += mapContainer_MouseDown;
            mapContainer.MouseUp += mapContainer_MouseUp;
            mapContainer.MouseMove += mapContainer_MouseMove;
        }

        /// <summary>
        /// When overridden in the derived class, handles tearing down event listeners for a <see cref="IToolTargetContainer"/>.
        /// Any event listeners set up in <see cref="Tool.ToolTargetContainerAdded"/> should be torn down here.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerRemoved(IToolTargetContainer c)
        {
            base.ToolTargetContainerRemoved(c);

            if (!CanSelect && !CanSelectArea)
                return;

            var mapContainer = c.AsMapContainer();
            if (mapContainer == null)
                return;

            mapContainer.MouseDown -= mapContainer_MouseDown;
            mapContainer.MouseUp -= mapContainer_MouseUp;
            mapContainer.MouseMove -= mapContainer_MouseMove;
        }

        /// <summary>
        /// When overridden in the derived class, handles performing drawing before the GUI for a <see cref="IDrawableMap"/> has been draw.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        /// <param name="imap">The <see cref="IDrawableMap"/> being drawn.</param>
        protected override void HandleBeforeDrawMapGUI(NetGore.Graphics.ISpriteBatch spriteBatch, NetGore.Graphics.IDrawableMap imap)
        {
            base.HandleBeforeDrawMapGUI(spriteBatch, imap);

            var map = imap as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            // Draw the selection area
            if (_isSelecting)
            {
                var a = camera.ToScreen(_selectionStart);
                var b = camera.ToScreen(_selectionEnd);
                if (a.QuickDistance(b) > _minSelectionAreaDrawSize)
                {
                    var rect = Rectangle.FromPoints(a,b);
                    RenderRectangle.Draw(spriteBatch, rect, _selectionAreaColorInner, _selectionAreaColorOuter);
                }
            }
        }

        static readonly Color _selectionAreaColorInner = new Color(0, 255, 0, 100);
        static readonly Color _selectionAreaColorOuter = new Color(0, 150, 0, 200);

        /// <summary>
        /// The minimum size the selection area must be before it is drawn.
        /// </summary>
        const int _minSelectionAreaDrawSize = 4;

        /// <summary>
        /// Handles the MouseDown event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseDown(object sender, MouseEventArgs e)
        {
            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            if (e.Button != _selectButton)
                return;

            _selectionStart = camera.ToWorld(e.Position());
            _selectionEnd = _selectionStart;
            _isSelecting = true;
        }

        /// <summary>
        /// Handles the MouseMove event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseMove(object sender, MouseEventArgs e)
        {
            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            if (!_isSelecting)
                return;

            _selectionEnd = camera.ToWorld(e.Position());
        }

        /// <summary>
        /// Handles the MouseUp event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != _selectButton)
                return;

            if (!_isSelecting)
                return;

            _isSelecting = false;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as Map;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            _selectionEnd = camera.ToWorld(e.Position());

            var area = Rectangle.FromPoints(_selectionStart, _selectionEnd);

            var selected = CursorSelectObjects(map, area);
            GlobalState.Instance.Map.SelectedObjsManager.SetManySelected(selected);

            _selectionStart = Vector2.Zero;
            _selectionEnd = Vector2.Zero;
        }
    }
}