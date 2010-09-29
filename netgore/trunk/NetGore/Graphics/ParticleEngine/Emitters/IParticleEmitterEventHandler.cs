namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IParticleEmitter"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IParticleEmitter"/> the event came from.</param>
    public delegate void IParticleEmitterEventHandler(IParticleEmitter sender);
}