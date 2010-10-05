using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Editor
{
    public class MapDisplayFilterTool : Tool
    {
        const string _filterCollectionNodeName = "FilterCollection";

        readonly FilterCollection _filterCollection;
        readonly IList<IToolTargetMapContainer> _mapContainers = new List<IToolTargetMapContainer>();

        MapDrawFilterHelper _currentFilter;

        // TODO: !! Make use of the FilterCollection... add ToolStripItems for each filter, ability to add/remove filters, etc

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDisplayFilterTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapDisplayFilterTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            _filterCollection = new FilterCollection();

            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.ToolTipText = "Filters what is displayed on the map";
            s.ClickToEnable = true;
        }

        /// <summary>
        /// Gets the filter currently being used. Will be null if none selected or this <see cref="Tool"/> is disabled.
        /// </summary>
        public MapDrawFilterHelper CurrentFilter
        {
            get
            {
                if (!IsEnabled)
                    return null;

                return _currentFilter;
            }
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Filter")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                ToolBarControlType = ToolBarControlType.SplitButton,
                EnabledImage = Resources.MapDisplayFilterTool_Enabled,
                DisabledImage = Resources.MapDisplayFilterTool_Disabled,
                EnabledByDefault = false,
                OnToolBarByDefault = true,
            };
        }

        /// <summary>
        /// When overridden in the derived class, handles updating the <see cref="Tool"/>.
        /// </summary>
        /// <param name="currentTime">The current game time.</param>
        protected override void HandleUpdate(TickCount currentTime)
        {
            base.HandleUpdate(currentTime);

            Func<IDrawable, bool> fFunc = null;

            var cf = CurrentFilter;
            if (cf != null)
                fFunc = cf.FilterFunc;

            // Ensure the correct filter is set on all the maps
            foreach (var mapC in _mapContainers)
            {
                var map = mapC.Map as Map;
                if (map != null)
                    map.DrawFilter = fFunc;
            }
        }

        /// <summary>
        /// Handles reading custom state information for this <see cref="Tool"/> from an <see cref="IValueReader"/> for when
        /// persisting the <see cref="Tool"/>'s state.
        /// When possible, it is preferred that you use the <see cref="SyncValueAttribute"/> instead of manually handling
        /// reading and writing the state.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        protected override void ReadCustomToolState(IValueReader reader)
        {
            base.ReadCustomToolState(reader);

            var filterCollectionReader = reader.ReadNode(_filterCollectionNodeName);

            _filterCollection.ReadState(filterCollectionReader);
        }

        /// <summary>
        /// When overridden in the derived class, handles setting up event listeners for a <see cref="IToolTargetContainer"/>.
        /// This will be invoked once for every <see cref="Tool"/> instance for every <see cref="IToolTargetContainer"/> available.
        /// When the <see cref="Tool"/> is newly added to the <see cref="ToolManager"/>, all existing <see cref="IToolTargetContainer"/>s
        /// will be sent through this method. As new ones are added while this <see cref="Tool"/> exists, those new
        /// <see cref="IToolTargetContainer"/>s will also be passed through. What events to listen to and on what instances is
        /// purely up to the derived <see cref="Tool"/>.
        /// Make sure that all attached event listeners are also removed in the <see cref="Tool.ToolTargetContainerRemoved"/> method.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerAdded(IToolTargetContainer c)
        {
            base.ToolTargetContainerAdded(c);

            var mapC = c.AsMapContainer();
            if (mapC != null)
                _mapContainers.Add(mapC);
        }

        /// <summary>
        /// When overridden in the derived class, handles tearing down event listeners for a <see cref="IToolTargetContainer"/>.
        /// Any event listeners set up in <see cref="Tool.ToolTargetContainerAdded"/> should be torn down here.
        /// </summary>
        /// <param name="c">The <see cref="IToolTargetContainer"/> to optionally listen to events on.</param>
        protected override void ToolTargetContainerRemoved(IToolTargetContainer c)
        {
            base.ToolTargetContainerRemoved(c);

            var mapC = c.AsMapContainer();
            if (mapC != null)
                _mapContainers.Remove(mapC);
        }

        /// <summary>
        /// Handles writing custom state information for this <see cref="Tool"/> to an <see cref="IValueWriter"/> for when
        /// persisting the <see cref="Tool"/>'s state.
        /// When possible, it is preferred that you use the <see cref="SyncValueAttribute"/> instead of manually handling
        /// reading and writing the state.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        protected override void WriteCustomToolState(IValueWriter writer)
        {
            base.WriteCustomToolState(writer);

            writer.WriteStartNode(_filterCollectionNodeName);
            {
                _filterCollection.WriteState(writer);
            }
        }

        class FilterCollection : IPersistable
        {
            const string _filterKeyValueName = "Name";
            const string _filterValueValueName = "Value";
            const string _filtersNodeName = "DisplayFilters";

            readonly IDictionary<string, MapDrawFilterHelper> _filters =
                new Dictionary<string, MapDrawFilterHelper>(StringComparer.Ordinal);

            public delegate void EventHandler(FilterCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter);

            public delegate void RenamedEventHandler(FilterCollection sender, KeyValuePair<string, MapDrawFilterHelper> filter, string oldName);

            public event EventHandler Added;
            public event EventHandler Removed;
            public event RenamedEventHandler Renamed;

            public IEnumerable<KeyValuePair<string, MapDrawFilterHelper>> Filters
            {
                get { return _filters; }
            }

            public void RenameFilter(string oldName, string newName)
            {
                var filter = TryGetFilter(oldName);
                if (filter == null)
                    return;

                if (TryGetFilter(newName) != null)
                    return;

                _filters.Remove(oldName);
                _filters.Add(newName, filter);

                if (Renamed != null)
                    Renamed(this, new KeyValuePair<string, MapDrawFilterHelper>(newName, filter), oldName);
            }

            public void AddFilter(string name, MapDrawFilterHelper filter)
            {
                if (_filters.ContainsKey(name))
                    return;

                _filters.Add(name, filter);

                if (Added != null)
                    Added(this, new KeyValuePair<string, MapDrawFilterHelper>(name, filter));
            }

            static KeyValuePair<string, MapDrawFilterHelper> ReadFilter(IValueReader reader)
            {
                var key = reader.ReadString(_filterKeyValueName);
                var valueReader = reader.ReadNode(_filterValueValueName);
                var value = new MapDrawFilterHelper(valueReader);

                return new KeyValuePair<string, MapDrawFilterHelper>(key, value);
            }

            public void RemoveFilter(string name)
            {
                var filter = TryGetFilter(name);
                if (filter == null)
                    return;

                bool removed = _filters.Remove(name);

                if (removed)
                {
                    if (Removed != null)
                        Removed(this, new KeyValuePair<string, MapDrawFilterHelper>(name, filter));
                }
            }

            public MapDrawFilterHelper TryGetFilter(string name)
            {
                MapDrawFilterHelper ret;
                if (!_filters.TryGetValue(name, out ret))
                    return null;

                return ret;
            }

            public string TryGetName(MapDrawFilterHelper filter)
            {
                return _filters.FirstOrDefault(x => x.Value == filter).Key;
            }

            static void WriteFilter(IValueWriter writer, KeyValuePair<string, MapDrawFilterHelper> kvp)
            {
                writer.Write(_filterKeyValueName, kvp.Key);
                kvp.Value.WriteState(writer);
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
}