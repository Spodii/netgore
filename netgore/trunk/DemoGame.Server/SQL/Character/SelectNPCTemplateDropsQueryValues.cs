using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class SelectNPCTemplateDropsQueryValues
    {
        public int ItemID { get; private set; }
        public NPCDropChance DropChance { get; private set; }

        public SelectNPCTemplateDropsQueryValues(int itemID, NPCDropChance dropChance)
        {
            ItemID = itemID;
            DropChance = dropChance;
        }
    }
}
