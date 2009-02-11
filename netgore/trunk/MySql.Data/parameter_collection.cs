using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing.Design;
using MySql.Data.MySqlClient.Properties;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// Represents a collection of parameters relevant to a <see cref="MySqlCommand"/> as well as their respective mappings to columns in a <see cref="System.Data.DataSet"/>. This class cannot be inherited.
    /// </summary>
    /// <include file='docs/MySqlParameterCollection.xml' path='MyDocs/MyMembers[@name="Class"]/*'/>
    [Editor("MySql.Data.MySqlClient.Design.DBParametersEditor,MySql.Design", typeof(UITypeEditor))]
    [ListBindable(true)]
    public sealed class MySqlParameterCollection : DbParameterCollection
    {
        readonly Hashtable indexHash;
        readonly ArrayList items = new ArrayList();

        /// <summary>
        /// Gets the number of MySqlParameter objects in the collection.
        /// </summary>
        public override int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="MySqlParameterCollection"/>
        /// has a fixed size. 
        /// </summary>
        public override bool IsFixedSize
        {
            get { return items.IsFixedSize; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="MySqlParameterCollection"/>
        /// is read-only. 
        /// </summary>
        public override bool IsReadOnly
        {
            get { return items.IsReadOnly; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="MySqlParameterCollection"/>
        /// is synchronized. 
        /// </summary>
        public override bool IsSynchronized
        {
            get { return items.IsSynchronized; }
        }

        /// <summary>
        /// Gets the <see cref="MySqlParameter"/> at the specified index.
        /// </summary>
        /// <overloads>Gets the <see cref="MySqlParameter"/> with a specified attribute.
        /// [C#] In C#, this property is the indexer for the <see cref="MySqlParameterCollection"/> class.
        /// </overloads>
        public new MySqlParameter this[int index]
        {
            get { return (MySqlParameter)GetParameter(index); }
            set { SetParameter(index, value); }
        }

        /// <summary>
        /// Gets the <see cref="MySqlParameter"/> with the specified name.
        /// </summary>
        public new MySqlParameter this[string name]
        {
            get { return (MySqlParameter)GetParameter(name); }
            set { SetParameter(name, value); }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the 
        /// <see cref="MySqlParameterCollection"/>. 
        /// </summary>
        public override object SyncRoot
        {
            get { return items.SyncRoot; }
        }

        internal MySqlParameterCollection()
        {
            indexHash = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            Clear();
        }

        /// <summary>
        /// Adds the specified <see cref="MySqlParameter"/> object to the <see cref="MySqlParameterCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="MySqlParameter"/> to add to the collection.</param>
        /// <returns>The newly added <see cref="MySqlParameter"/> object.</returns>
        public MySqlParameter Add(MySqlParameter value)
        {
            return InternalAdd(value, -1);
        }

        /// <summary>
        /// Adds a <see cref="MySqlParameter"/> to the <see cref="MySqlParameterCollection"/> given the specified parameter name and value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The <see cref="MySqlParameter.Value"/> of the <see cref="MySqlParameter"/> to add to the collection.</param>
        /// <returns>The newly added <see cref="MySqlParameter"/> object.</returns>
        [Obsolete(
            "Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value)")]
        public MySqlParameter Add(string parameterName, object value)
        {
            return Add(new MySqlParameter(parameterName, value));
        }

        /// <summary>
        /// Adds a <see cref="MySqlParameter"/> to the <see cref="MySqlParameterCollection"/> given the parameter name and the data type.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <returns>The newly added <see cref="MySqlParameter"/> object.</returns>
        public MySqlParameter Add(string parameterName, MySqlDbType dbType)
        {
            return Add(new MySqlParameter(parameterName, dbType));
        }

        /// <summary>
        /// Adds a <see cref="MySqlParameter"/> to the <see cref="MySqlParameterCollection"/> with the parameter name, the data type, and the column length.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <param name="size">The length of the column.</param>
        /// <returns>The newly added <see cref="MySqlParameter"/> object.</returns>
        public MySqlParameter Add(string parameterName, MySqlDbType dbType, int size)
        {
            return Add(new MySqlParameter(parameterName, dbType, size));
        }

        /// <summary>
        /// Adds a <see cref="MySqlParameter"/> to the <see cref="MySqlParameterCollection"/> with the parameter name, the data type, the column length, and the source column name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="dbType">One of the <see cref="MySqlDbType"/> values. </param>
        /// <param name="size">The length of the column.</param>
        /// <param name="sourceColumn">The name of the source column.</param>
        /// <returns>The newly added <see cref="MySqlParameter"/> object.</returns>
        public MySqlParameter Add(string parameterName, MySqlDbType dbType, int size, string sourceColumn)
        {
            return Add(new MySqlParameter(parameterName, dbType, size, sourceColumn));
        }

        /// <summary>
        /// Adds the specified <see cref="MySqlParameter"/> object to the <see cref="MySqlParameterCollection"/>.
        /// </summary>
        /// <param name="value">The <see cref="MySqlParameter"/> to add to the collection.</param>
        /// <returns>The index of the new <see cref="MySqlParameter"/> object.</returns>
        public override int Add(object value)
        {
            if (!(value is MySqlParameter))
                throw new MySqlException("Only MySqlParameter objects may be stored");

            MySqlParameter p = (MySqlParameter)value;

            if (string.IsNullOrEmpty(p.ParameterName))
                throw new MySqlException("Parameters must be named");

            p = Add(p);
            return IndexOf(p);
        }

        /// <summary>
        /// Adds an array of values to the end of the <see cref="MySqlParameterCollection"/>. 
        /// </summary>
        /// <param name="values"></param>
        public override void AddRange(Array values)
        {
            foreach (DbParameter p in values)
            {
                Add(p);
            }
        }

#pragma warning disable 1591
        public MySqlParameter AddWithValue(string parameterName, object value)
#pragma warning restore 1591
        {
            return Add(new MySqlParameter(parameterName, value));
        }

        void AdjustHash(int keyIndex, bool addEntry)
        {
            for (int i = 0; i < Count; i++)
            {
                MySqlParameter p = (MySqlParameter)items[i];
                if (!indexHash.ContainsKey(p.ParameterName))
                    return;
                int index = (int)indexHash[p.ParameterName];
                if (index < keyIndex)
                    continue;
                indexHash[p.ParameterName] = addEntry ? ++index : --index;
            }
        }

        void CheckIndex(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException("Parameter index is out of range.");
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public override void Clear()
        {
            foreach (MySqlParameter p in items)
            {
                p.Collection = null;
            }
            items.Clear();
            indexHash.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether a <see cref="MySqlParameter"/> with the specified parameter name exists in the collection.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="MySqlParameter"/> object to find.</param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public override bool Contains(string parameterName)
        {
            return IndexOf(parameterName) != -1;
        }

        /// <summary>
        /// Gets a value indicating whether a MySqlParameter exists in the collection.
        /// </summary>
        /// <param name="value">The value of the <see cref="MySqlParameter"/> object to find. </param>
        /// <returns>true if the collection contains the <see cref="MySqlParameter"/> object; otherwise, false.</returns>
        /// <overloads>Gets a value indicating whether a <see cref="MySqlParameter"/> exists in the collection.</overloads>
        public override bool Contains(object value)
        {
            return items.Contains(value);
        }

        /// <summary>
        /// Copies MySqlParameter objects from the MySqlParameterCollection to the specified array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public override void CopyTo(Array array, int index)
        {
            items.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="MySqlParameterCollection"/>. 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Retrieve the parameter with the given name.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected override DbParameter GetParameter(string parameterName)
        {
            int index = IndexOf(parameterName);
            if (index < 0)
            {
                // check to see if the user has added the parameter without a
                // parameter marker.  If so, kindly tell them what they did.
                if (parameterName.StartsWith("@") || parameterName.StartsWith("?"))
                {
                    string newParameterName = parameterName.Substring(1);
                    index = IndexOf(newParameterName);
                    if (index != -1)
                        return (DbParameter)items[index];
                }
                throw new ArgumentException("Parameter '" + parameterName + "' not found in the collection.");
            }
            return (DbParameter)items[index];
        }

        ///<summary>
        ///
        ///                    Returns the <see cref="T:System.Data.Common.DbParameter" /> object at the specified index in the collection.
        ///                
        ///</summary>
        ///
        ///<returns>
        ///
        ///                    The <see cref="T:System.Data.Common.DbParameter" /> object at the specified index in the collection.
        ///                
        ///</returns>
        ///
        ///<param name="index">
        ///                    The index of the <see cref="T:System.Data.Common.DbParameter" /> in the collection.
        ///                </param>
        protected override DbParameter GetParameter(int index)
        {
            CheckIndex(index);
            return (DbParameter)items[index];
        }

        internal MySqlParameter GetParameterFlexible(string parameterName, bool throwOnNotFound)
        {
            int index = IndexOf(parameterName);
            if (-1 == index)
                index = IndexOf("?" + parameterName);
            if (-1 == index)
                index = IndexOf("@" + parameterName);
            if (-1 == index)
            {
                if (parameterName.StartsWith("@") || parameterName.StartsWith("?"))
                    index = IndexOf(parameterName.Substring(1));
            }
            if (-1 != index)
                return this[index];
            if (throwOnNotFound)
                throw new ArgumentException("Parameter '" + parameterName + "' not found in the collection.");
            return null;
        }

        /// <summary>
        /// Gets the location of the <see cref="MySqlParameter"/> in the collection with a specific parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="MySqlParameter"/> object to retrieve. </param>
        /// <returns>The zero-based location of the <see cref="MySqlParameter"/> in the collection.</returns>
        public override int IndexOf(string parameterName)
        {
            object o = indexHash[parameterName];
            if (o == null)
                return -1;
            return (int)o;
        }

        /// <summary>
        /// Gets the location of a <see cref="MySqlParameter"/> in the collection.
        /// </summary>
        /// <param name="value">The <see cref="MySqlParameter"/> object to locate. </param>
        /// <returns>The zero-based location of the <see cref="MySqlParameter"/> in the collection.</returns>
        /// <overloads>Gets the location of a <see cref="MySqlParameter"/> in the collection.</overloads>
        public override int IndexOf(object value)
        {
            return items.IndexOf(value);
        }

        /// <summary>
        /// Inserts a MySqlParameter into the collection at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void Insert(int index, object value)
        {
            if (!(value is MySqlParameter))
                throw new MySqlException("Only MySqlParameter objects may be stored");
            InternalAdd((MySqlParameter)value, index);
        }

        MySqlParameter InternalAdd(MySqlParameter value, int index)
        {
            if (value == null)
                throw new ArgumentException("The MySqlParameterCollection only accepts non-null MySqlParameter type objects.",
                                            "value");

            // make sure we don't already have a parameter with this name
            string inComingName = value.ParameterName;
            if (indexHash.ContainsKey(inComingName))
                throw new MySqlException(String.Format(Resources.ParameterAlreadyDefined, value.ParameterName));
            if (inComingName[0] == '@' || inComingName[0] == '?')
                inComingName = inComingName.Substring(1, inComingName.Length - 1);
            if (indexHash.ContainsKey(inComingName))
                throw new MySqlException(String.Format(Resources.ParameterAlreadyDefined, value.ParameterName));

            if (index == -1)
            {
                index = items.Add(value);
                indexHash.Add(value.ParameterName, index);
            }
            else
            {
                items.Insert(index, value);
                AdjustHash(index, true);
                indexHash.Add(value.ParameterName, index);
            }

            value.Collection = this;
            return value;
        }

        internal void ParameterNameChanged(MySqlParameter p, string oldName, string newName)
        {
            int index = IndexOf(oldName);
            indexHash.Remove(oldName);
            indexHash.Add(newName, index);
        }

        /// <summary>
        /// Removes the specified MySqlParameter from the collection.
        /// </summary>
        /// <param name="value"></param>
        public override void Remove(object value)
        {
            MySqlParameter p = (MySqlParameter)value;
            p.Collection = null;
            int index = IndexOf(p);
            items.Remove(p);
            indexHash.Remove(p.ParameterName);
            AdjustHash(index, false);
        }

        /// <summary>
        /// Removes the specified <see cref="MySqlParameter"/> from the collection using the parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the <see cref="MySqlParameter"/> object to retrieve. </param>
        public override void RemoveAt(string parameterName)
        {
            DbParameter p = GetParameter(parameterName);
            Remove(p);
        }

        /// <summary>
        /// Removes the specified <see cref="MySqlParameter"/> from the collection using a specific index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter. </param>
        /// <overloads>Removes the specified <see cref="MySqlParameter"/> from the collection.</overloads>
        public override void RemoveAt(int index)
        {
            object o = items[index];
            Remove(o);
        }

        ///<summary>
        ///
        ///                    Sets the <see cref="T:System.Data.Common.DbParameter" /> object with the specified name to a new value.
        ///                
        ///</summary>
        ///
        ///<param name="parameterName">
        ///                    The name of the <see cref="T:System.Data.Common.DbParameter" /> object in the collection.
        ///                </param>
        ///<param name="value">
        ///                    The new <see cref="T:System.Data.Common.DbParameter" /> value.
        ///                </param>
        protected override void SetParameter(string parameterName, DbParameter value)
        {
            int index = IndexOf(parameterName);
            if (index < 0)
                throw new ArgumentException("Parameter '" + parameterName + "' not found in the collection.");
            SetParameter(index, value);
        }

        ///<summary>
        ///
        ///                    Sets the <see cref="T:System.Data.Common.DbParameter" /> object at the specified index to a new value. 
        ///                
        ///</summary>
        ///
        ///<param name="index">
        ///                    The index where the <see cref="T:System.Data.Common.DbParameter" /> object is located.
        ///                </param>
        ///<param name="value">
        ///                    The new <see cref="T:System.Data.Common.DbParameter" /> value.
        ///                </param>
        protected override void SetParameter(int index, DbParameter value)
        {
            CheckIndex(index);
            MySqlParameter p = (MySqlParameter)items[index];

            indexHash.Remove(p.ParameterName);
            items[index] = value;
            indexHash.Add(value.ParameterName, index);
        }
    }
}