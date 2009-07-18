using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class UserSPSynchronizer : CharacterSPSynchronizer
    {
        readonly User _user;
        SPValueType _lastSentHP;
        SPValueType _lastSentMP;

        public UserSPSynchronizer(User user)
            : base(user)
        {
            _user = user;
        }

        void SynchronizeSelf()
        {
            SPValueType hp = Character.HP;
            SPValueType mp = Character.MP;

            bool updateHP = hp != _lastSentHP;
            bool updateMP = mp != _lastSentMP;

            if (updateHP)
            {
                using (var pw = ServerPacket.SetHP(hp))
                    _user.Send(pw);
            }

            if (updateMP)
            {
                using (var pw = ServerPacket.SetMP(mp))
                    _user.Send(pw);
            }

            _lastSentHP = hp;
            _lastSentMP = mp;
        }

        public override void Synchronize()
        {
            base.Synchronize();
            SynchronizeSelf();
        }
    }
}
