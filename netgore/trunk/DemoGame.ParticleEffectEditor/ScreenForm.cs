using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace DemoGame.ParticleEffectEditor
{
    public partial class ScreenForm : Form, IGetTime
    {
        readonly Stopwatch _watch = new Stopwatch();

        ContentManager _content;
        ParticleEmitter _emitter;
        PointSpriteRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        public ScreenForm()
        {
            InitializeComponent();
            _watch.Start();
        }

        GraphicsDevice GraphicsDevice
        {
            get { return GameScreen.GraphicsDevice; }
        }

        /// <summary>
        /// Main entry point for all the screen drawing.
        /// </summary>
        public void DrawGame()
        {
            // Clear the background
            GraphicsDevice.Clear(Color.Black);
            _renderer.RenderEmitter(Emitter);
        }

        void ScreenForm_Load(object sender, EventArgs e)
        {
            GameScreen.Screen = this;

            _content = new ContentManager(GameScreen.Services, ContentPaths.Build.Root);

            var effect = _content.Load<Effect>("Fx/pointsprite");
            _renderer = new PointSpriteRenderer(GraphicsDevice, effect);
            var texture = _content.Load<Texture2D>("Grh/Particle/skull");
            Emitter = new ParticleEmitter(1000) { ParticleTexture = texture, Origin = new Vector2(300, 300) };
        }

        public ParticleEmitter Emitter
        {
            get { return _emitter; }
            set
            {
                if (_emitter == value)
                    return;

                _emitter = value;

                pgEffect.SelectedObject = Emitter;
            }
        }

        /// <summary>
        /// Main entry point for all the screen content updating.
        /// </summary>
        public void UpdateGame()
        {
            Emitter.Update(GetTime());
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public int GetTime()
        {
            return (int)_watch.ElapsedMilliseconds;
        }

        #endregion
    }
}