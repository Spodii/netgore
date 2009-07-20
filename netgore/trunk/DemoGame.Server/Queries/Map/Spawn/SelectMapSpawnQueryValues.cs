using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore;

namespace DemoGame.Server.Queries
{
    public class SelectMapSpawnQueryValues
    {
        public MapSpawnValuesID ID { get; private set; }
        public CharacterTemplateID CharacterTemplateID { get; private set; }
        public MapIndex MapIndex { get; private set; }
        public MapSpawnRect MapSpawnRect { get; private set; }

        public SelectMapSpawnQueryValues(MapSpawnValuesID id, CharacterTemplateID characterTemplateID, MapIndex mapIndex, MapSpawnRect mapSpawnRect)
        {
            ID = id;
            CharacterTemplateID = characterTemplateID;
            MapIndex = MapIndex;
            MapSpawnRect = mapSpawnRect;  
        }
    }
}
