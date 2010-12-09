using System;
using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// <see cref="EventArgs"/> for when an item in the <see cref="KnownSkillsCollection"/> changes.
    /// </summary>
    public class KnowSkillsCollectionChangedEventArgs : EventArgs
    {
        readonly SkillType _skill;
        readonly bool _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnowSkillsCollectionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="skill">The skill who's known state has changed.</param>
        /// <param name="value">The current known state (true if the skill was learned, false if it was forgotten).</param>
        public KnowSkillsCollectionChangedEventArgs(SkillType skill, bool value)
        {
            _skill = skill;
            _value = value;
        }

        /// <summary>
        /// Gets the skill who's known state has changed.
        /// </summary>
        public SkillType Skill
        {
            get { return _skill; }
        }

        /// <summary>
        /// Gets the current known state (true if the skill was learned, false if it was forgotten).
        /// </summary>
        public bool Value
        {
            get { return _value; }
        }
    }
}