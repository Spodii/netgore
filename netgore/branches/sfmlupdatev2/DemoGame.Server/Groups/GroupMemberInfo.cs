using System.Linq;
using NetGore.Features.Groups;

namespace DemoGame.Server.Groups
{
    /// <summary>
    /// A container that assists in managing the group state for group members.
    /// </summary>
    public class GroupMemberInfo : GroupMemberInfo<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMemberInfo"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public GroupMemberInfo(User owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets the name to use for a group's founder.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>The name to use for a group's founder.</returns>
        static string GetGroupFounderName(IGroup group)
        {
            return GetGroupMemberName(group.Founder);
        }

        /// <summary>
        /// Gets the name to use for a group's member.
        /// </summary>
        /// <param name="member">The group member.</param>
        /// <returns>The name to use for a group's member.</returns>
        static string GetGroupMemberName(IGroupable member)
        {
            var asCharacter = member as Character;

            if (asCharacter != null)
                return asCharacter.Name;
            else
                return member.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when the owner
        /// fails to join a group they were invited to.
        /// </summary>
        /// <param name="group">The group they failed to join.</param>
        protected override void OnFailedJoinGroup(IGroup group)
        {
            var founderName = GetGroupFounderName(group);

            // Give some more details on why they failed to join the group, if possible. Otherwise, just use the
            // generic "failed to join" message (which is not ideal, but better than nothing).
            if (group.Members.Count() == GroupSettings.Instance.MaxMembersPerGroup)
                Owner.Send(GameMessage.GroupJoinFailedGroupIsFull, ServerMessageType.GUI, founderName);
            else
                Owner.Send(GameMessage.GroupJoinFailedUnknownReason, ServerMessageType.GUI, founderName);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a group member
        /// joins the group the <see cref="GroupMemberInfo{T}.Owner"/> is currently in.
        /// </summary>
        /// <param name="groupMember">The group member that joined the group. This will never be equal to
        /// the <see cref="GroupMemberInfo{T}.Owner"/>. That is, we will only receive events related to other
        /// group members in our group.</param>
        protected override void OnGroupMemberJoined(IGroupable groupMember)
        {
            using (var pw = ServerPacket.GroupInfo(x => UserGroupInformation.WriteAddMember(x, groupMember)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            Owner.Send(GameMessage.GroupMemberJoined, ServerMessageType.GUI, GetGroupMemberName(groupMember));
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when a group member
        /// leaves the group the <see cref="GroupMemberInfo{T}.Owner"/> is currently in.
        /// </summary>
        /// <param name="groupMember">The group member that joined the group. This will never be equal to
        /// the <see cref="GroupMemberInfo{T}.Owner"/>. That is, we will only receive events related to other
        /// group members in our group.</param>
        protected override void OnGroupMemberLeft(IGroupable groupMember)
        {
            using (var pw = ServerPacket.GroupInfo(x => UserGroupInformation.WriteRemoveMember(x, groupMember)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            Owner.Send(GameMessage.GroupMemberLeft, ServerMessageType.GUI, GetGroupMemberName(groupMember));
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when the owner joins a group.
        /// </summary>
        /// <param name="group">The group the owner joined.</param>
        protected override void OnJoinGroup(IGroup group)
        {
            using (var pw = ServerPacket.GroupInfo(x => UserGroupInformation.WriteGroupInfo(x, group)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            // Don't send the message if we join our own group (since we already told we created the group)
            if (group.Founder != Owner)
                Owner.Send(GameMessage.GroupJoined, ServerMessageType.GUI, GetGroupFounderName(group));
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling of when the owner leaves a group.
        /// </summary>
        /// <param name="group">The group the owner left.</param>
        protected override void OnLeaveGroup(IGroup group)
        {
            using (var pw = ServerPacket.GroupInfo(x => UserGroupInformation.WriteGroupInfo(x, null)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            Owner.Send(GameMessage.GroupLeave, ServerMessageType.GUI);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling for when the owner
        /// is invited to a group.
        /// </summary>
        /// <param name="group">The group they were invited to.</param>
        protected override void OnReceiveInvite(IGroup group)
        {
            Owner.Send(GameMessage.GroupInvited, ServerMessageType.GUI, GetGroupFounderName(group));
        }
    }
}