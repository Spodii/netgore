using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles synchronizing the Character's SP values to the client(s).
    /// </summary>
    public class CharacterSPSynchronizer
    {
        readonly Character _character;
        byte _lastSentHPPercent;
        byte _lastSentMPPercent;
        readonly bool _isUser;

        /// <summary>
        /// Gets the Character that this CharacterSPSynchronizer is for.
        /// </summary>
        public Character Character { get { return _character; } }

        /// <summary>
        /// CharacterSPSynchronizer constructor.
        /// </summary>
        /// <param name="character">The Character to synchronize the values of.</param>
        public CharacterSPSynchronizer(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _isUser = (_character is User);
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void SynchronizePercentage()
        {
            const int _updatePercentDiff = 2;

            // Check if the percentage has changed
            var maxHP = _character.ModStats[StatType.MaxHP];
            var maxMP = _character.ModStats[StatType.MaxMP];

            if (maxHP < 1 || maxMP < 1)
            {
                const string errmsg = "MaxHP or MaxMP is less than 1 for Character `{0}`!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _character);
                Debug.Fail(string.Format(errmsg, _character));
                return;
            }

            byte newHPPercent = (byte)((_character.HP / maxHP) * 100);
            byte newMPPercent = (byte)((_character.MP / maxMP) * 100);

            bool updateHP = Math.Abs(newHPPercent - _lastSentHPPercent) >= _updatePercentDiff;
            bool updateMP = Math.Abs(newMPPercent - _lastSentMPPercent) >= _updatePercentDiff;

            if (!updateHP && !updateMP)
                return;

            _lastSentHPPercent = newHPPercent;
            _lastSentMPPercent = newMPPercent;

            // Get the map
            var map = _character.Map;
            if (map == null)
                return;

            // Get the users to send the update to (excluding this character)
            var users = map.Users;
            if (_isUser)
                users = users.Where(x => x != _character);
            if (users.Count() == 0)
                return;

            // Send the updates
            using (var pw = ServerPacket.GetWriter())
            {
                if (updateHP)
                {
                    pw.Reset();
                    ServerPacket.SetCharacterHPPercent(pw, _character.MapEntityIndex, newHPPercent);
                    foreach (var user in users)
                        user.Send(pw);
                }
                if (updateMP)
                {
                    pw.Reset();
                    ServerPacket.SetCharacterMPPercent(pw, _character.MapEntityIndex, newMPPercent);
                    foreach (var user in users)
                        user.Send(pw);
                }
            }
        }

        /// <summary>
        /// Performs any synchronization needed.
        /// </summary>
        public virtual void Synchronize()
        {
            SynchronizePercentage();
        }
    }
}
