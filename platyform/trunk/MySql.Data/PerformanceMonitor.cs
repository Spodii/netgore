using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    class PerformanceMonitor
    {
        static PerformanceCounter procedureHardQueries;
        static PerformanceCounter procedureSoftQueries;
        readonly MySqlConnection connection;

        public PerformanceMonitor(MySqlConnection connection)
        {
            this.connection = connection;

            string categoryName = Resources.PerfMonCategoryName;

            if (connection.Settings.UsePerformanceMonitor && procedureHardQueries == null)
            {
                try
                {
                    procedureHardQueries = new PerformanceCounter(categoryName, "HardProcedureQueries", false);
                    procedureSoftQueries = new PerformanceCounter(categoryName, "SoftProcedureQueries", false);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        public void AddHardProcedureQuery()
        {
            if (!connection.Settings.UsePerformanceMonitor || procedureHardQueries == null)
                return;
            procedureHardQueries.Increment();
        }

        public void AddSoftProcedureQuery()
        {
            if (!connection.Settings.UsePerformanceMonitor || procedureSoftQueries == null)
                return;
            procedureSoftQueries.Increment();
        }
    }
}