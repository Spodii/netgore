using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Features.Skills;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// A collection of skills that a <see cref="Character"/> knows.
    /// </summary>
    public class KnownSkillsCollection : KnownSkillsCollection<SkillType>
    {
        readonly Character _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownSkillsCollection"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="initialSkills">The initially known skills.</param>
        public KnownSkillsCollection(Character owner, IEnumerable<SkillType> initialSkills) : base(initialSkills)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;
        }

        /// <summary>
        /// Gets the <see cref="Character"/> that this collection is for.
        /// </summary>
        public Character Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling of when the known state of a skill changes.
        /// This is not raised for skills passed to the object's constructor.
        /// </summary>
        /// <param name="skill">The skill who's known state has changed.</param>
        /// <param name="value">The current known state (true if the skill was learned, false if it was forgotten).</param>
        protected override void OnKnowSkillChanged(SkillType skill, bool value)
        {
            base.OnKnowSkillChanged(skill, value);

            // Send update to client
            var netSender = Owner as INetworkSender;
            if (netSender != null)
            {
                using (var pw = ServerPacket.SkillSetKnown(skill, value))
                {
                    netSender.Send(pw, ServerMessageType.GUIUserStats);
                }
            }

            // For persistable characters, persist the skill's new state
            if (Owner.IsPersistent)
            {
                var kvp = new KeyValuePair<CharacterID, SkillType>(Owner.ID, skill);

                if (value)
                {
                    // Change to known
                    Owner.DbController.GetQuery<InsertCharacterSkillQuery>().Execute(kvp);
                }
                else
                {
                    // Change to unknown
                    Owner.DbController.GetQuery<DeleteCharacterSkillQuery>().Execute(kvp);
                }
            }
        }
    }
}