using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using MySql.Data.MySqlClient;
using Platyform;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads, caches, and manages a collection of NPCTemplates
    /// </summary>
    public class NPCTemplateManager
    {
        readonly AllianceManager _allianceManager;
        readonly NPCDropManager _npcDropManager;
        readonly DArray<NPCTemplate> _templates = new DArray<NPCTemplate>(false);
        MySqlConnection _conn;

        /// <summary>
        /// Gets or sets the MySqlConnection used to load NPCTemplates with
        /// </summary>
        public MySqlConnection MySqlConnection
        {
            get { return _conn; }
            set { _conn = value; }
        }

        /// <summary>
        /// NPCTemplateManager constructor.
        /// </summary>
        /// <param name="conn">MySqlConnection used to load NPCTemplates with.</param>
        /// <param name="allianceManager">AllianceManager containing the Alliances to be used.</param>
        /// <param name="npcDropManager">NPCDrops collection used to get the NPCDrop reference for each item the
        /// NPC drops.</param>
        public NPCTemplateManager(MySqlConnection conn, AllianceManager allianceManager, NPCDropManager npcDropManager)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");
            if (npcDropManager == null)
                throw new ArgumentNullException("npcDropManager");

            _conn = conn;
            _allianceManager = allianceManager;
            _npcDropManager = npcDropManager;
        }

        /// <summary>
        /// Gets a NPCTemplate by the given ID
        /// </summary>
        /// <param name="guid">Unique ID of the template to load</param>
        /// <returns>NPCTemplate by the given ID</returns>
        public NPCTemplate GetTemplate(int guid)
        {
            NPCTemplate ret = null;

            // Grab from cache if possible
            if (_templates.CanGet(guid))
                ret = _templates[guid];

            // If not cached, load it and place a copy in the cache
            if (ret == null)
            {
                ret = new NPCTemplate(guid, _conn, _allianceManager, _npcDropManager);
                _templates[guid] = ret;
            }

            return ret;
        }
    }
}