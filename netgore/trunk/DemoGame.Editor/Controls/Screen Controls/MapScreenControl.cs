using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
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
    public partial class MapScreenControl : GraphicsDeviceControl, IGetTime, IToolTargetMapContainer
    {
        readonly ICamera2D _camera;
        readonly DrawingManager _drawingManager;
        readonly TransBoxManager _transBoxManager;

        Vector2 _cameraVelocity = Vector2.Zero;
        TickCount _lastUpdateTime = TickCount.MinValue;
        EditorMap _map;

        /// <summary>
        /// If the cursor was under a transbox the last frame.
        /// </summary>
        bool _wasUnderTransBox = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapScreenControl"/> class.
        /// </summary>
        public MapScreenControl()
        {
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;

            _drawingManager = new DrawingManager();
            _transBoxManager = new TransBoxManager();
            _camera = new Camera2D(ClientSize.ToVector2()) { KeepInMap = true };

            if (DrawingManager.LightManager.DefaultSprite == null)
                DrawingManager.LightManager.DefaultSprite = new Grh(GrhInfo.GetData("Effect", "light"));

            var selectedObjsManager = GlobalState.Instance.Map.SelectedObjsManager;
            selectedObjsManager.SelectedChanged -= SelectedObjsManager_SelectedChanged;
            selectedObjsManager.SelectedChanged += SelectedObjsManager_SelectedChanged;

            lock (_instancesSync)
            {
                _instances.Add(this);
            }
        }

        /// <summary>
        /// Notifies listeners when the map has changed.
        /// </summary>
        public event TypedEventHandler<MapScreenControl, ValueChangedEventArgs<EditorMap>> MapChanged;

        /// <summary>
        /// Gets the camera used to view the map.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets the map being displayed on this <see cref="MapScreenControl"/>.
        /// </summary>
        [Browsable(false)]
        public EditorMap Map
        {
            get { return _map; }
            set
            {
                if (_map == value)
                    return;

                var oldValue = _map;
                _map = value;

                TransBoxManager.Clear(Camera);

                // Load
                if (_map != null)
                {
                    _map.Load(ContentPaths.Dev, true, EditorDynamicEntityFactory.Instance);
                    _map.Camera = Camera;
                }

                // Raise events
                OnMapChanged(oldValue, value);
                if (MapChanged != null)
                    MapChanged.Raise(this, ValueChangedEventArgs.Create(oldValue, value));
            }
        }

        /// <summary>
        /// Gets the <see cref="TransBoxManager"/> for this <see cref="MapScreenControl"/>.
        /// </summary>
        [Browsable(false)]
        public TransBoxManager TransBoxManager
        {
            get { return _transBoxManager; }
        }

        /// <summary>
        /// Changes the current map being displayed.
        /// </summary>
        /// <param name="mapID">The <see cref="MapID"/> to change to.</param>
        public void ChangeMap(MapID mapID)
        {
            if (Map != null && Map.ID == mapID)
                return;

            Map = new EditorMap(mapID, Camera, this);
        }

        /// <summary>
        /// Disposes the control
        /// </summary>
        /// <param name="disposing">If true, disposes of managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (!DesignMode)
            {
                ToolManager.Instance.ToolTargetContainers.Remove(this);
                GlobalState.Instance.Map.SelectedObjsManager.SelectedChanged -= SelectedObjsManager_SelectedChanged;
                GlobalState.Instance.Tick -= InvokeDrawing;

                if (disposing)
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
        /// Handles drawing the GUI for the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawMapGUI(ISpriteBatch sb)
        {
            ToolManager.Instance.InvokeBeforeDrawMapGUI(sb, Map);

            TransBoxManager.Draw(sb, Camera);

            ToolManager.Instance.InvokeAfterDrawMapGUI(sb, Map);
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

            TransBoxManager.Update(currentTime);

            // Update the cursor display for the transformation box. Only change the cursor if we are currently
            // under a transbox, or we have just stopped being under one. If we update it every frame, it screws with the
            // UI display for everything else (like when the cursor is over a textbox).
            var transBoxCursor = TransBoxManager.CurrentCursor;
            if (transBoxCursor == null)
            {
                if (_wasUnderTransBox)
                {
                    Cursor = Cursors.Default;
                    _wasUnderTransBox = false;
                }
            }
            else
            {
                Cursor = transBoxCursor;
                _wasUnderTransBox = true;
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
            try
            {
                var worldSB = DrawingManager.BeginDrawWorld(Camera);
                if (worldSB != null)
                    DrawMapWorld(worldSB);
            }
            finally
            {
                if (DrawingManager.State != DrawingManagerState.Idle)
                    DrawingManager.EndDrawWorld();
            }

            // Draw the GUI
            try
            {
                var guiSB = DrawingManager.BeginDrawGUI();
                if (guiSB != null)
                    DrawMapGUI(guiSB);
            }
            finally
            {
                if (DrawingManager.State != DrawingManagerState.Idle)
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

            TransBoxManager.GridAligner = GridAligner.Instance;

            ToolManager.Instance.ToolTargetContainers.Add(this);

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
                return base.IsInputKey(keyData);

            var s = EditorSettings.Default;

            if (keyData == s.Screen_ScrollLeft || keyData == s.Screen_ScrollLeft2 ||
                keyData == s.Screen_ScrollRight || keyData == s.Screen_ScrollRight2 ||
                keyData == s.Screen_ScrollUp || keyData == s.Screen_ScrollUp2 ||
                keyData == s.Screen_ScrollDown || keyData == s.Screen_ScrollDown)
            {
                return true;
            }

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

            if (e.Control)
                return;

            // Update the camera velocity
            var s = EditorSettings.Default;
            if (e.KeyCode == s.Screen_ScrollLeft || e.KeyCode == s.Screen_ScrollLeft2)
                _cameraVelocity.X = -s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollRight || e.KeyCode == s.Screen_ScrollRight2)
                _cameraVelocity.X = s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollUp || e.KeyCode == s.Screen_ScrollUp2)
                _cameraVelocity.Y = -s.Screen_ScrollPixelsPerSec;
            else if (e.KeyCode == s.Screen_ScrollDown || e.KeyCode == s.Screen_ScrollDown2)
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
            var s = EditorSettings.Default;
            if (e.KeyCode == s.Screen_ScrollLeft || e.KeyCode == s.Screen_ScrollLeft2 ||
                e.KeyCode == s.Screen_ScrollRight || e.KeyCode == s.Screen_ScrollRight2)
            {
                _cameraVelocity.X = 0;
            }
            else if (e.KeyCode == s.Screen_ScrollDown || e.KeyCode == s.Screen_ScrollDown2 ||
                e.KeyCode == s.Screen_ScrollUp || e.KeyCode == s.Screen_ScrollUp2)
            {
                _cameraVelocity.Y = 0;
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
        /// Handles the <see cref="MapScreenControl.MapChanged"/> event.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnMapChanged(EditorMap oldValue, EditorMap newValue)
        {
            // Update some references
            Camera.Map = newValue;

            // Remove all of the walls previously created from the MapGrhs
            if (newValue != null)
            {
                MapHelper.RemoveBoundWalls(newValue);
            }

            // Reset some of the state values
            Camera.Min = Vector2.Zero;

            // Remove all of the old lights and add the new ones
            if (oldValue != null)
            {
                foreach (var light in oldValue.Lights)
                {
                    DrawingManager.LightManager.Remove(light);
                }
            }

            if (newValue != null)
            {
                foreach (var light in newValue.Lights)
                {
                    DrawingManager.LightManager.Add(light);
                }
            }

            // Remove all of the old refraction effects and add the new ones
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
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode)
            {
                base.OnMouseDown(e);
                return;
            }

            if (TransBoxManager.HandleMouseDown(e, Camera))
                return;

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (DesignMode)
            {
                base.OnMouseMove(e);
                return;
            }

            var screenPos = e.Position();
            var worldPos = Camera.ToWorld(screenPos);
            MainForm.UpdateCursorPos(worldPos, screenPos);

            if (TransBoxManager.HandleMouseMove(e, Camera))
                return;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode)
            {
                base.OnMouseUp(e);
                return;
            }

            if (TransBoxManager.HandleMouseUp(e, Camera))
                return;

            base.OnMouseUp(e);
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

        void SelectedObjsManager_SelectedChanged(SelectedObjectsManager<object> sender, EventArgs e)
        {
            var items = sender.SelectedObjects.OfType<ISpatial>();
            TransBoxManager.SetItems(items, Camera);
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="deltaTime">The amount of time that has elapsed since the last update.</param>
        protected virtual void UpdateMap(TickCount currentTime, int deltaTime)
        {
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

        #region IToolTargetMapContainer Members

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> used to display the map.
        /// </summary>
        public IDrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        /// <summary>
        /// Gets the <see cref="IDrawableMap"/> that this <see cref="IToolTargetMapContainer"/> holds.
        /// Can be null.
        /// </summary>
        IDrawableMap IToolTargetMapContainer.Map
        {
            get { return Map; }
        }

        #endregion
    }
}