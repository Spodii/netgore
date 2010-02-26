using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Features.Friends
{
    public interface IFriendList : IDisposable
    {
        /// <summary>
        /// Notifies listeners when a new member has joined the guild.
        /// </summary>
        event FriendEventHandler MemberAdded;

        /// <summary>
        /// Gets the online guild members in this guild.
        /// </summary>
        IEnumerable<IFriend> OnlineFriends { get; }

        /// <summary>
        /// Adds the reference of an online guild member to this guild that is new to the guild.
        /// This does not make the user join or leave the guild in any way, just allows the guild to keep track of the
        /// members that are online.
        /// </summary>
        /// <param name="newMember">The online guild member to add.</param>
        void AddNewOnlineFriend(IFriend newFriend);

        /// <summary>
        /// Gets the name and rank for all the members in the guild.
        /// </summary>
        /// <returns>The name and rank for all the members in the guild.</returns>
        IEnumerable<IFriendList> GetFriends();

        /// <summary>
        /// Removes the reference of an online guild member from this guild. This does not make the user join or leave the
        /// guild in any way, just allows the guild to keep track of the members that are online.
        /// </summary>
        /// <param name="member">The online guild member to remove.</param>
        void RemoveOnlineFriend(IFriend friend);

        /// <summary>
        /// When overridden in the derived class, saves all of the guild's information to the database.
        /// </summary>
        void Save();

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to invite the <paramref name="target"/> to the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who inviting the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being invited to the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully invited the <paramref name="target"/>
        /// to the guild; otherwise false.</returns>
        bool TryAddFriend(IFriend invoker, IFriend target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to kick the <paramref name="target"/> out of the guild.
        /// </summary>
        /// <param name="invoker">The guild member is who kicking out the <paramref name="target"/>.</param>
        /// <param name="target">The guild member being kicked out of the guild.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully kicked the <paramref name="target"/>
        /// out of the guild; otherwise false.</returns>
        bool TryRemoveFriend(IFriend invoker, IFriend target);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the event log for the guild.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the log; otherwise false.</returns>
        bool TryViewEventLog(IFriend invoker);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the member list; otherwise false.</returns>
        bool TryViewFriends(IFriend invoker);

        /// <summary>
        /// Makes the <paramref name="invoker"/> try to view the list of online guild members.
        /// </summary>
        /// <param name="invoker">The guild member that invoked the event.</param>
        /// <returns>True if the <paramref name="invoker"/> successfully viewed the online member list; otherwise false.</returns>
        bool TryViewOnlineFriends(IFriend invoker);
    }
}