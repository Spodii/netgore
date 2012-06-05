using System;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="ICallProcedureQuery"/> for MySql.
    /// </summary>
    class MySqlCallProcedureQuery : CallProcedureQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlCallProcedureQuery"/> class.
        /// </summary>
        /// <param name="procedure">The name of the stored procedure.</param>
        /// <exception cref="ArgumentNullException"><paramref name="procedure"/> is null or empty.</exception>
        public MySqlCallProcedureQuery(string procedure) : base(procedure, MySqlQueryBuilderSettings.Instance)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // Base operator
            sb.Append("CALL ");
            sb.Append(Procedure);
            sb.Append("(");

            // Columns
            var values = ValueCollection.GetValues();
            if (values != null && values.Length > 0)
            {
                foreach (var v in values)
                {
                    sb.Append(v);
                    sb.Append(",");
                }

                sb.Length--;
            }

            sb.Append(")");

            return sb.ToString();
        }
    }
}