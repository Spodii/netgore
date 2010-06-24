using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Collections;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateItemFieldQuery : IDisposable
    {
        readonly DbConnectionPool _connectionPool;
        readonly ICache<string, InternalUpdateItemFieldQuery> _fieldQueryCache;

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

            _fieldQueryCache =
                new HashCache<string, InternalUpdateItemFieldQuery>(
                    x => new InternalUpdateItemFieldQuery(_connectionPool, GetFieldQueryString(x)));

            QueryAsserts.ArePrimaryKeys(ItemTable.DbKeyColumns, "id");
        }

        public void Execute(ItemID itemID, string field, object value)
        {
            var values = new QueryArgs(itemID, value);
            var fieldQuery = _fieldQueryCache[field];
            fieldQuery.Execute(values);
        }

        /// <summary>
        /// Gets the query string to use for a field.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The query string to use for the <paramref name="fieldName"/>.</returns>
        static string GetFieldQueryString(string fieldName)
        {
            const string baseQueryStr = "UPDATE `" + ItemTable.TableName + "` SET `{0}`=@value WHERE `id`=@itemID";

            return string.Format(baseQueryStr, fieldName);
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

            foreach (var query in _fieldQueryCache.GetCachedValues())
            {
                query.Dispose();
            }
        }

        #endregion

        sealed class InternalUpdateItemFieldQuery : DbQueryNonReader<QueryArgs>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InternalUpdateItemFieldQuery"/> class.
            /// </summary>
            /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
            /// <param name="commandText">String containing the command to use for the query.</param>
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
                return CreateParameters("itemID", "value");
            }

            /// <summary>
            /// When overridden in the derived class, sets the database parameters based on the specified item.
            /// </summary>
            /// <param name="p">Collection of database parameters to set the values for.</param>
            /// <param name="item">Item used to execute the query.</param>
            protected override void SetParameters(DbParameterValues p, QueryArgs item)
            {
                p["itemID"] = (int)item.ItemID;
                p["value"] = item.Value;
            }
        }

        /// <summary>
        /// Arguments for the UpdateItemFieldQuery.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// >The item ID. 
            /// </summary>
            public readonly ItemID ItemID;

            /// <summary>
            /// The value.
            /// </summary>
            public readonly object Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="itemID">The item ID.</param>
            /// <param name="value">The value.</param>
            public QueryArgs(ItemID itemID, object value)
            {
                ItemID = itemID;
                Value = value;
            }
        }
    }
}