using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
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
        static readonly DArray<CharacterTemplate> _templates = new DArray<CharacterTemplate>(false);
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static DBController _dbController;

        /// <summary>
        /// Gets if this class has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

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
                ret = LoadCharacterTemplate(_dbController, id);
                _templates[(int)id] = ret;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded CharacterTemplate `{0}`.", ret);
            }

            return ret;
        }

        /// <summary>
        /// CharacterTemplateManager constructor.
        /// </summary>
        /// <param name="dbController">DBController used to perform queries.</param>
        public static void Initialize(DBController dbController)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            if (dbController == null)
                throw new ArgumentNullException("dbController");

            if (!AllianceManager.IsInitialized)
                AllianceManager.Initialize(dbController);
            if (!ItemTemplateManager.IsInitialized)
                ItemTemplateManager.Initialize(dbController);

            _dbController = dbController;

            LoadAll();
        }

        static void LoadAll()
        {
            var ids = _dbController.GetQuery<SelectCharacterTemplateIDsQuery>().Execute();

            foreach (CharacterTemplateID id in ids)
            {
                GetTemplate(id);
            }
        }

        public static CharacterTemplate LoadCharacterTemplate(DBController dbController, CharacterTemplateID id)
        {
            CharacterTemplateTable v = dbController.GetQuery<SelectCharacterTemplateQuery>().Execute(id);
            var itemValues = dbController.GetQuery<SelectCharacterTemplateInventoryQuery>().Execute(id);
            var equippedValues = dbController.GetQuery<SelectCharacterTemplateEquippedQuery>().Execute(id);

            Debug.Assert(id == v.ID);
            Debug.Assert(itemValues.All(x => id == x.CharacterTemplateID));
            Debug.Assert(equippedValues.All(x => id == x.CharacterTemplateID));

            Alliance alliance = AllianceManager.GetAlliance(v.AllianceID);
            var items =
                itemValues.Select(
                    x =>
                    new CharacterTemplateInventoryItem(ItemTemplateManager.GetTemplate(x.ItemTemplateID), x.Min, x.Max, x.Chance));

            var euipped =
                equippedValues.Select(
                    x => new CharacterTemplateEquipmentItem(ItemTemplateManager.GetTemplate(x.ItemTemplateID), x.Chance));

            CharacterTemplate template = new CharacterTemplate(id, v.Name, v.AI, alliance, v.BodyID, v.Respawn, v.GiveExp,
                                                               v.GiveCash, v.Exp, v.StatPoints, v.Level,
                                                               v.Stats.Select(x => new StatTypeValue(x.Key, x.Value)), items,
                                                               euipped);

            return template;
        }
    }
}