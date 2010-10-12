using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateQuery : DbQueryReader<CharacterTemplateID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ArePrimaryKeys(CharacterTemplateTable.DbKeyColumns, "id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(CharacterTemplateTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("id"), s.Parameterize("id")));
            return q.ToString();
        }

        public ICharacterTemplateTable Execute(CharacterTemplateID templateID)
        {
            CharacterTemplateTable ret;

            using (var r = ExecuteReader(templateID))
            {
                if (!r.Read())
                    return null;

                ret = new CharacterTemplateTable();
                ret.ReadValues(r);
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="templateID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterTemplateID templateID)
        {
            p["id"] = (int)templateID;
        }
    }
}