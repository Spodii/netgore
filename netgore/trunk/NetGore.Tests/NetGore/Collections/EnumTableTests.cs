using System;
using System.Linq;
using System.Reflection;
using NetGore.Collections;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class EnumTableTests
    {
        #region Unit tests

        [Test]
        public void CreateSeqTest()
        {
            const BindingFlags flags =
                BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var t = EnumTable.Create<ESeq, object>();
            Assert.IsNotNull(t);
            var internalCollection = t.GetType().GetField("_table", flags).GetValue(t);
            var typeName = internalCollection.GetType().FullName;
            Assert.IsTrue(typeName.Contains("RawTable"), "Expected RawTable, but was " + typeName);
        }

        [Test]
        public void CreateSparseTest()
        {
            const BindingFlags flags =
                BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var t = EnumTable.Create<ESparse, object>();
            Assert.IsNotNull(t);
            var internalCollection = t.GetType().GetField("_table", flags).GetValue(t);
            var typeName = internalCollection.GetType().FullName;
            Assert.IsTrue(typeName.Contains("DictionaryTable"), "Expected DictionaryTable, but was " + typeName);
        }

        [Test]
        public void GetSetInvalidSeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();
            Assert.Throws<ArgumentOutOfRangeException>(() => t[(ESeq)(-999)] = 50);

#pragma warning disable 219
            object x;
#pragma warning restore 219

            Assert.Throws<ArgumentOutOfRangeException>(() => x = t[(ESeq)(-999)]);
        }

        [Test]
        public void GetSetInvalidSparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();
            Assert.Throws<ArgumentOutOfRangeException>(() => t[(ESparse)(-999)] = 50);

#pragma warning disable 219
            object x;
#pragma warning restore 219

            Assert.Throws<ArgumentOutOfRangeException>(() => x = t[(ESparse)(-999)]);
        }

        [Test]
        public void GetSetSeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();
            Assert.AreEqual(null, t[ESeq.a]);

            t[ESeq.a] = 5;
            Assert.AreEqual(5, t[ESeq.a]);

            t[ESeq.d] = 50;
            Assert.AreEqual(50, t[ESeq.d]);

            t[ESeq.h] = "hello";
            Assert.AreEqual("hello", t[ESeq.h]);

            t[ESeq.d] = 123;
            Assert.AreEqual(123, t[ESeq.d]);
        }

        [Test]
        public void GetSetSparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();
            Assert.AreEqual(null, t[ESparse.a]);

            t[ESparse.a] = 5;
            Assert.AreEqual(5, t[ESparse.a]);

            t[ESparse.d] = 50;
            Assert.AreEqual(50, t[ESparse.d]);

            t[ESparse.h] = "hello";
            Assert.AreEqual("hello", t[ESparse.h]);

            t[ESparse.d] = 123;
            Assert.AreEqual(123, t[ESparse.d]);
        }

        [Test]
        public void IsValidKeySeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();

            foreach (var v in EnumHelper<ESeq>.Values)
            {
                Assert.IsTrue(t.IsValidKey(v));
            }

            Assert.IsFalse(t.IsValidKey((ESeq)(-999)));
        }

        [Test]
        public void IsValidKeySparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();

            foreach (var v in EnumHelper<ESparse>.Values)
            {
                Assert.IsTrue(t.IsValidKey(v));
            }

            Assert.IsFalse(t.IsValidKey((ESparse)(-999)));
        }

        [Test]
        public void TryGetSetSeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();
            object o;

            var r = t.TryGetValue(ESeq.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual(null, o);

            r = t.TrySetValue(ESeq.a, "asdf");
            Assert.IsTrue(r);

            r = t.TryGetValue(ESeq.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual("asdf", o);

            r = t.TryGetValue((ESeq)(-999), out o);
            Assert.IsFalse(r);

            r = t.TrySetValue((ESeq)(-999), "asdf");
            Assert.IsFalse(r);
        }

        [Test]
        public void TryGetSetSparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();
            object o;

            var r = t.TryGetValue(ESparse.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual(null, o);

            r = t.TrySetValue(ESparse.a, "asdf");
            Assert.IsTrue(r);

            r = t.TryGetValue(ESparse.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual("asdf", o);

            r = t.TryGetValue((ESparse)(-999), out o);
            Assert.IsFalse(r);

            r = t.TrySetValue((ESparse)(-999), "asdf");
            Assert.IsFalse(r);
        }

        #endregion

        /// <summary>
        /// A sequential enum.
        /// </summary>
        enum ESeq
        {
            a = -2,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i,
            j
        }

        /// <summary>
        /// A sparse enum.
        /// </summary>
        enum ESparse
        {
            a = -100,
            b = 50,
            c = 10,
            d = 25,
            e = 813,
            f = 52,
            g = 10023,
            h = -2342,
            i = 1908124124,
            j = -959874
        }
    }
}