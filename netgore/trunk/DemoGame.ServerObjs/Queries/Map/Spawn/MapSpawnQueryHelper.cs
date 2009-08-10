using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

// TODO: !! Try to make obsolete

namespace DemoGame.Server.Queries
{
    public static class MapSpawnQueryHelper
    {
        public static IEnumerable<string> AllDBFields { get; private set; }

        public static IEnumerable<string> AllDBFieldsExceptID { get; private set; }

        static MapSpawnQueryHelper()
        {
            AllDBFieldsExceptID = new string[] { "map_id", "character_template_id", "amount", "x", "y", "width", "height" }.ToArray();
            AllDBFields = new string[] { "id" }.Concat(AllDBFieldsExceptID).ToArray();
        }

        public static SelectMapSpawnQueryValues ReadMapSpawnValues(IDataReader r)
        {
            MapSpawnValuesID id = r.GetInt32("id");
            MapIndex mapIndex = r.GetMapIndex("map_id");
            CharacterTemplateID charTemplate = r.GetCharacterTemplateID("character_template_id");
            byte amount = r.GetByte("amount");
            var x = r.GetNullableUInt16("x");
            var y = r.GetNullableUInt16("y");
            var w = r.GetNullableUInt16("width");
            var h = r.GetNullableUInt16("height");

            MapSpawnRect spawnRect = new MapSpawnRect(x, y, w, h);
            SelectMapSpawnQueryValues ret = new SelectMapSpawnQueryValues(id, charTemplate, mapIndex, amount, spawnRect);

            return ret;
        }

        public static void SetParameters(DbParameterValues p, MapSpawnValues spawnValues)
        {
            p["@id"] = (int)spawnValues.ID;
            p["@map_id"] = spawnValues.MapIndex;
            p["@character_template_id"] = spawnValues.CharacterTemplateID;
            p["@amount"] = spawnValues.SpawnAmount;
            p["@x"] = spawnValues.SpawnArea.X;
            p["@y"] = spawnValues.SpawnArea.Y;
            p["@width"] = spawnValues.SpawnArea.Width;
            p["@height"] = spawnValues.SpawnArea.Height;
        }
    }
}