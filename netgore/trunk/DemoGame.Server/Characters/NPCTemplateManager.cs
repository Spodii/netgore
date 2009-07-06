using System;
using DemoGame.Server.Queries;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads, caches, and manages a collection of NPCTemplates
    /// </summary>
    public class NPCTemplateManager
    {
        readonly AllianceManager _allianceManager;
        readonly ItemTemplates _itemTemplates;
        readonly SelectNPCTemplateQuery _query;
        readonly DArray<NPCTemplate> _templates = new DArray<NPCTemplate>(false);
        readonly SelectNPCTemplateDropsQuery _dropsQuery;

        /// <summary>
        /// NPCTemplateManager constructor.
        /// </summary>
        /// <param name="query">SelectNPCTemplateQuery used to load NPCTemplates with.</param>
        /// <param name="allianceManager">AllianceManager containing the Alliances to be used.</param>
        /// <param name="itemTemplates">ItemTemplates containing the templates for the items the NPCs will drop.</param>
        public NPCTemplateManager(SelectNPCTemplateQuery query, SelectNPCTemplateDropsQuery dropsQuery, AllianceManager allianceManager, ItemTemplates itemTemplates)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (dropsQuery == null)
                throw new ArgumentNullException("dropsQuery");
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");
            if (itemTemplates == null)
                throw new ArgumentNullException("itemTemplates");

            _query = query;
            _dropsQuery = dropsQuery;
            _allianceManager = allianceManager;
            _itemTemplates = itemTemplates;
        }

        /// <summary>
        /// Gets a NPCTemplate by the given ID
        /// </summary>
        /// <param name="id">Unique ID of the template to load.</param>
        /// <returns>NPCTemplate by the given ID</returns>
        public NPCTemplate GetTemplate(CharacterTemplateID id)
        {
            NPCTemplate ret = null;

            // Grab from cache if possible
            if (_templates.CanGet((int)id))
                ret = _templates[(int)id];

            // If not cached, load it and place a copy in the cache
            if (ret == null)
            {
                ret = new NPCTemplate(id, _query, _dropsQuery, _allianceManager, _itemTemplates);
                _templates[(int)id] = ret;
            }

            return ret;
        }
    }
}