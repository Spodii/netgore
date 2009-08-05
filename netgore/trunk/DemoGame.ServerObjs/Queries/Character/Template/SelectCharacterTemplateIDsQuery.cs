using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateIDsQuery : DbQueryReader
    {
        static readonly string _queryString = string.Format("SELECT `id` FROM `{0}`", DBTables.CharacterTemplate);

        public SelectCharacterTemplateIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<CharacterTemplateID> Execute()
        {
            var ret = new List<CharacterTemplateID>();

            using (IDataReader r = ExecuteReader())
            {
                while (r.Read())
                {
                    CharacterTemplateID id = r.GetCharacterTemplateID("id");
                    ret.Add(id);
                }
            }

            return ret;
        }
    }
}