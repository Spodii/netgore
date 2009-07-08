using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateEquippedQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `character_id`=@characterID", DBTables.CharacterTemplateEquipped);

        public SelectCharacterTemplateEquippedQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectCharacterTemplateEquippedQueryValues> Execute(CharacterTemplateID templateID)
        {
            List<SelectCharacterTemplateEquippedQueryValues> ret = new List<SelectCharacterTemplateEquippedQueryValues>();

            using (var r = ExecuteReader(templateID))
            {
                while (r.Read())
                {
                    CharacterTemplateID character = r.GetCharacterTemplateID("character_id");
                    ItemTemplateID item = r.GetItemTemplateID("item_id");
                    ItemChance chance = r.GetItemChance("chance");

                    var v = new SelectCharacterTemplateEquippedQueryValues(character, item, chance);
                    ret.Add(v);
                }
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@characterID");
        }

        protected override void SetParameters(DbParameterValues p, CharacterTemplateID id)
        {
            p["@characterID"] = id;
        }
    }
}
