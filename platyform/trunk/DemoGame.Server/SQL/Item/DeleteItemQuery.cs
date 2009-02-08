using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public class DeleteItemQuery : NonReaderQueryBase<int>
    {
        readonly MySqlParameter _id = new MySqlParameter("@guid", null);

        public DeleteItemQuery(MySqlConnection conn) : base(conn)
        {
            AddParameter(_id);
            Initialize("DELETE FROM `items` WHERE `guid`=@guid LIMIT 1");
        }

        protected override void SetParameters(int itemIndex)
        {
            _id.Value = itemIndex;
        }
    }
}