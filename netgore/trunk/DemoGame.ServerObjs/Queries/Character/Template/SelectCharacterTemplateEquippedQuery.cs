using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateEquippedQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `character_template_id`=@characterTemplateID",
                                                            DBTables.CharacterTemplateEquipped);

        public SelectCharacterTemplateEquippedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<CharacterTemplateEquippedTable> Execute(CharacterTemplateID templateID)
        {
            var ret = new List<CharacterTemplateEquippedTable>();

            using (IDataReader r = ExecuteReader(templateID))
            {
                while (r.Read())
                {
                    var item = new CharacterTemplateEquippedTable(r);
                    ret.Add(item);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterTemplateID");
        }

        protected override void SetParameters(DbParameterValues p, CharacterTemplateID id)
        {
            p["@characterTemplateID"] = (int)id;
        }
    }
}