using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateInventoryQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `character_id`=@characterID",
                                                            DBTables.CharacterTemplateInventory);

        public SelectCharacterTemplateInventoryQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public IEnumerable<SelectCharacterTemplateInventoryQueryValues> Execute(CharacterTemplateID templateID)
        {
            var ret = new List<SelectCharacterTemplateInventoryQueryValues>();

            using (IDataReader r = ExecuteReader(templateID))
            {
                while (r.Read())
                {
                    CharacterTemplateID character = r.GetCharacterTemplateID("character_id");
                    ItemTemplateID item = r.GetItemTemplateID("item_id");
                    byte min = r.GetByte("min");
                    byte max = r.GetByte("max");
                    ItemChance chance = r.GetItemChance("chance");

                    SelectCharacterTemplateInventoryQueryValues v = new SelectCharacterTemplateInventoryQueryValues(character,
                                                                                                                    item, min, max,
                                                                                                                    chance);
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
            p["@characterID"] = (int)id;
        }
    }
}