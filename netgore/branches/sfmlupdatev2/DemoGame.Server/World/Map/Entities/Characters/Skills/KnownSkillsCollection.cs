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
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        public KnownSkillsCollection(Character owner) : base(GetKnownSkillsFromDb(owner))
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            _owner = owner;

            // Send the initial collection of known skills
            SendAllKnownSkills();
        }

        /// <summary>
        /// Gets the <see cref="Character"/> that this collection is for.
        /// </summary>
        public Character Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Gets the known skills for a <see cref="Character"/> from the database.
        /// </summary>
        /// <param name="owner">The <see cref="Character"/> to get the known skills for.</param>
        /// <returns>The known skills for the <paramref name="owner"/> from the database.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        static IEnumerable<SkillType> GetKnownSkillsFromDb(Character owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            // Only grab the known skills for a persistent character
            if (!owner.IsPersistent)
                return Enumerable.Empty<SkillType>();

            // Create the query
            var q = owner.DbController.GetQuery<SelectCharacterSkillsQuery>();
            var ret = q.Execute(owner.ID);

            return ret;
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

        /// <summary>
        /// Sends the complete collection of known skills to the <see cref="Owner"/>, if possible.
        /// </summary>
        void SendAllKnownSkills()
        {
            var netSender = Owner as INetworkSender;
            if (netSender == null)
                return;

            using (var pw = ServerPacket.SkillSetKnownAll(KnownSkills))
            {
                netSender.Send(pw, ServerMessageType.GUIUserStats);
            }
        }
    }
}