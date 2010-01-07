using System.Linq;
using DemoGame.Emoticons;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains the <see cref="EmoticonInfoAttribute"/> for the appropriate <see cref="Emoticon"/>.
    /// </summary>
    public sealed class EmoticonInfoManager : EnumFieldAttributeManager<Emoticon, EmoticonInfoAttribute>
    {
        static readonly EmoticonInfoManager _instance;

        /// <summary>
        /// Initializes the <see cref="EmoticonInfoManager"/> class.
        /// </summary>
        static EmoticonInfoManager()
        {
            _instance = new EmoticonInfoManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonInfoManager"/> class.
        /// </summary>
        EmoticonInfoManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="EmoticonInfoManager"/> instance.
        /// </summary>
        public static EmoticonInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Allows for a chance to provide additional handling of when an attribute is loaded for an enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void HandleLoadAttribute(Emoticon enumValue, EmoticonInfoAttribute attribute)
        {
            attribute.Value = enumValue;

            base.HandleLoadAttribute(enumValue, attribute);
        }
    }
}