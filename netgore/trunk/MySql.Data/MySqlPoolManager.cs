using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Summary description for MySqlPoolManager.
    /// </summary>
    class MySqlPoolManager
    {
        static readonly List<MySqlPool> clearingPools = new List<MySqlPool>();
        static readonly Hashtable pools;

        static MySqlPoolManager()
        {
            pools = new Hashtable();
        }

        public static void ClearAllPools()
        {
            lock (pools.SyncRoot)
            {
                // Create separate keys list.
                var keys = new List<string>(pools.Count);

                foreach (string key in pools.Keys)
                {
                    keys.Add(key);
                }

                // Remove all pools by key.
                foreach (string key in keys)
                {
                    ClearPoolByText(key);
                }
            }
        }

        public static void ClearPool(MySqlConnectionStringBuilder settings)
        {
            string text = settings.GetConnectionString(true);
            ClearPoolByText(text);
        }

        static void ClearPoolByText(string key)
        {
            lock (pools.SyncRoot)
            {
                // add the pool to our list of pools being cleared
                MySqlPool pool = (MySqlPool)pools[key];
                clearingPools.Add(pool);

                // now tell the pool to clear itself
                pool.Clear();

                // and then remove the pool from the active pools list
                pools.Remove(key);
            }
        }

        public static MySqlPool GetPool(MySqlConnectionStringBuilder settings)
        {
            string text = settings.GetConnectionString(true);

            lock (pools.SyncRoot)
            {
                MySqlPool pool = (pools[text] as MySqlPool);

                if (pool == null)
                {
                    pool = new MySqlPool(settings);
                    pools.Add(text, pool);
                }
                else
                    pool.Settings = settings;

                return pool;
            }
        }

        public static void ReleaseConnection(Driver driver)
        {
            MySqlPool pool = driver.Pool;
            if (pool == null)
                return;

            pool.ReleaseConnection(driver);
        }

        public static void RemoveClearedPool(MySqlPool pool)
        {
            Debug.Assert(clearingPools.Contains(pool));
            clearingPools.Remove(pool);
        }

        public static void RemoveConnection(Driver driver)
        {
            MySqlPool pool = driver.Pool;
            if (pool == null)
                return;

            pool.RemoveConnection(driver);
        }
    }
}