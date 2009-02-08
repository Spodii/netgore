using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    class ProcedureCache
    {
        readonly Queue<int> hashQueue;
        readonly int maxSize;
        readonly Hashtable procHash;

        public ProcedureCache(int size)
        {
            maxSize = size;
            hashQueue = new Queue<int>(maxSize);
            procHash = new Hashtable(maxSize);
        }

        DataSet AddNew(MySqlConnection connection, string spName)
        {
            DataSet procData = GetProcData(connection, spName);
            if (maxSize > 0)
            {
                int hash = spName.GetHashCode();
                lock (procHash.SyncRoot)
                {
                    if (procHash.Keys.Count >= maxSize)
                        TrimHash();
                    if (!procHash.ContainsKey(hash))
                    {
                        procHash[hash] = procData;
                        hashQueue.Enqueue(hash);
                    }
                }
            }
            return procData;
        }

        static DataSet GetProcData(MySqlConnection connection, string spName)
        {
            int dotIndex = spName.IndexOf(".");
            string schema = spName.Substring(0, dotIndex);
            string name = spName.Substring(dotIndex + 1, spName.Length - dotIndex - 1);

            var restrictions = new string[4];
            restrictions[1] = schema.Length > 0 ? schema : connection.CurrentDatabase();
            restrictions[2] = name;
            DataTable procTable = connection.GetSchema("procedures", restrictions);
            if (procTable.Rows.Count > 1)
                throw new MySqlException(Resources.ProcAndFuncSameName);
            if (procTable.Rows.Count == 0)
                throw new MySqlException(String.Format(Resources.InvalidProcName, name, schema));

            // we don't use GetSchema here because that would cause another
            // query of procedures and we don't need that since we already
            // know the procedure we care about.
            ISSchemaProvider isp = new ISSchemaProvider(connection);
            DataTable parametersTable = isp.GetProcedureParameters(restrictions, procTable);

            DataSet ds = new DataSet();
            ds.Tables.Add(procTable);
            ds.Tables.Add(parametersTable);
            return ds;
        }

        public DataSet GetProcedure(MySqlConnection conn, string spName)
        {
            int hash = spName.GetHashCode();

            DataSet ds;
            lock (procHash.SyncRoot)
            {
                ds = (DataSet)procHash[hash];
            }
            if (ds == null)
            {
                ds = AddNew(conn, spName);
                conn.PerfMonitor.AddHardProcedureQuery();

                if (conn.Settings.Logging)
                    Logger.LogInformation(String.Format(Resources.HardProcQuery, spName));
            }
            else
            {
                conn.PerfMonitor.AddSoftProcedureQuery();

                if (conn.Settings.Logging)
                    Logger.LogInformation(String.Format(Resources.SoftProcQuery, spName));
            }
            return ds;
        }

        void TrimHash()
        {
            int oldestHash = hashQueue.Dequeue();
            procHash.Remove(oldestHash);
        }
    }
}