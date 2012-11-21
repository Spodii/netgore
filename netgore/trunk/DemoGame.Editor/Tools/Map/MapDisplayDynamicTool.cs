using System;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapDisplayDynamicTool : Tool
    {
        readonly Func<IDrawable, bool> _filter;

        public MapDisplayDynamicTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsButtonSettings();
            s.ToolTipText = "Toggle displaying sprites on the dynamic layer";
            s.ClickToEnable = true;
            _filter = DrawFilterImplementation;
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Dynamic")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapDisplayDynamicTool_Enabled,
                DisabledImage = Resources.MapDisplayDynamicTool_Disabled,
                EnabledByDefault = true,
                OnToolBarByDefault = true,
                HelpName = "Map Display Dynamic Tool",
                HelpWikiPage = "Map display dynamic tool",
            };
        }

        protected override void OnIsEnabledChanged(bool oldValue, bool newValue)
        {
            base.OnIsEnabledChanged(oldValue, newValue);

            var filters = GlobalState.Instance.Map.MapDrawFilters;
            if (IsEnabled)
                filters.Remove(_filter);
            else
                filters.Add(_filter);
        }

        bool DrawFilterImplementation(IDrawable drawable)
        {
            return IsEnabled || drawable.MapRenderLayer != MapRenderLayer.Dynamic;
        }
    }
}