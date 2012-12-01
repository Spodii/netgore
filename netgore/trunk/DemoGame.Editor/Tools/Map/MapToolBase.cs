using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// Base class for a <see cref="Tool"/> specifically for a map. Provides some helper methods to assist with working
    /// on maps, especially with dealing with input.
    /// </summary>
    public abstract class MapToolBase : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapToolBase"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        /// <param name="settings">The <see cref="ToolSettings"/> to use to create this <see cref="Tool"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="toolManager"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected MapToolBase(ToolManager toolManager, ToolSettings settings) : base(toolManager, settings)
        {
        }

        /// <summary>
        /// Handles when a key is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_KeyDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a key is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_KeyPress(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a key is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_KeyUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, KeyEventArgs e)
        {
        }

        /// <summary>
        /// Handles when a mouse button is pressed on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_MouseDown(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse moves over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_MouseMove(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse button is raised on a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_MouseUp(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Handles when the mouse wheel is moved while over a map.
        /// </summary>
        /// <param name="sender">The <see cref="IToolTargetMapContainer"/> the event came from. Cannot be null.</param>
        /// <param name="map">The <see cref="EditorMap"/>. Cannot be null.</param>
        /// <param name="camera">The <see cref="ICamera2D"/>. Cannot be null.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data. Cannot be null.</param>
        protected virtual void MapContainer_MouseWheel(IToolTargetMapContainer sender, EditorMap map, ICamera2D camera, MouseEventArgs e)
        {
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

            mapContainer.MouseDown -= mapContainer_MouseDownCallback;
            mapContainer.MouseDown += mapContainer_MouseDownCallback;

            mapContainer.MouseUp -= mapContainer_MouseUpCallback;
            mapContainer.MouseUp += mapContainer_MouseUpCallback;

            mapContainer.MouseMove -= mapContainer_MouseMoveCallback;
            mapContainer.MouseMove += mapContainer_MouseMoveCallback;

            mapContainer.MouseWheel -= mapContainer_MouseWheelCallback;
            mapContainer.MouseWheel += mapContainer_MouseWheelCallback;

            mapContainer.KeyDown -= mapContainer_KeyDownCallback;
            mapContainer.KeyDown += mapContainer_KeyDownCallback;

            mapContainer.KeyUp -= mapContainer_KeyUpCallback;
            mapContainer.KeyUp += mapContainer_KeyUpCallback;

            mapContainer.KeyPress -= mapContainer_KeyPress;
            mapContainer.KeyPress += mapContainer_KeyPress;
        }

        /// <summary>
        /// When overridden in the derived class, handles tearing down event listeners for a <see cref="IToolTargetContainer"/>.
        /// Any event listeners set up in <see cref="Tool.ToolTargetContainerAdded"/> should be torn down here.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerRemoved(IToolTargetContainer c)
        {
            base.ToolTargetContainerRemoved(c);

            var mapContainer = c.AsMapContainer();
            if (mapContainer == null)
                return;

            mapContainer.MouseDown -= mapContainer_MouseDownCallback;
            mapContainer.MouseUp -= mapContainer_MouseUpCallback;
            mapContainer.MouseMove -= mapContainer_MouseMoveCallback;
            mapContainer.MouseWheel -= mapContainer_MouseWheelCallback;
            mapContainer.KeyDown -= mapContainer_KeyDownCallback;
            mapContainer.KeyUp -= mapContainer_KeyUpCallback;
            mapContainer.KeyPress -= mapContainer_KeyPress;
        }

        /// <summary>
        /// Handles the KeyDownCallback event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void mapContainer_KeyDownCallback(object sender, KeyEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_KeyDown(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the KeyPress event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        void mapContainer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_KeyPress(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the KeyUpCallback event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void mapContainer_KeyUpCallback(object sender, KeyEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_KeyUp(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the MouseDown event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseDownCallback(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_MouseDown(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the MouseMove event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseMoveCallback(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_MouseMove(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the MouseUp event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseUpCallback(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_MouseUp(mapContainer, map, camera, e);
        }

        /// <summary>
        /// Handles the MouseWheelCallback event of the mapContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void mapContainer_MouseWheelCallback(object sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            var mapContainer = sender as IToolTargetMapContainer;
            if (mapContainer == null)
                return;

            var map = mapContainer.Map as EditorMap;
            if (map == null)
                return;

            var camera = map.Camera;
            if (camera == null)
                return;

            MapContainer_MouseWheel(mapContainer, map, camera, e);
        }
    }
}