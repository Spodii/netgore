using System.Linq;

namespace NetGore.Tests.Db.MySql
{
    struct QueryTestValues
    {
        public int A;
        public int B;
        public int C;

        public QueryTestValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}