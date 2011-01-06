using System.Linq;
using NetGore.Features.Groups;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> for groups.
    /// </summary>
    public partial class SayHandlerCommands
    {
        /// <summary>
        /// Creates a new group.
        /// </summary>
        [SayHandlerCommand("CreateGroup")]
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
        [SayHandlerCommand("GroupInvite")]
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
        [SayHandlerCommand("JoinGroup")]
        public void JoinGroup()
        {
            if (!RequireNotInGroup())
                return;

            User.TryJoinGroup();
        }

        /// <summary>
        /// Leaves the current group.
        /// </summary>
        [SayHandlerCommand("LeaveGroup")]
        public void LeaveGroup()
        {
            if (!RequireInGroup())
                return;

            ((IGroupable)User).Group.RemoveMember(User);
        }

        /// <summary>
        /// Sends a message to all group members.
        /// </summary>
        /// <param name="message">The message to send.</param>
        [SayHandlerCommand("GroupSay")]
        public void PartySay(string message)
        {
            if (!RequireInGroup())
                return;

            foreach (var groupMember in User.Group.Members.OfType<User>())
            {
                groupMember.Send(GameMessage.GroupSay, ServerMessageType.GUIChat, User.Name, message);
            }
        }
    }
}