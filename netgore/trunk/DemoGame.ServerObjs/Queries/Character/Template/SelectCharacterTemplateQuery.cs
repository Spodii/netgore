using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", DBTables.CharacterTemplate);

        public SelectCharacterTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public CharacterTemplateTable Execute(CharacterTemplateID templateID)
        {
            CharacterTemplateTable ret;

            using (IDataReader r = ExecuteReader(templateID))
            {
                if (!r.Read())
                {
                    const string errmsg = "No CharacterTemplate found for ID `{0}`.";
                    throw new ArgumentException(string.Format(errmsg, templateID), "templateID");
                }

                ret = new CharacterTemplateTable(r);
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, CharacterTemplateID templateID)
        {
            p["@id"] = (int)templateID;
        }
    }
}