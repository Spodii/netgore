using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class SelectCharacterTemplateQuery : DbQueryReader<CharacterTemplateID>
    {
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", DBTables.CharacterTemplate);
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SelectCharacterTemplateQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        public SelectCharacterTemplateQueryValues Execute(CharacterTemplateID templateID)
        {
            SelectCharacterTemplateQueryValues ret;

            using (IDataReader r = ExecuteReader(templateID))
            {
                if (!r.Read())
                {
                    const string errmsg = "No CharacterTemplate found for ID `{0}`.";
                    throw new ArgumentException(string.Format(errmsg, templateID), "templateID");
                }

                ret = CharacterTemplateQueryHelper.ReadCharacterTemplateValues(r);

#if DEBUG
                // Check that the correct record was grabbed (insanely unlikely this will ever happen)
                if (ret.ID != templateID)
                {
                    const string errmsg = "Performed SELECT for CharacterTemplate with id `{0}`, but got id `{1}`.";
                    string err = string.Format(errmsg, templateID, ret.ID);
                    log.Fatal(err);
                    Debug.Fail(err);
                    throw new DataException(err);
                }
#endif
            }

            return ret;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        protected override void SetParameters(DbParameterValues p, CharacterTemplateID templateID)
        {
            p["@id"] = templateID;
        }
    }
}