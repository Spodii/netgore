using System.Linq;
using NetGore;
using NetGore.Features.Skills;

namespace DemoGame.Client
{
    public class KnownSkillsCollection : KnownSkillsCollection<SkillType>
    {
        /// <summary>
        /// Notifies listeners when a known skill has changed.
        /// </summary>
        public event TypedEventHandler<KnownSkillsCollection, KnowSkillsCollectionChangedEventArgs> KnowSkillChanged;

        /// <summary>
        /// When overridden in the derived class, allows for handling of when the known state of a skill changes.
        /// This is not raised for skills passed to the object's constructor.
        /// </summary>
        /// <param name="skill">The skill who's known state has changed.</param>
        /// <param name="value">The current known state (true if the skill was learned, false if it was forgotten).</param>
        protected override void OnKnowSkillChanged(SkillType skill, bool value)
        {
            base.OnKnowSkillChanged(skill, value);

            // Raise event
            if (KnowSkillChanged != null)
                KnowSkillChanged.Raise(this, new KnowSkillsCollectionChangedEventArgs(skill, value));
        }
    }
}