using System;
using System.Text;
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
}
