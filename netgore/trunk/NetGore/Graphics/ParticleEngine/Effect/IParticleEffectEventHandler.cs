using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IParticleEffect"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IParticleEffect"/> the event came from.</param>
    public delegate void IParticleEffectEventHandler(IParticleEffect sender);
}