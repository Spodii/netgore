using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A specialization of a <see cref="MapGrh"/> that is used to display a <see cref="MapGrh"/> for only a short period of time
    /// as a map-based effect (such as for spells).
    /// </summary>
    public abstract class MapGrhEffect : MapGrh, ITemporaryMapEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffect"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        protected MapGrhEffect(Grh grh, Vector2 position, bool isForeground) : base(grh, position, isForeground)
        {
        }

        #region ITemporaryMapEffect Members

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map.
        /// </summary>
        public abstract bool IsAlive { get; }

        #endregion
    }
}