using System.Linq;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Guilds
{
    /// <summary>
    /// A container that assists in managing the guild state for guild members.
    /// </summary>
    public class GuildMemberInfo : GuildMemberInfo<User>
    {
        static readonly GuildSettings _guildSettings = GuildSettings.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberInfo"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IGuildMember"/> that this <see cref="GuildMemberInfo{T}"/>
        /// will be handling the state values for.</param>
        public GuildMemberInfo(User owner) : base(owner)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> is demoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected override void HandleDemotion(GuildRank rank)
        {
            if (!Owner.IsLoaded)
                return;

            Owner.Send(GameMessage.GuildDemotion, ServerMessageType.GUI, _guildSettings.GetRankName(rank));
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> joins a guild.
        /// </summary>
        /// <param name="guild">The guild that was joined.</param>
        protected override void HandleJoinGuild(IGuild guild)
        {
            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteGuildInfo(x, guild)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            if (!Owner.IsLoaded)
                return;

            Owner.Send(GameMessage.GuildJoin, ServerMessageType.GUI, guild.Name);

            WorldStatsTracker.Instance.AddUserGuildChange(Owner, guild.ID);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> leaves a guild.
        /// </summary>
        /// <param name="guild">The guild that was left.</param>
        protected override void HandleLeaveGuild(IGuild guild)
        {
            using (var pw = ServerPacket.GuildInfo(x => UserGuildInformation.WriteGuildInfo(x, null)))
            {
                Owner.Send(pw, ServerMessageType.GUI);
            }

            if (!Owner.IsLoaded)
                return;

            Owner.Send(GameMessage.GuildLeave, ServerMessageType.GUI, guild.Name);

            WorldStatsTracker.Instance.AddUserGuildChange(Owner, null);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the <see cref="GuildMemberInfo{T}.Owner"/> is promoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected override void HandlePromotion(GuildRank rank)
        {
            if (!Owner.IsLoaded)
                return;

            Owner.Send(GameMessage.GuildPromotion, ServerMessageType.GUI, _guildSettings.GetRankName(rank));
        }

        /// <summary>
        /// Gets if the <see cref="GuildMemberInfo{T}.Owner"/> is only having their guild values set because they are
        /// loading, not because they are joining/leaving a guild.
        /// </summary>
        /// <returns>True if they <see cref="GuildMemberInfo{T}.Owner"/> is loading; otherwise false.</returns>
        protected override bool IsLoading()
        {
            return !Owner.IsLoaded;
        }
    }
}