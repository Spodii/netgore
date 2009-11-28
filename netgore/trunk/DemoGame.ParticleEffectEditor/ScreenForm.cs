using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.Graphics.ParticleEngine;
using NetGore.IO;

namespace DemoGame.ParticleEffectEditor
{
    public partial class ScreenForm : Form, IGetTime
    {
        const string _defaultCategory = "Particle";

        readonly string _defaultTitle;
        readonly Stopwatch _watch = new Stopwatch();

        ContentManager _content;

        /// <summary>
        /// The file path of the currently loaded effect. Null if not loaded from file.
        /// </summary>
        string _currentEffectFilePath = null;

        ParticleEmitter _emitter;
        IParticleRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenForm"/> class.
        /// </summary>
        public ScreenForm()
        {
            InitializeComponent();

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _defaultTitle = Text;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            _watch.Start();
        }

        string CurrentEffectFilePath
        {
            get { return _currentEffectFilePath; }
            set
            {
                if (_currentEffectFilePath == value)
                    return;

                _currentEffectFilePath = value;

                string effectName;
                if (_currentEffectFilePath == null)
                    effectName = string.Empty;
                else
                {
                    effectName = " - ";
                    effectName += ParticleEffectHelper.GetEffectDisplayNameFromFile(_currentEffectFilePath);
                }

                Text = _defaultTitle + effectName;
            }
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

        GraphicsDevice GraphicsDevice
        {
            get { return GameScreen.GraphicsDevice; }
        }

        void btnLoad_Click(object sender, EventArgs e)
        {
            string filePath;
            ParticleEmitter emitter;
            var wasSuccessful = FileDialogs.TryOpenParticleEffect(out filePath, out emitter);

            if (wasSuccessful)
            {
                Emitter = emitter;
                CurrentEffectFilePath = filePath;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentEffectFilePath == null)
            {
                btnSaveAs_Click(this, null);
                return;
            }

            var emitterName = ParticleEffectHelper.GetEffectDisplayNameFromFile(CurrentEffectFilePath);
            ParticleEmitterFactory.SaveEmitter(ContentPaths.Dev, Emitter, emitterName);
        }

        void btnSaveAs_Click(object sender, EventArgs e)
        {
            string filePath;
            if (FileDialogs.TrySaveAsParticleEffect(Emitter, out filePath))
                CurrentEffectFilePath = filePath;
        }

        void cmbEmitter_SelectedEmitterChanged(ParticleEmitterComboBox sender, ParticleEmitter emitter)
        {
            if (_emitter == null)
            {
                Emitter = CreateInitialEmitter();
                return;
            }

            if (_emitter.GetType() == emitter.GetType())
                return;

            _emitter.CopyValuesTo(emitter);
            Emitter = emitter;
        }

        /// <summary>
        /// Creates the initial <see cref="ParticleEmitter"/> to display.
        /// </summary>
        /// <returns>The initial <see cref="ParticleEmitter"/> to display.</returns>
        ParticleEmitter CreateInitialEmitter()
        {
            var ret = new PointEmitter
            {
                SpriteCategorization = new SpriteCategorization(_defaultCategory, "ball"),
                Origin = new Vector2(GameScreen.Width, GameScreen.Height) / 2f,
                ReleaseRate = 35
            };

            var colorModifier = new ColorModifier
            { ReleaseColor = new Color(0, 255, 0, 255), UltimateColor = new Color(0, 0, 255, 175) };
            ret.Modifiers.Add(colorModifier);

            return ret;
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

        void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Emitter.Origin = new Vector2(e.X, e.Y);
        }

        void ScreenForm_Load(object sender, EventArgs e)
        {
            GameScreen.ScreenForm = this;

            _content = new ContentManager(GameScreen.Services, ContentPaths.Build.Root);
            GrhInfo.Load(ContentPaths.Build, _content);

            _renderer = new SpriteBatchRenderer(GraphicsDevice);
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