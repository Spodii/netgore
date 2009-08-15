using System;
using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains information for the User's Character.
    /// </summary>
    public class UserInfo
    {
        readonly CharacterStats _baseStats = new CharacterStats(StatCollectionType.Base);
        readonly UserEquipped _equipped = new UserEquipped();
        readonly Inventory _inventory;
        readonly CharacterStats _modStats = new CharacterStats(StatCollectionType.Modified);

        public CharacterStats BaseStats
        {
            get { return _baseStats; }
        }

        public uint Cash { get; set; }

        public UserEquipped Equipped
        {
            get { return _equipped; }
        }

        public uint Exp { get; set; }

        public SPValueType HP { get; set; }

        public byte HPPercent
        {
            get
            {
                int max = _modStats[StatType.MaxHP];
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
                int max = _modStats[StatType.MaxMP];
                if (max == 0)
                    return 100;

                return (byte)((MP / (float)max) * 100.0f).Clamp(0, 100);
            }
        }

        public uint StatPoints { get; set; }

        public UserInfo(ISocketSender socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _inventory = new Inventory(socket);
        }
    }
}