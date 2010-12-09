using System;
using System.Linq;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="ParticleEffectListBox"/>'s Create event.
    /// </summary>
    public class ParticleEffectListBoxCreateEventArgs : EventArgs
    {
        readonly string _effectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffectListBoxCreateEventArgs"/> class.
        /// </summary>
        /// <param name="effectName">The name of the effect.</param>
        public ParticleEffectListBoxCreateEventArgs(string effectName)
        {
            _effectName = effectName;
        }

        /// <summary>
        /// Gets the name of the effect.
        /// </summary>
        public string EffectName
        {
            get { return _effectName; }
        }
    }
}