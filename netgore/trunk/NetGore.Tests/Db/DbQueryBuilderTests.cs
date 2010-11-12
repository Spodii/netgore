using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Db.MySql.QueryBuilder;
using NetGore.Db.QueryBuilder;
using NUnit.Framework;

// ReSharper disable AccessToModifiedClosure

namespace NetGore.Tests.Db
{
    [TestFixture(
        Description =
            "Tests the aspects of the QueryBuilder that are not specific to a certain provider (e.g. checking the generated SQL)."
        )]
    public class DbQueryBuilderTests
    {
        static readonly IEnumerable<IQueryBuilder> _qbs;

        /// <summary>
        /// Initializes the <see cref="DbQueryBuilderTests"/> class.
        /// </summary>
        static DbQueryBuilderTests()
        {
            // Add all the IQueryBuilder instances into here so they become part of the tests
            _qbs = new IQueryBuilder[] { MySqlQueryBuilder.Instance };
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/>s to test.
        /// </summary>
        static IEnumerable<IQueryBuilder> QBs
        {
            get { return _qbs; }
        }

        #region Unit tests

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InsertOnDuplicateKeyQueryEmptyTest()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Insert("myTable").AddAutoParam("a", "b").ODKU().ToString(),
                    "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InsertOnDuplicateKeyQueryTest05()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(
                    () => qb.Insert("myTable").AddAutoParam("a", "b").ODKU().AddFromInsert().Add("a a", "55"),
                    "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InvalidColumnNameTest01()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Select("myTable").Add("a a"), "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InvalidColumnNameTest02()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Select("myTable").Add(""), "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InvalidColumnNameTest03()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Insert("myTable").Add("a a", "a"), "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void InvalidColumnNameTest04()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Insert("myTable").Add("", "a"), "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void SelectQueryEmptyTest()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Select("myTable").ToString(), "QueryBuilder: " + qb);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "QueryBuilder")]
        [Test]
        public void UpdateQueryEmptyTest()
        {
            foreach (var qb in QBs)
            {
                Assert.Throws<InvalidQueryException>(() => qb.Update("myTable").ToString(), "QueryBuilder: " + qb);
            }
        }

        #endregion
    }
}