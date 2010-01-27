using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectGuildByNameQuery : DbQueryReader<string>
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}` WHERE `name`=@name",
            GuildTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectGuildByNameQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectGuildByNameQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(GuildTable.DbColumns, "name");
        }

        public IGuildTable Execute(string guildName)
        {
            using (var r = ExecuteReader(guildName))
            {
                if (!r.Read())
                    return null;

                var ret = new GuildTable();
                ret.ReadValues(r);
                return ret;
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@name");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, string item)
        {
            p["@name"] = item;
        }
    }
}
