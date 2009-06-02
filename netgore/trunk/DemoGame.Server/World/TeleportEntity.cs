using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    public class TeleportEntity : TeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override event EntityEventHandler<CharacterEntity> OnUse;

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override bool Use(CharacterEntity charEntity)
        {
            // Check if we can use
            if (!CanUse(charEntity))
                return false;

            if (DestinationMap > 0)
            {
                Character c = (Character)charEntity;
                if (c.Map.Index != DestinationMap)
                {
                    Map newMap = c.World.GetMap(DestinationMap);
                    if (newMap == null)
                    {
                        const string errmsg = "Failed to teleport Character `{0}` - Invalid DestMap `{1}`.";
                        Debug.Fail(string.Format(errmsg, c, this));
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, c, this);
                        return false;
                    }
                    c.SetMap(newMap);
                }
            }

            // Teleport the CharacterEntity to our predefined location
            charEntity.Teleport(Destination);

            // Notify listeners
            if (OnUse != null)
                OnUse(this, charEntity);

            return true;
        }
    }
}