using System;
using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Holds the mutable name of a <see cref="ParticleEffect"/>. This allows multiple <see cref="ParticleEffect"/>s to use the
    /// same name without having to update them all whenever the name changes.
    /// </summary>
    sealed class ParticleEffectConfig
    {
        string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectConfig"/> class.
        /// </summary>
        internal ParticleEffectConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectConfig"/> class.
        /// </summary>
        /// <param name="name">The <see cref="ParticleEffect"/>'s name.</param>
        internal ParticleEffectConfig(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="ParticleEffect"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                if (ParticleEmitter.EmitterNameComparer.Equals(_name, value))
                    return;

                var oldName = _name;
                _name = ParticleEffectManager.Instance.GenerateUniqueEffectName(value);

                ParticleEffectManager.Instance.NotifyNameChanged(oldName, Name);
            }
        }
    }
}