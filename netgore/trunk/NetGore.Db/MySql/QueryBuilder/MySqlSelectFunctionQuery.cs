using System;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="ISelectFunctionQuery"/> for MySql.
    /// </summary>
    class MySqlSelectFunctionQuery : SelectFunctionQueryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlSelectFunctionQuery"/> class.
        /// </summary>
        /// <param name="function">The name of the function.</param>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null or empty.</exception>
        public MySqlSelectFunctionQuery(string function) : base(function, MySqlQueryBuilderSettings.Instance)
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
            sb.Append("SELECT ");
            sb.Append(Function);
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