using System.Collections.Generic;
using System.ComponentModel;
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
        readonly MapUndoManager _undoManager;

        /// <summary>
        /// Gets the MapUndoManager for this map.
        /// </summary>
        [Browsable(false)]
        public MapUndoManager UndoManager { get { return _undoManager; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorMap"/> class.
        /// </summary>
        /// <param name="mapID">The ID of the map.</param>
        /// <param name="camera">The camera used to view the map.</param>
        /// <param name="getTime">The object used to get the current time.</param>
        public EditorMap(MapID mapID, ICamera2D camera, IGetTime getTime) : base(mapID, camera, getTime)
        {
            _undoManager = new MapUndoManager(this, GlobalState.Instance.DynamicEntityFactory);
        }

        /// <summary>
        /// Updates the map
        /// </summary>
        /// <param name="deltaTime">The elapsed time since the last update.</param>
        public override void Update(int deltaTime)
        {
            base.Update(deltaTime);

            // Ensure the filter is set
            DrawFilter = GlobalState.Instance.Map.MapDrawFilter;
        }

        /// <summary>
        /// Gets all objects on the map that implement ISpatial.
        /// </summary>
        public IEnumerable<ISpatial> GetAllSpatials()
        {
            return Spatial.GetMany<ISpatial>()
                .Concat(Lights)
                .Concat(RefractionEffects)
                .Distinct()
                .ToArray();
        }
    }
}