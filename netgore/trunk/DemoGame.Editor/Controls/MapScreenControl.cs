using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// The <see cref="GraphicsDeviceControl"/> that provides all the actual displaying and interaction of a <see cref="Map"/>
    /// instance.
    /// </summary>
    public partial class MapScreenControl : GraphicsDeviceControl, IMapBoundControl, IGetTime
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="MapScreenControl"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MapScreenControl"/> the event came from.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public delegate void MapChangedEventHandler(MapScreenControl sender, Map oldValue, Map newValue);

        readonly ICamera2D _camera;

        Vector2 _cameraVelocity = Vector2.Zero;
        Vector2 _cursorPos;
        DrawingManager _drawingManager;
        TickCount _lastUpdateTime = TickCount.MinValue;
        Map _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapScreenControl"/> class.
        /// </summary>
        public MapScreenControl()
        {
            if (DesignMode)
                return;

            _camera = new Camera2D(ClientSize.ToVector2()) { KeepInMap = true };

            lock (_instancesSync)
            {
                _instances.Add(this);
            }
        }

        /// <summary>
        /// Disposes the control
        /// </summary>
        /// <param name="disposing">If true, disposes of managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!DesignMode)
                {
                    lock (_instancesSync)
                    {
                        _instances.Remove(this);
                    }
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the camera used to view the map.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets the current position of the cursor in the world.
        /// </summary>
        [Browsable(false)]
        public Vector2 CursorPos
        {
            get { return _cursorPos; }
            set { _cursorPos = value; }
        }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> used to display the map.
        /// </summary>
        public IDrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        /// <summary>
        /// Gets or sets the map being displayed on this <see cref="MapScreenControl"/>.
        /// </summary>
        [Browsable(false)]
        public Map Map
        {
            get { return _map; }
            set {
                if (_map == value)
                    return;

                var oldValue = _map;
                _map = value;

                OnMapChanged(oldValue, value);
                if (MapChanged != null)
                    MapChanged(this, oldValue, value);
            }
        }

        /// <summary>
        /// Changes the current map being displayed.
        /// </summary>
        /// <param name="mapID">The <see cref="MapID"/> to change to.</param>
        public void ChangeMap(MapID mapID)
        {
            if (Map != null && Map.ID == mapID)
                return;

            Map = new Map(mapID, Camera, this);
            Map.Load(ContentPaths.Dev, false, MapEditorDynamicEntityFactory.Instance);
        }

        /// <summary>
        /// Handles drawing the GUI for the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawMapGUI(ISpriteBatch sb)
        {
            foreach (var t in ToolManager.Instance.EnabledTools)
            {
                t.InvokeBeforeDrawMapGUI(sb, Map);
            }

            // Cursor coordinates
            var font = GlobalState.Instance.DefaultRenderFont;

            var cursorPosText = CursorPos.ToString();
            var cursorPosTextPos = new Vector2(ClientSize.Width, ClientSize.Height) - font.MeasureString(cursorPosText) -
                                   new Vector2(4);

            sb.DrawStringShaded(font, cursorPosText, cursorPosTextPos, Color.White, Color.Black);

            foreach (var t in ToolManager.Instance.EnabledTools)
            {
                t.InvokeAfterDrawMapGUI(sb, Map);
            }
        }

        /// <summary>
        /// Handles drawing the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawMapWorld(ISpriteBatch sb)
        {
            // Check for a valid map
            if (Map == null)
                return;

            DrawingManager.LightManager.Ambient = Map.AmbientLight;

            Map.Draw(sb);

            // TODO: !! Selection area
            /*
            CursorManager.DrawSelection(sb);
            */
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            if (DesignMode)
            {
                base.HandleDraw(currentTime);
                return;
            }

            int deltaTime;
            if (_lastUpdateTime == TickCount.MinValue)
                deltaTime = 30;
            else
                deltaTime = Math.Max(5, (int)(currentTime - _lastUpdateTime));

            _lastUpdateTime = currentTime;

            DrawingManager.Update(currentTime);

            _camera.Min += _cameraVelocity * (deltaTime / 1000f);

            // Update
            UpdateMap(currentTime, deltaTime);

            ToolManager.Instance.MapDrawingExtensions.Map = Map;

            // Draw the world
            var worldSB = DrawingManager.BeginDrawWorld(Camera);
            if (worldSB != null)
            {
                DrawMapWorld(worldSB);
                DrawingManager.EndDrawWorld();
            }

            // Draw the GUI
            var guiSB = DrawingManager.BeginDrawGUI();
            if (guiSB != null)
            {
                DrawMapGUI(guiSB);
                DrawingManager.EndDrawGUI();
            }
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // We don't want to initialize any of this stuff in the design mode
            if (DesignMode)
                return;

            _drawingManager = new DrawingManager(RenderWindow);

            // Add an event hook to the tick timer so we can update ourself
            GlobalState.Instance.Tick -= InvokeDrawing;
            GlobalState.Instance.Tick += InvokeDrawing;
        }

        /// <summary>
        /// Determines whether the specified key is a regular input key or a special key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values.</param>
        /// <returns>
        /// true if the specified key is a regular input key; otherwise, false.
        /// </returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (DesignMode)
            {
                return base.IsInputKey(keyData);
            }

            var s = Settings.Default;

            if (keyData == s.Screen_ScrollLeft || keyData == s.Screen_ScrollRight || keyData == s.Screen_ScrollUp ||
                keyData == s.Screen_ScrollDown)
                return true;

            return base.IsInputKey(keyData);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (DesignMode)
                return;

            // Update the camera velocity
            var s = Settings.Default;
            if (e.KeyCode == s.Screen_ScrollLeft)
                _cameraVelocity.X = -s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollRight)
                _cameraVelocity.X = s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollUp)
                _cameraVelocity.Y = -s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollDown)
                _cameraVelocity.Y = s.Screen_ScrollPixelsPerSec;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (DesignMode)
                return;

            // Update the camera velocity
            var s = Settings.Default;
            if (e.KeyCode == s.Screen_ScrollLeft || e.KeyCode == s.Screen_ScrollRight)
                _cameraVelocity.X = 0;
            else if (e.KeyCode == s.Screen_ScrollDown || e.KeyCode == s.Screen_ScrollUp)
                _cameraVelocity.Y = 0;
        }

        /// <summary>
        /// Notifies listeners when the map has changed.
        /// </summary>
        public event MapChangedEventHandler MapChanged;

        /// <summary>
        /// Handles the <see cref="MapScreenControl.MapChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMapChanged(Map oldValue, Map newValue)
        {
            // Update some references
            Camera.Map = Map;

            // Remove all of the walls previously created from the MapGrhs
            var grhWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(Map.MapGrhs, (pos, size) => new WallEntity(pos, size));
            var dupeWalls = Map.FindDuplicateWalls(grhWalls);
            foreach (var dupeWall in dupeWalls)
            {
                Map.RemoveEntity(dupeWall);
            }

            // Reset some of the state values
            Camera.Min = Vector2.Zero;

            // Remove all lights for the old map from the light manager
            if (oldValue != null)
            {
                foreach (var light in oldValue.Lights)
                {
                    DrawingManager.LightManager.Remove(light);
                }
            }

            // Add the lights from the new map
            if (newValue != null)
            {
                foreach (var light in newValue.Lights)
                {
                    DrawingManager.LightManager.Add(light);
                }
            }

            // Remove the refraction effects from the old map
            if (oldValue != null)
            {
                foreach (var fx in oldValue.RefractionEffects)
                {
                    DrawingManager.RefractionManager.Remove(fx);
                }
            }

            // Add the refraction effects for the new map
            if (newValue != null)
            {
                foreach (var fx in newValue.RefractionEffects)
                {
                    DrawingManager.RefractionManager.Add(fx);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            if (DesignMode)
                return;

            _cameraVelocity = Vector2.Zero;
        }

        /// <summary>
        /// Handles MouseDown events.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (DesignMode)
                return;

            if (((IMapBoundControl)this).IMap != null)
                Focus();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode)
                return;

            if (Camera != null)
                _cursorPos = Camera.ToWorld(e.X, e.Y);
        }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="GraphicsDeviceControl.RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="GraphicsDeviceControl.RenderWindow"/>.</param>
        protected override void OnRenderWindowCreated(RenderWindow newRenderWindow)
        {
            base.OnRenderWindowCreated(newRenderWindow);

            if (DesignMode)
                return;

            // Update the DrawingManager
            if (_drawingManager == null || _drawingManager.IsDisposed)
                _drawingManager = new DrawingManager(newRenderWindow);
            else
                _drawingManager.RenderWindow = newRenderWindow;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (DesignMode)
                return;

            Camera.Size = ClientSize.ToVector2() * Camera.Scale;
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="deltaTime">The amount of time that has elapsed since the last update.</param>
        protected virtual void UpdateMap(TickCount currentTime, int deltaTime)
        {
            Cursor = Cursors.Default;

            if (Map != null)
                Map.Update(deltaTime);
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public TickCount GetTime()
        {
            return TickCount.Now;
        }

        #endregion

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the current <see cref="IMapBoundControl.IMap"/>.
        /// </summary>
        IMap IMapBoundControl.IMap { get; set; }

        #endregion
    }
}