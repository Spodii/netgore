using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Features.Skills
{
    /// <summary>
    /// A general-purpose implementation of the <see cref="IKnownSkillsCollection{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of skills.</typeparam>
    public class KnownSkillsCollection<T> : IKnownSkillsCollection<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        readonly IEnumTable<T, bool> _collection = EnumTable.Create<T, bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownSkillsCollection{T}"/> class.
        /// </summary>
        public KnownSkillsCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownSkillsCollection{T}"/> class.
        /// </summary>
        /// <param name="initialSkills">The initially known skills.</param>
        public KnownSkillsCollection(IEnumerable<T> initialSkills)
        {
            if (initialSkills != null)
            {
                foreach (var skill in initialSkills)
                {
                    _collection.TrySetValue(skill, true);
                }
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of when the known state of a skill changes.
        /// This is not raised for skills passed to the object's constructor.
        /// </summary>
        /// <param name="skill">The skill who's known state has changed.</param>
        /// <param name="value">The current known state (true if the skill was learned, false if it was forgotten).</param>
        protected virtual void OnKnowSkillChanged(T skill, bool value)
        {
        }

        #region IKnownSkillsCollection<T> Members

        /// <summary>
        /// Gets the collection of known skills.
        /// </summary>
        public IEnumerable<T> KnownSkills
        {
            get { return _collection.Where(x => x.Value).Select(x => x.Key); }
        }

        /// <summary>
        /// Gets the collection of unknown skills.
        /// </summary>
        public IEnumerable<T> UnknownSkills
        {
            get { return _collection.Where(x => !x.Value).Select(x => x.Key); }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets if a skill is known.
        /// </summary>
        /// <param name="skill">The skill to check if known.</param>
        /// <returns>True if the skill is known; otherwise false.</returns>
        public bool Knows(T skill)
        {
            bool ret;
            if (!_collection.TryGetValue(skill, out ret))
                return false;

            return ret;
        }

        /// <summary>
        /// Sets all skills to either known or unknown.
        /// </summary>
        /// <param name="value">True to set all skills to known; false to set all to unknown.</param>
        public void SetAll(bool value)
        {
            var toChange = _collection.Where(x => x.Value != value).Select(x => x.Key).ToArray();

            for (var i = 0; i < toChange.Length; i++)
            {
                var s = toChange[i];
                Debug.Assert(Knows(s) != value);
                SetSkill(s, value);
                Debug.Assert(Knows(s) == value);
            }
        }

        /// <summary>
        /// Sets the known status for a skill.
        /// </summary>
        /// <param name="skill">The skill to set the status for.</param>
        /// <param name="value">True if the skill is to be set as known; false to be set as unknown.</param>
        public void SetSkill(T skill, bool value)
        {
            _collection.TrySetValue(skill, value);
            OnKnowSkillChanged(skill, value);  
        }

        /// <summary>
        /// Explicitly sets which skills are known.
        /// </summary>
        /// <param name="knownSkills">The skills to set as known. Any skill not in this collection will be set to unknown.</param>
        public void SetValues(IEnumerable<T> knownSkills)
        {
            _collection.Clear();

            if (knownSkills != null)
            {
                foreach (var ks in knownSkills)
                {
                    Debug.Assert(EnumHelper<T>.IsDefined(ks));
                    _collection.TrySetValue(ks, true);
                }
            }
        }

        #endregion
    }
}