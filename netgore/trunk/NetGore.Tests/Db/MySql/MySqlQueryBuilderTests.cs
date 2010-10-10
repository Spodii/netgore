using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;
using NUnit.Framework;

namespace NetGore.Tests.Db.MySql
{
    [TestFixture(Description="Tests the MySql queries for things specific to the MySql SQL syntax.")]
    public class MySqlQueryBuilderTests
    {

        [Test]
        public void SelectQueryInnerJoinTest01()
        {
            const string expected = "SELECT DISTINCT t.a,u.a FROM `myTable` t INNER JOIN `t2` u ON t.a=u.a";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoin("t2", "u", "t.a=u.a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest02()
        {
            const string expected = "SELECT DISTINCT t.a,u.a FROM `myTable` t INNER JOIN `t2` u ON u.a=t.a";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoinOnColumn("t2", "u", "a", "t", "a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void SelectQueryInnerJoinTest03()
        {
            const string expected = "SELECT DISTINCT t.a,u.a FROM `myTable` t INNER JOIN `t2` u ON u.a=t.a ORDER BY t.a LIMIT 1";
            var q =
                MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "u.a").InnerJoinOnColumn("t2", "u", "a", "t", "a").OrderBy(
                    "t.a").Limit(1);
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
        public void InsertOnDuplicateKeyQueryTest01()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `a`=@a,`b`=@b";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").OnDuplicateKeyUpdate().AddFromInsert();
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest02()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `b`=@b";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").OnDuplicateKeyUpdate().AddFromInsert().Remove("a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest03()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `b`=@b";
            var q =
                MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").OnDuplicateKeyUpdate().AddFromInsert().Remove("a").Remove("a");
            Assert.AreEqual(expected, q.ToString());
        }

        [Test]
        public void InsertOnDuplicateKeyQueryTest04()
        {
            const string expected = "INSERT INTO `myTable` (`a`,`b`) VALUES (@a,@b) ON DUPLICATE KEY UPDATE `a`=55,`b`=@b";
            var q = MySqlQueryBuilder.Instance.Insert("myTable").AddAutoParam("a", "b").OnDuplicateKeyUpdate().AddFromInsert().Add("a", "55");
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
            const string expected = "SELECT DISTINCT t.a,t.b FROM `myTable` t";
            var q = MySqlQueryBuilder.Instance.Select("myTable", "t").Distinct().Add("t.a", "t.b");
            Assert.AreEqual(expected, q.ToString());
        }
    }
}
