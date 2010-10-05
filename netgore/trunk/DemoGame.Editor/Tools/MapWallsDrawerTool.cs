using System.Collections.Generic;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Editor.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the walls on the map.
    /// </summary>
    public class MapWallsDrawerTool : Tool
    {
        /// <summary>
        /// Creates the <see cref="ToolSettings"/> to use for instantiating this class.
        /// </summary>
        /// <returns>The <see cref="ToolSettings"/>.</returns>
        static ToolSettings CreateSettings()
        {
            return new ToolSettings("Map Walls Drawer")
            {
                ToolBarVisibility = ToolBarVisibility.Map,
                MapDrawingExtensions = new IMapDrawingExtension[] { new MapGrhBoundWallsDrawingExtension() },
                OnToolBarByDefault = true,
                ToolBarControlType = ToolBarControlType.SplitButton,
                EnabledImage = Resources.MapWallsDrawerTool_Enabled,
                DisabledImage = Resources.MapWallsDrawerTool_Disabled
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGridDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapWallsDrawerTool(ToolManager toolManager)
            : base(toolManager, CreateSettings())
        {
            var s = ToolBarControl.ControlSettings.AsSplitButtonSettings();

            s.ToolTipText = "Toggles the display of the walls on the map";
            s.ClickToEnable = true;
        }

        class MapGrhBoundWallsDrawingExtension : MapDrawingExtension
        {
            /// <summary>
            /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
            /// </summary>
            /// <param name="map">The map the drawing is taking place on.</param>
            /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
            /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
            protected override void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
            {
                var clientMap = map as Map;
                if (clientMap == null)
                    return;

                foreach (var mg in clientMap.MapGrhs)
                {
                    if (!camera.InView(mg.Grh, mg.Position))
                        continue;

                    var boundWalls = GlobalState.Instance.MapGrhWalls[mg.Grh.GrhData];
                    if (boundWalls == null)
                        continue;

                    foreach (var wall in boundWalls)
                    {
                        EntityDrawer.Draw(spriteBatch, camera, wall, mg.Position);
                    }
                }
            }
        }
    }
}