using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame.Client
{
    /// <summary>
    /// A collection of <see cref="MapDrawFilterHelper"/>s.
    /// </summary>
    public class MapDrawFilterHelperCollection : IPersistable
    {
        /// <summary>
        /// Delegate for handling events from the <see cref="MapDrawFilterHelperCollection"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MapDrawFilterHelperCollection"/> the event came from.</param>
        /// <param name="filter">The name of the filter and the filter that the event is related to.</param>
        public delegate void EventHandler(MapDrawFilterHelperCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter);

        /// <summary>
        /// Delegate for handling events from the <see cref="MapDrawFilterHelperCollection"/>.
        /// </summary>
        /// <param name="sender">The <see cref="MapDrawFilterHelperCollection"/> the event came from.</param>
        /// <param name="filter">The name of the filter and the filter that the event is related to.</param>
        /// <param name="oldName">The old name of the filter.</param>
        public delegate void RenamedEventHandler(
            MapDrawFilterHelperCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter, string oldName);

        const string _filterKeyValueName = "Name";
        const string _filterValueValueName = "Value";
        const string _filtersNodeName = "DisplayFilters";

        /// <summary>
        /// The <see cref="StringComparer"/> to use to compare the filter names.
        /// </summary>
        static readonly StringComparer _filterNameComparer = StringComparer.Ordinal;

        readonly IDictionary<string, MapDrawFilterHelper> _filters =
            new Dictionary<string, MapDrawFilterHelper>(_filterNameComparer);

        /// <summary>
        /// Notifies listeners when a filter has been added to this collection.
        /// </summary>
        public event EventHandler Added;

        /// <summary>
        /// Notifies listeners when a filter has been removed from this collection.
        /// </summary>
        public event EventHandler Removed;

        /// <summary>
        /// Notifies listeners when a filter in this collection has been renamed.
        /// </summary>
        public event RenamedEventHandler Renamed;

        /// <summary>
        /// Gets the filters in this collection, where the key is the name and the value is the filter itself.
        /// </summary>
        public IEnumerable<KeyValuePair<string, MapDrawFilterHelper>> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// Adds a filter to this collection.
        /// </summary>
        /// <param name="name">The name of the filter to add.</param>
        /// <param name="filter">The <see cref="MapDrawFilterHelper"/>.</param>
        /// <returns>True if the filter was successfully added; otherwise false.</returns>
        public bool AddFilter(string name, MapDrawFilterHelper filter)
        {
            // Validate the name
            if (string.IsNullOrEmpty(name))
                return false;

            // Validate the filter object
            if (filter == null)
                return false;

            // Check if the name is already in use
            if (_filters.ContainsKey(name))
                return false;

            // Add
            _filters.Add(name, filter);

            // Raise events
            if (Added != null)
                Added(this, new KeyValuePair<string, MapDrawFilterHelper>(name, filter));

            return true;
        }

        /// <summary>
        /// Reads a filter from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The name of the filter and the filter object.</returns>
        static KeyValuePair<string, MapDrawFilterHelper> ReadFilter(IValueReader reader)
        {
            var key = reader.ReadString(_filterKeyValueName);

            var valueReader = reader.ReadNode(_filterValueValueName);
            var value = new MapDrawFilterHelper(valueReader);

            return new KeyValuePair<string, MapDrawFilterHelper>(key, value);
        }

        /// <summary>
        /// Removes a filter from this collection.
        /// </summary>
        /// <param name="name">The name of the filter to remove.</param>
        /// <returns>True if the filter with the given <paramref name="name"/> was successfully removed; otherwise false.</returns>
        public bool RemoveFilter(string name)
        {
            // Check for a valid name
            if (string.IsNullOrEmpty(name))
                return false;

            // Grab the filter object (so we can pass it to the event if successful)
            var filter = TryGetFilter(name);
            if (filter == null)
                return false;

            // Try to remove
            var removed = _filters.Remove(name);

            if (removed)
            {
                // Raise the event
                if (Removed != null)
                    Removed(this, new KeyValuePair<string, MapDrawFilterHelper>(name, filter));
            }

            return removed;
        }

        /// <summary>
        /// Renames a filter in this collection.
        /// </summary>
        /// <param name="oldName">The old (current) name of the filter.</param>
        /// <param name="newName">The new name to give the filter.</param>
        /// <returns>True if the renaming was successful; otherwise false.</returns>
        public bool RenameFilter(string oldName, string newName)
        {
            // Make sure the filter exists
            var filter = TryGetFilter(oldName);
            if (filter == null)
                return false;

            // Check if we are even changing the name at all
            if (_filterNameComparer.Equals(oldName, newName))
                return false;

            // Make sure the new name is not in use
            if (TryGetFilter(newName) != null)
                return false;

            // Remove the filter
            _filters.Remove(oldName);

            // Add back in with the ne wname
            _filters.Add(newName, filter);

            // Raise events
            if (Renamed != null)
                Renamed(this, new KeyValuePair<string, MapDrawFilterHelper>(newName, filter), oldName);

            return true;
        }

        /// <summary>
        /// Tries to get a <see cref="MapDrawFilterHelper"/> using its name.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <returns>The <see cref="MapDrawFilterHelper"/> with the given <paramref name="name"/>, or null if the <paramref name="name"/>
        /// is invalid or no such filter with the given <paramref name="name"/> exists.</returns>
        public MapDrawFilterHelper TryGetFilter(string name)
        {
            MapDrawFilterHelper ret;
            if (!_filters.TryGetValue(name, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Tries to get a filter's name.
        /// </summary>
        /// <param name="filter">The <see cref="MapDrawFilterHelper"/> to get the name of.</param>
        /// <returns>The name of the <paramref name="filter"/>, or null if the <paramref name="filter"/> is invalid or is not
        /// in this collection.</returns>
        public string TryGetName(MapDrawFilterHelper filter)
        {
            if (filter == null)
                return null;

            return _filters.FirstOrDefault(x => x.Value == filter).Key;
        }

        /// <summary>
        /// Writes a filter to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="kvp">The filter name and object to write.</param>
        static void WriteFilter(IValueWriter writer, KeyValuePair<string, MapDrawFilterHelper> kvp)
        {
            writer.Write(_filterKeyValueName, kvp.Key);

            writer.WriteStartNode(_filterValueValueName);
            {
                kvp.Value.WriteState(writer);
            }
            writer.WriteEndNode(_filterValueValueName);
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            var readFilters = reader.ReadManyNodes(_filtersNodeName, ReadFilter);

            // Remove the existing filters
            var toRemove = _filters.Keys.ToImmutable();
            foreach (var key in toRemove)
            {
                RemoveFilter(key);
            }

            Debug.Assert(_filters.IsEmpty());

            _filters.Clear();

            // Add the new filters
            foreach (var f in readFilters)
            {
                AddFilter(f.Key, f.Value);
            }
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            writer.WriteManyNodes(_filtersNodeName, _filters, WriteFilter);
        }

        #endregion
    }
}