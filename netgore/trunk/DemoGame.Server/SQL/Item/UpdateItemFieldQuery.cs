using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class UpdateItemFieldQuery : IDisposable
    {
        const string _queryString = "UPDATE `" + ReplaceItemQuery.ItemsTableName + "` SET `{0}`=@value WHERE `guid`=@itemGuid";
        readonly DbConnectionPool _connectionPool;

        readonly Dictionary<string, InternalUpdateItemFieldQuery> _fieldQueries =
            new Dictionary<string, InternalUpdateItemFieldQuery>(StringComparer.OrdinalIgnoreCase);

        bool _disposed;

        public UpdateItemFieldQuery(DbConnectionPool connectionPool)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");

            _connectionPool = connectionPool;
        }

        public void Execute(int itemGuid, string field, object value)
        {
            InternalUpdateItemFieldQuery fieldQuery;
            if (!_fieldQueries.TryGetValue(field, out fieldQuery))
            {
                string query = string.Format(_queryString, field.ToLower());
                fieldQuery = new InternalUpdateItemFieldQuery(_connectionPool, query);
                _fieldQueries.Add(field, fieldQuery);
            }

            QueryArgs values = new QueryArgs(itemGuid, value);
            fieldQuery.Execute(values);
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (InternalUpdateItemFieldQuery query in _fieldQueries.Values)
            {
                query.Dispose();
            }
        }

        #endregion

        sealed class InternalUpdateItemFieldQuery : DbQueryNonReader<QueryArgs>
        {
            public InternalUpdateItemFieldQuery(DbConnectionPool connectionPool, string commandText)
                : base(connectionPool, commandText)
            {
            }

            protected override IEnumerable<DbParameter> InitializeParameters()
            {
                return CreateParameters("@itemGuid", "@value");
            }

            protected override void SetParameters(DbParameterValues p, QueryArgs item)
            {
                p["@itemGuid"] = item.ItemGuid;
                p["@value"] = item.Value;
            }
        }

        /// <summary>
        /// Arguments for the UpdateItemFieldQuery.
        /// </summary>
        public struct QueryArgs
        {
            public readonly int ItemGuid;
            public readonly object Value;

            public QueryArgs(int itemGuid, object value)
            {
                ItemGuid = itemGuid;
                Value = value;
            }
        }
    }
}