using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace NetGore.Features.EventCounters
{
    /// <summary>
    /// Helper methods for the <see cref="IEventCounter{T,U}"/>.
    /// </summary>
    public static class EventCounterHelper
    {
        const string _counterParameterName = "counter";
        const string _eventParameterName = "event";
        const string _objectParameterName = "object";

        /// <summary>
        /// Creates an instance of a <see cref="DbQueryNonReader{T}"/> to be used to execute the queries
        /// for an <see cref="IEventCounter{T,U}"/>.
        /// </summary>
        /// <typeparam name="TObjectID">The type of object ID.</typeparam>
        /// <typeparam name="TEventID">The type of event ID.</typeparam>
        /// <param name="connectionPool">The connection pool the query will be executed on.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="objectKeyName">The name of the object key.</param>
        /// <param name="eventKeyName">The name of the event key.</param>
        /// <param name="counterColumnName">The name of the counter column. Default is "counter".</param>
        /// <returns>The <see cref="DbQueryNonReader{T}"/> instance.</returns>
        public static DbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>> CreateQuery<TObjectID, TEventID>(
            DbConnectionPool connectionPool, string tableName, string objectKeyName, string eventKeyName,
            string counterColumnName = "counter")
        {
            var queryStr = CreateQueryString(connectionPool.QueryBuilder, tableName, objectKeyName, eventKeyName,
                counterColumnName);
            var ecq = new EventCounterQuery<TObjectID, TEventID>(connectionPool, queryStr);
            return ecq;
        }

        /// <summary>
        /// Creates the query string.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> to use.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="objectKeyName">The name of the object key.</param>
        /// <param name="eventKeyName">The name of the event key.</param>
        /// <param name="counterColumnName">The name of the counter column. Default is "counter".</param>
        /// <returns>The query string.</returns>
        static string CreateQueryString(IQueryBuilder qb, string tableName, string objectKeyName, string eventKeyName,
                                        string counterColumnName)
        {
            /*
                INSERT INTO [table] (`o`,`e`,`c`)
                    VALUES (@object, @event, @amount)
                    ON DUPLICATE KEY UPDATE
                    SET `c` = `c` + @amount
            */

            var f = qb.Functions;
            var s = qb.Settings;

            var q =
                qb.Insert(tableName).AddParam(objectKeyName, _objectParameterName).AddParam(eventKeyName, _eventParameterName).
                    AddParam(counterColumnName, _counterParameterName).ODKU().Add(_counterParameterName,
                        f.Add(s.EscapeColumn(counterColumnName), s.Parameterize(_counterParameterName)));

            var ret = q.ToString();
            return ret;
        }

        class EventCounterQuery<TObjectID, TEventID> : DbQueryNonReader<ObjectEventAmount<TObjectID, TEventID>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EventCounterQuery{TObjectID, TEventID}"/> class.
            /// </summary>
            /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
            /// <param name="commandText">String containing the command to use for the query.</param>
            /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
            public EventCounterQuery(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
            {
            }

            /// <summary>
            /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
            /// </summary>
            /// <returns>The <see cref="DbParameter"/>s needed for this class to perform database queries.
            /// If null, no parameters will be used.</returns>
            protected override IEnumerable<DbParameter> InitializeParameters()
            {
                return CreateParameters(_objectParameterName, _eventParameterName, _counterParameterName);
            }

            /// <summary>
            /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
            /// based on the values specified in the given <paramref name="item"/> parameter.
            /// </summary>
            /// <param name="p">Collection of database parameters to set the values for.</param>
            /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
            protected override void SetParameters(DbParameterValues p, ObjectEventAmount<TObjectID, TEventID> item)
            {
                p[_objectParameterName] = item.ObjectID;
                p[_eventParameterName] = item.EventID;
                p[_counterParameterName] = item.Amount;
            }
        }
    }
}