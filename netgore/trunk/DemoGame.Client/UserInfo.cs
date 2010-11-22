using System;
using System.Linq;
using NetGore;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Network;
using NetGore.Stats;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains information for the User's Character.
    /// </summary>
    public class UserInfo
    {
        readonly StatCollection<StatType> _baseStats = new StatCollection<StatType>(StatCollectionType.Base);
        readonly UserEquipped _equipped = new UserEquipped();
        readonly UserGroupInformation _groupInfo = new UserGroupInformation();
        readonly UserGuildInformation _guildInfo = new UserGuildInformation();
        readonly HasQuestRequirementsTracker _hasFinishQuestRequirements;
        readonly HasQuestRequirementsTracker _hasStartQuestRequirements;
        readonly Inventory _inventory;
        readonly StatCollection<StatType> _modStats = new StatCollection<StatType>(StatCollectionType.Modified);
        readonly UserQuestInformation _questInfo;
        readonly INetworkSender _socket;
        readonly KnownSkillsCollection _knownSkills = new KnownSkillsCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public UserInfo(INetworkSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;

            // Create the collections for tracking if quest requirements statuses
            _hasStartQuestRequirements = new HasQuestRequirementsTracker(SendHasQuestStartRequirements);
            _hasFinishQuestRequirements = new HasQuestRequirementsTracker(SendHasQuestFinishRequirements);

            // Create some other stuff
            _inventory = new Inventory(socket);
            _questInfo = new UserQuestInformationExtended(this);
        }

        public StatCollection<StatType> BaseStats
        {
            get { return _baseStats; }
        }

        public int Cash { get; set; }

        public UserEquipped Equipped
        {
            get { return _equipped; }
        }

        public int Exp { get; set; }

        public UserGroupInformation GroupInfo
        {
            get { return _groupInfo; }
        }

        public UserGuildInformation GuildInfo
        {
            get { return _guildInfo; }
        }

        public SPValueType HP { get; set; }

        public byte HPPercent
        {
            get
            {
                var max = _modStats[StatType.MaxHP];
                if (max == 0)
                    return 100;

                return (byte)((HP / (float)max) * 100.0f).Clamp(0, 100);
            }
        }

        public HasQuestRequirementsTracker HasFinishQuestRequirements
        {
            get { return _hasFinishQuestRequirements; }
        }

        public HasQuestRequirementsTracker HasStartQuestRequirements
        {
            get { return _hasStartQuestRequirements; }
        }

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        public byte Level { get; set; }

        public SPValueType MP { get; set; }

        public byte MPPercent
        {
            get
            {
                var max = _modStats[StatType.MaxMP];
                if (max == 0)
                    return 100;

                return (byte)((MP / (float)max) * 100.0f).Clamp(0, 100);
            }
        }

        public KnownSkillsCollection KnownSkills { get { return _knownSkills; } }

        public StatCollection<StatType> ModStats
        {
            get { return _modStats; }
        }

        public UserQuestInformation QuestInfo
        {
            get { return _questInfo; }
        }

        public int StatPoints { get; set; }

        void SendHasQuestFinishRequirements(QuestID questID)
        {
            using (var pw = ClientPacket.HasQuestFinishRequirements(questID))
            {
                _socket.Send(pw, ClientMessageType.GUIQuestStatusRequest);
            }
        }

        void SendHasQuestStartRequirements(QuestID questID)
        {
            using (var pw = ClientPacket.HasQuestStartRequirements(questID))
            {
                _socket.Send(pw, ClientMessageType.GUIQuestStatusRequest);
            }
        }

        /// <summary>
        /// Implementation of the <see cref="UserQuestInformation"/> that is extended to perform some additional
        /// actions when the status of quests changes.
        /// </summary>
        class UserQuestInformationExtended : UserQuestInformation
        {
            readonly UserInfo _userInfo;

            /// <summary>
            /// Initializes a new instance of the <see cref="UserQuestInformationExtended"/> class.
            /// </summary>
            /// <param name="userInfo">The user info.</param>
            public UserQuestInformationExtended(UserInfo userInfo)
            {
                if (userInfo == null)
                    throw new ArgumentNullException("userInfo");

                _userInfo = userInfo;
            }

            UserInfo UserInfo
            {
                get { return _userInfo; }
            }

            /// <summary>
            /// When overridden in the derived class, allows for handling the
            /// <see cref="UserQuestInformation.ActiveQuestAdded"/> event.
            /// </summary>
            /// <param name="questID">The ID of the quest that was added.</param>
            protected override void OnActiveQuestAdded(QuestID questID)
            {
                base.OnActiveQuestAdded(questID);

                UserInfo.HasStartQuestRequirements.SetRequirementsStatus(questID, false);
                UserInfo.HasFinishQuestRequirements.Update(questID);
            }

            /// <summary>
            /// When overridden in the derived class, allows for handling the
            /// <see cref="UserQuestInformation.ActiveQuestRemoved"/> event.
            /// </summary>
            /// <param name="questID">The ID of the quest that was removed.</param>
            protected override void OnActiveQuestRemoved(QuestID questID)
            {
                base.OnActiveQuestRemoved(questID);

                UserInfo.HasStartQuestRequirements.Update(questID);
                UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(questID, false);
            }

            /// <summary>
            /// When overridden in the derived class, allows for handling the
            /// <see cref="UserQuestInformation.CompletedQuestAdded"/> event.
            /// </summary>
            /// <param name="questID">The ID of the quest that was added.</param>
            protected override void OnCompletedQuestAdded(QuestID questID)
            {
                base.OnCompletedQuestAdded(questID);

                UserInfo.HasStartQuestRequirements.Update(questID);
                UserInfo.HasFinishQuestRequirements.SetRequirementsStatus(questID, false);
            }
        }
    }
}