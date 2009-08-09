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
        static readonly string _queryString = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", CharacterTemplateTable.TableName);

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

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
        /// no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters based on the specified item.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="templateID">Item used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, CharacterTemplateID templateID)
        {
            p["@id"] = (int)templateID;
        }
    }
}