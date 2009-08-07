using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Contains all the code comments used in the generated code.
    /// </summary>
    internal static class Comments
    {
        /// <summary>
        /// Comments used in CreateMethodTryCopyValuesToDbParameterValues().
        /// </summary>
        public static class TryCopyValues
        {
            public static readonly string Summary =
                "Copies the column values into the given DbParameterValues using the database column name" + Environment.NewLine +
                "with a prefixed @ as the key. The key must already exist in the DbParameterValues" + Environment.NewLine +
                "for the value to be copied over. If any of the keys in the DbParameterValues do not" + Environment.NewLine +
                "match one of the column names, or if there is no field for a key, then it will be" + Environment.NewLine +
                "ignored. Because of this, it is important to be careful when using this method" + Environment.NewLine +
                "since columns or keys can be skipped without any indication.";

            public const string ParameterSource = "The object to copy the values from.";

            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";
        }

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
                " this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateMethodCopyValuesToDbParameterValues().
        /// </summary>
        public static class CopyToDPV
        {
            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";

            public const string ParameterSource = "The object to copy the values from.";

            public static readonly string Summary =
                "Copies the column values into the given DbParameterValues using the database column name" +
                Environment.NewLine + "with a prefixed @ as the key. The keys must already exist in the DbParameterValues;" +
                Environment.NewLine + " this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateCode().
        /// </summary>
        public static class CreateCode
        {
            public const string ColumnCollectionField = "The fields that are used in the column collection `{0}`.";

            public static readonly string ColumnCollectionProperty = "Gets an IEnumerable of strings containing the name of the database" + Environment.NewLine
                + "columns used in the column collection `{0}`.";

            public const string ClassSummary = "Provides a strongly-typed structure for the database table `{0}`.";

            public const string ColumnArrayField = "Array of the database column names.";

            public const string NonKeyColumnArrayField = "Array of the database column names for columns that are not primary keys.";

            public const string KeyColumnArrayField = "Array of the database column names for columns that are primary keys.";

            public const string ColumnCount = "The number of columns in the database table that this class represents.";

            public const string ColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.";

            public const string NonKeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.";

            public const string KeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are primary keys.";

            public const string ConstructorParameterIDataReader =
                "The IDataReader to read the values from. See method ReadValues() for details.";

            public static readonly string InterfaceCollectionGetter = "Gets the value of the database column in the column collection `{0}`" + Environment.NewLine +
                "that corresponds to the given key.";

            public static readonly string InterfaceCollectionSetter = "Sets the value of the database column in the column collection `{0}`" + Environment.NewLine +
                "that corresponds to the given key.";

            public const string InterfaceCollectionReturns = "The value of the database column with the corresponding key.";

            public const string InterfaceCollectionParamKey = "The key that represents the column in this column collection.";

            public const string InterfaceCollectionParamValue = "The value to assign to the column with the corresponding key.";

            public const string InterfaceGetProperty = "Gets the value of the database column `{0}`.";

            public const string InterfaceSummary =
                "Interface for a class that can be used to serialize values to the database table `{0}`.";

            public const string TableName = "The name of the database table that this class represents.";
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
            public const string Field = "The field that maps onto the database column `{0}`.";

            public const string PropertyHasDefaultValue = " with the default value of `{0}`.";
            public const string PropertyNoDefaultValue = ".";

            public static readonly string Property =
                "Gets or sets the value for the field that maps onto the database column `{0}`." + Environment.NewLine +
                "The underlying database type is `{1}`";

            public static readonly string PropertyDbComment = " The database column contains the comment: " +
                                                              Environment.NewLine + "\"{0}\".";
        }

        /// <summary>
        /// Comments used in CreateMethodTryReadValues().
        /// </summary>
        public static class TryReadValues
        {
            public static readonly string Summary =
                "Reads the values from an IDataReader and assigns the read values to this" + Environment.NewLine +
                "object's properties. Unlike ReadValues(), this method not only doesn't require" + Environment.NewLine +
                "all values to be in the IDataReader, but also does not require the values in" + Environment.NewLine +
                "the IDataReader to be a defined field for the table this class represents." + Environment.NewLine +
                "Because of this, you need to be careful when using this method because values" + Environment.NewLine +
                "can easily be skipped without any indication.";

            public const string ParameterDataReader =
                "The IDataReader to read the values from. Must already be ready to be read from.";
        }

        /// <summary>
        /// Comments used in CreateMethodReadValues().
        /// </summary>
        public static class ReadValues
        {
            public const string ParameterDataReader =
                "The IDataReader to read the values from. Must already be ready to be read from.";

            public static readonly string Summary =
                "Reads the values from an IDataReader and assigns the read values to this" + Environment.NewLine +
                "object's properties. The database column's name is used to as the key, so the value" + Environment.NewLine +
                "will not be found if any aliases are used or not all columns were selected.";
        }
    }
}
