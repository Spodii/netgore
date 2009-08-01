using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Db
{
    /// <summary>
    /// Manages the ObjectContexts.
    /// </summary>
    public static class ObjectContextManager
    {
        /// <summary>
        /// Gets an available ObjectContext.
        /// </summary>
        /// <returns>An available ObjectContext.</returns>
        public static DatabaseEntities GetObjectContext()
        {
            return new DatabaseEntities();
        }
    }
}
