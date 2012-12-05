using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An instance for a particle effect that is referenced by name and displayed at a custom position. This provides an easy
    /// way to reference and persist a particle effect by name instead of having to persist the a deep copy.
    /// </summary>
    public class ParticleEffectReference : IPersistable
    {
        const string _categoryName = "Map Particle Effect";

        IParticleEffect _particleEffect;
        string _particleEffectName;
        Vector2 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectReference"/> class.
        /// </summary>
        protected ParticleEffectReference()
        {
            // This constructor is required for adding effects via the editor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectReference"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public ParticleEffectReference(IValueReader reader)
        {
            ReadState(reader);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectReference"/> class.
        /// </summary>
        /// <param name="particleEffectName">The name of the particle effect.</param>
        public ParticleEffectReference(string particleEffectName)
        {
            ParticleEffectName = particleEffectName;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return ParticleEffectName ?? string.Empty;
        }

        /// <summary>
        /// Gets the <see cref="ParticleEffect"/> instance. Can be null.
        /// </summary>
        [Browsable(false)]
        protected IParticleEffect ParticleEffect
        {
            get { return _particleEffect; }
            private set
            {
                if (_particleEffect == value)
                    return;

                _particleEffect = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the particle effect to display. When the name is not a valid particle effect, the effect
        /// will be set to null.
        /// </summary>
        [SyncValue]
        [Category(_categoryName)]
        [DisplayName("Particle Effect Name")]
        [Description("The name of the particle effect to use.")]
        [Browsable(true)]
        public string ParticleEffectName
        {
            get { return _particleEffectName; }
            set
            {
                if (_particleEffectName == value)
                    return;

                _particleEffectName = value;

                ParticleEffect = ParticleEffectManager.Instance.TryCreateEffect(_particleEffectName);

                if (ParticleEffect != null)
                    ParticleEffect.Position = Position;
            }
        }

        /// <summary>
        /// Gets or sets the position of the particle effect.
        /// </summary>
        [SyncValue]
        [Category(_categoryName)]
        [DisplayName("Position")]
        [Browsable(true)]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)
                    return;

                _position = value;

                if (ParticleEffect != null)
                    ParticleEffect.Position = Position;
            }
        }

        /// <summary>
        /// Draws the <see cref="ParticleEffectReference"/>.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (ParticleEffect == null)
                return;

            ParticleEffect.Draw(sb);
        }

        /// <summary>
        /// Updates the <see cref="ParticleEffectReference"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        public void Update(TickCount currentTime)
        {
            if (ParticleEffect == null)
                return;

            ParticleEffect.Update(currentTime);

            if (ParticleEffect.IsExpired)
                ParticleEffect.Reset();
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        public bool InView(ICamera2D camera)
        {
            if (ParticleEffect == null)
                return false;

            return ParticleEffect.InView(camera);
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}