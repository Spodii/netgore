using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectItemTemplateQuery : DbQueryReader<ItemTemplateID>
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", ItemTemplateTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectItemTemplateQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectItemTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ArePrimaryKeys(ItemTemplateTable.DbKeyColumns, "id");
        }

        public IItemTemplateTable Execute(ItemTemplateID id)
        {
            var v = new ItemTemplateTable();

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new Exception(string.Format("No ItemTemplate found at ID `{0}`.", id));

                v.ReadValues(r);
            }

            return v;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ItemTemplateID item)
        {
            p["@id"] = item;
        }
    }
}