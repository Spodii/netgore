using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains the <see cref="StatusEffectInfoManager"/> for the appropriate <see cref="SkillType"/>.
    /// </summary>
    public sealed class StatusEffectInfoManager : EnumFieldAttributeManager<StatusEffectType, StatusEffectInfoAttribute>
    {
        static readonly StatusEffectInfoManager _instance;

        /// <summary>
        /// Initializes the <see cref="StatusEffectInfoManager"/> class.
        /// </summary>
        static StatusEffectInfoManager()
        {
            _instance = new StatusEffectInfoManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectInfoManager"/> class.
        /// </summary>
        StatusEffectInfoManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="StatusEffectInfoManager"/> instance.
        /// </summary>
        public static StatusEffectInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Allows for a chance to provide additional handling of when an attribute is loaded for an enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void HandleLoadAttribute(StatusEffectType enumValue, StatusEffectInfoAttribute attribute)
        {
            attribute.Value = enumValue;

            base.HandleLoadAttribute(enumValue, attribute);
        }
    }
}