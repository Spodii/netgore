using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    public class StatusEffectInfoManager<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly StatusEffectInfoManager<T> _instance;

        /// <summary>
        /// Gets the <see cref="StatusEffectInfoManager&lt;T&gt;"/> instance.
        /// </summary>
        public static StatusEffectInfoManager<T> Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="StatusEffectInfoManager&lt;T&gt;"/> class.
        /// </summary>
        static StatusEffectInfoManager()
        {
            _instance = new StatusEffectInfoManager<T>();
        }

        readonly Dictionary<T, StatusEffectInfoAttribute> _statusEffectInfos = new Dictionary<T, StatusEffectInfoAttribute>(EnumComparer<T>.Instance);
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoManager&lt;T&gt;"/> class.
        /// </summary>
        StatusEffectInfoManager()
        {
            var fields = typeof(T).GetFields().Where(x => !x.IsSpecialName);
            foreach (var field in fields)
            {
                T value = (T)field.GetValue(null);

                var attribute = field.GetCustomAttributes(typeof(StatusEffectInfoAttribute), true).OfType<StatusEffectInfoAttribute>().FirstOrDefault();
                if (attribute == null)
                {
                    const string errmsg = "Enum `{0}`'s value `{1}` does not contain the required StatusEffectInfoAttribute.";
                    string err = string.Format(errmsg, typeof(T).Name, field.Name);
                    log.Fatal(err);
                    throw new Exception(err);
                }

                attribute.Value = value;
                _statusEffectInfos.Add(value, attribute);
            }
        }

        /// <summary>
        /// Gets the <see cref="SkillInfoAttribute"/> for the given <paramref name="skillType"/>.
        /// </summary>
        /// <param name="skillType">The skill type.</param>
        /// <returns>The <see cref="SkillInfoAttribute"/> for the given <paramref name="skillType"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="skillType"/> is not a valid, defined value of
        /// enum type <typeparamref name="T"/>.</exception>
        public StatusEffectInfoAttribute GetStatusEffectInfo(T skillType)
        {
            return _statusEffectInfos[skillType];
        }
    }
}