using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for MySqlPool.
    /// </summary>
    sealed class MySqlPool
    {
        readonly AutoResetEvent autoEvent;
        readonly Queue<Driver> idlePool;
        readonly List<Driver> inUsePool;
        readonly uint maxSize;
        readonly uint minSize;
        readonly ProcedureCache procedureCache;
        int available;
        bool beingCleared;
        MySqlConnectionStringBuilder settings;

        /// <summary>
        /// Indicates whether this pool is being cleared.
        /// </summary>
        public bool BeingCleared
        {
            get { return beingCleared; }
        }

        /// <summary>
        /// It is assumed that this property will only be used from inside an active
        /// lock.
        /// </summary>
        bool HasIdleConnections
        {
            get { return idlePool.Count > 0; }
        }

        int NumConnections
        {
            get { return idlePool.Count + inUsePool.Count; }
        }

        public ProcedureCache ProcedureCache
        {
            get { return procedureCache; }
        }

        public MySqlConnectionStringBuilder Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public MySqlPool(MySqlConnectionStringBuilder settings)
        {
            minSize = settings.MinimumPoolSize;
            maxSize = settings.MaximumPoolSize;

            available = (int)maxSize;
            autoEvent = new AutoResetEvent(false);

            if (minSize > maxSize)
                minSize = maxSize;
            this.settings = settings;
            inUsePool = new List<Driver>((int)maxSize);
            idlePool = new Queue<Driver>((int)maxSize);

            // prepopulate the idle pool to minSize
            for (int i = 0; i < minSize; i++)
            {
                idlePool.Enqueue(CreateNewPooledConnection());
            }

            procedureCache = new ProcedureCache((int)settings.ProcedureCacheSize);

            beingCleared = false;
        }

        /// <summary>
        /// CheckoutConnection handles the process of pulling a driver
        /// from the idle pool, possibly resetting its state,
        /// and adding it to the in use pool.  We assume that this method is only
        /// called inside an active lock so there is no need to acquire a new lock.
        /// </summary>
        /// <returns>An idle driver object</returns>
        Driver CheckoutConnection()
        {
            Driver driver = idlePool.Dequeue();

            // first check to see that the server is still alive
            if (!driver.Ping())
            {
                driver.Close();
                driver = CreateNewPooledConnection();
            }

            // if the user asks us to ping/reset pooled connections
            // do so now
            if (settings.ConnectionReset)
                driver.Reset();

            return driver;
        }

        /// <summary>
        /// Clears this pool of all idle connections and marks this pool and being cleared
        /// so all other connections are closed when they are returned.
        /// </summary>
        internal void Clear()
        {
            lock ((idlePool as ICollection).SyncRoot)
            {
                // first, mark ourselves as being cleared
                beingCleared = true;

                // then we remove all connections sitting in the idle pool
                while (idlePool.Count > 0)
                {
                    Driver d = idlePool.Dequeue();
                    d.Close();
                }

                // there is nothing left to do here.  Now we just wait for all
                // in use connections to be returned to the pool.  When they are
                // they will be closed.  When the last one is closed, the pool will
                // be destroyed.
            }
        }

        /// <summary>
        /// It is assumed that this method is only called from inside an active lock.
        /// </summary>
        Driver CreateNewPooledConnection()
        {
            Debug.Assert((maxSize - NumConnections) > 0, "Pool out of sync.");

            Driver driver = Driver.Create(settings);
            driver.Pool = this;
            return driver;
        }

        public Driver GetConnection()
        {
            int fullTimeOut = (int)settings.ConnectionTimeout * 1000;
            int timeOut = fullTimeOut;

            DateTime start = DateTime.Now;

            while (timeOut > 0)
            {
                Driver driver = TryToGetDriver();
                if (driver != null)
                    return driver;

                // We have no tickets right now, lets wait for one.
                if (!autoEvent.WaitOne(timeOut, false))
                    break;
                timeOut = fullTimeOut - (int)DateTime.Now.Subtract(start).TotalMilliseconds;
            }
            throw new MySqlException(Resources.TimeoutGettingConnection);
        }

        /// <summary>
        /// It is assumed that this method is only called from inside an active lock.
        /// </summary>
        Driver GetPooledConnection()
        {
            Driver driver;

            // if we don't have an idle connection but we have room for a new
            // one, then create it here.
            lock ((idlePool as ICollection).SyncRoot)
            {
                if (!HasIdleConnections)
                    driver = CreateNewPooledConnection();
                else
                    driver = CheckoutConnection();
            }

            Debug.Assert(driver != null);
            lock ((inUsePool as ICollection).SyncRoot)
            {
                inUsePool.Add(driver);
            }
            return driver;
        }

        public void ReleaseConnection(Driver driver)
        {
            lock ((inUsePool as ICollection).SyncRoot)
            {
                if (inUsePool.Contains(driver))
                    inUsePool.Remove(driver);
            }

            if (driver.IsTooOld() || beingCleared)
            {
                driver.Close();
                Debug.Assert(!idlePool.Contains(driver));
            }
            else
            {
                lock ((idlePool as ICollection).SyncRoot)
                {
                    idlePool.Enqueue(driver);
                }
            }

            Interlocked.Increment(ref available);
            autoEvent.Set();
        }

        /// <summary>
        /// Removes a connection from the in use pool.  The only situations where this method 
        /// would be called are when a connection that is in use gets some type of fatal exception
        /// or when the connection is being returned to the pool and it's too old to be 
        /// returned.
        /// </summary>
        /// <param name="driver"></param>
        public void RemoveConnection(Driver driver)
        {
            lock ((inUsePool as ICollection).SyncRoot)
            {
                if (inUsePool.Contains(driver))
                {
                    inUsePool.Remove(driver);
                    Interlocked.Increment(ref available);
                    autoEvent.Set();
                }
            }

            // if we are being cleared and we are out of connections then have
            // the manager destroy us.
            if (beingCleared && NumConnections == 0)
                MySqlPoolManager.RemoveClearedPool(this);
        }

        Driver TryToGetDriver()
        {
            int count = Interlocked.Decrement(ref available);
            if (count < 0)
            {
                Interlocked.Increment(ref available);
                return null;
            }
            try
            {
                Driver driver = GetPooledConnection();
                return driver;
            }
            catch (Exception ex)
            {
                if (settings.Logging)
                    Logger.LogException(ex);
                Interlocked.Increment(ref available);
                throw;
            }
        }
    }
}