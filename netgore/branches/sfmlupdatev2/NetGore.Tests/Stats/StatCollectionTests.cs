using System.Collections.Generic;
using System.Linq;
using NetGore.Stats;
using NUnit.Framework;

namespace NetGore.Tests.Stats
{
    [TestFixture]
    public class StatCollectionTests
    {
        static IEnumerable<IStatCollection<ST>> CreateStatCollections()
        {
            return new IStatCollection<ST>[] { new StatCollection<ST>(StatCollectionType.Base) };
        }

        #region Unit tests

        [Test]
        public void DeepCopyTest()
        {
            foreach (var c in CreateStatCollections())
            {
                c.SetAll(50);
                c[ST.A] = 10;

                var copy = c.DeepCopy();
                Assert.AreNotSame(copy, c);
                Assert.IsTrue(c.HasSameValues(copy));
                Assert.AreEqual(c.StatCollectionType, copy.StatCollectionType);

                c[ST.A] = 15;

                Assert.AreEqual(c[ST.A], 15);
                Assert.AreEqual(copy[ST.A], 10);
            }
        }

        [Test]
        public void HasAllGreaterOrEqualValuesTest()
        {
            var b = new StatCollection<ST>(StatCollectionType.Base);
            b[ST.A] = 10;
            b[ST.B] = 11;

            foreach (var c in CreateStatCollections())
            {
                c[ST.A] = 10;
                c[ST.B] = 11;

                Assert.IsTrue(c.HasAllGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 11;
                Assert.IsTrue(c.HasAllGreaterOrEqualValues(b));
                Assert.IsFalse(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 10;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAllGreaterOrEqualValues(b));
                Assert.IsFalse(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 11;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAllGreaterOrEqualValues(b));
                Assert.IsFalse(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 9;
                c[ST.B] = 12;
                Assert.IsFalse(c.HasAllGreaterOrEqualValues(b));
                Assert.IsFalse(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 10;
                c[ST.B] = 10;
                Assert.IsFalse(c.HasAllGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAllGreaterOrEqualValues(c));

                c[ST.A] = 5;
                c[ST.B] = 5;
                Assert.IsFalse(c.HasAllGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAllGreaterOrEqualValues(c));
            }
        }

        [Test]
        public void HasAllGreaterValuesTest()
        {
            var b = new StatCollection<ST>(StatCollectionType.Base);
            b[ST.A] = 10;
            b[ST.B] = 11;

            foreach (var c in CreateStatCollections())
            {
                c[ST.A] = 10;
                c[ST.B] = 11;

                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 11;
                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 10;
                c[ST.B] = 12;
                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 11;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 9;
                c[ST.B] = 12;
                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 10;
                c[ST.B] = 10;
                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsFalse(b.HasAllGreaterValues(c));

                c[ST.A] = 5;
                c[ST.B] = 5;
                Assert.IsFalse(c.HasAllGreaterValues(b));
                Assert.IsTrue(b.HasAllGreaterValues(c));
            }
        }

        [Test]
        public void HasAnyGreaterOrEqualValuesTest()
        {
            var b = new StatCollection<ST>(StatCollectionType.Base);
            b[ST.A] = 10;
            b[ST.B] = 11;

            foreach (var c in CreateStatCollections())
            {
                c[ST.A] = 10;
                c[ST.B] = 11;

                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 11;
                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 10;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 11;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsFalse(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 9;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 10;
                c[ST.B] = 10;
                Assert.IsTrue(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));

                c[ST.A] = 5;
                c[ST.B] = 5;
                Assert.IsFalse(c.HasAnyGreaterOrEqualValues(b));
                Assert.IsTrue(b.HasAnyGreaterOrEqualValues(c));
            }
        }

        [Test]
        public void HasAnyGreaterValuesTest()
        {
            var b = new StatCollection<ST>(StatCollectionType.Base);
            b[ST.A] = 10;
            b[ST.B] = 11;

            foreach (var c in CreateStatCollections())
            {
                c[ST.A] = 10;
                c[ST.B] = 11;

                Assert.IsFalse(c.HasAnyGreaterValues(b));
                Assert.IsFalse(b.HasAnyGreaterValues(c));

                c[ST.A] = 11;
                Assert.IsTrue(c.HasAnyGreaterValues(b));
                Assert.IsFalse(b.HasAnyGreaterValues(c));

                c[ST.A] = 10;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterValues(b));
                Assert.IsFalse(b.HasAnyGreaterValues(c));

                c[ST.A] = 11;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterValues(b));
                Assert.IsFalse(b.HasAnyGreaterValues(c));

                c[ST.A] = 9;
                c[ST.B] = 12;
                Assert.IsTrue(c.HasAnyGreaterValues(b));
                Assert.IsTrue(b.HasAnyGreaterValues(c));

                c[ST.A] = 10;
                c[ST.B] = 10;
                Assert.IsFalse(c.HasAnyGreaterValues(b));
                Assert.IsTrue(b.HasAnyGreaterValues(c));

                c[ST.A] = 5;
                c[ST.B] = 5;
                Assert.IsFalse(c.HasAnyGreaterValues(b));
                Assert.IsTrue(b.HasAnyGreaterValues(c));
            }
        }

        [Test]
        public void HasSameValuesTest()
        {
            var b = new StatCollection<ST>(StatCollectionType.Base);
            b[ST.A] = 10;
            b[ST.B] = 11;

            foreach (var c in CreateStatCollections())
            {
                c[ST.A] = 0;
                c[ST.B] = 0;

                Assert.IsFalse(c.HasSameValues(b));
                Assert.IsFalse(b.HasSameValues(c));

                c[ST.A] = 10;

                Assert.IsFalse(c.HasSameValues(b));
                Assert.IsFalse(b.HasSameValues(c));

                c[ST.B] = 11;

                Assert.IsTrue(c.HasSameValues(b));
                Assert.IsTrue(b.HasSameValues(c));

                c[ST.A] = 0;

                Assert.IsFalse(c.HasSameValues(b));
                Assert.IsFalse(b.HasSameValues(c));
            }
        }

        [Test]
        public void SetAllTest()
        {
            foreach (var c in CreateStatCollections())
            {
                c.SetAll(50);
                foreach (var v in c)
                {
                    Assert.AreEqual(50, v.Value);
                }

                c.SetAll(75);
                foreach (var v in c)
                {
                    Assert.AreEqual(75, v.Value);
                }
            }
        }

        [Test]
        public void StatChangedTest()
        {
            foreach (var c in CreateStatCollections())
            {
                var changed = false;

                Assert.IsFalse(changed);

                c[ST.A] = 1;

                Assert.IsFalse(changed);

                c.StatChanged += delegate { changed = true; };

                Assert.IsFalse(changed);

                c[ST.A] = 1;

                Assert.IsFalse(changed);

                c[ST.A] = 2;

                Assert.IsTrue(changed);
            }
        }

        #endregion

        enum ST : byte
        {
            A,
            B
        }
    }
}