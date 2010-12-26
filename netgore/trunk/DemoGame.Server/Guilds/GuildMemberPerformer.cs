using System;
using System.Linq;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Guilds
{
    public class GuildMemberPerformer : GuildMemberPerformerBase
    {
        static readonly GuildManager _guildManager = GuildManager.Instance;

        readonly Func<string, IGuildMember> _findGuildMember;
        readonly SelectGuildMemberByNameQuery _selectGuildMemberQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildMemberPerformerBase"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <param name="findGuildMember">The <see cref="Func{T,U}"/> used to find a guild member by name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbController" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="findGuildMember" /> is <c>null</c>.</exception>
        public GuildMemberPerformer(IDbController dbController, Func<string, IGuildMember> findGuildMember)
            : base(GetSaveHandler(dbController))
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");
            if (findGuildMember == null)
                throw new ArgumentNullException("findGuildMember");

            _findGuildMember = findGuildMember;
            _selectGuildMemberQuery = dbController.GetQuery<SelectGuildMemberByNameQuery>();
        }

        /// <summary>
        /// Creates an <see cref="Action{T}"/> needed for the <see cref="GuildMemberPerformer"/> to save the guild
        /// state of a <see cref="IGuildMember"/>.
        /// </summary>
        /// <param name="dbController"></param>
        /// <returns></returns>
        static Action<IGuildMember> GetSaveHandler(IDbController dbController)
        {
            var replaceQuery = dbController.GetQuery<InsertGuildMemberQuery>();
            var deleteQuery = dbController.GetQuery<DeleteGuildMemberQuery>();

            return delegate(IGuildMember target)
            {
                if (target.Guild == null)
                {
                    var id = new CharacterID(target.ID);
                    deleteQuery.Execute(id);
                }
                else
                {
                    var id = new CharacterID(target.ID);
                    var guildID = target.Guild.ID;
                    var rank = target.GuildRank;
                    var args = new InsertGuildMemberQuery.QueryArgs(id, guildID, rank);
                    replaceQuery.Execute(args);
                }
            };
        }

        /// <summary>
        /// When overridden in the derived class, tries to acquire the guild member with the given <paramref name="name"/>
        /// who's object already exists in memory.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGuildMember"/> to get.</param>
        /// <param name="guildMember">When this method returns true, contains the <see cref="IGuildMember"/>
        /// for the guild member with the given <paramref name="name"/>.</param>
        /// <returns>True if the <see cref="IGuildMember"/> with the given <paramref name="name"/> was successfully
        /// loaded; otherwise false.</returns>
        protected override bool TryGetGuildMember(string name, out IGuildMember guildMember)
        {
            guildMember = _findGuildMember(name);
            return guildMember != null;
        }

        /// <summary>
        /// When overridden in the derived class, tries to load the guild member with the given <paramref name="name"/>
        /// from an external data source (such as the database). This will only be called when
        /// <see cref="GuildMemberPerformerBase.TryGetGuildMember"/> fails.
        /// </summary>
        /// <param name="name">The name of the <see cref="IGuildMember"/> to get.</param>
        /// <param name="values">When this method returns true, contains the needed loaded values for the
        /// <see cref="IGuildMember"/>.</param>
        /// <returns>True if the <see cref="IGuildMember"/> with the given <paramref name="name"/> was successfully
        /// loaded; otherwise false.</returns>
        protected override bool TryLoadGuildMember(string name, out TemporaryGuildMemberPoolValues values)
        {
            var v = _selectGuildMemberQuery.Execute(name);
            if (v == null)
            {
                values = new TemporaryGuildMemberPoolValues();
                return false;
            }

            var guild = _guildManager.GetGuild(v.GuildID);
            if (guild == null)
            {
                values = new TemporaryGuildMemberPoolValues();
                return false;
            }

            values = new TemporaryGuildMemberPoolValues(name, (int)v.CharacterID, guild, v.Rank);
            return true;
        }
    }
}