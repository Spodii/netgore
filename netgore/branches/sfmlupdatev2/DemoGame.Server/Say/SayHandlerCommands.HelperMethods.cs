using System;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Features.Groups;
using NetGore.Features.Guilds;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> helper methods. Does not contain any actual commands.
    /// </summary>
    public partial class SayHandlerCommands
    {
        /// <summary>
        /// Checks if the user meets the required guild rank.
        /// </summary>
        /// <param name="requiredRank">The required guild rank.</param>
        /// <returns>If false, the command should be aborted.</returns>
        bool CheckGuildPermissions(GuildRank requiredRank)
        {
            if (((IGuildMember)User).GuildRank < requiredRank)
            {
                User.Send(GameMessage.GuildInsufficientPermissions, ServerMessageType.GUI, GuildSettings.GetRankName(requiredRank));
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
    }
}