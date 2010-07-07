using System.Linq;
using NetGore.World;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="IDelayedMapEvent"/> that creates a <see cref="MapGrhEffect"/> on a map after a specified amount of time has elapsed.
    /// </summary>
    public class TimeDelayedMapGrhEffect : TimeDelayedMapEventBase
    {
        readonly MapGrhEffect _effect;
        readonly IDrawableMap _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeDelayedMapEventBase"/> class.
        /// </summary>
        /// <param name="map">The <see cref="IDrawableMap"/> to add the <paramref name="mapGrhEffect"/> to.</param>
        /// <param name="mapGrhEffect">The <see cref="MapGrhEffect"/> to add when the time elapses.</param>
        /// <param name="delay">The delay in milliseconds before the <paramref name="mapGrhEffect"/> is added to the map.</param>
        public TimeDelayedMapGrhEffect(IDrawableMap map, MapGrhEffect mapGrhEffect, int delay) : base(delay)
        {
            _map = map;
            _effect = mapGrhEffect;
        }

        /// <summary>
        /// When overridden in the derived class, handles executing the event.
        /// </summary>
        protected override void Execute()
        {
            if (_effect == null || _effect.Grh == null || _map == null)
                return;

            var g = _effect.Grh;

            // Set the Grh again, which will reset the animation back at the start
            g.SetGrh(g.GrhData, g.AnimType, TickCount.Now);

            // Add the MapGrhEffect to the map
            _map.AddTemporaryMapEffect(_effect);
        }
    }
}