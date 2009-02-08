using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides an interface between all objects and all the database handling methods.
    /// </summary>
    public class DBController : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly MySqlConnection _conn;
        readonly DeleteItemQuery _deleteItemQuery;
        readonly DeleteUserEquippedQuery _deleteUserEquipQuery;
        readonly DeleteUserItemQuery _deleteUserItemQuery;
        readonly List<IDisposable> _disposableQueries = new List<IDisposable>();
        readonly InsertUserEquippedQuery _insertUserEquipQuery;
        readonly InsertUserItemQuery _insertUserItemQuery;
        readonly InsertUserQuery _insertUserQuery;
        readonly ItemGuidCreator _itemGuidCreator;
        readonly ReplaceItemQuery _replaceItemQuery;
        readonly SelectItemQuery _selectItemQuery;
        readonly UpdateItemFieldQuery _updateItemFieldQuery;
        readonly UpdateUserQuery _updateUserQuery;

        bool _disposed;

        public MySqlConnection Connection
        {
            get
            {
                // LATER: (MySql dependency removal) Remove all usage of this
                return _conn;
            }
        }

        public DeleteItemQuery DeleteItem
        {
            get { return _deleteItemQuery; }
        }

        public DeleteUserEquippedQuery DeleteUserEquipped
        {
            get { return _deleteUserEquipQuery; }
        }

        public DeleteUserItemQuery DeleteUserItem
        {
            get { return _deleteUserItemQuery; }
        }

        public InsertUserQuery InsertUser
        {
            get { return _insertUserQuery; }
        }

        public InsertUserEquippedQuery InsertUserEquipped
        {
            get { return _insertUserEquipQuery; }
        }

        public InsertUserItemQuery InsertUserItem
        {
            get { return _insertUserItemQuery; }
        }

        public ItemGuidCreator ItemGuidCreator
        {
            get { return _itemGuidCreator; }
        }

        public ReplaceItemQuery ReplaceItem
        {
            get { return _replaceItemQuery; }
        }

        public SelectItemQuery SelectItem
        {
            get { return _selectItemQuery; }
        }

        public UpdateItemFieldQuery UpdateItemField
        {
            get { return _updateItemFieldQuery; }
        }

        public UpdateUserQuery UpdateUser
        {
            get { return _updateUserQuery; }
        }

        public DBController(string connectionString)
        {
            // Create the connection
            _conn = new MySqlConnection(connectionString);

            // Open the connection
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                const string msg = "Failed to connect to MySql database.";
                Debug.Fail(msg);
                if (log.IsFatalEnabled)
                    log.Fatal(msg, ex);
                Dispose();
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("MySql connection to database '{0}' open", _conn.Database);

            // Create the query classes
            _insertUserQuery = new InsertUserQuery(_conn);
            _disposableQueries.Add(_insertUserQuery);

            _updateUserQuery = new UpdateUserQuery(_conn);
            _disposableQueries.Add(_updateUserQuery);

            _insertUserEquipQuery = new InsertUserEquippedQuery(_conn);
            _disposableQueries.Add(_insertUserEquipQuery);

            _deleteUserEquipQuery = new DeleteUserEquippedQuery(_conn);
            _disposableQueries.Add(_deleteUserEquipQuery);

            _selectItemQuery = new SelectItemQuery(_conn);
            _disposableQueries.Add(_selectItemQuery);

            _insertUserItemQuery = new InsertUserItemQuery(_conn);
            _disposableQueries.Add(_insertUserItemQuery);

            _deleteUserItemQuery = new DeleteUserItemQuery(_conn);
            _disposableQueries.Add(_deleteUserItemQuery);

            _deleteItemQuery = new DeleteItemQuery(_conn);
            _disposableQueries.Add(_deleteItemQuery);

            _replaceItemQuery = new ReplaceItemQuery(_conn);
            _disposableQueries.Add(_replaceItemQuery);

            _updateItemFieldQuery = new UpdateItemFieldQuery(_conn);
            _disposableQueries.Add(_updateItemFieldQuery);

            _itemGuidCreator = new ItemGuidCreator(_conn);
        }

        /// <summary>
        /// Gets all of the Alliance data.
        /// </summary>
        /// <returns>IEnumerable of Dictionaries with the key as the field name and value as the field value.</returns>
        public IEnumerable<Dictionary<string, object>> GetAllianceData()
        {
            List<Dictionary<string, object>> ret;

            // Load all the data from the database
            using (MySqlCommand cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM `alliances`";
                using (IDataReader r = cmd.ExecuteReader())
                {
                    ret = ToDictionary(r);
                }
            }

            return ret;
        }

        /// <summary>
        /// Loads all the values in an IDataReader and converts it into a list of dictionaries.
        /// </summary>
        /// <param name="dataReader">IDataReader to load the field names and values from.</param>
        /// <returns>List of Dictionaries with a key of the field's name and value of the field's value.</returns>
        static List<Dictionary<string, object>> ToDictionary(IDataReader dataReader)
        {
            // Get the name for each ordinal
            var ordinalToName = new string[dataReader.FieldCount];
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                ordinalToName[i] = dataReader.GetName(i);
            }

            // Start reading the values
            var fields = new List<Dictionary<string, object>>();
            while (dataReader.Read())
            {
                // Add all the field values to the dictionary
                var field = new Dictionary<string, object>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    field.Add(ordinalToName[i], dataReader.GetValue(i));
                }

                fields.Add(field);
            }

            return fields;
        }

        #region IDisposable Members

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // Dispose of all the queries
            foreach (IDisposable item in _disposableQueries)
            {
                item.Dispose();
            }

            // Dispose of the connection
            _conn.Dispose();
        }

        #endregion
    }
}