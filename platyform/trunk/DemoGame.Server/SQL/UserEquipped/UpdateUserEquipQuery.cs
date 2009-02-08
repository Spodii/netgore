using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class InsertUserEquippedQuery : NonReaderQueryBase<InsertUserEquippedValues>
    {
        public const string UserEquippedTable = "user_equipped";

        readonly MySqlParameter _itemGuid = new MySqlParameter("@itemGuid", null);
        readonly MySqlParameter _slot = new MySqlParameter("@slot", null);
        readonly MySqlParameter _userGuid = new MySqlParameter("@userGuid", null);

        public InsertUserEquippedQuery(MySqlConnection conn) : base(conn)
        {
            string query = "INSERT INTO `{0}` SET `user_guid`=@userGuid,`item_guid`=@itemGuid,`slot`=@slot";
            query = string.Format(query, UserEquippedTable);

            var parameters = new MySqlParameter[] { _userGuid, _itemGuid, _slot };

            AddParameters(parameters);
            Initialize(query);
        }

        protected override void SetParameters(InsertUserEquippedValues item)
        {
            _userGuid.Value = item.UserGuid;
            _itemGuid.Value = item.ItemGuid;
            _slot.Value = item.Slot.GetIndex();
        }
    }
}