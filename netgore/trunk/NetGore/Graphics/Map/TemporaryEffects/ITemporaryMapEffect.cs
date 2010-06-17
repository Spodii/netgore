namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an effect that is displayed on the map for a short period of time.
    /// </summary>
    public interface ITemporaryMapEffect : ISpatial, IDrawable
    {
        /// <summary>
        /// Updates the map effect.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void Update(TickCount currentTime);

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Notifies listeners when this <see cref="ITemporaryMapEffect"/> has died. This is only raised once per
        /// <see cref="ITemporaryMapEffect"/>, and is raised when <see cref="ITemporaryMapEffect.IsAlive"/> is set to false.
        /// </summary>
        event TemporaryMapEffectDiedHandler Died;
    }
}