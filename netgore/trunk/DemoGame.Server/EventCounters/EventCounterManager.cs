using System.Linq;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.Features.EventCounters;
using NetGore.Features.Guilds;
using NetGore.Features.Quests;
using NetGore.Features.Shops;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the different <see cref="IEventCounter{TObjectID,TEventID}"/>s used by the server.
    /// </summary>
    public static class EventCounterManager
    {
        static readonly EventCounter<GuildID, GuildEventCounterType> _guildEventCounter;
        static readonly EventCounter<ItemTemplateID, ItemTemplateEventCounterType> _itemTemplateEventCounter;
        static readonly EventCounter<MapID, MapEventCounterType> _mapEventCounter;
        static readonly EventCounter<CharacterTemplateID, NPCEventCounterType> _npcEventCounter;
        static readonly EventCounter<QuestID, QuestEventCounterType> _questEventCounter;
        static readonly EventCounter<ShopID, ShopEventCounterType> _shopEventCounter;
        static readonly EventCounter<CharacterID, UserEventCounterType> _userEventCounter;

        /// <summary>
        /// Initializes the <see cref="EventCounterManager"/> class.
        /// </summary>
        static EventCounterManager()
        {
            // Create a dedicated dbController for the EventCounters since they are all executed in non-blocking mode,
            // none of the queries touch the normal game tables, and none of the queries do not rely on order of execution
            var dbConnSettings = new DbConnectionSettings();
            var dbController = new ServerDbController(dbConnSettings.GetMySqlConnectionString());

            // Create the query objects
            var pool = dbController.ConnectionPool;

            var userECQuery = EventCounterHelper.CreateQuery<CharacterID, UserEventCounterType>(pool,
                EventCountersUserTable.TableName, "user_id", "user_event_counter_id");

            var guildECQuery = EventCounterHelper.CreateQuery<GuildID, GuildEventCounterType>(pool,
                EventCountersGuildTable.TableName, "guild_id", "guild_event_counter_id");

            var shopECQuery = EventCounterHelper.CreateQuery<ShopID, ShopEventCounterType>(pool, EventCountersShopTable.TableName,
                "shop_id", "shop_event_counter_id");

            var mapECQuery = EventCounterHelper.CreateQuery<MapID, MapEventCounterType>(pool, EventCountersMapTable.TableName,
                "map_id", "map_event_counter_id");

            var questECQuery = EventCounterHelper.CreateQuery<QuestID, QuestEventCounterType>(pool,
                EventCountersQuestTable.TableName, "quest_id", "quest_event_counter_id");

            var itemTemplateECQuery = EventCounterHelper.CreateQuery<ItemTemplateID, ItemTemplateEventCounterType>(pool,
                EventCountersItemTemplateTable.TableName, "item_template_id", "item_template_event_counter_id");

            var npcECQuery = EventCounterHelper.CreateQuery<CharacterTemplateID, NPCEventCounterType>(pool,
                EventCountersNpcTable.TableName, "npc_template_id", "npc_event_counter_id");

            // Create the event counters
            _userEventCounter = new EventCounter<CharacterID, UserEventCounterType>(userECQuery);
            _guildEventCounter = new EventCounter<GuildID, GuildEventCounterType>(guildECQuery);
            _shopEventCounter = new EventCounter<ShopID, ShopEventCounterType>(shopECQuery);
            _mapEventCounter = new EventCounter<MapID, MapEventCounterType>(mapECQuery);
            _questEventCounter = new EventCounter<QuestID, QuestEventCounterType>(questECQuery);
            _itemTemplateEventCounter = new EventCounter<ItemTemplateID, ItemTemplateEventCounterType>(itemTemplateECQuery);
            _npcEventCounter = new EventCounter<CharacterTemplateID, NPCEventCounterType>(npcECQuery);
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="Guild"/> events.
        /// </summary>
        public static EventCounter<GuildID, GuildEventCounterType> Guild
        {
            get { return _guildEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="ItemTemplate"/> events.
        /// </summary>
        public static EventCounter<ItemTemplateID, ItemTemplateEventCounterType> ItemTemplate
        {
            get { return _itemTemplateEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="Map"/> events.
        /// </summary>
        public static EventCounter<MapID, MapEventCounterType> Map
        {
            get { return _mapEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="NPC"/> events.
        /// </summary>
        public static EventCounter<CharacterTemplateID, NPCEventCounterType> NPC
        {
            get { return _npcEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="Quest"/> events.
        /// </summary>
        public static EventCounter<QuestID, QuestEventCounterType> Quest
        {
            get { return _questEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="Shop"/> events.
        /// </summary>
        public static EventCounter<ShopID, ShopEventCounterType> Shop
        {
            get { return _shopEventCounter; }
        }

        /// <summary>
        /// Gets the <see cref="IEventCounter{T,U}"/> for <see cref="User"/> events.
        /// </summary>
        public static EventCounter<CharacterID, UserEventCounterType> User
        {
            get { return _userEventCounter; }
        }

        /// <summary>
        /// Flushes all of the <see cref="IEventCounter{T,U}"/>s.
        /// </summary>
        public static void FlushAll()
        {
            Guild.Flush();
            ItemTemplate.Flush();
            Map.Flush();
            Quest.Flush();
            Shop.Flush();
            User.Flush();
            NPC.Flush();
        }
    }
}