using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame
{
    public abstract class WorldBase : IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        int _lastUpdateTime;
        bool _isFirstUpdate = true;

        /// <summary>
        /// Updates the World.
        /// </summary>
        public virtual void Update()
        {
            // Get the current time
            int currentTime = GetTime();

            // Check if this is the very first update - if so, bring the timer up to date
            if (_isFirstUpdate)
            {
                _lastUpdateTime = currentTime;
                _isFirstUpdate = false;
            }

            // Grab the update rate
            int updateRate = GameData.WorldPhysicsUpdateRate;

            // Keep updating until there is not enough delta time to fit into the updateRate
            while (currentTime > _lastUpdateTime + updateRate)
            {
                UpdateMaps(updateRate);
                _lastUpdateTime += updateRate;
            }
        }
        
        /// <summary>
        /// When overridden in the derived class, handles updating all of the Maps in this World.
        /// </summary>
        /// <param name="deltaTime">Delta time to use for updating the maps.</param>
        protected abstract void UpdateMaps(int deltaTime);

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public abstract int GetTime();
    }
}
