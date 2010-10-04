using System.Collections.Generic;
using NetGore.EditorTools;
using NetGore.EditorTools.EditorTool;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="Tool"/> that displays the <see cref="ScreenGrid"/> for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class MapLightSourceDrawerTool : ToggledButtonTool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBorderDrawerTool"/> class.
        /// </summary>
        /// <param name="toolManager">The <see cref="ToolManager"/>.</param>
        protected MapLightSourceDrawerTool(ToolManager toolManager)
            : base(toolManager, "Map Light Source Drawer", ToolBarVisibility.Map)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="IMapDrawingExtension"/>s that are used by this
        /// <see cref="Tool"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="IMapDrawingExtension"/>s used by this <see cref="Tool"/>. Can be null or empty if none
        /// are used. Default is null.
        /// </returns>
        protected override IEnumerable<IMapDrawingExtension> GetMapDrawingExtensions()
        {
            return new IMapDrawingExtension[] { new MapBorderDrawingExtension() };
        }

        class MapBorderDrawingExtension : MapDrawingExtension
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

                var offset = AddLightCursor.LightSprite.Size / 2f;
                foreach (var light in lm)
                {
                    AddLightCursor.LightSprite.Draw(spriteBatch, light.Position - offset);
                }
            }
        }
    }
}