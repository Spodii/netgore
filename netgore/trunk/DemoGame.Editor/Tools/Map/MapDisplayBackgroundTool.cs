using System;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    public class MapDisplayBackgroundTool : Tool
    {
        readonly Func<IDrawable, bool> _filter;

        public MapDisplayBackgroundTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsButtonSettings();
            s.ToolTipText = "Toggle displaying sprites on the background layer";
            s.ClickToEnable = true;
            _filter = DrawFilterImplementation;
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Background")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                ToolBarControlType = ToolBarControlType.Button,
                EnabledImage = Resources.MapDisplayBackgroundTool_Enabled,
                DisabledImage = Resources.MapDisplayBackgroundTool_Disabled,
                EnabledByDefault = true,
                OnToolBarByDefault = true,
                HelpName = "Map Display Background Tool",
                HelpWikiPage = "Map display background tool",
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
            return IsEnabled || drawable.MapRenderLayer != MapRenderLayer.SpriteBackground;
        }
    }
}