using System.Linq;
using NetGore;

namespace DemoGame
{
    public abstract class WorldBase : IGetTime
    {
        bool _isFirstUpdate = true;
        int _lastUpdateTime;

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

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public abstract int GetTime();

        #endregion
    }
}