using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains the <see cref="SkillInfoAttribute"/> for the appropriate <see cref="SkillType"/>.
    /// </summary>
    public sealed class SkillInfoManager : EnumFieldAttributeManager<SkillType, SkillInfoAttribute>
    {
        static readonly SkillInfoManager _instance;

        /// <summary>
        /// Initializes the <see cref="SkillInfoManager"/> class.
        /// </summary>
        static SkillInfoManager()
        {
            _instance = new SkillInfoManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoManager"/> class.
        /// </summary>
        SkillInfoManager() : base(true)
        {
        }

        /// <summary>
        /// Gets the <see cref="SkillInfoManager"/> instance.
        /// </summary>
        public static SkillInfoManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Allows for a chance to provide additional handling of when an attribute is loaded for an enum value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void HandleLoadAttribute(SkillType enumValue, SkillInfoAttribute attribute)
        {
            attribute.Value = enumValue;

            base.HandleLoadAttribute(enumValue, attribute);
        }
    }
}