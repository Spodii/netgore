using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Contains all the code comments used in the generated code.
    /// </summary>
    static class Comments
    {
        /// <summary>
        /// Comments used in CreateMethodCopyValuesToDbParameterValues().
        /// </summary>
        public static class CopyToDPV
        {
            /// <summary>
            /// The Xml documentation to use for parameter `DbParameterValues`.
            /// </summary>
            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";

            /// <summary>
            /// The Xml documentation to use for parameter `source`.
            /// </summary>
            public const string ParameterSource = "The object to copy the values from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                @"Copies the column values into the given DbParameterValues using the database column name
with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
 this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateMethodCopyValuesToDict().
        /// </summary>
        public static class CopyToDict
        {
            /// <summary>
            /// The Xml documentation to use for parameter `dict`.
            /// </summary>
            public const string ParameterDict = "The Dictionary to copy the values into.";

            /// <summary>
            /// The Xml documentation to use for parameter `source`.
            /// </summary>
            public const string ParameterSource = "The object to copy the values from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                @"Copies the column values into the given Dictionary using the database column name
with a prefixed @ as the key. The keys must already exist in the Dictionary;
this method will not create them if they are missing.";
        }

        /// <summary>
        /// Comments used in CreateMethodCopyValuesFrom().
        /// </summary>
        public static class CopyValuesFrom
        {
            /// <summary>
            /// The Xml documentation to use for parameter `source`.
            /// </summary>
            public const string SourceParameter = "The {0} to copy the values from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Copies the values from the given source into this {0}.";
        }

        /// <summary>
        /// Comments used in CreateCode().
        /// </summary>
        public static class CreateCode
        {
            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string ClassSummary = "Provides a strongly-typed structure for the database table `{0}`.";

            /// <summary>
            /// The Xml documentation to use for the array of fields.
            /// </summary>
            public const string ColumnArrayField = "Array of the database column names.";

            /// <summary>
            /// The Xml documentation to use for the collection of fields.
            /// </summary>
            public const string ColumnCollectionField = "The fields that are used in the column collection `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a column colection property.
            /// </summary>
            public const string ColumnCollectionProperty =
                @"Gets an IEnumerable of strings containing the name of the database
columns used in the column collection `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a column colection value property.
            /// </summary>
            public const string ColumnCollectionValueProperty =
                @"Gets an IEnumerable of KeyValuePairs containing the values in the `{0}` collection. The
key is the collection's key and the value is the value for that corresponding key.";

            /// <summary>
            /// The Xml documentation to use for the column count.
            /// </summary>
            public const string ColumnCount = "The number of columns in the database table that this class represents.";

            /// <summary>
            /// The Xml documentation to use for the an IEnumerable of database column names.
            /// </summary>
            public const string ColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.";

            /// <summary>
            /// The Xml documentation to use for the self-referenced interface parameter on a constructor.
            /// </summary>
            public const string ConstructorInterfaceParameter = "{0} to copy the initial values from.";

            /// <summary>
            /// The Xml documentation to use for the IDataReader parameter on a constructor.
            /// </summary>
            public const string ConstructorParameterIDataReader =
                @"The <see cref=""IDataReader""/> to read the values from. See method ReadValues() for details.";

            /// <summary>
            /// The Xml documentation to use for a constructor.
            /// </summary>
            public const string ConstructorSummary = "Initializes a new instance of the <see cref=\"{0}\"/> class.";

            /// <summary>
            /// The Xml documentation to use for the return on DeepCopy.
            /// </summary>
            public const string DeepCopyReturn = "A deep copy of this table.";

            /// <summary>
            /// The Xml documentation to use for the DeepCopy method.
            /// </summary>
            public const string DeepCopySummary =
                @"Creates a deep copy of this table. All the values will be the same
but they will be contained in a different object instance.";

            /// <summary>
            /// The Xml documentation to use for an extension class.
            /// </summary>
            public const string ExtensionClassSummary =
                @"Contains extension methods for class {0} that assist in performing
reads and writes to and from a database.";

            /// <summary>
            /// The Xml documentation to use on an interface for a collection getter.
            /// </summary>
            public const string InterfaceCollectionGetter =
                @"Gets the value of the database column in the column collection `{0}`
that corresponds to the given key.";

            /// <summary>
            /// The Xml documentation to use for the `key` parameter on a collection on an interface.
            /// </summary>
            public const string InterfaceCollectionParamKey = "The key that represents the column in this column collection.";

            /// <summary>
            /// The Xml documentation to use for the `value` parameter on a collection on an interface.
            /// </summary>
            public const string InterfaceCollectionParamValue = "The value to assign to the column with the corresponding key.";

            /// <summary>
            /// The Xml documentation to use for the return on a collection on an interface.
            /// </summary>
            public const string InterfaceCollectionReturns = "The value of the database column with the corresponding key.";

            /// <summary>
            /// The Xml documentation to use on an interface for a collection setter.
            /// </summary>
            public const string InterfaceCollectionSetter =
                @"Sets the value of the database column in the column collection `{0}`
that corresponds to the given key.";

            /// <summary>
            /// The Xml documentation to use for the getter on an interface.
            /// </summary>
            public const string InterfaceGetProperty = "Gets the value of the database column `{0}`.";

            /// <summary>
            /// The Xml documentation to use for the summary of an interface.
            /// </summary>
            public const string InterfaceSummary =
                "Interface for a class that can be used to serialize values to the database table `{0}`.";

            /// <summary>
            /// The Xml documentation to use for the field for the collection of non-key columns.
            /// </summary>
            public const string KeyColumnArrayField = "Array of the database column names for columns that are primary keys.";

            /// <summary>
            /// The Xml documentation to use for collection of key columns.
            /// </summary>
            public const string KeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are primary keys.";

            /// <summary>
            /// The Xml documentation to use for the field for the collection of non-key columns.
            /// </summary>
            public const string NonKeyColumnArrayField =
                "Array of the database column names for columns that are not primary keys.";

            /// <summary>
            /// The Xml documentation to use for collection of non-key columns.
            /// </summary>
            public const string NonKeyColumnIEnumerableProperty =
                "Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.";

            /// <summary>
            /// The Xml documentation to use for the TableName const string.
            /// </summary>
            public const string TableName = "The name of the database table that this class represents.";
        }

        /// <summary>
        /// Comments used in CreateConstructor().
        /// </summary>
        public static class CreateConstructor
        {
            /// <summary>
            /// The Xml documentation to use for the parameter.
            /// </summary>
            public const string Parameter = "The initial value for the corresponding property.";
        }

        /// <summary>
        /// Comments used in CreateFields().
        /// </summary>
        public static class CreateFields
        {
            /// <summary>
            /// The Xml documentation to use for a collection field.
            /// </summary>
            public const string CollectionField = "Dictionary containing the values for the column collection `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a regular field.
            /// </summary>
            public const string Field = "The field that maps onto the database column `{0}`.";

            /// <summary>
            /// The Xml documentation to use on a property.
            /// </summary>
            public const string Property =
                @"Gets or sets the value for the field that maps onto the database column `{0}`.
The underlying database type is `{1}`";

            /// <summary>
            /// The Xml documentation to use on a property that contains a comment.
            /// </summary>
            public const string PropertyDbComment = @"The database column contains the comment: 
""{0}"".";

            /// <summary>
            /// The Xml documentation to use for a property with a default value.
            /// </summary>
            public const string PropertyHasDefaultValue = " with the default value of `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a property with no default value.
            /// </summary>
            public const string PropertyNoDefaultValue = ".";

            /// <summary>
            /// The Xml documentation to use for a getter for a column collection.
            /// </summary>
            public const string PublicMethodGet =
                "Gets the value of a database column for the corresponding key for the column collection `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a key parameter.
            /// </summary>
            public const string PublicMethodGetKeyParameter = "The key of the column to get.";

            /// <summary>
            /// The Xml documentation to use for the return value on a getter.
            /// </summary>
            public const string PublicMethodGetReturns = "The value of the database column for the corresponding key.";

            /// <summary>
            /// The Xml documentation to use for a setter for a column collection.
            /// </summary>
            public const string PublicMethodSet =
                "Sets the value of a database column for the corresponding key for the column collection `{0}`.";

            /// <summary>
            /// The Xml documentation to use for a `value` parameter on a method used to set the value for a column.
            /// </summary>
            public const string PublicMethodValueParameter = "The value to assign to the column for the corresponding key.";
        }

        /// <summary>
        /// Comments used with the extension methods.
        /// </summary>
        public static class Extensions
        {
            /// <summary>
            /// The Xml documentation to use for the parameter for the object being extended.
            /// </summary>
            public const string ExtensionParameter = "The object to add the extension method to.";
        }

        /// <summary>
        /// Comments used in CreateMethodGetColumnData().
        /// </summary>
        public static class GetColumnData
        {
            /// <summary>
            /// The Xml documentation to use for parameter `columnName`.
            /// </summary>
            public const string ColumnNameParameter = "The database name of the column to get the data for.";

            /// <summary>
            /// The Xml documentation to use for the return block.
            /// </summary>
            public const string Returns = "The data for the database column with the name columnName.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Gets the data for the database column that this table represents.";
        }

        /// <summary>
        /// Comments used in CreateMethodGetValue().
        /// </summary>
        public static class GetValue
        {
            /// <summary>
            /// The Xml documentation to use for parameter `columnName`.
            /// </summary>
            public const string ColumnNameParameter = "The database name of the column to get the value for.";

            /// <summary>
            /// The Xml documentation to use for the return block.
            /// </summary>
            public const string Returns = "The value of the column with the name columnName.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Gets the value of a column by the database column's name.";
        }

        /// <summary>
        /// Comments used in HasSameValues().
        /// </summary>
        public static class HasSameValues
        {
            /// <summary>
            /// The Xml documentation to use for parameter `other`.
            /// </summary>
            public const string OtherParameter = "The <see cref=\"{0}\"/> to compare the values to.";

            /// <summary>
            /// The Xml documentation to use for the return block.
            /// </summary>
            public const string Returns =
                "True if this <see cref=\"{0}\"/> contains the same values as the otherItem; otherwise false.";

            /// <summary>
            /// The Xml documentation to use for parameter `source`.
            /// </summary>
            public const string SourceParameter = "The source <see cref=\"{0}\"/>.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                "Checks if this <see cref=\"{0}\"/> contains the same values as another <see cref=\"{0}\"/>.";
        }

        public static class ReadState
        {
            /// <summary>
            /// The Xml documentation to use for parameter `writer`.
            /// </summary>
            public const string ParameterWriter = "The <see cref=\"IValueReader\"/> to read the values from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Reads the state of the object from an <see cref=\"IValueReader\"/>.";
        }

        /// <summary>
        /// Comments used in CreateMethodReadValues().
        /// </summary>
        public static class ReadValues
        {
            /// <summary>
            /// The Xml documentation to use for parameter `dataRecord`.
            /// </summary>
            public const string ParameterDataRecord =
                @"The <see cref=""IDataRecord""/> to read the values from. Must already be ready to be read from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                @"Reads the values from an <see cref=""IDataRecord""/> and assigns the read values to this
object's properties. The database column's name is used to as the key, so the value
will not be found if any aliases are used or not all columns were selected.";
        }

        /// <summary>
        /// Comments used in CreateMethodSetValue();
        /// </summary>
        public static class SetValue
        {
            /// <summary>
            /// The Xml documentation to use for parameter `columnName`.
            /// </summary>
            public const string ColumnNameParameter = "The database name of the column to get the value for.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Sets the value of a column by the database column's name.";

            /// <summary>
            /// The Xml documentation to use for parameter `value`.
            /// </summary>
            public const string ValueParameter = "Value to assign to the column.";
        }

        /// <summary>
        /// Comments used in CreateMethodTryCopyValuesToDbParameterValues().
        /// </summary>
        public static class TryCopyValues
        {
            /// <summary>
            /// The Xml documentation to use for parameter `dbParameterValues`.
            /// </summary>
            public const string ParameterDbParameterValues = "The DbParameterValues to copy the values into.";

            /// <summary>
            /// The Xml documentation to use for parameter `source`.
            /// </summary>
            public const string ParameterSource = "The object to copy the values from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                @"Copies the column values into the given DbParameterValues using the database column name
with a prefixed @ as the key. The key must already exist in the DbParameterValues
for the value to be copied over. If any of the keys in the DbParameterValues do not
match one of the column names, or if there is no field for a key, then it will be
ignored. Because of this, it is important to be careful when using this method
since columns or keys can be skipped without any indication.";
        }

        /// <summary>
        /// Comments used in CreateMethodTryReadValues().
        /// </summary>
        public static class TryReadValues
        {
            /// <summary>
            /// The Xml documentation to use for parameter `dataReader`.
            /// </summary>
            public const string ParameterDataReader =
                @"The <see cref=""IDataReader""/> to read the values from. Must already be ready to be read from.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary =
                @"Reads the values from an <see cref=""IDataReader""/> and assigns the read values to this
object's properties. Unlike ReadValues(), this method not only doesn't require
all values to be in the <see cref=""IDataReader""/>, but also does not require the values in
the <see cref=""IDataReader""/> to be a defined field for the table this class represents.
Because of this, you need to be careful when using this method because values
can easily be skipped without any indication.";
        }

        /// <summary>
        /// Comments used in WriteState().
        /// </summary>
        public static class WriteState
        {
            /// <summary>
            /// The Xml documentation to use for parameter `writer`.
            /// </summary>
            public const string ParameterWriter = "The <see cref=\"IValueWriter\"/> to write the values to.";

            /// <summary>
            /// The Xml documentation to use for the summary block.
            /// </summary>
            public const string Summary = "Writes the state of the object to an <see cref=\"IValueWriter\"/>.";
        }
    }
}