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
            User character = collider as User;
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

                    // Teleport the CharacterEntity to our predefined location
                    character.Teleport(newMap, Destination);
                    character.Position = Destination;
                }
            }
            // Teleport to an instanced map
            else if (DestinationMapInstance > 0)
            {
                // Check for a valid map
                var newMapInstance = character.World.GetMap(DestinationMapInstance);
                if (newMapInstance == null)
                {
                    const string errmsg = "Failed to teleport Character `{0}` - Invalid DestMap Instance `{1}`.";
                    Debug.Fail(string.Format(errmsg, character, this));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, character, this);
                    return;
                }

                // Try to create the map
                MapInstance instance;
                try
                {
                    instance = new MapInstance(newMapInstance.ID, character.World);
                }
                catch (System.Exception ex)
                {
                    string errmsg = "Failed to create instance: " + ex;
                    Debug.Fail(errmsg);
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    return;
                }

                // Check if the map placement is valid
                if (instance.IsValidPlacementPosition(Destination, character.Size))
                {
                    // Add the user to the map
                    character.Teleport(instance, Destination);
                    character.Position = Destination;
                }
                else
                {
                    string errmsg = "Failed to create instance: Not a valid map placement.";
                    Debug.Fail(errmsg);
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg);
                    return;
                }

                // Does this entity also teleport group members?
                if (TeleportGroupMembers)
                {
                    // Check if the user is in a group
                    if (character.Group == null)
                        return;

                    if (!character.Group.Members.IsEmpty())
                    {
                        // If yes, teleport those users too
                        foreach (var groupMember in character.Group.Members.OfType<User>())
                        {
                            groupMember.Teleport(instance, Destination);
                            groupMember.Position = Destination;
                        }
                    }
                }
            }
        }
    }
}