using System.Linq;

namespace NetGore.Tests.Db.MySql
{
    /// <summary>
    /// A struct for test query values.
    /// </summary>
    struct QueryTestValues
    {
        /// <summary>
        /// The A value.
        /// </summary>
        public readonly int A;

        /// <summary>
        /// The B value.
        /// </summary>
        public readonly int B;

        /// <summary>
        /// The C value.
        /// </summary>
        public readonly int C;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTestValues"/> struct.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        public QueryTestValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}