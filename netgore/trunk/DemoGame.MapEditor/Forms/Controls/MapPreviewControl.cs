using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Draws a preview of the whole map into the control, no matter the size of the map or control.
    /// </summary>
    class MapPreviewControl : GraphicsDeviceControl
    {
        readonly ICamera2D _camera = new Camera2D(Vector2.Zero);

        /// <summary>
        /// Gets or sets the camera used to view the map.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        protected override void Draw(ISpriteBatch spriteBatch)
        {
            // Clear the background
            GraphicsDevice.Clear(Color.Black);

            // Check for a valid map
            if (Camera == null || Camera.Map == null)
                return;

            var map = Camera.Map as Map;
            if (map == null)
                return;

            // Begin drawing
            var size = new Vector2(Size.Width, Size.Height);
            var scale = size / map.Size;
            var m = Matrix.Identity * Matrix.CreateScale(scale.X, scale.Y, 1f);
            spriteBatch.BeginUnfiltered(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, m);

            // Adjust the camera
            _camera.Scale = Math.Max(scale.X, scale.Y);
            _camera.Size = map.Size;
            _camera.Min = Vector2.Zero;

            // Store the current map values
            var oldDrawParticles = map.DrawParticles;
            var oldCamera = map.Camera;
            var oldDrawFilter = map.DrawFilter;

            // Set our custom map values
            map.DrawParticles = false;
            map.Camera = _camera;
            map.DrawFilter = null;

            // Draw the map
            map.Draw(spriteBatch);

            // Restore the old values
            map.DrawParticles = oldDrawParticles;
            map.Camera = oldCamera;
            map.DrawFilter = oldDrawFilter;

            // End drawing
            spriteBatch.End();
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
    }
}