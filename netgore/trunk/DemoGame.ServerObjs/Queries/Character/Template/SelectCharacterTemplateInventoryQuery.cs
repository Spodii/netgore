using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateInventoryQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `character_template_id`=@characterTemplateID",
                                                            DBTables.CharacterTemplateInventory);

        public SelectCharacterTemplateInventoryQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<CharacterTemplateInventoryTable> Execute(CharacterTemplateID templateID)
        {
            var ret = new List<CharacterTemplateInventoryTable>();

            using (IDataReader r = ExecuteReader(templateID))
            {
                while (r.Read())
                {
                    var item = new CharacterTemplateInventoryTable(r);
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