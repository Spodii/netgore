using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="MapGrhEffect"/> that dies after a specified amount of time.
    /// </summary>
    public class MapGrhEffectTimed : MapGrhEffect
    {
        readonly TickCount _expireTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffectTimed"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        /// <param name="life">How long the effect will last in milliseconds.</param>
        public MapGrhEffectTimed(Grh grh, Vector2 position, bool isForeground, int life)
            : base(grh, position, isForeground)
        {
            _expireTime = (TickCount)(TickCount.Now + life);
        }

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map.
        /// </summary>
        public override bool IsAlive
        {
            get { return TickCount.Now < _expireTime; }
        }
    }
}