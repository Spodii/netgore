using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server
{
    public class SelectUserEquippedItemsQuery : SelectItemQueryBase<int>
    {
        const string _queryString = "SELECT items.*,user_equipped.slot FROM `items`,`user_equipped` " +
                                  "WHERE user_equipped.user_guid = @userGuid " + "AND items.guid = user_equipped.item_guid";

        public SelectUserEquippedItemsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryString)
        {
        }

        public IDictionary<EquipmentSlot, ItemValues> Execute(int userGuid)
        {
            var retValues = new Dictionary<EquipmentSlot, ItemValues>();

            using (var r = ExecuteReader(userGuid))
            {
                while (r.Read())
                {
                    EquipmentSlot slot = r.GetEquipmentSlot("slot");
                    ItemValues values = GetItemValues(r);
                    retValues.Add(slot, values);
                }
            }

            return retValues;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@userGuid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@userGuid"] = item;
        }
    }
}
