using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public static class MapQueryHelper
    {
        public static IEnumerable<string> AllDBFields { get; private set; }

        public static IEnumerable<string> AllDBFieldsExceptID { get; private set; }

        static MapQueryHelper()
        {
            AllDBFieldsExceptID = new string[] { "name" }.ToArray();
            AllDBFields = new string[] { "id" }.Concat(AllDBFieldsExceptID).ToArray();
        }

        public static void SetParameters(DbParameterValues p, MapBase map)
        {
            p["@id"] = map.Index;
            p["@name"] = map.Name;
        }
    }
}
