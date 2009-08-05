using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `alliance`.
    /// </summary>
    public interface IAllianceTable
    {
        /// <summary>
        /// Gets the value for the database column `id`.
        /// </summary>
        Byte Id { get; }

        /// <summary>
        /// Gets the value for the database column `name`.
        /// </summary>
        String Name { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `alliance`.
    /// </summary>
    public class AllianceTable : IAllianceTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 2;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "alliance";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "id", "name" };

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Byte _id;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// AllianceTable constructor.
        /// </summary>
        public AllianceTable()
        {
        }

        /// <summary>
        /// AllianceTable constructor.
        /// </summary>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        public AllianceTable(Byte @id, String @name)
        {
            Id = @id;
            Name = @name;
        }

        /// <summary>
        /// AllianceTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public AllianceTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IAllianceTable source, IDictionary<String, Object> dic)
        {
            dic["@id"] = source.Id;
            dic["@name"] = source.Name;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(IAllianceTable source, DbParameterValues paramValues)
        {
            paramValues["@id"] = source.Id;
            paramValues["@name"] = source.Name;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Id = (Byte)dataReader.GetValue(dataReader.GetOrdinal("id"));
            Name = (String)dataReader.GetValue(dataReader.GetOrdinal("name"));
        }

        #region IAllianceTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}