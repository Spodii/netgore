using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using DemoGame.Editor.Properties;
using NetGore;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;
using Image = System.Drawing.Image;

namespace DemoGame.Editor
{
    /// <summary>
    /// Creates a preview of a <see cref="Map"/>.
    /// </summary>
    public class MapPreviewer : MapPreviewerBase<Map>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPreviewer"/> class.
        /// </summary>
        public MapPreviewer()
        {
            TextureSize = EditorSettings.Default.MapPreview_TextureSize;
        }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="mapID">The ID of the map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        /// <param name="filePath">The file path to save the created preview to.</param>
        public void CreatePreview(MapID mapID, ICollection<IMapDrawingExtension> drawExtensions, string filePath)
        {
            using (var map = CreateTempMap(mapID))
            {
                CreatePreview(map, drawExtensions, filePath);
            }
        }

        /// <summary>
        /// Creates the preview of a map.
        /// </summary>
        /// <param name="mapID">The ID of the map to create the preview of.</param>
        /// <param name="drawExtensions">The collection of <see cref="IMapDrawingExtension"/>s applied to the map.</param>
        public Image CreatePreview(MapID mapID, ICollection<IMapDrawingExtension> drawExtensions)
        {
            using (var map = CreateTempMap(mapID))
            {
                return CreatePreview(map, drawExtensions);
            }
        }

        /// <summary>
        /// Creates a temporary <see cref="Map"/>.
        /// </summary>
        /// <param name="mapID">The <see cref="MapID"/> of the map.</param>
        /// <returns>The temporary <see cref="Map"/>.</returns>
        static Map CreateTempMap(MapID mapID)
        {
            var camera = new Camera2D(new Vector2(800, 600));
            var map = new Map(mapID, camera, GetTimeDummy.Instance);
            map.Load(ContentPaths.Dev, false, EditorDynamicEntityFactory.Instance);
            return map;
        }

        /// <summary>
        /// When overridden in the derived class, sets the given values on the map.
        /// </summary>
        /// <param name="map">The map to set the values on.</param>
        /// <param name="camera">The camera to use.</param>
        /// <param name="drawFilter">The draw filter to use.</param>
        /// <param name="drawParticles">The draw particles value to use.</param>
        protected override void SetMapValues(Map map, ICamera2D camera, Func<IDrawable, bool> drawFilter, bool drawParticles)
        {
            map.Camera = camera;
            map.DrawFilter = drawFilter;
            map.DrawParticles = drawParticles;
        }
    }
}