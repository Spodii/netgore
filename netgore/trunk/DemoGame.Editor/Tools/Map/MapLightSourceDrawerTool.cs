using System.Linq;
using DemoGame.Editor.Properties;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the <see cref="ScreenGrid"/> for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class MapLightSourceDrawerTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBorderDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapLightSourceDrawerTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.ToolTipText = "Toggles the display of the map light sources";
            ToolBarControl.ControlSettings.AsButtonSettings().ClickToEnable = true;
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Display Light")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new MapLightSourceDrawingExtension() },
                ToolBarControlType = ToolBarControlType.Button,
                DisabledImage = Resources.MapLightSourceDrawerTool_Disabled,
                EnabledImage = Resources.MapLightSourceDrawerTool_Enabled,
                HelpName = "Map Display Light Tool",
                HelpWikiPage = "Map display light tool",
            };
        }

        class MapLightSourceDrawingExtension : MapDrawingExtension
        {
            /// <summary>
            /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
            /// </summary>
            /// <param name="map">The map the drawing is taking place on.</param>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
            protected override void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var msc = MapScreenControl.TryFindInstance(map);
                if (msc == null)
                    return;

                var dm = msc.DrawingManager;
                if (dm == null)
                    return;

                var lm = dm.LightManager;
                if (lm == null)
                    return;

                var lightSprite = SystemSprites.Lightblub;

                var offset = lightSprite.Size / 2f;
                foreach (var light in lm)
                {
                    lightSprite.Draw(spriteBatch, light.Center - offset);
                }
            }
        }
    }
}