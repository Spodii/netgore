using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes the type of code a generated code file contains.
    /// </summary>
    public enum GeneratedCodeType : byte
    {
        /// <summary>
        /// Code for the interface for a table.
        /// </summary>
        Interface,

        /// <summary>
        /// Code for the default implementation of a table.
        /// </summary>
        Class,

        /// <summary>
        /// Code for the column metadata class used by all tables. This is always created only once.
        /// </summary>
        ColumnMetadata,

        /// <summary>
        /// Code for the extension methods for a table class.
        /// </summary>
        ClassDbExtensions,

        /// <summary>
        /// Code for the collection used for column collections. This is generated once for each distinct column collection
        /// key type.
        /// </summary>
        ColumnCollectionClass
    }
}