using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    public class SelectNPCTemplateDropsQueryValues
    {
        public ItemID ItemID { get; private set; }
        public NPCDropChance DropChance { get; private set; }

        public SelectNPCTemplateDropsQueryValues(ItemID itemID, NPCDropChance dropChance)
        {
            ItemID = itemID;
            DropChance = dropChance;
        }
    }
}
