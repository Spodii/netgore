using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore
{
    /// <summary>
    /// Base class for a general implementation of the ISkill.
    /// </summary>
    /// <typeparam name="TSkillType">The type of skill type enum.</typeparam>
    /// <typeparam name="TStatType">The type of stat type enum.</typeparam>
    /// <typeparam name="TCharacter">The type of character.</typeparam>
    public abstract class SkillBase<TSkillType, TStatType, TCharacter> : ISkill<TSkillType, TStatType, TCharacter>
        where TSkillType : struct, IComparable, IConvertible, IFormattable
        where TStatType : struct, IComparable, IConvertible, IFormattable
        where TCharacter : Entity
    {
        readonly TSkillType _skillType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillBase&lt;TSkillType, TStatType, TCharacter&gt;"/> class.
        /// </summary>
        /// <param name="skillType">The type of skill that this object instance is for.</param>
        protected SkillBase(TSkillType skillType)
        {
            _skillType = skillType;
        }

        /// <summary>
        /// Allows for additional processing for after the skill has been successfully used.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        protected virtual void AfterUseSkill(TCharacter user, TCharacter target)
        {
        }

        /// <summary>
        /// Does the CanUse checks to see if the given Characters are in a valid state.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        protected virtual bool CheckValidCanUseCharacters(TCharacter user, TCharacter target)
        {
            return true;
        }

        /// <summary>
        /// When overridden in the derived class, makes the <paramref name="user"/> Character use this skill.
        /// </summary>
        /// <param name="user">The Character that used this skill. Will never be null.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the skill was successfully used; otherwise false.</returns>
        protected abstract bool HandleUse(TCharacter user, TCharacter target);

        #region ISkill<TSkillType,TStatType,TCharacter> Members

        /// <summary>
        /// Gets an IEnumerable of stats required by this SkillBase. Cannot be null.
        /// </summary>
        public virtual IEnumerable<KeyValuePair<TStatType, int>> RequiredStats
        {
            get { return Enumerable.Empty<KeyValuePair<TStatType, int>>(); }
        }

        /// <summary>
        /// When overridden in the derived class, gets if this Skill requires a target to be specified for the skill
        /// to be used. If this is true, the skill will never even attempt to be used unless there is a target.
        /// </summary>
        public abstract bool RequiresTarget { get; }

        /// <summary>
        /// Gets the type of skill that this skill object instance is for.
        /// </summary>
        public TSkillType SkillType
        {
            get { return _skillType; }
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        public bool CanUse(TCharacter user)
        {
            return CanUse(user, null);
        }

        /// <summary>
        /// Uses this Skill without a target.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        public bool Use(TCharacter user)
        {
            return Use(user, null);
        }

        /// <summary>
        /// Checks if the given Character can use this Skill.
        /// </summary>
        /// <param name="user">The Character to check if can use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the <paramref name="user"/> can use this Skill; otherwise false.</returns>
        public virtual bool CanUse(TCharacter user, TCharacter target)
        {
            // State checks
            if (!CheckValidCanUseCharacters(user, target))
                return false;

            // Check for a target
            if (RequiresTarget && target == null)
                return false;

            // Check for the required stats
            if (!HasRequiredStats(user))
                return false;

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, checks if the given <paramref name="character"/> has the required stats
        /// for using this Skill.
        /// </summary>
        /// <param name="character">The Character using the skill. Will not be null.</param>
        /// <returns>True if the <paramref name="character"/> has the required stats to use this skill; otherwise false.</returns>
        public abstract bool HasRequiredStats(TCharacter character);

        /// <summary>
        /// Uses this Skill.
        /// </summary>
        /// <param name="user">User to make use this Skill.</param>
        /// <param name="target">The optional Character that the skill was used on. Can be null if there was
        /// no targeted Character.</param>
        /// <returns>True if the Skill was successfully used; otherwise false.</returns>
        public bool Use(TCharacter user, TCharacter target)
        {
            // Ensure the skill can be used
            if (!CanUse(user, target))
                return false;

            // Use the skill
            bool useSuccessful = HandleUse(user, target);
            if (!useSuccessful)
                return false;

            AfterUseSkill(user, target);

            return true;
        }

        #endregion
    }

}
