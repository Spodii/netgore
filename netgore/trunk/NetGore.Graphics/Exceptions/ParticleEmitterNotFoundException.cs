using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Graphics
{
    public sealed class ParticleEmitterNotFoundException : ParticleEmitterException
    {
        public ParticleEmitterNotFoundException(string emitterName)
            : base(string.Format("Particle emitter `{0}` was not found.", emitterName))
        {
        }
    }
}
