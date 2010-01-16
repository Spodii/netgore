using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    public sealed class EmoticonHelper : EnumIOHelper<Emoticon>
    {
        static readonly EmoticonHelper _instance;

        /// <summary>
        /// Initializes the <see cref="EmoticonHelper"/> class.
        /// </summary>
        static EmoticonHelper()
        {
            _instance = new EmoticonHelper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonHelper"/> class.
        /// </summary>
        EmoticonHelper()
        {
        }

        /// <summary>
        /// Gets the <see cref="EmoticonHelper"/> instance.
        /// </summary>
        public static EmoticonHelper Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <see cref="Emoticon"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <see cref="Emoticon"/>.</returns>
        public override Emoticon FromInt(int value)
        {
            return (Emoticon)value;
        }

        /// <summary>
        /// When overridden in the derived class, casts type <see cref="Emoticon"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        public override int ToInt(Emoticon value)
        {
            return (int)value;
        }
    }
}