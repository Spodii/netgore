using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// An attribute that describes some additional information for skills.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SkillInfoAttribute : Attribute
    {
        readonly string _displayName;
        readonly string _description;
        readonly GrhIndex _icon;
        readonly byte _group;
       
        /// <summary>
        /// Gets the actual enum value that this <see cref="SkillInfoAttribute"/> was attached to.
        /// </summary>
        public object Value { get; internal set; }

        /// <summary>
        /// Gets the name to display for the skill.
        /// </summary>
        public string DisplayName { get { return _displayName; } }

        /// <summary>
        /// Gets the description of the skill.
        /// </summary>
        public string Description { get { return _description; } }

        /// <summary>
        /// Gets the icon to display for the skill.
        /// </summary>
        public GrhIndex Icon { get { return _icon; } }

        /// <summary>
        /// Gets the cooldown group of skills the skill belongs to.
        /// </summary>
        public byte CooldownGroup { get { return _group; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillInfoAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The name to display for the skill.</param>
        /// <param name="description">The description of the skill.</param>
        /// <param name="iconGrhIndex">The icon to display for the skill.</param>
        /// <param name="cooldownGroup">The cooldown group of skills the skill belongs to.</param>
        public SkillInfoAttribute(string displayName, string description, int iconGrhIndex, byte cooldownGroup)
        {
            _displayName = displayName;
            _description = description;
            _icon = new GrhIndex(iconGrhIndex);
            _group = cooldownGroup;
        }
    }

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
