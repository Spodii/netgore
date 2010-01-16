using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public class PolygonOriginHelper : EnumIOHelper<PolygonOrigin>
    {
        static readonly PolygonOriginHelper _instance;

        /// <summary>
        /// Initializes the <see cref="PolygonOriginHelper"/> class.
        /// </summary>
        static PolygonOriginHelper()
        {
            _instance = new PolygonOriginHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonOriginHelper"/> class.
        /// </summary>
        PolygonOriginHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="PolygonOriginHelper"/> instance.
        /// </summary>
        public static PolygonOriginHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="PolygonOrigin"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="PolygonOrigin"/>.</returns>
        public override PolygonOrigin FromInt(int value)
        {
            return (PolygonOrigin)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="PolygonOrigin"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        public override int ToInt(PolygonOrigin value)
        {
            return (int)value;
        }
    }
}