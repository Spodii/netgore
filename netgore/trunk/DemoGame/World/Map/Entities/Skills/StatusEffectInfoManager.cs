using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Contains the <see cref="StatusEffectInfoManager"/> for the appropriate <see cref="SkillType"/>.
    /// </summary>
    public class StatusEffectInfoManager : EnumFieldAttributeManager<StatusEffectType, StatusEffectInfoAttribute>
    {
        static readonly StatusEffectInfoManager _instance;

        /// <summary>
        /// Gets the <see cref="StatusEffectInfoManager"/> instance.
        /// </summary>
        public static StatusEffectInfoManager Instance { get { return _instance; } }

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
        StatusEffectInfoManager()
            : base(true)
        {
        }
    }
}