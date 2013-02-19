using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using DemoGame.Editor.Tools;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;
using SFML.Window;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace DemoGame.Editor
{

    public class GrhAtlasView : GraphicsDeviceControl, IGetTime
    {
        readonly ICamera2D _camera;
        readonly TransBoxManager _transBoxManager;

        public TilesetConfiguration TilesetConfiguration { get; set; }

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
        public GrhAtlasView()
        {
            TilesetConfiguration = null;
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Runtime)
                return;
            _transBoxManager = new TransBoxManager();
            _camera = new Camera2D(ClientSize.ToVector2()) { KeepInMap = false };





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
        /// Gets the <see cref="TransBoxManager"/> for this <see cref="MapScreenControl"/>.
        /// </summary>
        [Browsable(false)]
        public TransBoxManager TransBoxManager
        {
            get { return _transBoxManager; }
        }



        /// <summary>
        /// Disposes the control
        /// </summary>
        /// <param name="disposing">If true, disposes of managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (!DesignMode)
            {

            }

            base.Dispose(disposing);
        }

        private int grhX = 0;
        private int grhY = 0;

        void SelectTile()
        {
            var tileSize = EditorSettings.Default.GridSize;
            int yAccu = (int) ((grhY/tileSize.X*TilesetConfiguration.Width) + grhX/tileSize.Y);

            // Do nothing if you go out of bounds
            if (yAccu > TilesetConfiguration.GrhDatas.Count - 1)
                return;

            GlobalState.Instance.Map.SetGrhToPlace(TilesetConfiguration.GrhDatas[yAccu].GrhIndex);

            var toolManager = ToolManager.Instance;

            var pencilTool = toolManager.TryGetTool<MapGrhPencilTool>();
            if (pencilTool != null && pencilTool.IsOnToolBar)
            {
                var fillTool = toolManager.TryGetTool<MapGrhFillTool>();
                if (fillTool == null || !fillTool.IsEnabled)
                {
                    pencilTool.IsEnabled = true;
                }
            }

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

            RenderWindow.Clear(Color.Black);

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


            // TODO: Implement drawing logic
            // Do some actual work here

            _spriteBatch.Begin(BlendMode.Alpha, _camera);


            if (TilesetConfiguration != null)
            {
                int x = 0;
                int y = 0;

                for (int i = 0; i < TilesetConfiguration.GrhDatas.Count; i++)
                {
                    var grh = TilesetConfiguration.GrhDatas[i];
                    Grh nGrh = new Grh(grh);

                    nGrh.Draw(_spriteBatch, new Vector2(y * EditorSettings.Default.GridSize.X, x * EditorSettings.Default.GridSize.Y));

                    y++;

                    if (y % TilesetConfiguration.Width == 0)
                    {
                        x++;
                        y = 0;
                    }

                }

                // Draw little selection box
                RenderRectangle.Draw(_spriteBatch, new Rectangle(grhX, grhY, EditorSettings.Default.GridSize.X, EditorSettings.Default.GridSize.Y), Color.TransparentWhite, Color.White, -3f);
            }


            _spriteBatch.End();

            int deltaTime;
            if (_lastUpdateTime == TickCount.MinValue)
                deltaTime = 30;
            else
                deltaTime = Math.Max(5, (int)(currentTime - _lastUpdateTime));

            _lastUpdateTime = currentTime;

            _camera.Min += _cameraVelocity * (deltaTime / 1000f);





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
            _spriteBatch = new SpriteBatch(RenderWindow);

            // Add an event hook to the tick timer so we can update ourself
            GlobalState.Instance.Tick -= InvokeDrawing;
            GlobalState.Instance.Tick += InvokeDrawing;
        }

        private SpriteBatch _spriteBatch;

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

        protected override void OnClick(EventArgs e)
        {
            if (DesignMode)
            {
                base.OnClick(e);
                return;
            }

            if (TilesetConfiguration == null)
                return;

            var mouse = Mouse.GetPosition(RenderWindow);
            var worldCordinates = Camera.ToWorld(mouse.X, mouse.Y);
            
            // Select the tile we need

            var tileSize = EditorSettings.Default.GridSize;

            grhX = (int) ((int) (worldCordinates.X/tileSize.X) * tileSize.X);
            grhY = (int) ((int) (worldCordinates.Y/tileSize.Y) * tileSize.Y);

            SelectTile();
        }

        void SelectedObjsManager_SelectedChanged(SelectedObjectsManager<object> sender, EventArgs e)
        {
            var items = sender.SelectedObjects.OfType<ISpatial>();
            TransBoxManager.SetItems(items, Camera);
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

    }
}