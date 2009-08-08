using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `map`.
    /// </summary>
    public interface IMapTable
    {
        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        MapIndex Id { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `map`.
    /// </summary>
    public class MapTable : IMapTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 2;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "map";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "id", "name" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "name" };

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt16 _id;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// MapTable constructor.
        /// </summary>
        public MapTable()
        {
        }

        /// <summary>
        /// MapTable constructor.
        /// </summary>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        public MapTable(MapIndex @id, String @name)
        {
            Id = @id;
            Name = @name;
        }

        /// <summary>
        /// MapTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public MapTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        public MapTable(IMapTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IMapTable source, IDictionary<String, Object> dic)
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
        public static void CopyValues(IMapTable source, DbParameterValues paramValues)
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

        public void CopyValuesFrom(IMapTable source)
        {
            Id = source.Id;
            Name = source.Name;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "id":
                    return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "name":
                    return new ColumnMetadata("name", "", "varchar(255)", null, typeof(String), false, false, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "id":
                    return Id;

                case "name":
                    return Name;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("id");
            Id = (MapIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("name");
            Name = dataReader.GetString(i);
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "id":
                    Id = (MapIndex)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(IMapTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@id":
                        paramValues[i] = source.Id;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;
                }
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void TryCopyValues(DbParameterValues paramValues)
        {
            TryCopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void TryReadValues(IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "id":
                        Id = (MapIndex)dataReader.GetUInt16(i);
                        break;

                    case "name":
                        Name = dataReader.GetString(i);
                        break;
                }
            }
        }

        #region IMapTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public MapIndex Id
        {
            get { return (MapIndex)_id; }
            set { _id = (UInt16)value; }
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