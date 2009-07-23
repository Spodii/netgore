using System.Collections.Generic;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class DeleteItemQuery : DbQueryNonReader<ItemID>
    {
        static readonly string _queryString = string.Format("DELETE FROM `{0}` WHERE `id`=@id LIMIT 1", DBTables.Item);

        public DeleteItemQuery(DbConnectionPool conn) : base(conn, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, ItemID id)
        {
            p["@id"] = (int)id;
        }
    }
}