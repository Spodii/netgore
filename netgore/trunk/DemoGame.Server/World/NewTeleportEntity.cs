using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace DemoGame.Server
{
    public class NewTeleportEntity : NewTeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override event EntityEventHandler<CharacterEntity> OnUse;
    }
}
