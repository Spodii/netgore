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
        event GuildEventHandler OnDestroy;

        /// <summary>
        /// Notifies listeners when a member has been invited into the guild.
        /// </summary>
        event GuildInvokeEventWithTargetHandler OnInviteMember;
        
        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of online guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        bool TryViewOnlineMembers(IGuildMember invoker);
        
        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        bool TryViewMembers(IGuildMember invoker);
        
        /// <summary>
        /// Destroys the guild completely and removes all members from it.
        /// </summary>
        void DestroyGuild();

        /// <summary>
        /// Notifies listeners when a member has been kicked from the guild.
        /// </summary>
        event GuildInvokeEventWithTargetHandler OnKickMember;

        /// <summary>
        /// Notifies listeners when a member has been promoted.
        /// </summary>
        event GuildInvokeEventWithTargetHandler OnPromoteMember;

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
        /// Adds the reference of an online guild member to this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to add.</param>
        void AddOnlineMember(IGuildMember member);

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
        /// <param name="newName">The new name of the guild.</param>
        /// <returns>True if the name was successfully changed; otherwise false.</returns>
        bool TryChangeName(string newName);

        /// <summary>
        /// Tries to change the tag of the guild.
        /// </summary>
        /// <param name="newTag">The new tag of the guild.</param>
        /// <returns>True if the tag was successfully changed; otherwise false.</returns>
        bool TryChangeTag(string newTag);

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
    }
}