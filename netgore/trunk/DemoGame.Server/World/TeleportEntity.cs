using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    public class TeleportEntity : TeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Vector2 _initialSize;
        Vector2 _initialPosition;

        protected override void AfterCreation()
        {
            _initialPosition = Position;
            _initialSize = Size;
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

        float _returnCounter;

        public override void Update(IMap imap, float deltaTime)
        {
            base.Update(imap, deltaTime);

            if (_returnCounter > 0)
            {
                _returnCounter -= deltaTime;
            }
            else
            {
                if (Position.X > _initialPosition.X)
                    Position -= new Vector2(10, 0);
                if (Size.X > _initialSize.X)
                    Resize(Size - new Vector2(1, 1));
            }
        }

        public override void CollideFrom(Entity collider, Vector2 displacement)
        {
            // NOTE: Temporary little test
            Position += new Vector2(2, 0);
            Resize(Size + new Vector2(0.1f, 0.1f));
            _returnCounter = 700f;
        }

        public override bool CanUse(CharacterEntity charEntity)
        {
            return true;
        }

        public override event EntityEventHandler<CharacterEntity> OnUse;
    }
}
