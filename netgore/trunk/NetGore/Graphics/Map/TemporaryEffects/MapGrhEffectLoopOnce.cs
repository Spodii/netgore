using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="MapGrhEffect"/> that dies after one loop.
    /// </summary>
    public class MapGrhEffectLoopOnce : MapGrhEffect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffectLoopOnce"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        public MapGrhEffectLoopOnce(Grh grh, Vector2 position, bool isForeground)
            : base(grh, position, isForeground)
        {
        }

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map.
        /// </summary>
        public override bool IsAlive
        {
            get { return Grh == null || Grh.AnimType != AnimType.Loop; }
        }
    }
}