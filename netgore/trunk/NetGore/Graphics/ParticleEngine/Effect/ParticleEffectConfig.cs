using System;
using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    sealed class ParticleEffectConfig
    {
        string _name;

        internal ParticleEffectConfig()
        {
        }

        internal ParticleEffectConfig(string name)
        {
            _name = name;
        }

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