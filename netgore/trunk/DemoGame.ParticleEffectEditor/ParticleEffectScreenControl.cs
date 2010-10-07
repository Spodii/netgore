using System;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using SFML.Graphics;

namespace DemoGame.ParticleEffectEditor
{
    public class ParticleEffectScreenControl : GraphicsDeviceControl
    {
        readonly DrawingManager _drawingManager = new DrawingManager();
        readonly ICamera2D _camera = new Camera2D(new Vector2(400,300));

        /// <summary>
        /// Gets or sets the <see cref="IParticleEffect"/> to display.
        /// </summary>
        public IParticleEffect ParticleEffect { get; set; }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/>.
        /// </summary>
        public IDrawingManager DrawingManager { get { return _drawingManager; } }

        /// <summary>
        /// Gets the <see cref="ICamera2D"/>.
        /// </summary>
        public ICamera2D Camera { get { return _camera; } }

        /// <summary>
        /// Allows derived classes to handle when the <see cref="RenderWindow"/> is created or re-created.
        /// </summary>
        /// <param name="newRenderWindow">The current <see cref="RenderWindow"/>.</param>
        protected override void OnRenderWindowCreated(SFML.Graphics.RenderWindow newRenderWindow)
        {
            base.OnRenderWindowCreated(newRenderWindow);

            _drawingManager.RenderWindow = newRenderWindow;

            Camera.Size = ClientSize.ToVector2();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Camera.Size = ClientSize.ToVector2();
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            Camera.Size = ClientSize.ToVector2();

            Timer t = new Timer { Interval = 1000 / 60 };
            t.Tick += (EventHandler)((x, y) => InvokeDrawing(TickCount.Now));
            t.Start();
        }

        /// <summary>
        /// When overridden in the derived class, draws the graphics to the control.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        protected override void HandleDraw(NetGore.TickCount currentTime)
        {
            var pe = ParticleEffect;
            if (pe == null)
                return;

            // Update
            pe.Update(currentTime);

            // Draw the world
            try
            {
                var worldSB = DrawingManager.BeginDrawWorld(Camera);
                if (worldSB != null)
                {
                    DrawWorld(worldSB);
                }
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
                {
                    DrawGUI(guiSB);
                }
            }
            finally
            {
                if (DrawingManager.State != DrawingManagerState.Idle)
                    DrawingManager.EndDrawGUI();
            }
        }

        /// <summary>
        /// Handles drawing the GUI for the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawGUI(ISpriteBatch sb)
        {
        }

        /// <summary>
        /// Handles drawing the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawWorld(ISpriteBatch sb)
        {
            var pe = ParticleEffect;
            if (pe == null)
                return;

            pe.Draw(sb);
        }
    }
}