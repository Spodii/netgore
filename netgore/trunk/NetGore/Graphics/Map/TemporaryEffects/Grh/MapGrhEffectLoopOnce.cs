using System.Linq;
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
        public MapGrhEffectLoopOnce(Grh grh, Vector2 position, bool isForeground) : base(grh, position, isForeground)
        {
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapGrhEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method should be overridden instead of
        /// <see cref="MapGrh.Update"/>. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected override void UpdateEffect(TickCount currentTime)
        {
            if (Grh == null || Grh.AnimType != AnimType.Loop)
                Kill();
        }
    }
}