using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.World;
using SFML;
using SFML.Graphics;

namespace NetGore.Editor.Grhs
{
    public class GrhPreviewScreenControl : GraphicsDeviceControl
    {
        static readonly Color _autoWallColor = new Color(255, 255, 255, 150);

        readonly Camera2D _camera = new Camera2D(new Vector2(400, 300));
        readonly DrawingManager _drawingManager = new DrawingManager();

        Grh _grh;

        /// <summary>
        /// Gets the camera.
        /// </summary>
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Grh"/> to draw.
        /// </summary>
        public Grh Grh
        {
            get { return _grh; }
            set
            {
                if (_grh == value)
                    return;

                _grh = value;

                ResetCamera();
            }
        }

        /// <summary>
        /// Gets the size of the screen.
        /// </summary>
        Vector2 ScreenSize
        {
            get {
                if (RenderWindow == null)
                    return ClientSize.ToVector2();
                else
                return new Vector2(RenderWindow.Width, RenderWindow.Height); }
        }

        /// <summary>
        /// Gets or sets the collection of walls to be drawn.
        /// </summary>
        public IEnumerable<WallEntityBase> Walls { get; set; }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(TickCount currentTime)
        {
            base.HandleDraw(currentTime);

            if (DesignMode)
                return;

            if (_drawingManager.RenderWindow == null)
                return;

            if (Grh == null)
                return;

            Grh.Update(currentTime);

            RenderWindow.CurrentView = RenderWindow.CurrentView;
            _drawingManager.Update(currentTime);

            var sb = _drawingManager.BeginDrawWorld(_camera);
            if (sb == null)
                return;

            RenderWindow.CurrentView.Reset(new FloatRect(Camera.Min.X, Camera.Min.Y, Camera.Size.X, Camera.Size.Y));

            try
            {
                try
                {
                    Grh.Draw(sb, Vector2.Zero, Color.White);
                }
                catch (LoadingFailedException)
                {
                    // A LoadingFailedException is generally fine here since it probably means the graphic file was invalid
                    // or does not exist
                }

                // Draw the walls
                if (Walls != null)
                {
                    foreach (var wall in Walls)
                    {
                        var rect = wall.ToRectangle();
                        RenderRectangle.Draw(sb, rect, _autoWallColor);
                    }
                }
            }
            finally
            {
                _drawingManager.EndDrawWorld();
            }
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _camera.Size = ScreenSize;
        }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="RenderWindow"/>.</param>
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

            _camera.Size = ScreenSize;

            ResetCamera();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            var newValue = Camera.Scale + (e.Delta / 1200f);
            if (newValue < 0.1f)
                newValue = 0.1f;

            Camera.Scale = newValue;
        }

        /// <summary>
        /// Resets the camera back to the default view.
        /// </summary>
        public void ResetCamera()
        {
            Camera.Rotation = 0.0f;
            Camera.Min = Vector2.Zero;
            Camera.Size = ScreenSize;
        }
    }
}