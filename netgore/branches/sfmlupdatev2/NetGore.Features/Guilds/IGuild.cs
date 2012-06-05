using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Guilds
{
    /// <summary>
    /// Interface for a single guild.
    /// </summary>
    public interface IGuild : IDisposable
    {
        /// <summary>
        /// Notifies listeners when the guild has been destroyed.
        /// </summary>
        event TypedEventHandler<IGuild> Destroyed;

        /// <summary>
        /// Notifies listeners when a new member has joined the guild.
        /// </summary>
        event TypedEventHandler<IGuild, EventArgs<IGuildMember>> MemberAdded;

        /// <summary>
        /// Notifies listeners when a member has been demoted.
        /// </summary>
        event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberDemoted;

        /// <summary>
        /// Notifies listeners when a member has been invited into the guild.
        /// </summary>
        event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberInvited;

        /// <summary>
        /// Notifies listeners when a member has been kicked from the guild.
        /// </summary>
        event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberKicked;

        /// <summary>
        /// Notifies listeners when a member has been promoted.
        /// </summary>
        event TypedEventHandler<IGuildMember, EventArgs<IGuildMember>> MemberPromoted;

        /// <summary>
        /// Notifies listeners when the guild's name has been changed.
        /// </summary>
        event TypedEventHandler<IGuild, GuildRenameEventArgs> NameChanged;

        /// <summary>
        /// Notifies listeners when a member of this guild has come online.
        /// </summary>
        event TypedEventHandler<IGuild, EventArgs<IGuildMember>> OnlineUserAdded;

        /// <summary>
        /// Notifies listeners when a member of this guild has gone offline.
        /// </summary>
        event TypedEventHandler<IGuild, EventArgs<IGuildMember>> OnlineUserRemoved;

        /// <summary>
        /// Notifies listeners when the guild's tag has been changed.
        /// </summary>
        event TypedEventHandler<IGuild, GuildRenameEventArgs> TagChanged;

        /// <summary>
        /// Gets the <see cref="IGuildManager"/> managing this guild.
        /// </summary>
        IGuildManager GuildManager { get; }

        /// <summary>
        /// Gets the unique ID of the guild.
        /// </summary>
        GuildID ID { get; }

        /// <summary>
        /// Gets if this guild has been destroyed. If this is true, nobody should be in this guild.
        /// </summary>
        bool IsDestroyed { get; }

        /// <summary>
        /// Gets the unique name of the guild.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the online guild members in this guild.
        /// </summary>
        IEnumerable<IGuildMember> OnlineMembers { get; }

        /// <summary>
        /// Gets the guild's unique tag.
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// Adds the reference of an online guild member to this guild that is new to the guild.
        /// This does not make the user join or leave the guild in any way, just allows the guild to keep track of the
        /// members that are online.
        /// </summary>
        /// <param name="newMember">The online guild member to add.</param>
        void AddNewOnlineMember(IGuildMember newMember);

        /// <summary>
        /// Adds the reference of an online guild member to this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to add.</param>
        void AddOnlineMember(IGuildMember member);

        /// <summary>
        /// Destroys the guild completely and removes all members from it.
        /// </summary>
        void DestroyGuild();

        /// <summary>
        /// Gets the name and rank for all the members in the guild.
        /// </summary>
        /// <returns>The name and rank for all the members in the guild.</returns>
        IEnumerable<GuildMemberNameRank> GetMembers();

        /// <summary>
        /// Removes the reference of an online guild member from this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to remove.</param>
        void RemoveOnlineMember(IGuildMember member);

        /// <summary>
        /// When overridden in the derived class, saves all of the guild's information to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Tries to change the name of the guild.
        /// </summary>
        /// <param name="invoker">The guild member trying to change the guild's name.</param>
        /// <param name="newName">The new name of the guild.</param>
        /// <returns>True if the name was successfully changed; otherwise false.</returns>
        bool TryChangeName(IGuildMember invoker, string newName);

        /// <summary>
        /// Tries to change the tag of the guild.
        /// </summary>
        /// <param name="invoker">The guild member trying to change the guild's name.</param>
        /// <param name="newTag">The new tag of the guild.</param>
        /// <returns>True if the tag was successfully changed; otherwise false.</returns>
        bool TryChangeTag(IGuildMember invoker, string newTag);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to demote the <paramref name="target"/>.
        /// </summary>
        /// <param name="invoker">The guild member is who demoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being demoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully demoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        bool TryDemoteMember(IGuildMember invoker, IGuildMember target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to invite the <paramref name="target"/> to the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who inviting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being invited to the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully invited the <paramref name="target"/>
        /// to the guild; otherwise false.</returns>
        bool TryInviteMember(IGuildMember invoker, IGuildMember target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to kick the <paramref name="target"/> out of the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who kicking out the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being kicked out of the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully kicked the <paramref name="target"/>
        /// out of the guild; otherwise false.</returns>
        bool TryKickMember(IGuildMember invoker, IGuildMember target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to leave the guild.
        /// </summary>
        /// <param name="invoker">The guild member who is leaving.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully left the guild; otherwise false.</returns>
        bool TryLeaveGuild(IGuildMember invoker);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to promote the <paramref name="target"/>.
        /// </summary>
        /// <param name="invoker">The guild member is who promoting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being promoted.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully promoted the <paramref name="target"/>;
        /// otherwise false.</returns>
        bool TryPromoteMember(IGuildMember invoker, IGuildMember target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the event log for the guild.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the log; otherwise false.</returns>
        bool TryViewEventLog(IGuildMember invoker);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        bool TryViewMembers(IGuildMember invoker);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of online guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        bool TryViewOnlineMembers(IGuildMember invoker);
    }
}