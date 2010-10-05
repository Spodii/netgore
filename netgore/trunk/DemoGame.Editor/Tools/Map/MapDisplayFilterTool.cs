using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

        readonly MapDrawFilterHelperCollection _filterCollection;
        readonly IList<IToolTargetMapContainer> _mapContainers = new List<IToolTargetMapContainer>();
        readonly ToolStripButton _tsManageFilters;

        MapDrawFilterHelper _currentFilter;

        // TODO: !! Make use of the FilterCollection... add ToolStripItems for each filter, ability to add/remove filters, etc
      
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
        }

        /// <summary>
        /// Handles the Click event of the <see cref="_tsManageFilters"/> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void _tsManageFilters_Click(object sender, EventArgs e)
        {
            using (var frm = new DisplayFilterManagerForm())
            {
                frm.Collection = _filterCollection;
                frm.ShowDialog(sender as IWin32Window);
            }
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
            writer.WriteEndNode(_filterCollectionNodeName);
        }
    }
}