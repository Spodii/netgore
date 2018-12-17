using System.Linq;
using MySql.Data.MySqlClient;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// Extension methods for the <see cref="DbConnectionSettings"/>.
    /// </summary>
    public static class DbConnectionSettingsExtensions
    {
        // Ping the server to stay alive if nothing has been queried within two hours
        private const int KeepAliveTimeSeconds = 7200;

        /// <summary>
        /// Makes a MySql connection string using the given settings.
        /// </summary>
        /// <returns>The MySql connection string.</returns>
        public static string GetMySqlConnectionString(this DbConnectionSettings settings)
        {
            var sb = new MySqlConnectionStringBuilder
            {
                Database = settings.Database,
                UserID = settings.User,
                Password = settings.Pass,
                Server = settings.Host,
                Port = settings.Port,
                IgnorePrepare = true,
                Pooling = true,
                UseCompression = false,
                UsePerformanceMonitor = false,
                UseUsageAdvisor = false,
                InteractiveSession = false,
                Keepalive = KeepAliveTimeSeconds
            };

            return sb.ToString();
        }
    }
}