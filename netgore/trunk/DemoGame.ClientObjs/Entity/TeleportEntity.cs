using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;

namespace DemoGame.Client
{
    public class TeleportEntity : TeleportEntityBase
    {
        public override bool Use(CharacterEntity charEntity)
        {
            throw new MethodAccessException("The client may not actually an IUsableEntity, only send requests to use them.");
        }

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override event EntityEventHandler<CharacterEntity> OnUse
        {
            add { }
            remove { }
        }
    }
}
