using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Client;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// The <see cref="GraphicsDeviceControl"/> that provides all the actual displaying and interaction of a <see cref="Map"/>
    /// instance.
    /// </summary>
    public class MapScreenControl : GraphicsDeviceControl, IMapBoundControl, IGetTime
    {
        readonly ScreenGrid _grid;

        DrawingManager _drawingManager;
        Vector2 _cursorPos;
        Map _map;
        MouseButtons _mouseButton;

        /// <summary>
        /// Allows derived classes to handle when the <see cref="GraphicsDeviceControl.RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="GraphicsDeviceControl.RenderWindow"/>.</param>
        protected override void OnRenderWindowCreated(RenderWindow newRenderWindow)
        {
            base.OnRenderWindowCreated(newRenderWindow);

            // Update the DrawingManager
            if (_drawingManager == null || _drawingManager.IsDisposed)
                _drawingManager = new DrawingManager(newRenderWindow);
            else
                _drawingManager.RenderWindow = newRenderWindow;
        }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/> used to display the map.
        /// </summary>
        public IDrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapScreenControl"/> class.
        /// </summary>
        public MapScreenControl()
        {
            if (DesignMode)
                return;

            _grid = new ScreenGrid();
        }

        /// <summary>
        /// Gets or sets the camera used to view the map.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera { get; set; }

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
        /// Gets or sets the method used to draw this control.
        /// </summary>
        public Action<ISpriteBatch> DrawHandler { get; set; }

        /// <summary>
        /// Gets the <see cref="ScreenGrid"/> to display for the <see cref="Map"/>.
        /// </summary>
        [Browsable(false)]
        public ScreenGrid Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Gets or sets the map being displayed on this <see cref="MapScreenControl"/>.
        /// </summary>
        [Browsable(false)]
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Gets the <see cref="MouseButtons"/> current pressed.
        /// </summary>
        [Browsable(false)]
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
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // We don't want to initialize any of this stuff in the design mode
            if (DesignMode)
                return;

            _drawingManager = new DrawingManager(RenderWindow);
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

            if (Camera != null)
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