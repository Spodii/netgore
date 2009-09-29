using System.Linq;

namespace NetGore.Graphics
{
    public sealed class BackgroundLayerLayoutHelper : EnumHelper<BackgroundLayerLayout>
    {
        static readonly BackgroundLayerLayoutHelper _instance;

        /// <summary>
        /// Gets the <see cref="BackgroundLayerLayoutHelper"/> instance.
        /// </summary>
        public static BackgroundLayerLayoutHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes the <see cref="BackgroundLayerLayoutHelper"/> class.
        /// </summary>
        static BackgroundLayerLayoutHelper()
        {
            _instance = new BackgroundLayerLayoutHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundLayerLayoutHelper"/> class.
        /// </summary>
        BackgroundLayerLayoutHelper()
        {
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="BackgroundLayerLayout"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="BackgroundLayerLayout"/>.</returns>
        protected override BackgroundLayerLayout FromInt(int value)
        {
            return (BackgroundLayerLayout)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="BackgroundLayerLayout"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected override int ToInt(BackgroundLayerLayout value)
        {
            return (int)value;
        }
    }
}