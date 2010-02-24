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
        readonly CharacterStats _baseStats = new CharacterStats(StatCollectionType.Base);
        readonly UserEquipped _equipped = new UserEquipped();
        readonly UserGroupInformation _groupInfo = new UserGroupInformation();
        readonly UserGuildInformation _guildInfo = new UserGuildInformation();
        readonly HasQuestRequirementsTracker _hasStartQuestRequirements;
        readonly Inventory _inventory;
        readonly CharacterStats _modStats = new CharacterStats(StatCollectionType.Modified);
        readonly UserQuestInformation _questInfo = new UserQuestInformation();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="socket">The socket.</param>
        public UserInfo(ISocketSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _inventory = new Inventory(socket);

            _hasStartQuestRequirements = new HasQuestRequirementsTracker(delegate(QuestID x)
            {
                using (var pw = ClientPacket.HasQuestStartRequirements(x))
                {
                    socket.Send(pw);
                }
            });
        }

        public CharacterStats BaseStats
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

        public HasQuestRequirementsTracker HasStartQuestRequirements
        {
            get { return _hasStartQuestRequirements; }
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

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        public byte Level { get; set; }

        public CharacterStats ModStats
        {
            get { return _modStats; }
        }

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

        public UserQuestInformation QuestInfo
        {
            get { return _questInfo; }
        }

        public int StatPoints { get; set; }
    }
}