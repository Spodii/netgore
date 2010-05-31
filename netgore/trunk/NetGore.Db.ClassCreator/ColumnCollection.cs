using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.ClassCreator
{
    /// <summary>
    /// Describes multiple database columns that are to be treated as a single property and accessed through a getter and setter
    /// instead of accessing each column directly by name.
    /// </summary>
    public class ColumnCollection
    {
        static readonly char[] _vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnCollection"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="keyType">Type of the key.</param>
        /// <param name="externalType">The exposed type.</param>
        /// <param name="internalType">Type of the value and the internal storage type.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="columns">The columns.</param>
        public ColumnCollection(string name, Type keyType, Type externalType, Type internalType, IEnumerable<string> tables,
                                IEnumerable<ColumnCollectionItem> columns)
        {
            Name = name;
            KeyType = keyType;
            InternalType = internalType;
            ExternalType = externalType;
            Tables = tables;
            Columns = columns;
        }

        /// <summary>
        /// Gets the name of the collection property.
        /// </summary>
        /// <value>The name of the collection property.</value>
        public string CollectionPropertyName
        {
            get
            {
                if (Name.EndsWith("s"))
                    return Name + "es";
                else if (Name.Length > 1 && Name.EndsWith("y"))
                {
                    if (!_vowels.Contains(Name[Name.Length - 2]))
                        return Name.Substring(0, Name.Length - 1) + "ies";
                    else
                        return Name + "s";
                }
                else
                    return Name + "s";
            }
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public IEnumerable<ColumnCollectionItem> Columns { get; private set; }

        /// <summary>
        /// Gets or sets the external type.
        /// </summary>
        /// <value>The type of the value.</value>
        public Type ExternalType { get; private set; }

        /// <summary>
        /// Gets or sets the internal type.
        /// </summary>
        /// <value>The type of the value.</value>
        public Type InternalType { get; private set; }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public Type KeyType { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the tables.
        /// </summary>
        /// <value>The tables.</value>
        public IEnumerable<string> Tables { get; private set; }
    }
}