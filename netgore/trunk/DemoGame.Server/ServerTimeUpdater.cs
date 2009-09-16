using System;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Helps manage the queries made to the database to update the current server time.
    /// </summary>
    public class ServerTimeUpdater
    {
        readonly DbQueryNonReader _updateQuery;
        int _lastUpdateTime;
        int _updateRate = 3000;

        /// <summary>
        /// Gets or sets the rate, in milliseconds, that the server time is updated in the database.
        /// </summary>
        public int UpdateRate
        {
            get { return _updateRate; }
            set { _updateRate = value; }
        }

        public ServerTimeUpdater(DbQueryNonReader updateQuery)
        {
            if (updateQuery == null)
                throw new ArgumentNullException("updateQuery");

            _updateQuery = updateQuery;
        }

        /// <summary>
        /// Updates the server time.
        /// </summary>
        /// <param name="currentTime">The current game time. Used to determine if enough time has elapsed since
        /// the last update.</param>
        public void Update(int currentTime)
        {
            if (currentTime - _lastUpdateTime < UpdateRate)
                return;

            _lastUpdateTime = currentTime;
            _updateQuery.Execute();
        }
    }
}