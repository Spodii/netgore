using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;

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

    /*
    public class DeleteItemQuery : DbQueryNonReader<int>
    {
        const string _queryString = "DELETE FROM `items` WHERE `guid`=@guid LIMIT 1";
        static readonly IEnumerable<DbParameter> _dbParameters = new DbParameter[] {
            new MySqlParameter("@guid", null)};

        public DeleteItemQuery(DbConnectionPool conn) : base(conn, _queryString, _dbParameters)
        {
        }

        protected override void SetParameters(DbParameterCollection parameters, int item)
        {
            parameters["@guid"].Value = item;
        }
    }
    */
}