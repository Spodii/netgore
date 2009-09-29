using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;
using NetGore.RPGComponents;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateItemFieldQuery : IDisposable
    {
        const string _queryString = "UPDATE `" + ItemTable.TableName + "` SET `{0}`=@value WHERE `id`=@itemID";
        readonly DbConnectionPool _connectionPool;

        readonly Dictionary<string, InternalUpdateItemFieldQuery> _fieldQueries =
            new Dictionary<string, InternalUpdateItemFieldQuery>(StringComparer.OrdinalIgnoreCase);

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateItemFieldQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public UpdateItemFieldQuery(DbConnectionPool connectionPool)
        {
            if (connectionPool == null)
                throw new ArgumentNullException("connectionPool");

            _connectionPool = connectionPool;

            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        public void Execute(ItemID itemID, string field, object value)
        {
            // TODO: Generate all field queries at construction so we never have to worry about threading conflicts
            InternalUpdateItemFieldQuery fieldQuery;
            if (!_fieldQueries.TryGetValue(field, out fieldQuery))
            {
                var query = string.Format(_queryString, field.ToLower());
                fieldQuery = new InternalUpdateItemFieldQuery(_connectionPool, query);
                _fieldQueries.Add(field, fieldQuery);
            }

            var values = new QueryArgs(itemID, value);
            fieldQuery.Execute(values);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (var query in _fieldQueries.Values)
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

            /// <summary>
            /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
            /// </summary>
            /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
            /// no parameters will be used.</returns>
            protected override IEnumerable<DbParameter> InitializeParameters()
            {
                return CreateParameters("@itemID", "@value");
            }

            /// <summary>
            /// When overridden in the derived class, sets the database parameters based on the specified item.
            /// </summary>
            /// <param name="p">Collection of database parameters to set the values for.</param>
            /// <param name="item">Item used to execute the query.</param>
            protected override void SetParameters(DbParameterValues p, QueryArgs item)
            {
                p["@itemID"] = (int)item.ItemID;
                p["@value"] = item.Value;
            }
        }

        /// <summary>
        /// Arguments for the UpdateItemFieldQuery.
        /// </summary>
        public struct QueryArgs
        {
            public readonly ItemID ItemID;
            public readonly object Value;

            public QueryArgs(ItemID itemID, object value)
            {
                ItemID = itemID;
                Value = value;
            }
        }
    }
}