using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// An attribute that describes some additional information for status effects.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StatusEffectInfoAttribute : Attribute
    {
        readonly string _description;
        readonly string _displayName;
        readonly GrhIndex _icon;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEffectInfoAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The name to display for the status effect.</param>
        /// <param name="description">The description of the status effect.</param>
        /// <param name="iconGrhIndex">The icon to display for the status effect.</param>
        public StatusEffectInfoAttribute(string displayName, string description, int iconGrhIndex)
        {
            _displayName = displayName;
            _description = description;
            _icon = new GrhIndex(iconGrhIndex);
        }

        /// <summary>
        /// Gets the description of the status effect.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the name to display for the status effect.
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
        }

        /// <summary>
        /// Gets the icon to display for the status effect.
        /// </summary>
        public GrhIndex Icon
        {
            get { return _icon; }
        }

        /// <summary>
        /// Gets the actual enum value that this <see cref="StatusEffectInfoAttribute"/> was attached to.
        /// </summary>
        public StatusEffectType Value { get; internal set; }
    }
}