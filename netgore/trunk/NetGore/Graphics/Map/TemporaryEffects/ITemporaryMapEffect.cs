using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an effect that is displayed on the map for a short period of time.
    /// </summary>
    public interface ITemporaryMapEffect
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ITemporaryMapEffect"/> has died. This is only raised once per
        /// <see cref="ITemporaryMapEffect"/>, and is raised when <see cref="ITemporaryMapEffect.IsAlive"/> is set to false.
        /// </summary>
        event TypedEventHandler<ITemporaryMapEffect> Died;

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        bool IsAlive { get; }

        MapRenderLayer MapRenderLayer { get; }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        void Draw(ISpriteBatch sb);

        /// <summary>
        /// Forcibly kills the effect.
        /// </summary>
        /// <param name="immediate">If true, the effect will be killed immediately and <see cref="IsAlive"/> will be
        /// false by the time the method returns. If false, the effect is allowed to enter itself into a "terminating" state,
        /// allowing it to cleanly transition the effect out.</param>
        void Kill(bool immediate);

        /// <summary>
        /// Updates the map effect.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void Update(TickCount currentTime);

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        bool InView(ICamera2D camera);
    }
}