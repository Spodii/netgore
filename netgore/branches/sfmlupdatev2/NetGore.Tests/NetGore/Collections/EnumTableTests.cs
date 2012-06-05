using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class EnumTableTests
    {
        #region Unit tests

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RawTableBool")]
        [Test]
        public void CreateSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            Assert.IsNotNull(t);
            var typeName = t.GetType().FullName;
            Assert.IsTrue(typeName.Contains("RawTableBool"), "Expected RawTableBool, but was " + typeName);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RawTable")]
        [Test]
        public void CreateSeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();
            Assert.IsNotNull(t);
            var typeName = t.GetType().FullName;
            Assert.IsTrue(typeName.Contains("RawTable"), "Expected RawTable, but was " + typeName);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DictionaryTable")]
        [Test]
        public void CreateSparseBoolTest()
        {
            var t = EnumTable.Create<ESparse, bool>();
            Assert.IsNotNull(t);
            var typeName = t.GetType().FullName;
            Assert.IsTrue(typeName.Contains("DictionaryTable"), "Expected DictionaryTable, but was " + typeName);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DictionaryTable")]
        [Test]
        public void CreateSparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();
            Assert.IsNotNull(t);
            var typeName = t.GetType().FullName;
            Assert.IsTrue(typeName.Contains("DictionaryTable"), "Expected DictionaryTable, but was " + typeName);
        }

        [Test]
        public void DeepCopySeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            t.SetAll(true);
            Assert.IsTrue(t.All(x => x.Value));

            var t2 = t.DeepCopy();
            Assert.IsTrue(t2.All(x => x.Value));
        }

        [Test]
        public void DeepCopySeqTest()
        {
            var t = EnumTable.Create<ESeq, string>();
            t.SetAll("hi");
            Assert.IsTrue(t.All(x => x.Value == "hi"));

            var t2 = t.DeepCopy();
            Assert.IsTrue(t2.All(x => x.Value == "hi"));
        }

        [Test]
        public void DeepCopySparseTest()
        {
            var t = EnumTable.Create<ESparse, string>();
            t.SetAll("hi");
            Assert.IsTrue(t.All(x => x.Value == "hi"));

            var t2 = t.DeepCopy();
            Assert.IsTrue(t2.All(x => x.Value == "hi"));
        }

        [Test]
        public void EnumerateSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            t[ESeq.c] = true;

            foreach (var v in t)
            {
                if (v.Key == ESeq.c)
                    Assert.AreEqual(true, v.Value);
                else
                    Assert.AreEqual(false, v.Value);
            }

            var keys = t.Select(x => x.Key).ToArray();
            var expectedKeys = EnumHelper<ESeq>.Values.ToArray();

            Assert.AreEqual(keys.Length, expectedKeys.Length);
            Assert.IsTrue(keys.ContainSameElements(expectedKeys));
        }

        [Test]
        public void EnumerateSeqTest()
        {
            var t = EnumTable.Create<ESeq, object>();
            t[ESeq.c] = "hello";

            foreach (var v in t)
            {
                if (v.Key == ESeq.c)
                    Assert.AreEqual("hello", v.Value);
                else
                    Assert.AreEqual(null, v.Value);
            }

            var keys = t.Select(x => x.Key).ToArray();
            var expectedKeys = EnumHelper<ESeq>.Values.ToArray();

            Assert.AreEqual(keys.Length, expectedKeys.Length);
            Assert.IsTrue(keys.ContainSameElements(expectedKeys));
        }

        [Test]
        public void EnumerateSparseTest()
        {
            var t = EnumTable.Create<ESparse, object>();
            t[ESparse.c] = "hello";

            foreach (var v in t)
            {
                if (v.Key == ESparse.c)
                    Assert.AreEqual("hello", v.Value);
                else
                    Assert.AreEqual(null, v.Value);
            }

            var keys = t.Select(x => x.Key).ToArray();
            var expectedKeys = EnumHelper<ESparse>.Values.ToArray();

            Assert.AreEqual(keys.Length, expectedKeys.Length);
            Assert.IsTrue(keys.ContainSameElements(expectedKeys));
        }

        [Test]
        public void GetSetInvalidSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            Assert.Throws<ArgumentOutOfRangeException>(() => t[(ESeq)(-999)] = true);

#pragma warning disable 219
            bool x;
#pragma warning restore 219

            Assert.Throws<ArgumentOutOfRangeException>(() => x = t[(ESeq)(-999)]);
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
        public void GetSetSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            Assert.AreEqual(false, t[ESeq.a]);

            t[ESeq.a] = true;
            Assert.AreEqual(true, t[ESeq.a]);

            t[ESeq.d] = true;
            Assert.AreEqual(true, t[ESeq.d]);

            t[ESeq.h] = false;
            Assert.AreEqual(false, t[ESeq.h]);

            t[ESeq.d] = true;
            Assert.AreEqual(true, t[ESeq.d]);
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
        public void IsValidKeySeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();

            foreach (var v in EnumHelper<ESeq>.Values)
            {
                Assert.IsTrue(t.IsValidKey(v));
            }

            Assert.IsFalse(t.IsValidKey((ESeq)(-999)));
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
        public void SetAllAndClearSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            Assert.IsTrue(t.All(x => x.Value == default(bool)));

            t.SetAll(true);
            Assert.IsTrue(t.All(x => x.Value));

            t.Clear();
            Assert.IsTrue(t.All(x => x.Value == default(bool)));
        }

        [Test]
        public void SetAllAndClearSeqTest()
        {
            var t = EnumTable.Create<ESeq, string>();
            Assert.IsTrue(t.All(x => x.Value == null));

            t.SetAll("hi");
            Assert.IsTrue(t.All(x => x.Value == "hi"));

            t.Clear();
            Assert.IsTrue(t.All(x => x.Value == null));
        }

        [Test]
        public void SetAllAndClearSparseTest()
        {
            var t = EnumTable.Create<ESparse, string>();
            Assert.IsTrue(t.All(x => x.Value == null));

            t.SetAll("hi");
            Assert.IsTrue(t.All(x => x.Value == "hi"));

            t.Clear();
            Assert.IsTrue(t.All(x => x.Value == null));
        }

        [Test]
        public void TryGetSetSeqBoolTest()
        {
            var t = EnumTable.Create<ESeq, bool>();
            bool o;

            var r = t.TryGetValue(ESeq.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual(false, o);

            r = t.TrySetValue(ESeq.a, true);
            Assert.IsTrue(r);

            r = t.TryGetValue(ESeq.a, out o);
            Assert.IsTrue(r);
            Assert.AreEqual(true, o);

            r = t.TryGetValue((ESeq)(-999), out o);
            Assert.IsFalse(r);

            r = t.TrySetValue((ESeq)(-999), true);
            Assert.IsFalse(r);
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