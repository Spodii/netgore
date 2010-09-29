using System.Linq;
using NetGore.EditorTools;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Editor
{
    /// <summary>
    /// Describes the global configuration for this editor. This contains state that is shared across multiple parts of the editor and
    /// can be utilized by any part of the editor. When something is specific to a single control instance, it belongs in that
    /// control instance and not here.
    /// </summary>
    public class GlobalConfig
    {
        static readonly GlobalConfig _instance;

        readonly MapConfig _mapConfig;

        /// <summary>
        /// Initializes the <see cref="GlobalConfig"/> class.
        /// </summary>
        static GlobalConfig()
        {
            _instance = new GlobalConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalConfig"/> class.
        /// </summary>
        GlobalConfig()
        {
            _mapConfig = new MapConfig();
        }

        /// <summary>
        /// Gets the <see cref="GlobalConfig"/> instance.
        /// </summary>
        public static GlobalConfig Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="MapConfig"/>.
        /// </summary>
        public MapConfig Map
        {
            get { return _mapConfig; }
        }

        /// <summary>
        /// Describes the configuration related to editing the maps.
        /// </summary>
        public class MapConfig
        {
            readonly SelectedObjectsManager<object> _selectedObjsManager = new SelectedObjectsManager<object>();

            /// <summary>
            /// Initializes a new instance of the <see cref="MapConfig"/> class.
            /// </summary>
            internal MapConfig()
            {
            }

            /// <summary>
            /// Gets or sets the <see cref="Grh"/> that has been selected to be placed on the map.
            /// </summary>
            public Grh GrhToPlace { get; set; }

            /// <summary>
            /// Gets the <see cref="SelectedObjectsManager{T}"/> that contains the currently selected map objects.
            /// </summary>
            public SelectedObjectsManager<object> SelectedObjsManager
            {
                get { return _selectedObjsManager; }
            }
        }
    }
}