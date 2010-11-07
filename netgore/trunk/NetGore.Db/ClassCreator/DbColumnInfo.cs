using System;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes a single database column.
    /// </summary>
    public class DbColumnInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumnInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="databaseType">Type of the database.</param>
        /// <param name="type">The type.</param>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="keyType">Type of the key.</param>
        public DbColumnInfo(string name, string databaseType, Type type, bool nullable, object defaultValue, string comment,
                            DbColumnKeyType keyType)
        {
            Name = name;
            DatabaseType = databaseType;
            Type = type;
            IsNullable = nullable;
            DefaultValue = defaultValue;
            Comment = comment;
            KeyType = keyType;
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>The type of the database.</value>
        public string DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DbColumnInfo"/> is nullable.
        /// </summary>
        /// <value><c>true</c> if nullable; otherwise, <c>false</c>.</value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public DbColumnKeyType KeyType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }
    }
}