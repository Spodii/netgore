using System;
using System.Linq;
using System.Text;
using DemoGame.DbObjs;
using DemoGame.Server.Guilds;
using NetGore;
using NetGore.Features.Banning;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;
using NetGore.World;
using SFML.Graphics;

// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace DemoGame.Server
{
    /// <summary>
    /// Handles processing what Users say.
    /// </summary>
    public class SayHandler : SayHandlerBase<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SayHandler"/> class.
        /// </summary>
        /// <param name="server">Server that the commands are coming from.</param>
        public SayHandler(Server server) : base(new SayCommands(server))
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles the output from a command.
        /// </summary>
        /// <param name="user">The user that the command came from.</param>
        /// <param name="text">The output text from the command. Will not be null or empty.</param>
        protected override void HandleCommandOutput(User user, string text)
        {
            ThreadAsserts.IsMainThread();

            using (var pw = ServerPacket.Chat(text))
            {
                user.Send(pw, ServerMessageType.GUIChat);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles text that was not a command.
        /// </summary>
        /// <param name="user">The user the <paramref name="text"/> came from.</param>
        /// <param name="text">The text that wasn't a command.</param>
        protected override void HandleNonCommand(User user, string text)
        {
            ThreadAsserts.IsMainThread();

            using (var pw = ServerPacket.ChatSay(user.Name, user.MapEntityIndex, text))
            {
                user.Map.SendToArea(user, pw, ServerMessageType.GUIChat);
            }
        }

        /// <summary>
        /// The actual class that handles the Say commands.
        /// </summary>
        public class SayCommands : ISayCommands<User>
        {
            static readonly GuildManager _guildManager = GuildManager.Instance;
            static readonly GuildSettings _guildSettings = GuildSettings.Instance;

            readonly Server _server;

            /// <summary>
            /// Initializes a new instance of the <see cref="SayCommands"/> class.
            /// </summary>
            /// <param name="server">The Server that the commands will come from.</param>
            public SayCommands(Server server)
            {
                if (server == null)
                    throw new ArgumentNullException("server");

                _server = server;
            }

            public IGroupManager GroupManager
            {
                get { return Server.GroupManager; }
            }

            /// <summary>
            /// Gets the Server that the commands are coming from.
            /// </summary>
            public Server Server
            {
                get { return _server; }
            }

            /// <summary>
            /// Gets the World that the User belongs to.
            /// </summary>
            public World World
            {
                get { return Server.World; }
            }

            /// <summary>
            /// Sends a message to everyone online.
            /// </summary>
            /// <param name="message">The message to send.</param>
            [SayCommand("Shout")]
            public void Shout(string message)
            {
                using (var pw = ServerPacket.SendMessage(GameMessage.CommandShout, User.Name, message))
                {
                    World.Send(pw, ServerMessageType.GUIChat);
                }
            }

            /// <summary>
            /// Sends a message to a single user.
            /// </summary>
            /// <param name="userName">The name of the user to whisper to.</param>
            /// <param name="message">The message to send to the user.</param>
            [SayCommand("Tell")]
            [SayCommand("Whisper")]
            public void Tell(string userName, string message)
            {
                // Check for a message to tell
                if (string.IsNullOrEmpty(userName))
                {
                    // Invalid message
                    User.Send(GameMessage.CommandTellNoName, ServerMessageType.GUIChat);
                    return;
                }

                // Find the user to tell
                if (string.IsNullOrEmpty(message))
                {
                    // No or invalid message
                    User.Send(GameMessage.CommandTellNoMessage, ServerMessageType.GUIChat);
                    return;
                }

                var target = World.FindUser(userName);

                // Check if the target user is available or not
                if (target != null)
                {
                    // Message to sender ("You tell...")
                    User.Send(GameMessage.CommandTellSender, ServerMessageType.GUIChat, target.Name, message);

                    // Message to receivd ("X tells you...")
                    target.Send(GameMessage.CommandTellReceiver, ServerMessageType.GUIChat, User.Name, message);
                }
                else
                {
                    // User not found
                    User.Send(GameMessage.CommandTellInvalidUser, ServerMessageType.GUIChat, userName);
                }
            }

            /// <summary>
            /// Starts a trade with another user.
            /// </summary>
            /// <param name="userName">The name of the user to trade with.</param>
            [SayCommand("Trade")]
            public void Trade(string userName)
            {
                var target = World.FindUser(userName);
                if (target == null)
                {
                    User.Send(GameMessage.PeerTradingInvalidTarget, ServerMessageType.GUI);
                    return;
                }

                User.TryStartPeerTrade(target);
            }

            #region Helper methods

            /// <summary>
            /// Checks if the user meets the required guild rank.
            /// </summary>
            /// <param name="requiredRank">The required guild rank.</param>
            /// <returns>If false, the command should be aborted.</returns>
            bool CheckGuildPermissions(GuildRank requiredRank)
            {
                if (((IGuildMember)User).GuildRank < requiredRank)
                {
                    User.Send(GameMessage.GuildInsufficientPermissions, ServerMessageType.GUI,
                              _guildSettings.GetRankName(requiredRank));
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Handles kicking someone out of a guild.
            /// </summary>
            /// <param name="target">The <see cref="IGuildMember"/> being kicked out of the guild.</param>
            /// <param name="userName">The name of the <paramref name="target"/>.</param>
            void GuildMemberPerformer_GuildKick(IGuildMember target, string userName)
            {
                if (target == null)
                {
                    User.Send(GameMessage.GuildKickFailedInvalidUser, ServerMessageType.GUI, userName);
                    return;
                }

                if (target.Guild != User.Guild)
                {
                    User.Send(GameMessage.GuildKickFailedNotInGuild, ServerMessageType.GUI, target.Name);
                    return;
                }

                if (target.GuildRank > ((IGuildMember)User).GuildRank)
                {
                    User.Send(GameMessage.GuildKickFailedTooHighRank, ServerMessageType.GUI, target.Name);
                    return;
                }

                if (!User.Guild.TryKickMember(User, target))
                {
                    User.Send(GameMessage.GuildKickFailedUnknownReason, ServerMessageType.GUI, target.Name);
                    return;
                }

                User.Send(GameMessage.GuildKick, ServerMessageType.GUI, target.Name);
            }

            /// <summary>
            /// Requires the user to not be in a group.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireInGroup()
            {
                if (((IGroupable)User).Group == null)
                {
                    User.Send(GameMessage.InvalidCommandMustBeInGroup, ServerMessageType.GUI);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Requires the user to not be in a group.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireNotInGroup()
            {
                if (((IGroupable)User).Group != null)
                {
                    User.Send(GameMessage.InvalidCommandMustNotBeInGroup, ServerMessageType.GUI);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Requires the user to have the specified permission level.
            /// </summary>
            /// <param name="level">The require <see cref="UserPermissions"/> level.</param>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequirePermissionLevel(UserPermissions level)
            {
                if (User.Permissions.IsSet(level))
                    return true;

                User.Send(GameMessage.InsufficientPermissions, ServerMessageType.GUI);
                return false;
            }

            /// <summary>
            /// Requires the user to be in a guild.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireUserInGuild()
            {
                if (User.Guild == null)
                {
                    User.Send(GameMessage.InvalidCommandMustBeInGuild, ServerMessageType.GUI);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Requires the user to not be in a guild.
            /// </summary>
            /// <returns>If false, the command should be aborted.</returns>
            bool RequireUserNotInGuild()
            {
                if (User.Guild != null)
                {
                    User.Send(GameMessage.InvalidCommandMustNotBeInGuild, ServerMessageType.GUI);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Sends a chat message to the <see cref="User"/>. This is provided purely for convenience since it can
            /// become quite redundant having to constantly create the <see cref="ServerPacket.Chat"/> calls.
            /// </summary>
            /// <param name="message">The message to send.</param>
            void UserChat(string message)
            {
                using (var pw = ServerPacket.Chat(message))
                {
                    User.Send(pw, ServerMessageType.GUIChat);
                }
            }

            /// <summary>
            /// Sends a chat message to the <see cref="User"/>. This is provided purely for convenience since it can
            /// become quite redundant having to constantly create the <see cref="ServerPacket.Chat"/> calls.
            /// </summary>
            /// <param name="message">The message to send.</param>
            /// <param name="args">The string formatting arguments.</param>
            void UserChat(string message, params object[] args)
            {
                var msgFormatted = string.Format(message, args);
                using (var pw = ServerPacket.Chat(msgFormatted))
                {
                    User.Send(pw, ServerMessageType.GUIChat);
                }
            }

            void UserChat(IAccountBanTable banInfo, int index, int total)
            {
                UserChat("Ban {0} of {1}:", index, total);
                UserChat("   BanID: {0}", banInfo.ID);
                UserChat("   Issued by: {0}", banInfo.IssuedBy);
                UserChat("   Start: {0}", banInfo.StartTime.ToString("U"));
                UserChat("   End: {0}", banInfo.EndTime.ToString("U"));
                UserChat("   Duration: {0}", (banInfo.EndTime - banInfo.StartTime).Duration());
                UserChat("   Remaining (approx): {0}", (DateTime.Now - banInfo.EndTime).Duration());
                UserChat("   Reason: {0}", banInfo.Reason);
            }

            #endregion

            #region Groups

            /// <summary>
            /// Creates a new group.
            /// </summary>
            [SayCommand("CreateGroup")]
            public void CreateGroup()
            {
                if (!RequireNotInGroup())
                    return;

                var group = GroupManager.TryCreateGroup(User);

                if (group == null)
                    User.Send(GameMessage.GroupCreateFailedUnknownReason, ServerMessageType.GUI);
                else
                    User.Send(GameMessage.GroupCreated, ServerMessageType.GUI);
            }

            /// <summary>
            /// Sends an invite to another user to join the group.
            /// </summary>
            /// <param name="userName">The name of the user to invite to the group.</param>
            [SayCommand("GroupInvite")]
            public void GroupInvite(string userName)
            {
                if (!RequireInGroup())
                    return;

                var target = World.FindUser(userName);

                if (target == null)
                {
                    User.Send(GameMessage.GroupInviteFailedInvalidUser, ServerMessageType.GUI, userName);
                    return;
                }

                if (target == User)
                {
                    User.Send(GameMessage.GroupInviteFailedCannotInviteSelf, ServerMessageType.GUI);
                    return;
                }

                if (!(((IGroupable)User).Group.TryInvite(target)))
                {
                    // Invite failed
                    if (((IGroupable)target).Group != null)
                        User.Send(GameMessage.GroupInviteFailedAlreadyInGroup, ServerMessageType.GUI, target.Name);
                    else
                        User.Send(GameMessage.GroupInviteFailedUnknownReason, ServerMessageType.GUI, target.Name);
                }
                else
                {
                    // Invite successful
                    using (var pw = ServerPacket.SendMessage(GameMessage.GroupInvite, User.Name, target.Name))
                    {
                        foreach (var u in ((IGroupable)User).Group.Members.OfType<User>())
                        {
                            u.Send(pw, ServerMessageType.GUI);
                        }
                    }
                }
            }

            /// <summary>
            /// Accepts an invitation to join a group.
            /// </summary>
            [SayCommand("JoinGroup")]
            public void JoinGroup()
            {
                if (!RequireNotInGroup())
                    return;

                User.TryJoinGroup();
            }

            /// <summary>
            /// Leaves the current group.
            /// </summary>
            [SayCommand("LeaveGroup")]
            public void LeaveGroup()
            {
                if (!RequireInGroup())
                    return;

                ((IGroupable)User).Group.RemoveMember(User);
            }

            #endregion

            #region Guilds

            /// <summary>
            /// Creates a new guild.
            /// </summary>
            /// <param name="name">The name of the guild.</param>
            /// <param name="tag">The guild tag.</param>
            [SayCommand("CreateGuild")]
            public void CreateGuild(string name, string tag)
            {
                if (!RequireUserNotInGuild())
                    return;

                // Valid name
                if (!_guildSettings.IsValidName(name))
                {
                    User.Send(GameMessage.GuildCreationFailedNameInvalid, ServerMessageType.GUI, name);
                    return;
                }

                if (!_guildManager.IsNameAvailable(name))
                {
                    User.Send(GameMessage.GuildCreationFailedNameNotAvailable, ServerMessageType.GUI, name);
                    return;
                }

                // Valid tag
                if (!_guildSettings.IsValidTag(tag))
                {
                    User.Send(GameMessage.GuildCreationFailedTagInvalid, ServerMessageType.GUI, tag);
                    return;
                }

                if (!_guildManager.IsTagAvailable(tag))
                {
                    User.Send(GameMessage.GuildCreationFailedTagNotAvailable, ServerMessageType.GUI, tag);
                    return;
                }

                // Create
                var guild = _guildManager.TryCreateGuild(User, name, tag);
                if (guild == null)
                    User.Send(GameMessage.GuildCreationFailedUnknownReason, ServerMessageType.GUI, name, tag);
                else
                    User.Send(GameMessage.GuildCreationSuccessful, ServerMessageType.GUI, name, tag);
            }

            /// <summary>
            /// Demotes the guild rank of another member in the guild.
            /// </summary>
            /// <param name="userName">The name of the fellow guild member to demote.</param>
            [SayCommand("Demote")]
            public void Demote(string userName)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankDemote))
                    return;

                var success = false;
                World.GuildMemberPerformer.Perform(userName, x => success = User.Guild.TryDemoteMember(User, x));

                if (success)
                    User.Send(GameMessage.GuildDemote, ServerMessageType.GUI, userName);
                else
                    User.Send(GameMessage.GuildDemoteFailed, ServerMessageType.GUI, userName);
            }

            /// <summary>
            /// Displays the guild commands.
            /// </summary>
            [SayCommand("GuildHelp")]
            public void GuildHelp()
            {
                var sb = new StringBuilder();
                sb.AppendLine("Guild commands:");
                sb.AppendLine("/JoinGuild [name]");
                sb.AppendLine("/CreateGuild [name] [symbol]");
                sb.AppendLine("/LeaveGuild");
                sb.AppendLine("/GuildMembers");
                sb.AppendLine("/GuildOnline");
                sb.AppendLine("/GuildKick [user]");
                sb.AppendLine("/Promote [user]");
                sb.AppendLine("/Demote [user]");
                sb.AppendLine("/RenameGuild [name]");
                sb.AppendLine("/RetagGuild [tag]");
                sb.AppendLine("/GuildInvite [user]");
                sb.AppendLine("/GuildLog");
                sb.AppendLine("/GuildSay [message]");

                UserChat(sb.ToString());
            }

            /// <summary>
            /// Invites a user to join the guild.
            /// </summary>
            /// <param name="toInvite">The name of the user to invite into the guild.</param>
            [SayCommand("GuildInvite")]
            public void GuildInvite(string toInvite)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankInvite))
                    return;

                var invitee = World.FindUser(toInvite);
                if (invitee == null)
                {
                    User.Send(GameMessage.GuildInviteFailedInvalidUser, ServerMessageType.GUI, toInvite);
                    return;
                }

                if (invitee == User)
                {
                    User.Send(GameMessage.GuildInviteFailedCannotInviteSelf, ServerMessageType.GUI);
                    return;
                }

                if (invitee.Guild != null)
                {
                    User.Send(GameMessage.GuildInviteFailedAlreadyInGuild, ServerMessageType.GUI, invitee.Name);
                    return;
                }

                var success = User.Guild.TryInviteMember(User, invitee);

                if (!success)
                    User.Send(GameMessage.GuildInviteFailedUnknownReason, ServerMessageType.GUI, invitee.Name);
                else
                    User.Send(GameMessage.GuildInviteSuccess, ServerMessageType.GUI, invitee.Name);
            }

            /// <summary>
            /// Kicks a fellow guild member out of the guild.
            /// </summary>
            /// <param name="userName">The name of the user to kick out of the guild.</param>
            [SayCommand("GuildKick")]
            public void GuildKick(string userName)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankKick))
                    return;

                World.GuildMemberPerformer.Perform(userName, x => GuildMemberPerformer_GuildKick(x, userName));
            }

            /// <summary>
            /// Displays the latest entries of the guild log.
            /// </summary>
            [SayCommand("GuildLog")]
            public void GuildLog()
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankViewLog))
                    return;

                User.Guild.TryViewEventLog(User);
            }

            /// <summary>
            /// Displays all of the guild members.
            /// </summary>
            [SayCommand("GuildMembers")]
            public void GuildMembers()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild.TryViewMembers(User);
            }

            /// <summary>
            /// Displays the guild members that are currently online.
            /// </summary>
            [SayCommand("GuildOnline")]
            public void GuildOnline()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild.TryViewOnlineMembers(User);
            }

            /// <summary>
            /// Sends a message to all online fellow guild members.
            /// </summary>
            /// <param name="message">The message to send.</param>
            [SayCommand("GuildSay")]
            public void GuildSay(string message)
            {
                if (!RequireUserInGuild())
                    return;

                foreach (var guildMember in User.Guild.GetMembers().OfType<User>())
                {
                    guildMember.Send(GameMessage.GuildSay, ServerMessageType.GUIChat, User.Name, message);
                }
            }

            /// <summary>
            /// Accepts an invitation to join a guild.
            /// </summary>
            /// <param name="guildName">The name of the guild to join.</param>
            [SayCommand("JoinGuild")]
            public void JoinGuild(string guildName)
            {
                if (!RequireUserNotInGuild())
                    return;

                if (!User.TryJoinGuild(guildName))
                    User.Send(GameMessage.GuildJoinFailedInvalidOrNoInvite, ServerMessageType.GUI, guildName);
            }

            /// <summary>
            /// Leaves the current guild.
            /// </summary>
            [SayCommand("LeaveGuild")]
            public void LeaveGuild()
            {
                if (!RequireUserInGuild())
                    return;

                User.Guild = null;
            }

            /// <summary>
            /// Promotes the guild rank of another member in the guild.
            /// </summary>
            /// <param name="userName">The name of the fellow guild member to promote.</param>
            [SayCommand("Promote")]
            public void Promote(string userName)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankPromote))
                    return;

                var success = false;
                World.GuildMemberPerformer.Perform(userName, x => success = User.Guild.TryPromoteMember(User, x));

                if (success)
                    User.Send(GameMessage.GuildPromote, ServerMessageType.GUI, userName);
                else
                    User.Send(GameMessage.GuildPromoteFailed, ServerMessageType.GUI, userName);
            }

            /// <summary>
            /// Changes the name of the guild.
            /// </summary>
            /// <param name="newName">The new guild name.</param>
            [SayCommand("RenameGuild")]
            public void RenameGuild(string newName)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankRename))
                    return;

                if (!_guildSettings.IsValidName(newName))
                {
                    User.Send(GameMessage.GuildRenameFailedInvalidValue, ServerMessageType.GUI, newName);
                    return;
                }

                if (!_guildManager.IsNameAvailable(newName))
                {
                    User.Send(GameMessage.GuildRenameFailedNameNotAvailable, ServerMessageType.GUI, newName);
                    return;
                }

                if (!User.Guild.TryChangeName(User, newName))
                    User.Send(GameMessage.GuildRenameFailedUnknownReason, ServerMessageType.GUI, newName);
            }

            /// <summary>
            /// Changes the tag of the guild.
            /// </summary>
            /// <param name="newTag">The new guild tag.</param>
            [SayCommand("RetagGuild")]
            public void RetagGuild(string newTag)
            {
                if (!RequireUserInGuild() || !CheckGuildPermissions(_guildSettings.MinRankRename))
                    return;

                if (!_guildSettings.IsValidTag(newTag))
                {
                    User.Send(GameMessage.GuildRetagFailedInvalidValue, ServerMessageType.GUI, newTag);
                    return;
                }

                if (!_guildManager.IsTagAvailable(newTag))
                {
                    User.Send(GameMessage.GuildRetagFailedNameNotAvailable, ServerMessageType.GUI, newTag);
                    return;
                }

                if (!User.Guild.TryChangeTag(User, newTag))
                    User.Send(GameMessage.GuildRetagFailedUnknownReason, ServerMessageType.GUI, newTag);
            }

            #endregion

            #region Test/development commands

            /// <summary>
            /// Creates a new map instance and places the user on that map.
            /// </summary>
            /// <param name="mapID">The ID of the map to create the instance of.</param>
            [SayCommand("CreateMapInstance")]
            public void CreateMapInstance(MapID mapID)
            {
                if (!RequirePermissionLevel(UserPermissions.Admin))
                    return;

                // Check for a valid map
                if (!MapBase.IsMapIDValid(mapID))
                {
                    UserChat("Invalid map ID: " + mapID);
                    return;
                }

                // Try to create the map
                MapInstance instance;
                try
                {
                    instance = new MapInstance(mapID, World);
                }
                catch (Exception ex)
                {
                    UserChat("Failed to create instance: " + ex);
                    return;
                }

                // Add the user to the map
                User.ChangeMap(instance, new Vector2(50, 50));
            }

            /// <summary>
            /// Leaves an instanced map if the map the user is on is for an instanced map. The user is warped
            /// to their respawn position.
            /// </summary>
            [SayCommand("LeaveMapInstance")]
            public void LeaveMapInstance()
            {
                if (!RequirePermissionLevel(UserPermissions.Admin))
                    return;

                // Check for a valid map
                if (User.Map == null || !User.Map.IsInstanced)
                {
                    UserChat("You must be on an instanced map to do that.");
                    return;
                }

                // Get the map to respawn on
                var mapID = User.RespawnMapID;
                Map map = null;

                if (mapID.HasValue)
                    map = World.GetMap(mapID.Value);

                if (map == null)
                {
                    UserChat("Could not teleport you to your respawn location - your respawn map is null for some reason...");
                    return;
                }

                // Teleport to respawn map/position
                User.ChangeMap(map, User.RespawnPosition);
            }

            /// <summary>
            /// Causes you to kill yourself.
            /// </summary>
            [SayCommand("Suicide")]
            [SayCommand("Seppuku")]
            public void Suicide()
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                User.Kill();
            }

            #endregion

            #region Lesser Admin commands

            /// <summary>
            /// Sends a message globally to the entire world.
            /// </summary>
            /// <param name="message">The message to announce.</param>
            [SayCommand("Announce")]
            public void Announce(string message)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                World.Send(GameMessage.CommandAnnounce, ServerMessageType.GUIChat, message);
            }

            /// <summary>
            /// Warps the user to the player specified.
            /// </summary>
            /// <param name="userName">The name of the player to approach.</param>
            [SayCommand("Approach")]
            public void Approach(string userName)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                // Get the user we want
                var target = World.FindUser(userName);

                // Check that the user could be found
                if (target == null)
                {
                    User.Send(GameMessage.CommandTellInvalidUser, ServerMessageType.GUIChat);
                    return;
                }

                // Target user was found, so teleport the user that issued the command to the target user
                User.Teleport(target.Map, target.Position);
            }

            /// <summary>
            /// Displays all ban information for a user.
            /// </summary>
            /// <param name="username">The name of the user to show the ban info for.</param>
            [SayCommand("BanHistory")]
            public void BanHistory(string username)
            {
                if (!RequirePermissionLevel(UserPermissions.LesserAdmin))
                    return;

                var banInfos = BanningManager.Instance.GetAccountBanInfo(username);

                var count = banInfos.Count();
                UserChat(string.Format("All bans on user `{0}`: {1}", username, count));

                var i = 1;
                foreach (var banInfo in banInfos)
                {
                    UserChat(banInfo, i++, count);
                }
            }

            /// <summary>
            /// Displays the active ban information for a user.
            /// </summary>
            /// <param name="username">The name of the user to show the ban info for.</param>
            [SayCommand("BanInfo")]
            public void BanInfo(string username)
            {
                if (!RequirePermissionLevel(UserPermissions.LesserAdmin))
                    return;

                var banInfos = BanningManager.Instance.GetAccountBanInfo(username).Where(x => !x.Expired);

                var count = banInfos.Count();
                UserChat(string.Format("Active bans on user `{0}`: {1}", username, count));

                var i = 1;
                foreach (var banInfo in banInfos)
                {
                    UserChat(banInfo, i++, count);
                }
            }

            /// <summary>
            /// Bans a user.
            /// </summary>
            /// <param name="username">The name of the user to ban.</param>
            /// <param name="duration">For how long to ban the user. For duration format, see <see cref="DurationParser"/>.</param>
            /// <param name="reason">The reason the user is to be banned.</param>
            [SayCommand("BanUser")]
            public void BanUser(string username, string duration, string reason)
            {
                if (!RequirePermissionLevel(UserPermissions.LesserAdmin))
                    return;

                // Check for valid parameters
                if (!GameData.UserName.IsValid(username))
                {
                    User.Send(GameMessage.CommandGeneralInvalidUser, ServerMessageType.GUI, username);
                    return;
                }

                if (string.IsNullOrEmpty(duration))
                {
                    User.Send(GameMessage.CommandGeneralInvalidParameter, ServerMessageType.GUI, "duration");
                    return;
                }

                // Parse the duration
                TimeSpan dur;
                string parseFailReason;
                if (!DurationParser.TryParse(duration, out dur, out parseFailReason))
                {
                    User.Send(GameMessage.CommandGeneralInvalidParameterEx, ServerMessageType.GUI, "duration", parseFailReason);
                    return;
                }

                // Perform the ban and notify the issuer if it was successful or not
                BanManagerFailReason banFailReason;
                if (!BanningManager.Instance.TryAddUserBan(username, dur, reason, User.Name, out banFailReason))
                {
                    // Ban failed
                    User.Send(GameMessage.BanUserFailed, ServerMessageType.GUI, username, banFailReason.GetDetailedString());
                }
                else
                {
                    // Ban successful
                    User.Send(GameMessage.BanUserSuccessful, ServerMessageType.GUI, username);
                }
            }

            /// <summary>
            /// Creates an instance of an item from a template and adds it to your inventory. Any items that cannot
            /// fit into the caller's inventory are destroyed.
            /// </summary>
            /// <param name="id">The ID of the item template to use.</param>
            /// <param name="amount">The number of items to create.</param>
            [SayCommand("CreateItem")]
            public void CreateItem(ItemTemplateID id, byte amount)
            {
                if (!RequirePermissionLevel(UserPermissions.LesserAdmin))
                    return;

                // Get the item template
                var template = ItemTemplateManager.Instance[id];
                if (template == null)
                {
                    UserChat("Invalid item template ID: " + id);
                    return;
                }

                // Create the item
                var item = new ItemEntity(template, amount);

                // Give to user
                var remainder = User.Inventory.Add(item);

                // Delete any that failed to be added
                if (remainder != null)
                {
                    UserChat(remainder.Amount + " units could not be added to your inventory.");
                    remainder.Dispose();
                }
            }

            /// <summary>
            /// Desummons all NPCs on the current map.
            /// </summary>
            [SayCommand("Dethrall")]
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
            /// Kicks the specified user from the world.
            /// </summary>
            /// <param name="userName">The player to kick.</param>
            /// <param name="reason">The reason the player is being kicked.</param>
            [SayCommand("Kick")]
            public void Kick(string userName, string reason)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                // Get the user we want
                var target = World.FindUser(userName);

                // Check that the user could be found
                if (target == null)
                {
                    User.Send(GameMessage.CommandTellInvalidUser, ServerMessageType.GUIChat);
                    return;
                }

                // User was found, so disconnect them and give the reason for the disconnect
                target.Conn.Disconnect(GameMessage.DisconnectUserKicked, reason);
            }

            /// <summary>
            /// Kills the specified user.
            /// </summary>
            /// <param name="userName">The player to kill.</param>
            [SayCommand("Kill")]
            public void Kill(string userName)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                // Get the user we want
                var target = World.FindUser(userName);

                // Check that the user could be found
                if (target == null)
                {
                    User.Send(GameMessage.CommandTellInvalidUser, ServerMessageType.GUIChat);
                    return;
                }

                target.Kill();
            }

            /// <summary>
            /// Warps the specified player to the user of the command.
            /// </summary>
            /// <param name="userName">The name of the player to summon.</param>
            [SayCommand(("Summon"))]
            public void Summon(string userName)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                // Get the user we want
                var target = World.FindUser(userName);

                // Check that the user could be found
                if (target == null)
                {
                    User.Send(GameMessage.CommandTellInvalidUser, ServerMessageType.GUIChat);
                    return;
                }

                // Target user was found, so teleport them to the user that issued the command 
                target.Teleport(User.Map, User.Position);
            }

            /// <summary>
            /// Summons NPCs on the current map.
            /// </summary>
            /// <param name="id">The ID of the NPCs to spawn.</param>
            /// <param name="amount">The amount of NPCs to spawn.</param>
            [SayCommand("Thrall")]
            public void Thrall(CharacterTemplateID id, int amount)
            {
                if (!RequirePermissionLevel(UserPermissions.LesserAdmin))
                    return;

                Rectangle thrallArea = new Rectangle();
                bool useThrallArea = false;

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

                    thrallArea = new Rectangle((int)minX, (int)y, (int)(maxX - minX + 1), 1);
                }

                for (var i = 0; i < amount; i++)
                {
                    // Create a ThralledNPC and add it to the world
                    var npc = new ThralledNPC(World, CharacterTemplateManager.Instance[id], User.Map, User.Position);
                    
                    // When using the thrallArea, move the NPC to the correct area
                    if (useThrallArea)
                    {
                        var npcSize = npc.Size;
                        int minX = thrallArea.Left;
                        int maxX = thrallArea.Right - (int)npcSize.X;
                        int x;

                        if (maxX <= minX)
                        {
                            x = minX;
                        }
                        else
                        {
                            x = RandomHelper.NextInt(minX, maxX);
                        }
                        
                        int y = thrallArea.Y - (int)npc.Size.Y;
                        npc.Teleport(new Vector2(x, y));
                    }
                }
            }

            /// <summary>
            /// Warps the user to the specified map and position.
            /// </summary>
            /// <param name="mapId">The mapID to be warped to.</param>
            /// <param name="x">The position along the x-axis to be warped to.</param>
            /// <param name="y">The position along the y-axis to be warped to.</param>
            [SayCommand(("Warp"))]
            public void Warp(MapID mapId, int x, int y)
            {
                if (!RequirePermissionLevel(UserPermissions.Moderator))
                    return;

                // Check for a valid map
                if (!MapBase.IsMapIDValid(mapId))
                {
                    UserChat("Invalid map ID: " + mapId);
                    return;
                }

                // Move the user
                User.Teleport(World.GetMap(mapId), new Vector2(x, y));
            }

            #endregion

            #region ISayCommands<User> Members

            /// <summary>
            /// Gets or sets the User that the current command came from.
            /// </summary>
            public User User { get; set; }

            #endregion
        }
    }
}