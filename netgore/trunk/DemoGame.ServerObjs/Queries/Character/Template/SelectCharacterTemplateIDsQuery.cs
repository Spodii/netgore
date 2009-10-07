using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectCharacterTemplateIDsQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT `id` FROM `{0}`", CharacterTemplateTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCharacterTemplateIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectCharacterTemplateIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
            QueryAsserts.ArePrimaryKeys(CharacterTemplateTable.DbKeyColumns, "id");
        }

        public IEnumerable<CharacterTemplateID> Execute()
        {
            var ret = new List<CharacterTemplateID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var id = r.GetCharacterTemplateID("id");
                    ret.Add(id);
                }
            }

            return ret;
        }
    }
}