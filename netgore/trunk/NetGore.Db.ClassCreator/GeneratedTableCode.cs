using System.Linq;

namespace NetGore.Db.ClassCreator
{
    public struct GeneratedTableCode
    {
        public readonly string ClassName;
        public readonly string Code;
        public readonly string Table;

        public GeneratedTableCode(string table, string className, string code)
        {
            Table = table;
            ClassName = className;
            Code = code;
        }
    }
}