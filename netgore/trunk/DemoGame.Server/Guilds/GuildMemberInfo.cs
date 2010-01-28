using System.Linq;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Guilds
{
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
        /// When overridden in the derived class, handles when the owner is demoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected override void HandleDemotion(GuildRank rank)
        {
            Owner.Send(GameMessage.GuildDemotion, _guildSettings.GetRankName(rank));
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner joins a guild.
        /// </summary>
        /// <param name="guild">The guild that was joined.</param>
        protected override void HandleJoinGuild(IGuild guild)
        {
            Owner.Send(GameMessage.GuildJoin, guild.Name);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner leaves a guild.
        /// </summary>
        /// <param name="guild">The guild that was left.</param>
        protected override void HandleLeaveGuild(IGuild guild)
        {
            Owner.Send(GameMessage.GuildLeave, guild.Name);
        }

        /// <summary>
        /// When overridden in the derived class, handles when the owner is promoted.
        /// </summary>
        /// <param name="rank">The new rank.</param>
        protected override void HandlePromotion(GuildRank rank)
        {
            Owner.Send(GameMessage.GuildPromotion, _guildSettings.GetRankName(rank));
        }
    }
}