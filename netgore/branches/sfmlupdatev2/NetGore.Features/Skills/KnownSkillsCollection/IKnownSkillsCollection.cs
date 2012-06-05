using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.World;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// An interface for a collection of skills known by an <see cref="Entity"/>.
    /// </summary>
    /// <typeparam name="T">The type of skills.</typeparam>
    public interface IKnownSkillsCollection<T> : IEnumerable<KeyValuePair<T, bool>>
        where T : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Gets the collection of known skills.
        /// </summary>
        IEnumerable<T> KnownSkills { get; }

        /// <summary>
        /// Gets the collection of unknown skills.
        /// </summary>
        IEnumerable<T> UnknownSkills { get; }

        /// <summary>
        /// Gets if a skill is known.
        /// </summary>
        /// <param name="skill">The skill to check if known.</param>
        /// <returns>True if the skill is known; otherwise false.</returns>
        bool Knows(T skill);

        /// <summary>
        /// Sets all skills to either known or unknown.
        /// </summary>
        /// <param name="value">True to set all skills to known; false to set all to unknown.</param>
        void SetAll(bool value);

        /// <summary>
        /// Sets the known status for a skill.
        /// </summary>
        /// <param name="skill">The skill to set the status for.</param>
        /// <param name="value">True if the skill is to be set as known; false to be set as unknown.</param>
        void SetSkill(T skill, bool value);

        /// <summary>
        /// Explicitly sets which skills are known.
        /// </summary>
        /// <param name="knownSkills">The skills to set as known. Any skill not in this collection will be set to unknown.</param>
        void SetValues(IEnumerable<T> knownSkills);
    }
}