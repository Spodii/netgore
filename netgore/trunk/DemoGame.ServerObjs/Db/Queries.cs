using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.DbEntities;

// TODO: Rename DbQueries to Queries after I get rid of the namespace NetGore.Server.Queries

namespace DemoGame.Server.Db
{
    /// <summary>
    /// The master class for all of the custom, reusable queries. Try to keep all common queries in this
    /// class so custom queries can be grouped together as much as possible.
    /// </summary>
    public static class DbQueries
    {
        /// <summary>
        /// The QueryHelper used to help execute the queries.
        /// </summary>
        static readonly QueryHelper _qh = QueryHelper.Instance;

        /// <summary>
        /// Queries that focus on the Alliance.
        /// </summary>
        public static class Alliance
        {
            static readonly Func<DatabaseEntities, int, alliance> _getAlliance =
                CompiledQuery.Compile((DatabaseEntities db, int id) => db.alliance.First(x => x.id == id));

            static readonly Func<DatabaseEntities, IQueryable<byte>> _getAllianceIDs =
                CompiledQuery.Compile((DatabaseEntities db) => db.alliance.Select(x => x.id));

            /// <summary>
            /// Gets the data for an Alliance.
            /// </summary>
            /// <param name="id">ID of the Alliance to get the data for.</param>
            /// <returns>The data for the Alliance with the given <paramref name="id"/>.</returns>
            public static alliance GetAlliance(AllianceID id)
            {
                var result = _qh.Invoke(_getAlliance, (int)id);
                Debug.Assert(result != null);
                Debug.Assert(result.id == id);
                return result;
            }

            static readonly Func<DatabaseEntities, int, IQueryable<alliance_attackable>> _getAllianceAttackable =
                CompiledQuery.Compile((DatabaseEntities db, int id) => db.alliance_attackable.Where(x => x.alliance_id == id));

            public static IEnumerable<alliance_attackable> GetAllianceAttackable(AllianceID id)
            {
                var result = _qh.InvokeAndSelectMany(_getAllianceAttackable, x => x, (int)id);
                return result;
            }

            /// <summary>
            /// Gets all of the AllianceIDs.
            /// </summary>
            /// <returns>All of the AllianceIDs.</returns>
            public static IEnumerable<AllianceID> GetAllianceIDs()
            {
                var result = _qh.InvokeAndSelectMany(_getAllianceIDs, x => new AllianceID(x));
                Debug.Assert(result.Count() > 0);
                return result;
            }
        }
    }
}
