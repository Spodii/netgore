using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    public abstract class DamageTrapEntityBase : DynamicEntity
    {
        int _damage;

        [SyncValue]
        public int Damage { get { return _damage; } set { _damage = value; } }
    }
}
