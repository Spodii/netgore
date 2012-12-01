using System;
using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the background properties.
    /// </summary>
    public class MapEditBackgroundTool : Tool
    {
        MapEditBackgroundToolForm _form;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapEditBackgroundTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapEditBackgroundTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Edit the background layers";
            ToolBarControl.ControlSettings.Click -= ControlSettings_Click;
            ToolBarControl.ControlSettings.Click += ControlSettings_Click;
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

            // Display BG properties
            if (_form == null || _form.IsDisposed)
                _form = new MapEditBackgroundToolForm();

            _form.Map = map;
            _form.Show();
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Edit Background")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapEditBackgroundTool,
                HelpName = "Map Edit Background Tool",
                HelpWikiPage = "Map edit background tool",
            };
        }
    }
}