using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    public abstract class DbQueryNonReader : DbQueryBase, IDbQueryNonReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryNonReader"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return null;
        }

        #region IDbQueryNonReader Members

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        public void Execute()
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                r.ExecuteNonReader(cmd, this);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        public int ExecuteWithResult()
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                return r.ExecuteNonReaderWithResult(cmd, this);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <param name="lastInsertedId">Contains the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.</param>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        public virtual int ExecuteWithResult(out long lastInsertedId)
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                return r.ExecuteNonReaderWithResult(cmd, this, out lastInsertedId);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        #endregion
    }

    /// <summary>
    /// Base class for all database queries that execute queries that do not use a reader.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public abstract class DbQueryNonReader<T> : DbQueryBase, IDbQueryNonReader<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryNonReader{T}"/> class.
        /// </summary>
        /// <param name="connectionPool">The <see cref="DbConnectionPool"/> to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionPool"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandText"/> is null or empty.</exception>
        protected DbQueryNonReader(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected abstract void SetParameters(DbParameterValues p, T item);

        #region IDbQueryNonReader<T> Members

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        public void Execute(T item)
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                if (HasParameters)
                {
                    using (var p = DbParameterValues.Create(cmd.Parameters))
                    {
                        SetParameters(p, item);
                    }
                }

                r.ExecuteNonReader(cmd, this);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        public int ExecuteWithResult(T item)
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                if (HasParameters)
                {
                    using (var p = DbParameterValues.Create(cmd.Parameters))
                    {
                        SetParameters(p, item);
                    }
                }

                return r.ExecuteNonReaderWithResult(cmd, this);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <param name="lastInsertedId">Contains the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.</param>
        /// <returns>Number of rows affected by the query.</returns>
        /// <exception cref="DuplicateKeyException">Tried to perform an insert query for a key that already exists.</exception>
        public virtual int ExecuteWithResult(T item, out long lastInsertedId)
        {
            // Update the query stats
            var stats = ConnectionPool.QueryStats;
            if (stats != null)
                stats.QueryExecuted(this);

            var r = ConnectionPool.QueryRunner;

            // Get and set up the command
            var cmd = GetCommand(r.Connection);
            try
            {
                if (HasParameters)
                {
                    using (var p = DbParameterValues.Create(cmd.Parameters))
                    {
                        SetParameters(p, item);
                    }
                }

                return r.ExecuteNonReaderWithResult(cmd, this, out lastInsertedId);
            }
            catch (MySqlException ex)
            {
                // Throw a custom exception for common errors
                if (ex.Number == 1062)
                    throw new DuplicateKeyException(ex);

                // Everything else, just throw the default exception
                throw;
            }
        }

        #endregion
    }
}