using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Draws a preview of the whole map into the control, no matter the size of the map or control.
    /// </summary>
    public class MapPreviewScreenControl : GraphicsDeviceControl
    {
        readonly SpriteBatch _spriteBatch;

        ICamera2D _camera = new Camera2D(Vector2.Zero);

        EditorMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewScreenControl"/> class.
        /// </summary>
        public MapPreviewScreenControl()
        {
            if (!DesignMode && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                _spriteBatch = new SpriteBatch(null);
        }

        /// <summary>
        /// Gets or sets the camera used to view the map. The <see cref="ICamera2D.Map"/> property must be
        /// set for the map to be drawn.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        /// <summary>
        /// Gets or sets the map to preview.
        /// </summary>
        public EditorMap Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Disposes the control
        /// </summary>
        /// <param name="disposing">If true, disposes of managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (!DesignMode)
                GlobalState.Instance.Tick -= InvokeDrawing;

            base.Dispose(disposing);
        }

        void FocusCameraAtScreenPoint(MouseEventArgs e)
        {
            if (Camera == null || Camera.Map == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var percent = new Vector2(e.X, e.Y) / new Vector2(Size.Width, Size.Height);
                var min = Vector2.Zero;
                var max = Vector2.One;
                Vector2.Clamp(ref percent, ref min, ref max, out percent);

                var worldPos = Camera.Map.Size * percent;
                Camera.CenterOn(worldPos);
            }
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            // Clear the background
            RenderWindow.Clear(Color.Black);

            // Check for a valid camera
            var cam = Camera;
            if (cam == null)
                return;

            var map = Map;
            if (map == null)
                return;

            // Adjust the camera
            cam.Scale = 1;
            cam.Size = map.Size;
            cam.Min = Vector2.Zero;

            // Store the current map values
            var oldCameraMap = cam.Map;
            var oldDrawParticles = map.DrawParticles;
            var oldCamera = map.Camera;
            var oldDrawFilter = map.DrawFilter;

            try
            {
                // Set our custom map values
                cam.Map = _map;
                map.DrawParticles = false;
                map.Camera = _camera;
                map.DrawFilter = null;

                // Begin drawing
                _spriteBatch.Begin(BlendMode.Alpha, cam);

                // Draw the map
                map.Draw(_spriteBatch);
            }
            finally
            {
                // End drawing
                _spriteBatch.End();

                // Restore the old values
                cam.Map = oldCameraMap;
                map.DrawParticles = oldDrawParticles;
                map.Camera = oldCamera;
                map.DrawFilter = oldDrawFilter;
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

            // Add an event hook to the tick timer so we can update ourself
            GlobalState.Instance.Tick -= InvokeDrawing;
            GlobalState.Instance.Tick += InvokeDrawing;
        }

        /// <summary>
        /// Handles MouseDown events.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            FocusCameraAtScreenPoint(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            FocusCameraAtScreenPoint(e);
        }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="GraphicsDeviceControl.RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="GraphicsDeviceControl.RenderWindow"/>.</param>
        protected override void OnRenderWindowCreated(RenderWindow newRenderWindow)
        {
            base.OnRenderWindowCreated(newRenderWindow);

            _spriteBatch.RenderTarget = newRenderWindow;
        }
    }
}