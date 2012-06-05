using System.Linq;

namespace DemoGame.Server
{
    public class UserSPSynchronizer : CharacterSPSynchronizer
    {
        readonly User _user;
        SPValueType _lastSentHP;
        SPValueType _lastSentMP;

        public UserSPSynchronizer(User user) : base(user)
        {
            _user = user;
        }

        public override void Synchronize()
        {
            base.Synchronize();
            SynchronizeSelf();
        }

        void SynchronizeSelf()
        {
            var hp = Character.HP;
            var mp = Character.MP;

            var updateHP = hp != _lastSentHP;
            var updateMP = mp != _lastSentMP;

            if (updateHP)
            {
                using (var pw = ServerPacket.SetHP(hp))
                {
                    _user.Send(pw, ServerMessageType.GUIUserStats);
                }
            }

            if (updateMP)
            {
                using (var pw = ServerPacket.SetMP(mp))
                {
                    _user.Send(pw, ServerMessageType.GUIUserStats);
                }
            }

            _lastSentHP = hp;
            _lastSentMP = mp;
        }
    }
}