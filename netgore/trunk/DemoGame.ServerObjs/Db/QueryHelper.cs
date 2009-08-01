using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.DbEntities;

// ReSharper disable EmptyConstructor

namespace DemoGame.Server.Db
{
    /// <summary>
    /// Assists in executing queries on the game database.
    /// </summary>
    public sealed class QueryHelper : QueryHelperBase<DatabaseEntities>
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        static readonly QueryHelper _instance = new QueryHelper();

        /// <summary>
        /// QueryHelper constructor.
        /// </summary>
        static QueryHelper()
        {
            // Constructor required for Singleton design - do not remove
        }

        /// <summary>
        /// QueryHelper constructor.
        /// </summary>
        QueryHelper()
        {
            // Constructor required for Singleton design - do not remove
        }

        /// <summary>
        /// Gets an instance of the QueryHelper.
        /// </summary>
        public static QueryHelper Instance { get { return _instance; } }

        /// <summary>
        /// Gets an available ObjectContext.
        /// </summary>
        /// <returns>An available ObjectContext.</returns>
        protected override DatabaseEntities GetObjectContext()
        {
            return ObjectContextManager.GetObjectContext();
        }
    }
}
