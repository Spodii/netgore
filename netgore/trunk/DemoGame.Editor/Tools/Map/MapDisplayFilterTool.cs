using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using log4net;
using NetGore;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.Editor.Tools
{
    public class MapDisplayFilterTool : Tool // TODO: !! Delete
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const string _filterCollectionNodeName = "FilterCollection";

        readonly MapDrawFilterHelperCollection _filterCollection;
        readonly IList<IToolTargetMapContainer> _mapContainers = new List<IToolTargetMapContainer>();

        readonly Dictionary<MapDrawFilterHelper, FilterToolStripItem> _tsFilters = new Dictionary<MapDrawFilterHelper, FilterToolStripItem>();

        readonly ToolStripButton _tsManageFilters;

        MapDrawFilterHelper _currentFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDisplayFilterTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        public MapDisplayFilterTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            _filterCollection = new MapDrawFilterHelperCollection();

            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.ToolTipText = "Filters what is displayed on the map";
            s.ClickToEnable = true;

            // Set up the DropDownItems
            _tsManageFilters = new ToolStripButton("Manage Filters...", null, _tsManageFilters_Click);

            s.DropDownItems.Add(new ToolStripSeparator());
            s.DropDownItems.Add(_tsManageFilters);

            // Forward any existing filters to the _filterCollection_Added method (easy way to deal with the fact that we
            // haven't added the event hooks yet)
            foreach (var f in _filterCollection.Filters)
            {
                _filterCollection_Added(_filterCollection, new MapDrawFilterHelperCollectionEventArgs(f));
            }

            // Listen to the filter collection events
            _filterCollection.Added += _filterCollection_Added;
            _filterCollection.Removed += _filterCollection_Removed;
            _filterCollection.Renamed += _filterCollection_Renamed;
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
                OnToolBarByDefault = false,
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
                var map = mapC.Map as EditorMap;
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
            writer.WriteEndNode(_filterCollectionNodeName);
        }

        /// <summary>
        /// Handles hwen a filter is added to the collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MapDrawFilterHelperCollectionEventArgs"/> instance containing the event data.</param>
        void _filterCollection_Added(MapDrawFilterHelperCollection sender, MapDrawFilterHelperCollectionEventArgs e)
        {
            // Add the new filter to the ToolBarControl's DropDownItems

            var ftsi = new FilterToolStripItem(e.Filter, e.FilterName);
            ftsi.CheckedChanged += ftsi_CheckedChanged;

            _tsFilters.Add(e.Filter, ftsi);

            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();
            s.DropDownItems.Insert(0, ftsi);
        }

        /// <summary>
        /// Handles when a filter is removed from the collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MapDrawFilterHelperCollectionEventArgs"/> instance containing the event data.</param>
        void _filterCollection_Removed(MapDrawFilterHelperCollection sender, MapDrawFilterHelperCollectionEventArgs e)
        {
            // Remove the filter from the ToolBarControl's DropDownItems

            FilterToolStripItem ftsi;
            if (!_tsFilters.TryGetValue(e.Filter, out ftsi))
            {
                const string errmsg = "Failed to find FilterToolStripItem for filter `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, e.Filter);
                Debug.Fail(string.Format(errmsg, e.Filter));
                return;
            }

            Debug.Assert(ftsi.Filter == e.Filter);

            _tsFilters.Remove(e.Filter);
            ftsi.Dispose();

            // If the filter we just got rid of was the active one, set the active filter to null
            if (e.Filter == _currentFilter)
                _currentFilter = null;
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MapDrawFilterHelperCollectionRenameEventArgs"/> instance containing the event data.</param>
        void _filterCollection_Renamed(MapDrawFilterHelperCollection sender, MapDrawFilterHelperCollectionRenameEventArgs e)
        {
            // Update the displayed name

            FilterToolStripItem ftsi;
            if (!_tsFilters.TryGetValue(e.Filter, out ftsi))
            {
                const string errmsg = "Failed to find FilterToolStripItem for filter `{0}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, e.Filter);
                Debug.Fail(string.Format(errmsg, e.Filter));
                return;
            }

            Debug.Assert(ftsi.Filter == e.Filter);

            ftsi.FilterNameChanged(e.FilterName);
        }

        /// <summary>
        /// Handles the Click event of the <see cref="_tsManageFilters"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _tsManageFilters_Click(object sender, EventArgs e)
        {
            var frm = new DisplayFilterManagerForm { Collection = _filterCollection };
            frm.Show();
        }

        void ftsi_CheckedChanged(object sender, EventArgs e)
        {
            var ftsi = sender as FilterToolStripItem;
            if (ftsi == null)
            {
                const string errmsg = "Was expecting `{0}` to be type FilterToolStripItem, but was `{1}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, sender, sender.GetType());
                Debug.Fail(string.Format(errmsg, sender, sender.GetType()));
                return;
            }

            // If checked, uncheck all other filters, then set this one as the active one
            if (ftsi.Checked)
            {
                foreach (var f in _tsFilters.Values.Where(f => f != ftsi))
                {
                    f.Checked = false;
                }

                _currentFilter = ftsi.Filter;
            }
        }

        /// <summary>
        /// A <see cref="ToolStripButton"/> for one of the <see cref="MapDrawFilterHelper"/>s.
        /// </summary>
        class FilterToolStripItem : ToolStripButton
        {
            readonly MapDrawFilterHelper _filter;

            /// <summary>
            /// Initializes a new instance of the <see cref="FilterToolStripItem"/> class.
            /// </summary>
            /// <param name="filter">The <see cref="MapDrawFilterHelper"/>.</param>
            /// <param name="filterName">Name of the filter.</param>
            /// <exception cref="ArgumentNullException"><paramref name="filter"/> is null.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="filterName"/> is null.</exception>
            public FilterToolStripItem(MapDrawFilterHelper filter, string filterName)
            {
                if (filter == null)
                    throw new ArgumentNullException("filter");

                _filter = filter;

                Name = filterName;
                Text = filterName;

                CheckOnClick = true;
                Checked = false;
            }

            /// <summary>
            /// Gets the <see cref="MapDrawFilterHelper"/> that this <see cref="FilterToolStripItem"/> is for.
            /// </summary>
            public MapDrawFilterHelper Filter
            {
                get { return _filter; }
            }

            /// <summary>
            /// Handles when the filter's name is changed.
            /// </summary>
            /// <param name="filterName">The new name of the filter.</param>
            public void FilterNameChanged(string filterName)
            {
                Name = filterName;
                Text = filterName;
            }
        }
    }
}