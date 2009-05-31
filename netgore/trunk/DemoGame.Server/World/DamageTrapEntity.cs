using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    public class DamageTrapEntity : DamageTrapEntityBase
    {
        public override void CollideFrom(Entity collider, Vector2 displacement)
        {
            Character other = collider as Character;
            if (other == null)
                return;

            other.Damage(this, 1);
        }

        public override void CollideInto(Entity collideWith, Vector2 displacement)
        {
            CollideFrom(collideWith, displacement);
        }
    }
}