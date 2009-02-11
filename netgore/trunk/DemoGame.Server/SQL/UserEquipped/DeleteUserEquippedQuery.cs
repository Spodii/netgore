using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;

namespace DemoGame.Server
{
    public class DeleteUserEquippedQuery : NonReaderQueryBase<int>
    {
        readonly MySqlParameter _itemGuid = new MySqlParameter("@itemGuid", null);

        public DeleteUserEquippedQuery(MySqlConnection conn) : base(conn)
        {
            string query = "DELETE FROM `{0}` WHERE `item_guid`=@itemGuid LIMIT 1";
            query = string.Format(query, InsertUserEquippedQuery.UserEquippedTable);

            AddParameters(_itemGuid);
            Initialize(query);
        }

        protected override void SetParameters(int item)
        {
            _itemGuid.Value = item;
        }
    }
}