using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using log4net;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides an interface between all objects and all the database handling methods.
    /// </summary>
    public class DBController : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DbConnectionPool _connectionPool;

        readonly DeleteItemQuery _deleteItemQuery;
        readonly DeleteCharacterEquippedItemQuery _deleteCharacterEquippedItemQuery;
        readonly DeleteCharacterInventoryItemQuery _deleteCharacterInventoryItemQuery;
        readonly List<IDisposable> _disposableQueries = new List<IDisposable>();
        readonly InsertCharacterEquippedItemQuery _insertCharacterEquippedItemQuery;
        readonly InsertCharacterInventoryItemQuery _insertCharacterInventoryItemQuery;
        readonly InsertUserQuery _insertUserQuery;
        readonly ItemIDCreator _itemIDCreator;
        readonly ReplaceItemQuery _replaceItemQuery;
        readonly SelectItemQuery _selectItemQuery;
        readonly SelectItemsQuery _selectItemsQuery;
        readonly SelectItemTemplatesQuery _selectItemTemplatesQuery;
        readonly SelectNPCTemplateDropsQuery _selectNPCTemplateDropsQuery;
        readonly SelectNPCTemplateQuery _selectNPCTemplateQuery;
        readonly SelectCharacterEquippedItemsQuery _selectCharacterEquippedItemsQuery;
        readonly SelectCharacterInventoryItemsQuery _selectCharacterInventoryItemsQuery;
        readonly SelectUserPasswordQuery _selectUserPasswordQuery;
        readonly SelectCharacterQuery _selectCharacterQuery;
        readonly SelectCharacterByIDQuery _selectCharacterByIDQuery;
        readonly UpdateItemFieldQuery _updateItemFieldQuery;
        readonly UpdateCharacterQuery _updateCharacterQuery;
        readonly UserExistsQuery _userExistsQuery;
        readonly SelectAllianceIDsQuery _selectAllianceIDsQuery;
        readonly SelectAllianceQuery _selectAllianceQuery;
        readonly SelectAllianceHostileQuery _selectAllianceHostileQuery;
        readonly SelectAllianceAttackableQuery _selectAllianceAttackableQuery;

        bool _disposed;

        public SelectAllianceIDsQuery SelectAllianceIDs { get { return _selectAllianceIDsQuery; } }
        public SelectAllianceQuery SelectAlliance { get { return _selectAllianceQuery; } }
        public SelectAllianceHostileQuery SelectAllianceHostile { get { return _selectAllianceHostileQuery; } }
        public SelectAllianceAttackableQuery SelectAllianceAttackable { get { return _selectAllianceAttackableQuery; } }

        public DeleteItemQuery DeleteItem
        {
            get { return _deleteItemQuery; }
        }

        public SelectNPCTemplateDropsQuery SelectNPCTemplateDrops
        {
            get { return _selectNPCTemplateDropsQuery; }
        }

        public DeleteCharacterEquippedItemQuery DeleteCharacterEquippedItem
        {
            get { return _deleteCharacterEquippedItemQuery; }
        }

        public DeleteCharacterInventoryItemQuery DeleteCharacterInventoryItem
        {
            get { return _deleteCharacterInventoryItemQuery; }
        }

        public InsertUserQuery InsertUser
        {
            get { return _insertUserQuery; }
        }

        public InsertCharacterEquippedItemQuery InsertCharacterEquippedItem
        {
            get { return _insertCharacterEquippedItemQuery; }
        }

        public InsertCharacterInventoryItemQuery InsertCharacterInventoryItem
        {
            get { return _insertCharacterInventoryItemQuery; }
        }

        public ItemIDCreator ItemIDCreator
        {
            get { return _itemIDCreator; }
        }

        public ReplaceItemQuery ReplaceItem
        {
            get { return _replaceItemQuery; }
        }

        public SelectItemQuery SelectItem
        {
            get { return _selectItemQuery; }
        }

        public SelectItemsQuery SelectItems
        {
            get { return _selectItemsQuery; }
        }

        public SelectItemTemplatesQuery SelectItemTemplates
        {
            get { return _selectItemTemplatesQuery; }
        }

        public SelectNPCTemplateQuery SelectNPCTemplate
        {
            get { return _selectNPCTemplateQuery; }
        }

        public SelectCharacterQuery SelectCharacter
        {
            get { return _selectCharacterQuery; }
        }

        public SelectCharacterByIDQuery SelectCharacterByID
        {
            get { return _selectCharacterByIDQuery; }
        }

        public SelectCharacterEquippedItemsQuery SelectCharacterEquippedItems
        {
            get { return _selectCharacterEquippedItemsQuery; }
        }

        public SelectCharacterInventoryItemsQuery SelectCharacterInventoryItems
        {
            get { return _selectCharacterInventoryItemsQuery; }
        }

        public SelectUserPasswordQuery SelectUserPassword
        {
            get { return _selectUserPasswordQuery; }
        }

        public UpdateItemFieldQuery UpdateItemField
        {
            get { return _updateItemFieldQuery; }
        }

        public UpdateCharacterQuery UpdateCharacter
        {
            get { return _updateCharacterQuery; }
        }

        public UserExistsQuery UserExistsQuery
        {
            get { return _userExistsQuery; }
        }

        public DBController(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            // Create the connection pool
            try
            {
                _connectionPool = new MySqlDbConnectionPool(connectionString);
            }
            catch (Exception ex)
            {
                const string msg = "Failed to create connection to MySql database.";
                Debug.Fail(msg);
                if (log.IsFatalEnabled)
                    log.Fatal(msg, ex);
                Dispose();
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Database connection pool created.");

            // NOTE: It would be REALLY nice to find a better way to construct the query objects...
            // Create the query objects
            _insertUserQuery = new InsertUserQuery(_connectionPool);
            _disposableQueries.Add(_insertUserQuery);

            _updateCharacterQuery = new UpdateCharacterQuery(_connectionPool);
            _disposableQueries.Add(_updateCharacterQuery);

            _insertCharacterEquippedItemQuery = new InsertCharacterEquippedItemQuery(_connectionPool);
            _disposableQueries.Add(_insertCharacterEquippedItemQuery);

            _deleteCharacterEquippedItemQuery = new DeleteCharacterEquippedItemQuery(_connectionPool);
            _disposableQueries.Add(_deleteCharacterEquippedItemQuery);

            _selectItemQuery = new SelectItemQuery(_connectionPool);
            _disposableQueries.Add(_selectItemQuery);

            _insertCharacterInventoryItemQuery = new InsertCharacterInventoryItemQuery(_connectionPool);
            _disposableQueries.Add(_insertCharacterInventoryItemQuery);

            _deleteCharacterInventoryItemQuery = new DeleteCharacterInventoryItemQuery(_connectionPool);
            _disposableQueries.Add(_deleteCharacterInventoryItemQuery);

            _deleteItemQuery = new DeleteItemQuery(_connectionPool);
            _disposableQueries.Add(_deleteItemQuery);

            _replaceItemQuery = new ReplaceItemQuery(_connectionPool);
            _disposableQueries.Add(_replaceItemQuery);

            _updateItemFieldQuery = new UpdateItemFieldQuery(_connectionPool);
            _disposableQueries.Add(_updateItemFieldQuery);

            _selectItemsQuery = new SelectItemsQuery(_connectionPool);
            _disposableQueries.Add(_selectItemsQuery);

            _selectCharacterEquippedItemsQuery = new SelectCharacterEquippedItemsQuery(_connectionPool);
            _disposableQueries.Add(_selectCharacterEquippedItemsQuery);

            _selectCharacterInventoryItemsQuery = new SelectCharacterInventoryItemsQuery(_connectionPool);
            _disposableQueries.Add(_selectCharacterInventoryItemsQuery);

            _selectCharacterQuery = new SelectCharacterQuery(_connectionPool);
            _disposableQueries.Add(_selectCharacterQuery);

            _selectCharacterByIDQuery = new SelectCharacterByIDQuery(_connectionPool);
            _disposableQueries.Add(_selectCharacterByIDQuery);

            _selectUserPasswordQuery = new SelectUserPasswordQuery(_connectionPool);
            _disposableQueries.Add(_selectUserPasswordQuery);

            _selectItemTemplatesQuery = new SelectItemTemplatesQuery(_connectionPool);
            _disposableQueries.Add(_selectItemTemplatesQuery);

            _selectNPCTemplateDropsQuery = new SelectNPCTemplateDropsQuery(_connectionPool);
            _disposableQueries.Add(_selectNPCTemplateDropsQuery);

            _selectNPCTemplateQuery = new SelectNPCTemplateQuery(_connectionPool);
            _disposableQueries.Add(_selectNPCTemplateQuery);

            _userExistsQuery = new UserExistsQuery(_connectionPool);
            _disposableQueries.Add(_userExistsQuery);

            _selectAllianceIDsQuery = new SelectAllianceIDsQuery(_connectionPool);
            _disposableQueries.Add(_selectAllianceIDsQuery);

            _selectAllianceQuery = new SelectAllianceQuery(_connectionPool);
            _disposableQueries.Add(_selectAllianceQuery);

            _selectAllianceHostileQuery = new SelectAllianceHostileQuery(_connectionPool);
            _disposableQueries.Add(_selectAllianceHostileQuery);
            
            _selectAllianceAttackableQuery = new SelectAllianceAttackableQuery(_connectionPool);
            _disposableQueries.Add(_selectAllianceAttackableQuery);

            _itemIDCreator = new ItemIDCreator(_connectionPool);

            if (log.IsInfoEnabled)
                log.Info("DBController successfully initialized all queries.");
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // Dispose of all the individual queries
            foreach (IDisposable item in _disposableQueries)
            {
                item.Dispose();
            }

            // Dispose of the DbConnectionPool
            _connectionPool.Dispose();
        }

        #endregion
    }
}