using System.Linq;
using NetGore;
using NetGore.Network;

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
            SPValueType hp = Character.HP;
            SPValueType mp = Character.MP;

            bool updateHP = hp != _lastSentHP;
            bool updateMP = mp != _lastSentMP;

            if (updateHP)
            {
                using (PacketWriter pw = ServerPacket.SetHP(hp))
                {
                    _user.Send(pw);
                }
            }

            if (updateMP)
            {
                using (PacketWriter pw = ServerPacket.SetMP(mp))
                {
                    _user.Send(pw);
                }
            }

            _lastSentHP = hp;
            _lastSentMP = mp;
        }
    }
}