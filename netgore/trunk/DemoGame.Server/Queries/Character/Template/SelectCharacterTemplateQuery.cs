using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", CharacterTemplateTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTemplateTable.DbKeyColumns, "id");
        }

        public CharacterTemplateTable Execute(CharacterTemplateID templateID)
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
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified characterID.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="templateID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterTemplateID templateID)
        {
            p["@id"] = (int)templateID;
        }
    }
}