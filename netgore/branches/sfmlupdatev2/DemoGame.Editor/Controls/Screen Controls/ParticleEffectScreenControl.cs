using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.Editor;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public class ParticleEffectScreenControl : GraphicsDeviceControl
    {
        readonly ICamera2D _camera;
        readonly DrawingManager _drawingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectScreenControl"/> class.
        /// </summary>
        public ParticleEffectScreenControl()
        {
            if (!DesignMode && LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                _camera = new Camera2D(new Vector2(400, 300));
                _drawingManager = new DrawingManager();
                DrawingManager.BackgroundColor = BackColor.ToColor();
            }
        }

        /// <summary>
        /// Gets the <see cref="ICamera2D"/>.
        /// </summary>
        [Browsable(false)]
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets the <see cref="IDrawingManager"/>.
        /// </summary>
        [Browsable(false)]
        public IDrawingManager DrawingManager
        {
            get { return _drawingManager; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IParticleEffect"/> to display.
        /// </summary>
        [Browsable(false)]
        public IParticleEffect ParticleEffect { get; set; }

        /// <summary>
        /// Handles drawing the GUI for the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawGUI(ISpriteBatch sb)
        {
            if (DesignMode)
                return;
        }

        /// <summary>
        /// Handles drawing the map.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        protected virtual void DrawWorld(ISpriteBatch sb)
        {
            if (DesignMode)
                return;

            var pe = ParticleEffect;
            if (pe == null)
                return;

            pe.Draw(sb);
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

            var pe = ParticleEffect;
            if (pe == null)
                return;

            if (pe.IsExpired)
                pe.Reset();

            // Update
            pe.Update(currentTime);

            // Draw the world
            try
            {
                var worldSB = DrawingManager.BeginDrawWorld(Camera);
                if (worldSB != null)
                    DrawWorld(worldSB);
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
                    DrawGUI(guiSB);
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

            if (DesignMode)
                return;

            Camera.Size = ClientSize.ToVector2();
            Camera.CenterOn(Vector2.Zero);

            var t = new Timer { Interval = 1000 / 60 };
            t.Tick += (EventHandler)((x, y) => InvokeDrawing(this, EventArgsHelper.Create(TickCount.Now)));
            t.Start();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.BackColorChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            if (DesignMode)
                return;

            if (DrawingManager != null)
                DrawingManager.BackgroundColor = BackColor.ToColor();
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

            DrawingManager.RenderWindow = newRenderWindow;

            var oldCenter = Camera.Center;
            Camera.Size = ClientSize.ToVector2();
            Camera.CenterOn(oldCenter);
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

            var oldCenter = Camera.Center;
            Camera.Size = ClientSize.ToVector2();
            Camera.CenterOn(oldCenter);
        }
    }
}