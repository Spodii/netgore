using System;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectServerSettingsQuery : DbQueryReader
    {
        static readonly string _queryStr = string.Format("SELECT * FROM {0}", ServerSettingTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectServerSettingsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectServerSettingsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public IServerSettingTable Execute()
        {
            ServerSettingTable retSettings;

            using (var r = ExecuteReader())
            {
                if (!r.Read()) return null;

                retSettings = new ServerSettingTable();
                retSettings.ReadValues(r);
            }

            return retSettings;
        }
    }
}