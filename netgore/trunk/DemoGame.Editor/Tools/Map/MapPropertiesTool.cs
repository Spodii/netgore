using System;
using System.Linq;
using DemoGame.Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor.Tools
{
    public class MapPropertiesTool : Tool
    {
        MapPropertiesToolForm _currentPropertiesForm;
        DockPanel _dockPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPropertiesTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapPropertiesTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Displays the properties for the current map";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
        }

        /// <summary>
        /// Gets or sets the <see cref="DockPanel"/> to dock the <see cref="MapPropertiesToolForm"/> to.
        /// Can be null.
        /// </summary>
        public DockPanel DockPanel
        {
            get { return _dockPanel; }
            set
            {
                if (_dockPanel == value)
                    return;

                _dockPanel = value;

                if (_currentPropertiesForm != null && _currentPropertiesForm.Visible)
                {
                    _currentPropertiesForm.Hide();

                    if (DockPanel == null)
                        _currentPropertiesForm.Show();
                    else
                        _currentPropertiesForm.Show(DockPanel);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ControlSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ControlSettings_Click(object sender, EventArgs e)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            if (tb == null)
                return;

            var map = tb.DisplayObject as EditorMap;
            if (map == null)
                return;

            ShowMapProperties(map);
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Properties")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapPropertiesTool,
                HelpName = "Map Properties Tool",
                HelpWikiPage = "Map properties tool",
            };
        }

        /// <summary>
        /// Shows the properties for a map.
        /// </summary>
        /// <param name="map">The map to show the properties for.</param>
        public void ShowMapProperties(EditorMap map)
        {
            // Create new form instance if needed
            if (_currentPropertiesForm == null || _currentPropertiesForm.Disposing || _currentPropertiesForm.IsDisposed)
                _currentPropertiesForm = new MapPropertiesToolForm();

            // If not already visible, show it
            if (!_currentPropertiesForm.Visible)
            {
                if (DockPanel == null)
                    _currentPropertiesForm.Show();
                else
                    _currentPropertiesForm.Show(DockPanel);
            }

            // Set the map
            _currentPropertiesForm.Map = map;
        }
    }
}