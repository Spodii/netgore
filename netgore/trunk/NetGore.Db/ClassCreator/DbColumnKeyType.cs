using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes the type of key a database column uses.
    /// </summary>
    public enum DbColumnKeyType : byte
    {
        /// <summary>
        /// No key.
        /// </summary>
        None,

        /// <summary>
        /// Primary key.
        /// </summary>
        Primary,

        /// <summary>
        /// Foreign key.
        /// </summary>
        Foreign
    }
}