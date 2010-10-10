using System;
using System.Collections.Generic;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    public class MySqlDeleteQuery : DeleteQueryBase
    {
        public MySqlDeleteQuery(string table) : base(table, MySqlQueryBuilderSettings.Instance)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("DELETE FROM ");
            sb.Append(Settings.EscapeTable(Table));

            return sb.ToString();
        }

        protected override IQueryResultFilter CreateResultFilter(object parent)
        {
            return new MySqlQueryResultFilter(parent);
        }
    }
}