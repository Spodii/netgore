using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Contains all the code comments used in the generated code.
    /// </summary>
    static class Comments
    {
        /// <summary>
        /// Comments used in CreateMethodCopyValuesToDict().
        /// </summary>
        public static class CopyToDict
        {
            public const string ParameterDict = "The Dictionary to copy the values into.";

            public const string ParameterSource = "The object to copy the values from.";

            public static readonly string Summary =
                "Copies the column values into the given Dictionary using the database column name" + Environment.NewLine +
                "with a prefixed @ as the key. The keys must already exist in the Dictionary;" + Environment.NewLine +
                "this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateMethodCopyValuesToDbParameterValues().
        /// </summary>
        public static class CopyToDPV
        {
            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";

            public const string ParameterSource = "The object to copy the values from.";

            public static readonly string Summary =
                "Copies the column values into the given DbParameterValues using the database column name" + Environment.NewLine +
                "with a prefixed @ as the key. The keys must already exist in the DbParameterValues;" + Environment.NewLine +
                " this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateMethodCopyValuesFrom().
        /// </summary>
        public static class CopyValuesFrom
        {
            public const string SourceParameter = "The {0} to copy the values from.";
            public const string Summary = "Copies the values from the given source into this {0}.";
        }

        /// <summary>
        /// Comments used in CreateCode().
        /// </summary>
        public static class CreateCode
        {
            public const string ClassSummary = "Provides a strongly-typed structure for the database table `{0}`.";

            public const string ColumnArrayField = "Array of the database column names.";

            public const string ColumnCollectionField = "The fields that are used in the column collection `{0}`.";

            public const string ColumnCount = "The number of columns in the database table that this class represents.";

            public const string ColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.";

            public const string ConstructorInterfaceParameter = "{0} to copy the initial values from.";

            public const string ConstructorParameterIDataReader =
                "The IDataReader to read the values from. See method ReadValues() for details.";

            public const string ConstructorSummary = "{0} constructor.";
            public const string DeepCopyReturn = "A deep copy of this table.";

            public const string InterfaceCollectionParamKey = "The key that represents the column in this column collection.";

            public const string InterfaceCollectionParamValue = "The value to assign to the column with the corresponding key.";
            public const string InterfaceCollectionReturns = "The value of the database column with the corresponding key.";

            public const string InterfaceGetProperty = "Gets the value of the database column `{0}`.";

            public const string InterfaceSummary =
                "Interface for a class that can be used to serialize values to the database table `{0}`.";

            public const string KeyColumnArrayField = "Array of the database column names for columns that are primary keys.";

            public const string KeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are primary keys.";

            public const string NonKeyColumnArrayField =
                "Array of the database column names for columns that are not primary keys.";

            public const string NonKeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.";

            public const string TableName = "The name of the database table that this class represents.";

            public static readonly string ColumnCollectionProperty =
                "Gets an IEnumerable of strings containing the name of the database" + Environment.NewLine +
                "columns used in the column collection `{0}`.";

            public static readonly string ColumnCollectionValueProperty =
                "Gets an IEnumerable of KeyValuePairs containing the values in the `{0}` collection. The" + Environment.NewLine +
                "key is the collection's key and the value is the value for that corresponding key.";

            public static readonly string DeepCopySummary = "Creates a deep copy of this table. All the values will be the same" +
                                                            Environment.NewLine +
                                                            "but they will be contained in a different object instance.";

            public static readonly string ExtensionClassSummary =
                "Contains extension methods for class {0} that assist in performing" + Environment.NewLine +
                "reads and writes to and from a database.";

            public static readonly string InterfaceCollectionGetter =
                "Gets the value of the database column in the column collection `{0}`" + Environment.NewLine +
                "that corresponds to the given key.";

            public static readonly string InterfaceCollectionSetter =
                "Sets the value of the database column in the column collection `{0}`" + Environment.NewLine +
                "that corresponds to the given key.";
        }

        /// <summary>
        /// Comments used in CreateConstructor().
        /// </summary>
        public static class CreateConstructor
        {
            public const string Parameter = "The initial value for the corresponding property.";
        }

        /// <summary>
        /// Comments used in CreateFields().
        /// </summary>
        public static class CreateFields
        {
            public const string CollectionField = "Dictionary containing the values for the column collection `{0}`.";

            public const string Field = "The field that maps onto the database column `{0}`.";

            public const string PropertyHasDefaultValue = " with the default value of `{0}`.";
            public const string PropertyNoDefaultValue = ".";

            public const string PublicMethodGet =
                "Gets the value of a database column for the corresponding key for the column collection `{0}`.";

            public const string PublicMethodGetKeyParameter = "The key of the column to get.";
            public const string PublicMethodGetReturns = "The value of the database column for the corresponding key.";

            public const string PublicMethodSet =
                "Sets the value of a database column for the corresponding key for the column collection `{0}`.";

            public const string PublicMethodValueParameter = "The value to assign to the column for the corresponding key.";

            public static readonly string Property =
                "Gets or sets the value for the field that maps onto the database column `{0}`." + Environment.NewLine +
                "The underlying database type is `{1}`";

            public static readonly string PropertyDbComment = " The database column contains the comment: " + Environment.NewLine +
                                                              "\"{0}\".";
        }

        /// <summary>
        /// Comments used with the extension methods.
        /// </summary>
        public static class Extensions
        {
            public const string ExtensionParameter = "The object to add the extension method to.";
        }

        /// <summary>
        /// Comments used in CreateMethodGetColumnData().
        /// </summary>
        public static class GetColumnData
        {
            public const string ColumnNameParameter = "The database name of the column to get the data for.";

            public const string Returns = "The data for the database column with the name columnName.";
            public const string Summary = "Gets the data for the database column that this table represents.";
        }

        /// <summary>
        /// Comments used in CreateMethodGetValue().
        /// </summary>
        public static class GetValue
        {
            public const string ColumnNameParameter = "The database name of the column to get the value for.";

            public const string Returns = "The value of the column with the name columnName.";
            public const string Summary = "Gets the value of a column by the database column's name.";
        }

        /// <summary>
        /// Comments used in CreateMethodReadValues().
        /// </summary>
        public static class ReadValues
        {
            public const string ParameterDataReader =
                "The IDataReader to read the values from. Must already be ready to be read from.";

            public static readonly string Summary = "Reads the values from an IDataReader and assigns the read values to this" +
                                                    Environment.NewLine +
                                                    "object's properties. The database column's name is used to as the key, so the value" +
                                                    Environment.NewLine +
                                                    "will not be found if any aliases are used or not all columns were selected.";
        }

        /// <summary>
        /// Comments used in CreateMethodSetValue();
        /// </summary>
        public static class SetValue
        {
            public const string ColumnNameParameter = "The database name of the column to get the value for.";
            public const string Summary = "Sets the value of a column by the database column's name.";

            public const string ValueParameter = "Value to assign to the column.";
        }

        /// <summary>
        /// Comments used in CreateMethodTryCopyValuesToDbParameterValues().
        /// </summary>
        public static class TryCopyValues
        {
            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";
            public const string ParameterSource = "The object to copy the values from.";

            public static readonly string Summary =
                "Copies the column values into the given DbParameterValues using the database column name" + Environment.NewLine +
                "with a prefixed @ as the key. The key must already exist in the DbParameterValues" + Environment.NewLine +
                "for the value to be copied over. If any of the keys in the DbParameterValues do not" + Environment.NewLine +
                "match one of the column names, or if there is no field for a key, then it will be" + Environment.NewLine +
                "ignored. Because of this, it is important to be careful when using this method" + Environment.NewLine +
                "since columns or keys can be skipped without any indication.";
        }

        /// <summary>
        /// Comments used in CreateMethodTryReadValues().
        /// </summary>
        public static class TryReadValues
        {
            public const string ParameterDataReader =
                "The IDataReader to read the values from. Must already be ready to be read from.";

            public static readonly string Summary = "Reads the values from an IDataReader and assigns the read values to this" +
                                                    Environment.NewLine +
                                                    "object's properties. Unlike ReadValues(), this method not only doesn't require" +
                                                    Environment.NewLine +
                                                    "all values to be in the IDataReader, but also does not require the values in" +
                                                    Environment.NewLine +
                                                    "the IDataReader to be a defined field for the table this class represents." +
                                                    Environment.NewLine +
                                                    "Because of this, you need to be careful when using this method because values" +
                                                    Environment.NewLine + "can easily be skipped without any indication.";
        }
    }
}