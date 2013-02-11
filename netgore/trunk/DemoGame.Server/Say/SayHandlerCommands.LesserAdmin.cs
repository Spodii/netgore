using System;
using System.Linq;
using NetGore;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> for <see cref="UserPermissions.LesserAdmin"/>.
    /// </summary>
    public partial class SayHandlerCommands
    {
        /// <summary>
        /// Creates an instance of an item from a template and adds it to your inventory. Any items that cannot
        /// fit into the caller's inventory are destroyed.
        /// </summary>
        /// <param name="id">The ID of the item template to use.</param>
        /// <param name="amount">The number of items to create.</param>
        [SayHandlerCommand("CreateItem", UserPermissions.LesserAdmin)]
        public void CreateItem(ItemTemplateID id, byte amount)
        {
            // Get the item template
            var template = ItemTemplateManager.Instance[id];
            if (template == null)
            {
                UserChat("Invalid item template ID: " + id);
                return;
            }

            if (amount <= 0)
            {
                UserChat("Invalid amount specified. The amount must be 1 or more.");
                return;
            }

            // Create the item
            var item = new ItemEntity(template, amount);

            // Give to user
            var remainder = User.Inventory.TryAdd(item);

            // Delete any that failed to be added
            if (remainder != null)
            {
                UserChat(remainder.Amount + " units could not be added to your inventory.");
                remainder.Destroy();
            }
        }

        /// <summary>
        /// Desummons all NPCs on the current map.
        /// </summary>
        [SayHandlerCommand("Dethrall", UserPermissions.LesserAdmin)]
        public void Dethrall()
        {
            var userMap = User.Map;
            if (userMap == null)
                return;

            // Get the thralled NPCs
            var toKill = userMap.NPCs.OfType<ThralledNPC>().Where(x => x.IsAlive).ToImmutable();

            // Kill all the found thralled NPCs
            foreach (var thralledNPC in toKill)
            {
                thralledNPC.Kill();
            }
        }

        /// <summary>
        /// Desummons all NPCs in the current world.
        /// </summary>
        [SayHandlerCommand("DethrallWorld", UserPermissions.LesserAdmin)]
        public void DethrallWorld()
        {

            // Iterate over every map and kill all the mobs that are of type 'ThralledNPC'
            foreach (var userMap in World.Maps)
            {
                // Get the thralled NPCs
                var toKill = userMap.NPCs.OfType<ThralledNPC>().Where(x => x.IsAlive).ToImmutable();

                // Kill all the found thralled NPCs
                foreach (var thralledNPC in toKill)
                {
                    thralledNPC.Kill();
                }
            }
        }


        /// <summary>
        /// Kills the specified user.
        /// </summary>
        /// <param name="userName">The player to kill.</param>
        [SayHandlerCommand("KillUser", UserPermissions.LesserAdmin)]
        public void KillUser(string userName)
        {
            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            target.Kill();
        }

        /// <summary>
        /// Teaches the specific user the skill given by the ID
        /// </summary>
        /// <param name="userName">The player to give the skill.</param>
        /// <param name="skillID">The skill ID</param>
        [SayHandlerCommand("TeachSkill", UserPermissions.LesserAdmin)]
        public void TeachSkill(string userName, int skillID)
        {
            // Get the user we want
            var target = World.FindUser(userName);
            var skillCount = Enum.GetNames(typeof(SkillType)).Length;


            if (skillID > skillCount)
                return;

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            var skillType = (SkillType)skillID;

            target.KnownSkills.SetSkill(skillType, true);

        }

        /// <summary>
        /// Teaches the specific user the skill given by the skill name.
        /// This is represented by the <see cref="SkillType"/> enum name; not the display name client-side.
        /// </summary>
        /// <param name="userName">The player to give the skill.</param>
        /// <param name="skillName">The skill name to teach the user</param>
        [SayHandlerCommand("TeachSkill", UserPermissions.LesserAdmin)]
        public void TeachSkill(string userName, string skillName)
        {
            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            SkillType skillType;
            var result = Enum.TryParse(skillName, true, out skillType);

            if (result)
                target.KnownSkills.SetSkill(skillType, true);

        }

        /// <summary>
        /// Unteaches the specific user the skill given by the skill name.
        /// This is represented by the <see cref="SkillType"/> enum name; not the display name client-side.
        /// </summary>
        /// <param name="userName">The player to give the skill.</param>
        /// <param name="skillName">The skill name to teach the user</param>
        [SayHandlerCommand("UnteachSkill", UserPermissions.LesserAdmin)]
        public void UnteachSkill(string userName, string skillName)
        {
            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            SkillType skillType;
            var result = Enum.TryParse(skillName, true, out skillType);

            if (result)
                target.KnownSkills.SetSkill(skillType, false);

        }


        /// <summary>
        /// Unteaches the specific user the skill given by the ID
        /// </summary>
        /// <param name="userName">The player to give the skill.</param>
        /// /// <param name="skillID">The skill ID</param>
        [SayHandlerCommand("UnteachSkill", UserPermissions.LesserAdmin)]
        public void UnteachSkill(string userName, int skillID)
        {
            // Get the user we want
            var target = World.FindUser(userName);
            var skillCount = Enum.GetNames(typeof(SkillType)).Length;


            if (skillID > skillCount)
                return;

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            var skillType = (SkillType)skillID;

            target.KnownSkills.SetSkill(skillType, false);

        }


        /// <summary>
        /// Awards the specific user with cash
        /// </summary>
        /// <param name="userName">The player to give cash to</param>
        /// <param name="amount">The amount of cash to give the player</param>
        [SayHandlerCommand("GiveCash", UserPermissions.LesserAdmin)]
        public void GiveCash(string userName, int amount)
        {
            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            target.Cash += amount;

            // Notify the player
            target.Send(GameMessage.AdminGaveCash, ServerMessageType.GUIChat, amount);
        }

        /// <summary>
        /// Removes the specific user of cash by a specific amount
        /// </summary>
        /// <param name="userName">The player to take cash from</param>
        /// <param name="amount">The amount of cash to take from the player</param>
        [SayHandlerCommand("TakeCash", UserPermissions.LesserAdmin)]
        public void TakeCash(string userName, int amount)
        {
            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            // Clamp the amount of cash to be taken so you can never have below 0
            target.Cash = (int)MathHelper.Clamp(target.Cash - amount, 0, int.MaxValue);

            // Notify the player
            target.Send(GameMessage.AdminTookCash, ServerMessageType.GUIChat, amount);
        }



        /// <summary>
        /// Warps the specified player to the user of the command.
        /// </summary>
        /// <param name="userName">The name of the player to summon.</param>
        [SayHandlerCommand("Summon", UserPermissions.LesserAdmin)]
        public void Summon(string userName)
        {
            var userMap = User.Map;
            if (userMap == null)
                return;

            // Get the user we want
            var target = World.FindUser(userName);

            // Check that the user could be found
            if (target == null)
            {
                User.Send(GameMessage.CommandGeneralUnknownUser, ServerMessageType.GUIChat, userName);
                return;
            }

            // Target user was found, so teleport them to the user that issued the command 
            target.Teleport(userMap, User.Position);
        }

        /// <summary>
        /// Summons NPCs on the current map.
        /// </summary>
        /// <param name="id">The ID of the NPCs to spawn.</param>
        /// <param name="amount">The amount of NPCs to spawn.</param>
        [SayHandlerCommand("Thrall", UserPermissions.LesserAdmin)]
        public void Thrall(CharacterTemplateID id, int amount)
        {
            var charTemplate = CharacterTemplateManager.Instance[id];
            if (charTemplate == null)
            {
                UserChat("Invalid character template `{0}`.", id);
                return;
            }

            if (amount < 1)
            {
                UserChat("Must thrall 1 or more characters.");
                return;
            }

            var thrallArea = new Rectangle();
            var useThrallArea = false;

            // When standing on top of something, also spawn the NPCs on the thing the User is standing on,
            // and spread them out a bit on it without exceeding the size of it
            var userStandingOn = User.StandingOn;
            if (userStandingOn != null)
            {
                useThrallArea = true;

                var minX = userStandingOn.Position.X;
                var maxX = userStandingOn.Max.X;
                var y = userStandingOn.Position.Y;

                minX = Math.Max(minX, User.Position.X - 96);
                maxX = Math.Min(maxX, User.Position.X + 96);

                thrallArea = new Rectangle(minX, y, maxX - minX + 1, 1);
            }

            for (var i = 0; i < amount; i++)
            {
                // Create a ThralledNPC and add it to the world
                var npc = new ThralledNPC(World, charTemplate, User.Map, User.Position);

                // When using the thrallArea, move the NPC to the correct area
                if (useThrallArea)
                {
                    var npcSize = npc.Size;
                    var minX = thrallArea.Left;
                    var maxX = thrallArea.Right - (int)npcSize.X;
                    int x;

                    if (maxX <= minX)
                        x = minX;
                    else
                        x = RandomHelper.NextInt(minX, maxX);

                    var y = thrallArea.Y - (int)npc.Size.Y;
                    npc.Position = new Vector2(x, y);
                }
            }
        }

        /// <summary>
        /// Warps the user to the specified map and position.
        /// </summary>
        /// <param name="mapID">The mapID to be warped to.</param>
        /// <param name="x">The position along the x-axis to be warped to.</param>
        /// <param name="y">The position along the y-axis to be warped to.</param>
        [SayHandlerCommand("Warp", UserPermissions.LesserAdmin)]
        public void Warp(MapID mapID, int x, int y)
        {
            // Check for a valid map
            if (!MapBase.MapIDExists(mapID))
            {
                UserChat("Invalid map ID `{0}`.", mapID);
                return;
            }

            var map = World.GetMap(mapID);
            if (map == null)
            {
                UserChat("No map with ID `{0}` exists.", mapID);
                return;
            }

            var pos = new Vector2(x, y);

            if (pos.X < 0 || pos.Y < 0 || pos.X > map.Width || pos.Y > map.Height)
            {
                UserChat("The specified coordinates are out of the map's range. Map size: {0}", map.Size);
                return;
            }

            // Move the user
            User.Teleport(map, pos);
        }


        /// <summary>
        /// Warps the user to the specified map.
        /// </summary>
        /// <param name="mapID">The mapID to be warped to.</param>
        [SayHandlerCommand("Warp", UserPermissions.LesserAdmin)]
        public void Warp(MapID mapID)
        {
            // Check for a valid map
            if (!MapBase.MapIDExists(mapID))
            {
                UserChat("Invalid map ID `{0}`.", mapID);
                return;
            }

            var map = World.GetMap(mapID);
            if (map == null)
            {
                UserChat("No map with ID `{0}` exists.", mapID);
                return;
            }

            var pos = User.Position;

            if (pos.X < 0 || pos.Y < 0 || pos.X > map.Width || pos.Y > map.Height)
            {
                UserChat("The position of your current coordinates are out of the map's range. Map size: {0}", map.Size);
                return;
            }

            // Move the user
            User.Teleport(map, pos);
        }

        /// Warps the user to the specified position.
        /// </summary>
        /// <param name="mapID">The mapID to be warped to.</param>
        /// <param name="x">The position along the x-axis to be warped to.</param>
        /// <param name="y">The position along the y-axis to be warped to.</param>
        [SayHandlerCommand("Warp", UserPermissions.LesserAdmin)]
        public void Warp(int x, int y)
        {
            var map = User.Map;

            var pos = new Vector2(x, y);

            if (pos.X < 0 || pos.Y < 0 || pos.X > map.Width || pos.Y > map.Height)
            {
                UserChat("That position is out of the map's range. Map size: {0}", map.Size);
                return;
            }

            // Move the user
            User.Teleport(map, pos);
        }

        /// <summary>
        /// Toggles the immortality flag on the given user.
        /// When set, the user cannot take any damage via conventional means.
        /// </summary>
        [SayHandlerCommand("Immortal", UserPermissions.LesserAdmin)]
        public void MakeImmortal()
        {
            User.Immortal = !User.Immortal;
        }

        /// <summary>
        /// Toggles the visibility flag on the given user.
        /// When set, the user cannot be seen by any other clients or take agression from monsters.
        /// </summary>
        [SayHandlerCommand("Invisible", UserPermissions.LesserAdmin)]
        public void MakeInvisible()
        {
            User.Invisible = !User.Invisible;
        }


    }
}