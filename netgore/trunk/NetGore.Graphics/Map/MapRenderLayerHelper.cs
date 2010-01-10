using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    public sealed class MapRenderLayerHelper : EnumIOHelper<MapRenderLayer>
    {
        static readonly MapRenderLayerHelper _instance;

        /// <summary>
        /// Initializes the <see cref="MapRenderLayerHelper"/> class.
        /// </summary>
        static MapRenderLayerHelper()
        {
            _instance = new MapRenderLayerHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRenderLayerHelper"/> class.
        /// </summary>
        MapRenderLayerHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayerHelper"/> instance.
        /// </summary>
        public static MapRenderLayerHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="MapRenderLayer"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="MapRenderLayer"/>.</returns>
        protected override MapRenderLayer FromInt(int value)
        {
            return (MapRenderLayer)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="MapRenderLayer"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(MapRenderLayer value)
        {
            return (int)value;
        }
    }
}