using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Features.Guilds;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> for guilds.
    /// </summary>
    public partial class SayHandlerCommands
    {
        /// <summary>
        /// Creates a new guild.
        /// </summary>
        /// <param name="name">The name of the guild.</param>
        /// <param name="tag">The guild tag.</param>
        [SayHandlerCommand("CreateGuild")]
        public void CreateGuild(string name, string tag)
        {
            if (!RequireUserNotInGuild())
                return;

            // Valid name
            if (!GuildSettings.IsValidName(name))
            {
                User.Send(GameMessage.GuildCreationFailedNameInvalid, ServerMessageType.GUI, name);
                return;
            }

            if (!GuildManager.IsNameAvailable(name))
            {
                User.Send(GameMessage.GuildCreationFailedNameNotAvailable, ServerMessageType.GUI, name);
                return;
            }

            // Valid tag
            if (!GuildSettings.IsValidTag(tag))
            {
                User.Send(GameMessage.GuildCreationFailedTagInvalid, ServerMessageType.GUI, tag);
                return;
            }

            if (!GuildManager.IsTagAvailable(tag))
            {
                User.Send(GameMessage.GuildCreationFailedTagNotAvailable, ServerMessageType.GUI, tag);
                return;
            }

            // Create
            var guild = GuildManager.TryCreateGuild(User, name, tag);
            if (guild == null)
                User.Send(GameMessage.GuildCreationFailedUnknownReason, ServerMessageType.GUI, name, tag);
            else
                User.Send(GameMessage.GuildCreationSuccessful, ServerMessageType.GUI, name, tag);
        }

        /// <summary>
        /// Demotes the guild rank of another member in the guild.
        /// </summary>
        /// <param name="userName">The name of the fellow guild member to demote.</param>
        [SayHandlerCommand("Demote")]
        public void Demote(string userName)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankDemote))
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
        [SayHandlerCommand("GuildHelp")]
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
        [SayHandlerCommand("GuildInvite")]
        public void GuildInvite(string toInvite)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankInvite))
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
        [SayHandlerCommand("GuildKick")]
        public void GuildKick(string userName)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankKick))
                return;

            World.GuildMemberPerformer.Perform(userName, x => GuildMemberPerformer_GuildKick(x, userName));
        }

        /// <summary>
        /// Displays the latest entries of the guild log.
        /// </summary>
        [SayHandlerCommand("GuildLog")]
        public void GuildLog()
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankViewLog))
                return;

            User.Guild.TryViewEventLog(User);
        }

        /// <summary>
        /// Displays all of the guild members.
        /// </summary>
        [SayHandlerCommand("GuildMembers")]
        public void GuildMembers()
        {
            if (!RequireUserInGuild())
                return;

            User.Guild.TryViewMembers(User);
        }

        /// <summary>
        /// Displays the guild members that are currently online.
        /// </summary>
        [SayHandlerCommand("GuildOnline")]
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
        [SayHandlerCommand("GuildSay")]
        public void GuildSay(string message)
        {
            if (!RequireUserInGuild())
                return;

            foreach (var guildMember in User.Guild.OnlineMembers.OfType<User>())
            {
                guildMember.Send(GameMessage.GuildSay, ServerMessageType.GUIChat, User.Name, message);
            }
        }

        /// <summary>
        /// Accepts an invitation to join a guild.
        /// </summary>
        /// <param name="guildName">The name of the guild to join.</param>
        [SayHandlerCommand("JoinGuild")]
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
        [SayHandlerCommand("LeaveGuild")]
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
        [SayHandlerCommand("Promote")]
        public void Promote(string userName)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankPromote))
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
        [SayHandlerCommand("RenameGuild")]
        public void RenameGuild(string newName)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankRename))
                return;

            if (!GuildSettings.IsValidName(newName))
            {
                User.Send(GameMessage.GuildRenameFailedInvalidValue, ServerMessageType.GUI, newName);
                return;
            }

            if (!GuildManager.IsNameAvailable(newName))
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
        [SayHandlerCommand("RetagGuild")]
        public void RetagGuild(string newTag)
        {
            if (!RequireUserInGuild() || !CheckGuildPermissions(GuildSettings.MinRankRename))
                return;

            if (!GuildSettings.IsValidTag(newTag))
            {
                User.Send(GameMessage.GuildRetagFailedInvalidValue, ServerMessageType.GUI, newTag);
                return;
            }

            if (!GuildManager.IsTagAvailable(newTag))
            {
                User.Send(GameMessage.GuildRetagFailedNameNotAvailable, ServerMessageType.GUI, newTag);
                return;
            }

            if (!User.Guild.TryChangeTag(User, newTag))
                User.Send(GameMessage.GuildRetagFailedUnknownReason, ServerMessageType.GUI, newTag);
        }
    }
}