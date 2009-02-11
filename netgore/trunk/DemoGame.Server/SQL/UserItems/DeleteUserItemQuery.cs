using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;

namespace DemoGame.Server
{
    public class DeleteUserItemQuery : NonReaderQueryBase<int>
    {
        readonly MySqlParameter _itemGuid = new MySqlParameter("@item_guid", null);

        public DeleteUserItemQuery(MySqlConnection conn) : base(conn)
        {
            AddParameters(_itemGuid);

            Initialize("DELETE QUICK FROM `user_inventory` WHERE `item_guid`=@item_guid LIMIT 1");
        }

        protected override void SetParameters(int item)
        {
            _itemGuid.Value = item;
        }
    }
}