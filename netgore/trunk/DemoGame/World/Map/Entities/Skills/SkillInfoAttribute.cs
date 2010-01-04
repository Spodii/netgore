using System;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// An attribute that describes some additional information for skills.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class SkillInfoAttribute : Attribute
    {
        readonly string _description;
        readonly string _displayName;
        readonly byte _group;
        readonly GrhIndex _icon;

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

        /// <summary>
        /// Gets the cooldown group of skills the skill belongs to.
        /// </summary>
        public byte CooldownGroup
        {
            get { return _group; }
        }

        /// <summary>
        /// Gets the description of the skill.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the name to display for the skill.
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
        }

        /// <summary>
        /// Gets the icon to display for the skill.
        /// </summary>
        public GrhIndex Icon
        {
            get { return _icon; }
        }

        /// <summary>
        /// Gets the actual enum value that this <see cref="SkillInfoAttribute"/> was attached to.
        /// </summary>
        public SkillType Value { get; internal set; }
    }
}