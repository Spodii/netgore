using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    public class DamageTrapEntity : DamageTrapEntityBase
    {
        public override void CollideFrom(NetGore.Entity collider, Vector2 displacement)
        {
            Character other = collider as Character;
            if (other == null)
                return;

            other.Damage(this, 1);
        }

        public override void CollideInto(NetGore.Entity collideWith, Vector2 displacement)
        {
            CollideFrom(collideWith, displacement);
        }
    }
}
