using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for the primary manager of database connections and commands.
    /// </summary>
    public interface IDbManager
    {
        IPoolableDbConnection GetConnection();

        DbCommand GetCommand();

        DbCommand GetCommand(string commandText);
    }
}
