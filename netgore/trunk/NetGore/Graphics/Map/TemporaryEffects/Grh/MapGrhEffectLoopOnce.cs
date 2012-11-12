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
        public MapGrhEffectLoopOnce(Grh grh, Vector2 position) : base(grh, position)
        {
        }

        /// <summary>
        /// Forcibly kills the effect.
        /// </summary>
        /// <param name="immediate">When false and the <see cref="Grh"/> is animated, the animation will finish
        /// before terminating.</param>
        public override void Kill(bool immediate)
        {
            if (!immediate && Grh != null && Grh.AnimType != AnimType.None)
            {
                if (Grh.AnimType == AnimType.Loop)
                    Grh.AnimType = AnimType.LoopOnce;

                return;
            }

            base.Kill(immediate);
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapGrhEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method should be overridden instead of
        /// <see cref="MapGrh.Update"/>. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected override void UpdateEffect(TickCount currentTime)
        {
            if (Grh == null || Grh.AnimType == AnimType.None)
                Kill(true);
        }
    }
}