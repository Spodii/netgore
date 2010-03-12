using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using DemoGame.Server.Quests;
using NetGore.Db;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages the <see cref="CharacterTemplate"/> instances.
    /// </summary>
    public class CharacterTemplateManager : DbTableDataManager<CharacterTemplateID, CharacterTemplate>
    {
        static readonly CharacterTemplateManager _instance;
        static readonly QuestManager _questManager = QuestManager.Instance;
        SelectCharacterTemplateQuery _selectCharacterTemplateQuery;

        SelectCharacterTemplateEquippedQuery _selectEquippedQuery;
        SelectCharacterTemplateInventoryQuery _selectInventoryQuery;
        SelectCharacterTemplateQuestsQuery _selectQuestsQuery;

        /// <summary>
        /// Initializes the <see cref="CharacterTemplateManager"/> class.
        /// </summary>
        static CharacterTemplateManager()
        {
            _instance = new CharacterTemplateManager(DbControllerBase.GetInstance());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateManager"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        CharacterTemplateManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// Gets an instance of the <see cref="CharacterTemplateManager"/>.
        /// </summary>
        public static CharacterTemplateManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// When overridden in the derived class, provides a chance to cache frequently used queries instead of
        /// having to grab the query from the <see cref="IDbController"/> every time. Caching is completely
        /// optional, but if you do cache any queries, it should be done here. Do not use this method for
        /// anything other than caching queries from the <paramref name="dbController"/>.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to grab the queries from.</param>
        protected override void CacheDbQueries(IDbController dbController)
        {
            _selectCharacterTemplateQuery = dbController.GetQuery<SelectCharacterTemplateQuery>();
            _selectInventoryQuery = dbController.GetQuery<SelectCharacterTemplateInventoryQuery>();
            _selectEquippedQuery = dbController.GetQuery<SelectCharacterTemplateEquippedQuery>();
            _selectQuestsQuery = dbController.GetQuery<SelectCharacterTemplateQuestsQuery>();

            base.CacheDbQueries(dbController);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected override IEnumerable<CharacterTemplateID> GetIDs()
        {
            return DbController.GetQuery<SelectCharacterTemplateIDsQuery>().Execute();
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected override int IDToInt(CharacterTemplateID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the int to a <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The int as a <paramref name="value"/>.</returns>
        public override CharacterTemplateID IntToID(int value)
        {
            return new CharacterTemplateID(value);
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected override CharacterTemplate LoadItem(CharacterTemplateID id)
        {
            var v = _selectCharacterTemplateQuery.Execute(id);
            var itemValues = _selectInventoryQuery.Execute(id);
            var equippedValues = _selectEquippedQuery.Execute(id);
            var questValues = _selectQuestsQuery.Execute(id);

            Debug.Assert(id == v.ID);
            Debug.Assert(itemValues.All(x => id == x.CharacterTemplateID));
            Debug.Assert(equippedValues.All(x => id == x.CharacterTemplateID));

            ItemTemplateManager itm = ItemTemplateManager.Instance;

            var items = itemValues.Select(x => new CharacterTemplateInventoryItem(itm[x.ItemTemplateID], x.Min, x.Max, x.Chance));
            var equipped = equippedValues.Select(x => new CharacterTemplateEquipmentItem(itm[x.ItemTemplateID], x.Chance));
            var quests = questValues.Select(x => _questManager.GetQuest(x));

            CharacterTemplate template = new CharacterTemplate(v, items, equipped, quests);

            return template;
        }
    }
}