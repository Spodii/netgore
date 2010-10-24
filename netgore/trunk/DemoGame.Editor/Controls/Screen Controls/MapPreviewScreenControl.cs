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
    class MapPreviewControl : GraphicsDeviceControl
    {
        readonly ICamera2D _camera = new Camera2D(Vector2.Zero);
        readonly SpriteBatch _spriteBatch = new SpriteBatch(null);

        /// <summary>
        /// Gets or sets the camera used to view the map. The <see cref="ICamera2D.Map"/> property must be
        /// set for the map to be drawn.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera { get; set; }

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

            // Check for a valid map
            if (Camera == null || Camera.Map == null)
                return;

            var map = Camera.Map as EditorMap;
            if (map == null)
                return;

            // Adjust the camera
            _camera.Scale = 1;
            _camera.Size = map.Size;
            _camera.Min = Vector2.Zero;

            // Begin drawing
            _spriteBatch.Begin(BlendMode.Alpha, _camera);
            try
            {
                // Store the current map values
                var oldDrawParticles = map.DrawParticles;
                var oldCamera = map.Camera;
                var oldDrawFilter = map.DrawFilter;

                // Set our custom map values
                map.DrawParticles = false;
                map.Camera = _camera;
                map.DrawFilter = null;

                // Draw the map
                map.Draw(_spriteBatch);

                // Restore the old values
                map.DrawParticles = oldDrawParticles;
                map.Camera = oldCamera;
                map.DrawFilter = oldDrawFilter;
            }
            finally
            {
                // End drawing
                _spriteBatch.End();
            }
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