namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an effect that is displayed on the map for a short period of time.
    /// </summary>
    public interface ITemporaryMapEffect
    {
        /// <summary>
        /// Updates the map effect.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void Update(TickCount currentTime);

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map.
        /// </summary>
        bool IsAlive { get; }
    }
}