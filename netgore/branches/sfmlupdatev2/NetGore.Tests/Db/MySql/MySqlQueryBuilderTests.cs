using System.Linq;
using NetGore.Db.MySql.QueryBuilder;
using NetGore.Db.QueryBuilder;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture(Description = "Tests the MySql queries for things specific to the MySql SQL syntax.")]
    public class MySqlQueryBuilderTests
    {
        #region Unit tests

        [Test]
        public void CallProcedureQueryTest01()
        {
            const string expected = "CALL MyProc()";
            var q = MySqlQueryBuilder.Instance.CallProcedure("MyProc");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void CallProcedureQueryTest02()
        {
            const string expected = "CALL MyProc(5)";
            var q = MySqlQueryBuilder.Instance.CallProcedure("MyProc").Add("5");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void CallProcedureQueryTest03()
        {
            const string expected = "CALL MyProc(5,a,x,b,@assd)";
            var q = MySqlQueryBuilder.Instance.CallProcedure("MyProc").Add("5", "a", "x", "b").AddParam("assd");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void DeleteQueryTest01()
        {
            const string expected = "DELETE FROM `myTable`";
            var q = MySqlQueryBuilder.Instance.Delete("myTable");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void DeleteQueryTest02()
        {
            const string expected = "DELETE FROM `myTable` WHERE 0 = 1";
            var q = MySqlQueryBuilder.Instance.Delete("myTable").Where("0 = 1");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void DeleteQueryTest03()
        {
            const string expected = "DELETE FROM `myTable` WHERE `a`=5 LIMIT 1";
            var q = MySqlQueryBuilder.Instance.Delete("myTable").Where("`a`=5").Limit(1);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionAbsTest()
        {
            const string expected = "SELECT ABS(-5) AS a FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Abs("-5"), "a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionAddTest()
        {
            const string expected = "SELECT 5 + 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Add("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionAndTest()
        {
            const string expected = "SELECT 5 AND 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.And("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionBitAndTest()
        {
            const string expected = "SELECT 5 & 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.BitAnd("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionBitNotTest()
        {
            const string expected = "SELECT ~5 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.BitNot("5"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionBitOrTest()
        {
            const string expected = "SELECT 5 | 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.BitOr("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionBitXorTest()
        {
            const string expected = "SELECT 5 ^ 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.BitXor("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionCeilingTest()
        {
            const string expected = "SELECT CEILING(5.3) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Ceiling("5.3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionCoalesceTest()
        {
            const string expected = "SELECT COALESCE(NULL,3) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Coalesce("NULL", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionCountTest()
        {
            const string expected = "SELECT COUNT(*) AS c FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Count(), "c");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionCountXTest()
        {
            const string expected = "SELECT COUNT(a) AS cnt FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Count("a"), "cnt");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDateAddTest1()
        {
            const string expected = "SELECT DATE_ADD(NOW(),INTERVAL 5 MINUTE) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.DateAddInterval(f.Now(), f.Interval(QueryIntervalType.Minute, 5)));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDateAddTest2()
        {
            const string expected = "SELECT DATE_ADD(NOW(),INTERVAL 5 MINUTE) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.DateAddInterval(f.Now(), QueryIntervalType.Minute, 5));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDateSubtractTest1()
        {
            const string expected = "SELECT DATE_SUB(NOW(),INTERVAL 5 HOUR) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.DateSubtractInterval(f.Now(), f.Interval(QueryIntervalType.Hour, 5)));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDateSubtractTest2()
        {
            const string expected = "SELECT DATE_SUB(NOW(),INTERVAL 5 HOUR) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.DateSubtractInterval(f.Now(), QueryIntervalType.Hour, 5));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDefaultTest()
        {
            const string expected = "SELECT DEFAULT(a) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Default("a"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionDivideTest()
        {
            const string expected = "SELECT 5 / 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Divide("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionEqualsTest()
        {
            const string expected = "SELECT 1 = 2 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Equals("1", "2"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionFloorTest()
        {
            const string expected = "SELECT FLOOR(5) FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Floor("5"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionIsNotNullTest()
        {
            const string expected = "SELECT 'a' IS NOT NULL FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.IsNotNull("'a'"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionIsNullTest()
        {
            const string expected = "SELECT 'a' IS NULL FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.IsNull("'a'"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionModTest()
        {
            const string expected = "SELECT 5 % 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Mod("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionMultiplyTest()
        {
            const string expected = "SELECT 5 * 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Multiply("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionNotEqualsTest()
        {
            const string expected = "SELECT 1 != 2 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.NotEqual("1", "2"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionNowTest()
        {
            const string expected = "SELECT NOW() FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Now());
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionOrTest()
        {
            const string expected = "SELECT 5 OR 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Or("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionSubtractTest()
        {
            const string expected = "SELECT 5 - 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Subtract("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void FunctionXorTest()
        {
            const string expected = "SELECT 5 XOR 3 FROM `myTable`";
            var qb = MySqlQueryBuilder.Instance;
            var f = qb.Functions;
            var q = qb.Select("myTable").AddFunc(f.Xor("5", "3"));
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest01()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `a`=@a,`b`=@b";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").ODKU().AddFromInsert();
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest02()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `b`=@b";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").ODKU().AddFromInsert().Remove("a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest03()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `b`=@b";
            var q =
                MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").ODKU().AddFromInsert().Remove("a").Remove("a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest04()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `b`=@b,`a`=55";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").ODKU().AddFromInsert().Add("a", "55");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertQueryTest01()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b)";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertQueryTest02()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES ('asdf',1)";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").Add("a", "'asdf'").Add("b", "1");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertQueryTest03()
        {
            const string expected = "INSERT INTO `myTable` (`a`) VALUES ('asdf')";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").Add("a", "'asdf'").Add("b", "1").Remove("b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertQueryTest04()
        {
            const string expected = "INSERT IGNORE INTO `myTable` (`a`) VALUES ('asdf')";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").IgnoreExists().Add("a", "'asdf'").Add("b", "1").Remove("b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertQueryTest05()
        {
            const string expected = "INSERT IGNORE INTO `myTable` (`a`,`x`,`b`,`fa`,`zzz`,`aaa`) VALUES (@a,@x,@b,@fa,@zzz,@aaa)";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").IgnoreExists().AddAutoParam("a", "x", "b", "fa", "zzz", "aaa");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectFunctionQueryTest01()
        {
            const string expected = "SELECT MyFunc()";
            var q = MySqlQueryBuilder.Instance.SelectFunction("MyFunc");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectFunctionQueryTest02()
        {
            const string expected = "SELECT MyFunc(5)";
            var q = MySqlQueryBuilder.Instance.SelectFunction("MyFunc").Add("5");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectFunctionQueryTest03()
        {
            const string expected = "SELECT MyFunc(5,a,x,b,@assd)";
            var q = MySqlQueryBuilder.Instance.SelectFunction("MyFunc").Add("5", "a", "x", "b").AddParam("assd");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest01()
        {
            const string expected = "SELECT DISTINCT t.a,u.a FROM `myTable` AS t INNER JOIN `t2` u ON t.a=u.a";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoin("t2", "u", "t.a=u.a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest02()
        {
            const string expected = "SELECT DISTINCT t.a,u.a FROM `myTable` AS t INNER JOIN `t2` u ON u.a=t.a";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoinOnColumn("t2", "u",
                "a", "t", "a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest03()
        {
            const string expected =
                "SELECT DISTINCT t.a,u.a FROM `myTable` AS t INNER JOIN `t2` u ON u.a=t.a ORDER BY t.a LIMIT 1";
            var q =
                MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoinOnColumn("t2", "u", "a",
                    "t", "a").OrderBy("t.a").Limit(1);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest04()
        {
            const string expected =
                "SELECT DISTINCT t.*,u.a FROM `myTable` AS t INNER JOIN `t2` u ON u.a=t.a ORDER BY t.a LIMIT 1";
            var q =
                MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().AllColumns("t").Add("u.a").InnerJoinOnColumn("t2",
                    "u", "a", "t", "a").OrderBy("t.a").Limit(1);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryTest01()
        {
            const string expected = "SELECT * FROM `myTable`";
            var q = MySqlQueryBuilder.Instance.Select("myTable").AllColumns();
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryTest02()
        {
            const string expected = "SELECT `a`,`b` FROM `myTable`";
            var q = MySqlQueryBuilder.Instance.Select("myTable").Add("a", "b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryTest03()
        {
            const string expected = "SELECT DISTINCT `a`,`b` FROM `myTable`";
            var q = MySqlQueryBuilder.Instance.Select("myTable").Distinct().Add("a", "b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryTest04()
        {
            const string expected = "SELECT DISTINCT t.a,t.b FROM `myTable` AS t";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "t.b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryTest05()
        {
            const string expected = "SELECT `a`,`b`,`c` FROM `myTable` ORDER BY `a`, `b` DESC";
            var q = MySqlQueryBuilder.Instance.Select("myTable").Add("a", "b", "c").OrderByColumn("a").OrderByColumn("b",
                OrderByType.Descending);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void UpdateQueryTest01()
        {
            const string expected = "UPDATE `myTable` SET `a`=@a,`b`=@b";
            var q = MySqlQueryBuilder.Instance.Update("myTable").AddAutoParam("a", "b");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void UpdateQueryTest02()
        {
            const string expected = "UPDATE `myTable` SET `a`=55,`b`=@b,`c`=@cParam";
            var q = MySqlQueryBuilder.Instance.Update("myTable").Add("a", "55").AddAutoParam("b").AddParam("c", "cParam");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void UpdateQueryTest03()
        {
            const string expected = "UPDATE `myTable` SET `a`=@a,`b`=@b LIMIT 2";
            var q = MySqlQueryBuilder.Instance.Update("myTable").AddAutoParam("a", "b").Limit(2);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void UpdateQueryTest04()
        {
            const string expected = "UPDATE `myTable` SET `a`=@a,`b`=@b ORDER BY `a` LIMIT 1";
            var q = MySqlQueryBuilder.Instance.Update("myTable").AddAutoParam("a", "b").OrderByColumn("a").Limit(1);
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void UpdateQueryTest05()
        {
            const string expected = "UPDATE `myTable` SET `a`=@a,`b`=@b ORDER BY a LIMIT 1";
            var q = MySqlQueryBuilder.Instance.Update("myTable").AddAutoParam("a", "b").OrderBy("a").Limit(1);
            Assert.AreEqual(expected, q.ToString());
        }

        #endregion
    }
}