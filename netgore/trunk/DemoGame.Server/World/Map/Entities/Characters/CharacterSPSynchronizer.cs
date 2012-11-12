using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NetGore;
using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// Handles synchronizing the Character's SP values to the client(s).
    /// </summary>
    public class CharacterSPSynchronizer
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Character _character;
        readonly bool _isUser;

        byte _lastSentHPPercent;
        byte _lastSentMPPercent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSPSynchronizer"/> class.
        /// </summary>
        /// <param name="character">The Character to synchronize the values of.</param>
        /// <exception cref="ArgumentNullException"><paramref name="character" /> is <c>null</c>.</exception>
        public CharacterSPSynchronizer(Character character)
        {
            if (character == null)
                throw new ArgumentNullException("character");

            _character = character;
            _isUser = (_character is User);
        }

        /// <summary>
        /// Gets the Character that this CharacterSPSynchronizer is for.
        /// </summary>
        public Character Character
        {
            get { return _character; }
        }

        /// <summary>
        /// Forces synchronization.
        /// </summary>
        public virtual void ForceSynchronize()
        {
            // Since the percent will never be at this value, this will force a synchronization on next Synchronize call
            _lastSentHPPercent = byte.MaxValue;
            _lastSentMPPercent = byte.MaxValue;
        }

        public virtual void ForceSynchronizeTo(User user)
        {
            using (var pw = ServerPacket.GetWriter())
            {
                ServerPacket.SetCharacterHPPercent(pw, _character.MapEntityIndex, _lastSentHPPercent);
                ServerPacket.SetCharacterMPPercent(pw, _character.MapEntityIndex, _lastSentMPPercent);
                user.Send(pw, ServerMessageType.MapCharacterSP);
            }
        }

        /// <summary>
        /// Performs any synchronization needed.
        /// </summary>
        public virtual void Synchronize()
        {
            SynchronizePercentage();
        }

        protected void SynchronizePercentage()
        {
            const int _updatePercentDiff = 2;

            // Check if the percentage has changed
            int maxHP = _character.ModStats[StatType.MaxHP];
            int maxMP = _character.ModStats[StatType.MaxMP];

            if (maxHP < 1)
            {
                const string errmsg = "MaxHP is less than 1 for Character `{0}`!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, _character);
                Debug.Fail(string.Format(errmsg, _character));
                return;
            }

            var newHPPercent = (byte)(((float)_character.HP / maxHP) * 100.0f);
            byte newMPPercent = 100;
            if (maxMP > 0)
                newMPPercent = (byte)(((float)_character.MP / maxMP) * 100.0f);

            var updateHP = Math.Abs(newHPPercent - _lastSentHPPercent) >= _updatePercentDiff;
            var updateMP = Math.Abs(newMPPercent - _lastSentMPPercent) >= _updatePercentDiff;

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
            if (users.IsEmpty())
                return;

            // Send the updates
            using (var pw = ServerPacket.GetWriter())
            {
                if (updateHP)
                {
                    pw.Reset();
                    ServerPacket.SetCharacterHPPercent(pw, _character.MapEntityIndex, newHPPercent);
                    foreach (var user in users)
                    {
                        user.Send(pw, ServerMessageType.MapCharacterSP);
                    }
                }
                if (updateMP)
                {
                    pw.Reset();
                    ServerPacket.SetCharacterMPPercent(pw, _character.MapEntityIndex, newMPPercent);
                    foreach (var user in users)
                    {
                        user.Send(pw, ServerMessageType.MapCharacterSP);
                    }
                }
            }
        }
    }
}