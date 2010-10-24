using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    public abstract class WorldBase : IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The maximum number of times the world can be updated per tick. This is provided to ensure that when the program
        /// is paused for an extended period or gets really far behind, it doesn't get stuck updating over and over for a huge
        /// amount of time. The information is probably stale at this point anyways.
        /// </summary>
        const int _maxUpdateDeltaTime = 5000;

        bool _isFirstUpdate = true;
        TickCount _lastUpdateTime;

        /// <summary>
        /// Updates the World.
        /// </summary>
        public virtual void Update()
        {
            // Get the current time
            var currentTime = GetTime();

            // Check if this is the very first update - if so, bring the timer up to date
            if (_isFirstUpdate)
            {
                _lastUpdateTime = currentTime;
                _isFirstUpdate = false;
            }

            // Grab the update rate
            var updateRate = GameData.WorldPhysicsUpdateRate;

            // Check if we fell too far behind
            if (currentTime - _lastUpdateTime > _maxUpdateDeltaTime)
            {
                const string errmsg = "Fell too far behind in updates to catch up. Jumping ahead to current time.";
                if (log.IsWarnEnabled)
                    log.Warn(errmsg);

                // Push the time ahead to a bit before the current time
                _lastUpdateTime = (TickCount)(currentTime - updateRate - 1);
            }

            // Keep updating until there is not enough delta time to fit into the updateRate
            while (currentTime > _lastUpdateTime + updateRate)
            {
                UpdateMaps(updateRate);
                _lastUpdateTime += (uint)updateRate;
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles updating all of the Maps in this World.
        /// </summary>
        /// <param name="deltaTime">Delta time to use for updating the maps.</param>
        protected abstract void UpdateMaps(int deltaTime);

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public abstract TickCount GetTime();

        #endregion
    }
}