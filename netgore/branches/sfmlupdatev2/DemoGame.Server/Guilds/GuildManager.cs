using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Db;
using NetGore.Features.Guilds;

namespace DemoGame.Server.Guilds
{
    public class GuildManager : GuildManagerBase<Guild>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly GuildManager _instance;

        readonly IDbController _dbController;
        readonly InsertGuildEventQuery _insertGuildEventQuery;
        readonly InsertGuildQuery _insertGuildQuery;
        readonly SelectGuildByNameQuery _selectGuildByNameQuery;
        readonly SelectGuildByTagQuery _selectGuildByTagQuery;

        /// <summary>
        /// Initializes the <see cref="GuildManager"/> class.
        /// </summary>
        static GuildManager()
        {
            _instance = new GuildManager(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuildManagerBase{T}"/> class.
        /// </summary>
        GuildManager(IDbController dbController)
        {
            _dbController = dbController;

            // Cache the queries this object will be using
            _selectGuildByNameQuery = _dbController.GetQuery<SelectGuildByNameQuery>();
            _selectGuildByTagQuery = _dbController.GetQuery<SelectGuildByTagQuery>();
            _insertGuildEventQuery = _dbController.GetQuery<InsertGuildEventQuery>();
            _insertGuildQuery = _dbController.GetQuery<InsertGuildQuery>();

            Initialize();
        }

        /// <summary>
        /// Gets the <see cref="GuildManager"/> instance.
        /// </summary>
        public static GuildManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="name"/> is an available guild name.
        /// </summary>
        /// <param name="name">The guild name to check if available.</param>
        /// <returns>True if the <paramref name="name"/> is available; otherwise false.</returns>
        protected override bool InternalIsNameAvailable(string name)
        {
            return _selectGuildByNameQuery.Execute(name) == null;
        }

        /// <summary>
        /// When overridden in the derived class, gets if the <paramref name="tag"/> is an available guild tag.
        /// </summary>
        /// <param name="tag">The guild tag to check if available.</param>
        /// <returns>True if the <paramref name="tag"/> is available; otherwise false.</returns>
        protected override bool InternalIsTagAvailable(string tag)
        {
            return _selectGuildByTagQuery.Execute(tag) == null;
        }

        /// <summary>
        /// When overridden in the derived class, logs an event from a guild.
        /// </summary>
        /// <param name="eventCreator">The guild member that created the event.</param>
        /// <param name="guildEvent">The type of event that took place.</param>
        /// <param name="eventTarget">Optionally contains the other guild member that the event involves. This member
        /// may or may not actually be in the guild at this point.</param>
        /// <param name="arg0">The optional first argument string.</param>
        /// <param name="arg1">The optional second argument string.</param>
        /// <param name="arg2">The optional third argument string.</param>
        protected override void InternalLogEvent(IGuildMember eventCreator, GuildEvents guildEvent, IGuildMember eventTarget,
                                                 string arg0, string arg1, string arg2)
        {
            var guildID = eventCreator.Guild.ID;
            var charID = (CharacterID)eventCreator.ID;
            var eventID = (byte)guildEvent;
            var targetID = (eventTarget == null ? null : (CharacterID?)eventTarget.ID);

            var args = new InsertGuildEventQuery.QueryArgs(guildID, charID, targetID, eventID, arg0, arg1, arg2);
            _insertGuildEventQuery.Execute(args);
        }

        /// <summary>
        /// Tries to create a new guild.
        /// </summary>
        /// <param name="creator">The one trying to create the guild.</param>
        /// <param name="name">The name of the guild to create.</param>
        /// <param name="tag">The tag for the guild to create.</param>
        /// <returns>The created guild instance if successfully created, or null if the guild could not
        /// be created.</returns>
        protected override Guild InternalTryCreateGuild(IGuildMember creator, string name, string tag)
        {
            // Let the database assign the ID for us
            var dummyID = new GuildID(_dbController.ConnectionPool.AutoIncrementValue);

            // We want to insert the guild into the database first since if that query fails, we know that
            // we can't create the guild with the given values for whatever reason
            var values = new GuildTable(iD: dummyID, name: name, tag: tag, created: DateTime.Now);
            long lastInsertedId;
            var rowsAffected = _insertGuildQuery.ExecuteWithResult(values, out lastInsertedId);

            if (rowsAffected <= 0)
            {
                const string errmsg =
                    "Failed to create guild using values (name={0}, tag={1}) - insert query" +
                    " returned `{2}` rows affected. Most likely one of the values are not unique.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, name, tag, rowsAffected);
                return null;
            }

            // Set the guild ID
            Debug.Assert(lastInsertedId <= int.MaxValue);
            Debug.Assert(lastInsertedId >= int.MinValue);
            values.ID = new GuildID((int)lastInsertedId);

            // Create the guild instance using the values we just inserted into the database
            return new Guild(this, values);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the guilds loaded from the database.
        /// </summary>
        /// <returns>All of the guilds loaded from the database.</returns>
        protected override IEnumerable<Guild> LoadGuilds()
        {
            var guildValues = _dbController.GetQuery<SelectGuildsQuery>().Execute();
            return guildValues.Select(x => new Guild(this, x));
        }
    }
}