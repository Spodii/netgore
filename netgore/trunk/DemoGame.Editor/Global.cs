using System.Linq;
using NetGore.Graphics;

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
            /// <summary>
            /// Gets or sets the <see cref="Grh"/> that has been selected to be placed on the map.
            /// </summary>
            public Grh GrhToPlace { get; set; }
        }
    }
}