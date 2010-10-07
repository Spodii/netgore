namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Delegate for handling events from the <see cref="IParticleEffect"/> related to a <see cref="IParticleEmitter"/>.
    /// </summary>
    /// <param name="sender">The <see cref="IParticleEffect"/> the event came from.</param>
    /// <param name="emitter">The <see cref="IParticleEmitter"/> related to the event.</param>
    public delegate void IParticleEffectEmitterEventHandler(IParticleEffect sender, IParticleEmitter emitter);
}