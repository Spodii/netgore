using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.World;

namespace DemoGame.Server
{
    public class TeleportEntity : TeleportEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Handles when another Entity collides into us. Not synonymous CollideInto since the
        /// <paramref name="collider"/> Entity is the one who collided into us. For example, if the
        /// two entities in question were a moving Character and a stationary wall, this Entity would be
        /// the Wall and <paramref name="collider"/> would be the Character.
        /// </summary>
        /// <param name="collider">Entity that collided into us.</param>
        /// <param name="displacement">Displacement between the two Entities.</param>
        public override void CollideFrom(Entity collider, SFML.Graphics.Vector2 displacement)
        {
            // When a character touches this, teleport the character to the destination
            Character character = collider as Character;
            if (character == null)
                return;

            // Teleport to a new map
            if (DestinationMap > 0)
            {
                if (character.Map.ID != DestinationMap)
                {
                    var newMap = character.World.GetMap(DestinationMap);
                    if (newMap == null)
                    {
                        const string errmsg = "Failed to teleport Character `{0}` - Invalid DestMap `{1}`.";
                        Debug.Fail(string.Format(errmsg, character, this));
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, character, this);
                        return;
                    }

                    character.Teleport(newMap, Destination);
                }
            }

            // Teleport the CharacterEntity to our predefined location
            character.Position = Destination;
        }
    }
}