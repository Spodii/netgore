using System;
using System.Linq;

namespace NetGore.Features.Friends
{
    public interface IFriend : IDisposable
    {
        /// <summary>
        /// Gets or sets the guild member's current guild. Will be null if they are not part of any guilds.
        /// This value should only be set by the <see cref="IGuildManager"/>. When the value is changed,
        /// <see cref="IGuild.RemoveOnlineMember"/> should be called for the old value (if not null) and
        /// <see cref="IGuild.AddOnlineMember"/> should be called for the new value (if not null).
        /// </summary>
        IFriendList friendList { get; set; }

        /// <summary>
        /// Gets an ID that can be used to distinguish this <see cref="IGuildMember"/> from any other
        /// <see cref="IGuildMember"/> instance.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Gets the unique name of the guild member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Notifies the guild member that they have been invited to join a guild.
        /// </summary>
        /// <param name="inviter">The guild member that invited them into the guild.</param>
        /// <param name="guild">The guild they are being invited to join.</param>
        void SendFriendRequest(IFriend inviter, IFriendList friend);
    }
}
