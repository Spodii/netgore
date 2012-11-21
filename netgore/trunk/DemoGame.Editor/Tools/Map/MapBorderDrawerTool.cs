using System.Linq;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor.Tools
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the <see cref="ScreenGrid"/> for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class MapBorderDrawerTool : Tool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBorderDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapBorderDrawerTool(ToolManager toolManager) : base(toolManager, CreateSettings())
        {
            ToolBarControl.ControlSettings.AsButtonSettings().ClickToEnable = true;
        }

        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Border Drawer")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new MapBorderDrawingExtension() },
                OnToolBarByDefault = false,
                ToolBarControlType = ToolBarControlType.Button,
                HelpName = "Map Border Drawer",
                HelpWikiPage = "Map border drawer tool",
            };
        }

        class MapBorderDrawingExtension : MapDrawingExtension
        {
            readonly MapBorderDrawer _border = new MapBorderDrawer();

            /// <summary>
            /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
            /// </summary>
            /// <param name="map">The map the drawing is taking place on.</param>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
            protected override void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
            {
                _border.Draw(spriteBatch, map, camera);
            }
        }
    }
}