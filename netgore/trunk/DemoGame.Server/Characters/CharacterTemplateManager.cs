using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads, caches, and manages a collection of CharacterTemplates.
    /// </summary>
    public static class CharacterTemplateManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static AllianceManager _allianceManager;
        static DBController _dbController;
        static ItemTemplates _itemTemplates;
        static readonly DArray<CharacterTemplate> _templates = new DArray<CharacterTemplate>(false);

        /// <summary>
        /// CharacterTemplateManager constructor.
        /// </summary>
        /// <param name="dbController">DBController used to perform queries.</param>
        /// <param name="allianceManager">AllianceManager containing the Alliances to be used.</param>
        /// <param name="itemTemplates">ItemTemplates containing the templates for the items the NPCs will drop.</param>
        public static void Initialize(DBController dbController, AllianceManager allianceManager, ItemTemplates itemTemplates)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");
            if (itemTemplates == null)
                throw new ArgumentNullException("itemTemplates");

            _dbController = dbController;
            _allianceManager = allianceManager;
            _itemTemplates = itemTemplates;

            LoadAll();
        }

        /// <summary>
        /// Gets a CharacterTemplate by the given CharacterTemplateID.
        /// </summary>
        /// <param name="id">ID of the template to get.</param>
        /// <returns>NPCTemplate by the given <paramref name="id"/>.</returns>
        public static CharacterTemplate GetTemplate(CharacterTemplateID id)
        {
            CharacterTemplate ret = null;

            // Grab from cache if possible
            if (_templates.CanGet((int)id))
                ret = _templates[(int)id];

            // If not cached, load it and place a copy in the cache
            if (ret == null)
            {
                ret = LoadCharacterTemplate(_dbController, _allianceManager, _itemTemplates, id);
                _templates[(int)id] = ret;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded CharacterTemplate `{0}`.", ret);
            }

            return ret;
        }

        static void LoadAll()
        {
            var ids = _dbController.GetQuery<SelectCharacterTemplateIDsQuery>().Execute();

            foreach (CharacterTemplateID id in ids)
            {
                GetTemplate(id);
            }
        }

        public static CharacterTemplate LoadCharacterTemplate(DBController dbController, AllianceManager allianceManager,
                                                              ItemTemplates itemTemplates, CharacterTemplateID id)
        {
            SelectCharacterTemplateQueryValues v = dbController.GetQuery<SelectCharacterTemplateQuery>().Execute(id);
            var itemValues = dbController.GetQuery<SelectCharacterTemplateInventoryQuery>().Execute(id);
            var equippedValues = dbController.GetQuery<SelectCharacterTemplateEquippedQuery>().Execute(id);

            Debug.Assert(id == v.ID);
            Debug.Assert(itemValues.All(x => id == x.CharacterTemplateID));
            Debug.Assert(equippedValues.All(x => id == x.CharacterTemplateID));

            Alliance alliance = allianceManager[v.AllianceID];
            var items =
                itemValues.Select(x => new CharacterTemplateInventoryItem(itemTemplates[x.ItemTemplateID], x.Min, x.Max, x.Chance));
            var euipped = equippedValues.Select(x => new CharacterTemplateEquipmentItem(itemTemplates[x.ItemTemplateID], x.Chance));

            CharacterTemplate template = new CharacterTemplate(id, v.Name, v.AIName, alliance, v.BodyIndex, v.Respawn, v.GiveExp,
                                                               v.GiveCash, v.Exp, v.StatPoints, v.Level, v.StatValues, items,
                                                               euipped);

            return template;
        }
    }
}