using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    class NPCInventory : Inventory
    {
        readonly NPC _npc;

        /// <summary>
        /// Gets the Character that this Inventory belongs to.
        /// </summary>
        public override Character Character
        {
            get { return _npc; }
        }

        public NPCInventory(NPC npc)
        {
            if (npc == null)
                throw new ArgumentNullException("npc");

            _npc = npc;
        }
    }
}