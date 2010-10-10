﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Builds a collection of database column names.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public class ColumnCollectionBuilder<T> : IColumnCollectionBuilder<T>
    {
        readonly IList<string> _columns;
        readonly T _owner;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnCollectionBuilder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public ColumnCollectionBuilder(T owner, IQueryBuilderSettings settings)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (owner == null)
                throw new ArgumentNullException("owner");
            // ReSharper restore CompareNonConstrainedGenericWithNull

            if (settings == null)
                throw new ArgumentNullException("settings");

            _settings = settings;
            _owner = owner;
            _columns = new List<string>();
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> being used.
        /// </summary>
        public IQueryBuilderSettings Settings { get { return _settings; } }

        /// <summary>
        /// Gets the column names in this collection.
        /// </summary>
        /// <returns>The column names.</returns>
        public string[] GetValues()
        {
            return _columns.ToArray();
        }

        /// <summary>
        /// Adds a column to the collection if it does not already exist.
        /// </summary>
        /// <param name="column">The name of the column to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public T Add(string column)
        {
            Settings.IsValidColumnName(column, true);

            if (!_columns.Contains(column, Settings.ColumnNameComparer))
                _columns.Add(column);

            return _owner;
        }

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Add(IEnumerable<string> columns)
        {
            foreach (var c in columns)
                Add(c);

            return _owner;
        }

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Add(params string[] columns)
        {
            return Add((IEnumerable<string>)columns);
        }

        /// <summary>
        /// Removes a column from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public T Remove(string column)
        {
            Settings.IsValidColumnName(column, true);

            for (int i = 0; i < _columns.Count; i++)
            {
                if (Settings.ColumnNameComparer.Equals(column, _columns[i]))
                {
                    _columns.RemoveAt(i);
                    break;
                }
            }

            return _owner;
        }

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Remove(IEnumerable<string> columns)
        {
            foreach (var c in columns)
                Remove(c);

            return _owner;
        }

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Remove(params string[] columns)
        {
            return Remove((IEnumerable<string>)columns);
        }
    }
}