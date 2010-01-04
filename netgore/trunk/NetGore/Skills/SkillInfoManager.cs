using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;

namespace DemoGame
{
    public class SkillInfoManager<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly SkillInfoManager<T> _instance;

        /// <summary>
        /// Gets the <see cref="SkillInfoManager&lt;T&gt;"/> instance.
        /// </summary>
        public static SkillInfoManager<T> Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="SkillInfoManager&lt;T&gt;"/> class.
        /// </summary>
        static SkillInfoManager()
        {
            _instance = new SkillInfoManager<T>();
        }

        readonly Dictionary<T, SkillInfoAttribute> _skillInfos = new Dictionary<T, SkillInfoAttribute>(EnumComparer<T>.Instance);

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoManager&lt;T&gt;"/> class.
        /// </summary>
        SkillInfoManager()
        {
            var fields = typeof(T).GetFields().Where(x => !x.IsSpecialName);
            foreach (var field in fields)
            {
                T value = (T)field.GetValue(null);

                var attribute = field.GetCustomAttributes(typeof(SkillInfoAttribute), true).OfType<SkillInfoAttribute>().FirstOrDefault();
                if (attribute == null)
                {
                    const string errmsg = "Enum `{0}`'s value `{1}` does not contain the required SkillInfoAttribute.";
                    string err = string.Format(errmsg, typeof(T).Name, field.Name);
                    log.Fatal(err);
                    throw new Exception(err);
                }

                attribute.Value = value;
                _skillInfos.Add(value, attribute);
            }
        }

        /// <summary>
        /// Gets the <see cref="SkillInfoAttribute"/> for the given <paramref name="skillType"/>.
        /// </summary>
        /// <param name="skillType">The skill type.</param>
        /// <returns>The <see cref="SkillInfoAttribute"/> for the given <paramref name="skillType"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="skillType"/> is not a valid, defined value of
        /// enum type <typeparamref name="T"/>.</exception>
        public SkillInfoAttribute GetSkillInfo(T skillType)
        {
            return _skillInfos[skillType];
        }
    }
}