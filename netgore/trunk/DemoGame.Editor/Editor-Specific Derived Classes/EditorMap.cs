using System.Linq;
using DemoGame.Client;
using NetGore;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Editor
{
    /// <summary>
    /// Implementation of <see cref="Map"/> specifically for the editor.
    /// </summary>
    public class EditorMap : Map
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorMap"/> class.
        /// </summary>
        /// <param name="mapID">The ID of the map.</param>
        /// <param name="camera">The camera used to view the map.</param>
        /// <param name="getTime">The object used to get the current time.</param>
        public EditorMap(MapID mapID, ICamera2D camera, IGetTime getTime) : base(mapID, camera, getTime)
        {
        }

        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);

            // Ensure the filter is set
            DrawFilter = GlobalState.Instance.Map.MapDrawFilter;
        }
    }
}