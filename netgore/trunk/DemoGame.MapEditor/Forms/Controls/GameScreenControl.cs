using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    class GameScreenControl : GraphicsDeviceControl, IMapBoundControl
    {
        Vector2 _cursorPos;
        MouseButtons _mouseButton;

        /// <summary>
        /// Gets or sets the camera used to view the map.
        /// </summary>
        public ICamera2D Camera { get; set; }

        /// <summary>
        /// Gets or sets the current position of the cursor in the world.
        /// </summary>
        public Vector2 CursorPos
        {
            get { return _cursorPos; }
            set { _cursorPos = value; }
        }

        /// <summary>
        /// Gets or sets the method used to draw this control.
        /// </summary>
        public Action<ISpriteBatch> DrawHandler { get; set; }

        /// <summary>
        /// Gets the <see cref="MouseButtons"/> current pressed.
        /// </summary>
        public MouseButtons MouseButton
        {
            get { return _mouseButton; }
        }

        /// <summary>
        /// Gets or sets the method used to update this control.
        /// </summary>
        public Action UpdateHandler { get; set; }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use for drawing.</param>
        protected override void Draw(ISpriteBatch spriteBatch)
        {
            if (UpdateHandler != null)
                UpdateHandler();

            if (DrawHandler != null)
                DrawHandler(spriteBatch);
        }

        /// <summary>
        /// Handles MouseDown events.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _mouseButton = e.Button;

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

            _mouseButton = e.Button;
            _cursorPos = Camera.ToWorld(e.X, e.Y);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _mouseButton = e.Button;
        }

        #region IMapBoundControl Members

        /// <summary>
        /// Gets or sets the current <see cref="IMapBoundControl.IMap"/>.
        /// </summary>
        IMap IMapBoundControl.IMap { get; set; }

        #endregion
    }
}