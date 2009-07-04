using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class SelectNPCTemplateDropsQueryValues
    {
        public int ItemGuid { get; private set; }
        public NPCDropChance DropChance { get; private set; }

        public SelectNPCTemplateDropsQueryValues(int itemGuid, NPCDropChance dropChance)
        {
            ItemGuid = itemGuid;
            DropChance = dropChance;
        }
    }
}
