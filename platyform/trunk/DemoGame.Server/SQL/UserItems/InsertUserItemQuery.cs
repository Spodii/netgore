using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class InsertUserItemQuery : NonReaderQueryBase<InsertInventoryValues>
    {
        readonly MySqlParameter _itemGuid = new MySqlParameter("@item_guid", null);
        readonly MySqlParameter _userGuid = new MySqlParameter("@user_guid", null);

        public InsertUserItemQuery(MySqlConnection conn) : base(conn)
        {
            var sqlParams = new MySqlParameter[] { _userGuid, _itemGuid };
            AddParameters(sqlParams);

            Initialize("INSERT INTO `user_inventory` SET `user_guid`=@user_guid,`item_guid`=@item_guid");
        }

        protected override void SetParameters(InsertInventoryValues item)
        {
            _itemGuid.Value = item.ItemGuid;
            _userGuid.Value = item.UserGuid;
        }
    }

    public struct InsertInventoryValues
    {
        public readonly int ItemGuid;
        public readonly ushort UserGuid;

        public InsertInventoryValues(ushort userGuid, int itemGuid)
        {
            UserGuid = userGuid;
            ItemGuid = itemGuid;
        }
    }
}