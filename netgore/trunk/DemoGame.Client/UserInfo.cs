using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Client
{
    /// <summary>
    /// Contains information for the User's Character.
    /// </summary>
    public class UserInfo
    {
        readonly CharacterStats _baseStats = new CharacterStats(StatCollectionType.Base);
        readonly UserEquipped _equipped = new UserEquipped();
        readonly CharacterStats _modStats = new CharacterStats(StatCollectionType.Modified);
        readonly Inventory _inventory;
        readonly ClientSockets _socket;

        public SPValueType HP { get; set; }
        public SPValueType MP { get; set; }

        public byte HPPercent
        {
            get
            {
                var max = _modStats[StatType.MaxHP];
                if (max == 0)
                    return 100;
                return (byte)((HP / max) * 100);
            }
        }

        public byte MPPercent { get 
        { 
            var max =  _modStats[StatType.MaxMP];
            if (max == 0)
                return 100;
            return (byte)((MP / max) * 100); 
        } }

        public UserInfo(ClientSockets socket)
        {
            if (socket == null)
                throw new ArgumentNullException("socket");

            _socket = socket;
            _inventory = new Inventory(socket);
        }

        public Inventory Inventory
        {
            get { return _inventory; }
        }

        public CharacterStats BaseStats
        {
            get { return _baseStats; }
        }

        public UserEquipped Equipped
        {
            get { return _equipped; }
        }

        public CharacterStats ModStats
        {
            get { return _modStats; }
        }
    }
}
