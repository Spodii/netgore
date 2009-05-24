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

        public int Damage { get { return _damage; } }

        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(Damage);
        }

        protected override void WriteCustomValues(INamedValueWriter writer)
        {
            writer.Write("Damage", Damage);
        }

        protected override void ReadCustomValues(IValueReader reader)
        {
            _damage = reader.ReadInt();
        }

        protected override void ReadCustomValues(INamedValueReader reader)
        {
            _damage = reader.ReadInt("Damage");
        }
    }
}
