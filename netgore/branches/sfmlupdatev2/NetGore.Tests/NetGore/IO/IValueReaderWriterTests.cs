using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class IValueReaderWriterTests
    {
        /// <summary>
        /// Handler for creating a value from a double.
        /// </summary>
        /// <typeparam name="T">The Type of value to create.</typeparam>
        /// <param name="value">The value to give the new type. Round is fine as long as it is consistent.</param>
        /// <returns>The new value as type <typeparamref name="T"/>.</returns>
        delegate T CreateValueTypeHandler<out T>(double value);

        /// <summary>
        /// Handler for reading a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="r">IValueReader to read from.</param>
        /// <param name="name">Name to use for reading.</param>
        /// <returns>The read value.</returns>
        delegate T ReadTestValuesHandler<out T>(IValueReader r, string name);

        /// <summary>
        /// Handler for writing a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="name">Name to use for writing.</param>
        /// <param name="value">Value to write.</param>
        delegate void WriteTestValuesHandler<in T>(IValueWriter w, string name, T value);

        static readonly object[] _emptyObjArray = new object[0];

        static void AssertArraysEqual<T>(IList<T> expected, IList<T> actual)
        {
            AssertArraysEqual(expected, actual, string.Empty, _emptyObjArray);
        }

        static void AssertArraysEqual<T>(IList<T> expected, IList<T> actual, string msg, params object[] objs)
        {
            var customMsg = string.Empty;
            if (!string.IsNullOrEmpty(msg))
            {
                if (objs != null && objs.Length > 0)
                    customMsg = string.Format(msg, objs);
                else
                    customMsg = msg;
            }

            Assert.AreEqual(expected.Count, actual.Count, "Lengths not equal. Type: `{0}`. Message: {1}", typeof(T), customMsg);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "Type: `{0}`  Index: `{1}`  Message: {2}", typeof(T), i, customMsg);
            }
        }

        /// <summary>
        /// Gets the key for a value.
        /// </summary>
        /// <param name="i">The index of the value.</param>
        /// <returns>The key for a value with index of <paramref name="i"/>.</returns>
        static string GetValueKey(int i)
        {
            return "V" + Parser.Invariant.ToString(i);
        }

        static string Implode(IEnumerable<string> src)
        {
            var sb = new StringBuilder();
            foreach (var s in src)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a range of values.
        /// </summary>
        /// <typeparam name="T">The Type to get the value as.</typeparam>
        /// <param name="start">The start value.</param>
        /// <param name="count">The number of values to get.</param>
        /// <param name="step">The step between each value.</param>
        /// <param name="creator">Handler to convert a double to type <typeparamref name="T"/>.</param>
        /// <returns>The array of values.</returns>
        static T[] Range<T>(double start, int count, double step, CreateValueTypeHandler<T> creator)
        {
            var ret = new T[count];

            var current = start;
            for (var i = 0; i < count; i++)
            {
                ret[i] = creator(current);
                current += step;
            }

            return ret;
        }

        /// <summary>
        /// Reads multiple test values. This is not like IValueReader.ReadValues as it does not use nodes
        /// nor does it track the number of items written. This is just to make it easy to read many
        /// values over a loop.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="r">IValueReader to read from.</param>
        /// <param name="expected">The values expected to be read.</param>
        /// <param name="readHandler">The read handler.</param>
        static void ReadTestValues<T>(IValueReader r, IList<T> expected, ReadTestValuesHandler<T> readHandler)
        {
            var actual = new T[expected.Count];

            for (var i = 0; i < expected.Count; i++)
            {
                actual[i] = readHandler(r, GetValueKey(i));
            }

            const string errmsg = "Writer Type: `{0}`";
            AssertArraysEqual(expected, actual, errmsg, r.GetType());
        }

        /// <summary>
        /// Writes multiple test values. This is not like IValueWriter.WriteValues as it does not use nodes
        /// nor does it track the number of items written. This is just to make it easy to write many
        /// values over a loop.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeHandler">The write handler.</param>
        static void WriteTestValues<T>(IValueWriter w, IList<T> values, WriteTestValuesHandler<T> writeHandler)
        {
            for (var i = 0; i < values.Count; i++)
            {
                writeHandler(w, GetValueKey(i), values[i]);
            }
        }

        #region Unit tests

        [Test]
        public void TestBools()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => x % 3 == 0);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadBool(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestBytes()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (byte)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadByte(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestDoubles()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadDouble(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestEnumNameWithNameLookup()
        {
            var values = new TestEnum[]
            {
                TestEnum.A, TestEnum.Dee, TestEnum.Eeie, TestEnum.Cee, TestEnum.Eeie, TestEnum.Ayche, TestEnum.B, TestEnum.B,
                TestEnum.Cee, TestEnum.G, TestEnum.Effffuh, TestEnum.A, TestEnum.B, TestEnum.Cee
            };

            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumName(GetValueKey(i), values[i]);
                        }
                    }

                    var r = creator.GetReader();
                    {
                        Assert.AreEqual(values[3], r.ReadEnumName<TestEnum>(GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumName<TestEnum>(GetValueKey(5)));
                        Assert.AreEqual(values[0], r.ReadEnumName<TestEnum>(GetValueKey(0)));
                        Assert.AreEqual(values[1], r.ReadEnumName<TestEnum>(GetValueKey(1)));
                        Assert.AreEqual(values[3], r.ReadEnumName<TestEnum>(GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumName<TestEnum>(GetValueKey(5)));
                        Assert.AreEqual(values[4], r.ReadEnumName<TestEnum>(GetValueKey(4)));
                        Assert.AreEqual(values[4], r.ReadEnumName<TestEnum>(GetValueKey(4)));
                    }
                }
            }
        }

        [Test]
        public void TestEnumNameWithoutNameLookup()
        {
            var values = new TestEnum[]
            {
                TestEnum.A, TestEnum.Dee, TestEnum.Eeie, TestEnum.Cee, TestEnum.Eeie, TestEnum.Ayche, TestEnum.B, TestEnum.B,
                TestEnum.Cee, TestEnum.G, TestEnum.Effffuh, TestEnum.A, TestEnum.B, TestEnum.Cee
            };

            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumName(GetValueKey(i), values[i]);
                        }
                    }

                    var r = creator.GetReader();
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            Assert.AreEqual(values[i], r.ReadEnumName<TestEnum>(GetValueKey(i)));
                        }
                    }
                }
            }
        }

        [Test]
        public void TestEnumValueWithoutNameLookup()
        {
            var values = new TestEnum[]
            {
                TestEnum.A, TestEnum.Dee, TestEnum.Eeie, TestEnum.Cee, TestEnum.Eeie, TestEnum.Ayche, TestEnum.B, TestEnum.B,
                TestEnum.Cee, TestEnum.G, TestEnum.Effffuh, TestEnum.A, TestEnum.B, TestEnum.Cee
            };

            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumValue(GetValueKey(i), values[i]);
                        }
                    }

                    var r = creator.GetReader();
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            Assert.AreEqual(values[i], r.ReadEnumValue<TestEnum>(GetValueKey(i)));
                        }
                    }
                }
            }
        }

        [Test]
        public void TestEnumValuesWithNameLookup()
        {
            var values = new TestEnum[]
            {
                TestEnum.A, TestEnum.Dee, TestEnum.Eeie, TestEnum.Cee, TestEnum.Eeie, TestEnum.Ayche, TestEnum.B, TestEnum.B,
                TestEnum.Cee, TestEnum.G, TestEnum.Effffuh, TestEnum.A, TestEnum.B, TestEnum.Cee
            };

            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumValue(GetValueKey(i), values[i]);
                        }
                    }

                    var r = creator.GetReader();
                    {
                        Assert.AreEqual(values[3], r.ReadEnumValue<TestEnum>(GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumValue<TestEnum>(GetValueKey(5)));
                        Assert.AreEqual(values[0], r.ReadEnumValue<TestEnum>(GetValueKey(0)));
                        Assert.AreEqual(values[1], r.ReadEnumValue<TestEnum>(GetValueKey(1)));
                        Assert.AreEqual(values[3], r.ReadEnumValue<TestEnum>(GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumValue<TestEnum>(GetValueKey(5)));
                        Assert.AreEqual(values[4], r.ReadEnumValue<TestEnum>(GetValueKey(4)));
                        Assert.AreEqual(values[4], r.ReadEnumValue<TestEnum>(GetValueKey(4)));
                    }
                }
            }
        }

        [Test]
        public void TestFloats()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (float)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadFloat(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestIllegalXmlCharactersInStrings()
        {
            var illegalStrs = new string[] { "<", ">", "\\", "/", "&", "'", "\"", "?", Environment.NewLine };
            var allStrings = Implode(illegalStrs);

            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < illegalStrs.Length; i++)
                        {
                            w.Write(GetValueKey(i), illegalStrs[i]);
                        }

                        w.Write("All", allStrings);
                    }

                    var r = creator.GetReader();

                    for (var i = 0; i < illegalStrs.Length; i++)
                    {
                        Assert.AreEqual(illegalStrs[i], r.ReadString(GetValueKey(i)));
                    }

                    Assert.AreEqual(allStrings, r.ReadString("All"));
                }
            }
        }

        [Test]
        public void TestInts()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (int)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadInt(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestLongs()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (long)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadLong(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestNameLookup()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    const bool a = true;
                    const int b = 10;
                    const float c = 133.2f;
                    const int d = 2051;
                    const bool e = false;
                    const string f = "asdf asdf lkjwreoiuwalj jk wark qoiuwer";
                    const int g = 2092142;

                    using (var w = creator.GetWriter())
                    {
                        w.Write("a", a);
                        w.Write("b", b);
                        w.Write("c", c);
                        w.Write("d", d);
                        w.Write("e", e);
                        w.Write("f", f);
                        w.Write("g", g);
                    }

                    var r = creator.GetReader();
                    {
                        Assert.AreEqual(f, r.ReadString("f"));
                        Assert.AreEqual(c, r.ReadFloat("c"));
                        Assert.AreEqual(b, r.ReadInt("b"));
                        Assert.AreEqual(g, r.ReadInt("g"));
                        Assert.AreEqual(d, r.ReadInt("d"));
                        Assert.AreEqual(e, r.ReadBool("e"));
                        Assert.AreEqual(f, r.ReadString("f"));
                        Assert.AreEqual(d, r.ReadInt("d"));
                        Assert.AreEqual(e, r.ReadBool("e"));
                        Assert.AreEqual(f, r.ReadString("f"));
                    }
                }
            }
        }

        [Test]
        public void TestNameLookupCaseSensitivity()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    // Name lookups should be case-insensitive

                    const bool a = true;
                    const int b = 10;
                    const float c = 133.2f;
                    const int d = 2051;
                    const bool e = false;
                    const string f = "asdf asdf lkjwreoiuwalj jk wark qoiuwer";
                    const int g = 2092142;

                    using (var w = creator.GetWriter())
                    {
                        w.Write("A", a);
                        w.Write("B", b);
                        w.Write("c", c);
                        w.Write("D", d);
                        w.Write("e", e);
                        w.Write("F", f);
                        w.Write("G", g);
                    }

                    var r = creator.GetReader();
                    {
                        Assert.AreEqual(f, r.ReadString("f"));
                        Assert.AreEqual(c, r.ReadFloat("c"));
                        Assert.AreEqual(b, r.ReadInt("b"));
                        Assert.AreEqual(g, r.ReadInt("g"));
                        Assert.AreEqual(d, r.ReadInt("d"));
                        Assert.AreEqual(e, r.ReadBool("e"));
                        Assert.AreEqual(f, r.ReadString("f"));
                        Assert.AreEqual(d, r.ReadInt("d"));
                        Assert.AreEqual(e, r.ReadBool("e"));
                        Assert.AreEqual(f, r.ReadString("f"));
                    }
                }
            }
        }

        [Test]
        public void TestNameLookupWithNodes()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    // Name lookups should be case-insensitive

                    const bool a1 = true;
                    const int b1 = 10;
                    const float c1 = 133.2f;
                    const int d1 = 2051;
                    const bool e1 = false;
                    const string f1 = "asdf asdf lkjwreoiuwalj jk wark qoiuwer";
                    const int g1 = 2092142;

                    const bool a2 = true;
                    const int b2 = 578;
                    const float c2 = 17833.2f;
                    const int d2 = 204551;
                    const bool e2 = false;
                    const string f2 = "asdfaasdfasdfwerqwerasdvxcvasdfaewalj jk wark qoiuwer";
                    const int g2 = 2092142;

                    const bool a3 = false;
                    const int b3 = 1054;
                    const float c3 = 13993.2f;
                    const int d3 = 201151;
                    const bool e3 = false;
                    const string f3 = "asdf asdasfwerqwreadsahhewwqrqwreqref lkjwreoiuwalj jk wark qoiuwer";
                    const int g3 = 2342;

                    using (var w = creator.GetWriter())
                    {
                        w.WriteStartNode("NodeA");
                        w.Write("a", a1);
                        w.Write("b", b1);
                        w.Write("c", c1);
                        w.Write("d", d1);
                        w.Write("e", e1);
                        w.Write("f", f1);
                        w.Write("g", g1);
                        w.WriteEndNode("NodeA");

                        w.WriteStartNode("NodeB");
                        w.Write("d", d2);
                        w.Write("e", e2);
                        w.Write("f", f2);
                        w.Write("g", g2);
                        w.Write("a", a2);
                        w.Write("b", b2);
                        w.Write("c", c2);
                        w.WriteEndNode("NodeB");

                        w.WriteStartNode("NodeC");
                        w.Write("d", d3);
                        w.Write("e", e3);
                        w.Write("f", f3);
                        w.Write("g", g3);
                        w.Write("a", a3);
                        w.Write("b", b3);
                        w.Write("c", c3);
                        w.WriteEndNode("NodeC");
                    }

                    var r = creator.GetReader();
                    {
                        var nodeB = r.ReadNode("NodeB");
                        var nodeC = r.ReadNode("NodeC");
                        var nodeA = r.ReadNode("NodeA");

                        Assert.AreEqual(f2, nodeB.ReadString("f"));
                        Assert.AreEqual(c2, nodeB.ReadFloat("c"));
                        Assert.AreEqual(b2, nodeB.ReadInt("b"));
                        Assert.AreEqual(g2, nodeB.ReadInt("g"));
                        Assert.AreEqual(d2, nodeB.ReadInt("d"));
                        Assert.AreEqual(e2, nodeB.ReadBool("e"));
                        Assert.AreEqual(f2, nodeB.ReadString("f"));
                        Assert.AreEqual(d2, nodeB.ReadInt("d"));
                        Assert.AreEqual(e2, nodeB.ReadBool("e"));
                        Assert.AreEqual(f2, nodeB.ReadString("f"));

                        Assert.AreEqual(f1, nodeA.ReadString("f"));
                        Assert.AreEqual(c1, nodeA.ReadFloat("c"));
                        Assert.AreEqual(b1, nodeA.ReadInt("b"));
                        Assert.AreEqual(g1, nodeA.ReadInt("g"));
                        Assert.AreEqual(d1, nodeA.ReadInt("d"));
                        Assert.AreEqual(e1, nodeA.ReadBool("e"));
                        Assert.AreEqual(f1, nodeA.ReadString("f"));
                        Assert.AreEqual(d1, nodeA.ReadInt("d"));
                        Assert.AreEqual(e1, nodeA.ReadBool("e"));
                        Assert.AreEqual(f1, nodeA.ReadString("f"));

                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                        Assert.AreEqual(c3, nodeC.ReadFloat("c"));
                        Assert.AreEqual(b3, nodeC.ReadInt("b"));
                        Assert.AreEqual(g3, nodeC.ReadInt("g"));
                        Assert.AreEqual(d3, nodeC.ReadInt("d"));
                        Assert.AreEqual(e3, nodeC.ReadBool("e"));
                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                        Assert.AreEqual(d3, nodeC.ReadInt("d"));
                        Assert.AreEqual(e3, nodeC.ReadBool("e"));
                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                    }
                }
            }
        }

        [Test]
        public void TestNameLookupWithNodesCaseSensitivity()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    // Name lookups should be case-insensitive

                    const bool a1 = true;
                    const int b1 = 10;
                    const float c1 = 133.2f;
                    const int d1 = 2051;
                    const bool e1 = false;
                    const string f1 = "asdf asdf lkjwreoiuwalj jk wark qoiuwer";
                    const int g1 = 2092142;

                    const bool a2 = true;
                    const int b2 = 578;
                    const float c2 = 17833.2f;
                    const int d2 = 204551;
                    const bool e2 = false;
                    const string f2 = "asdfaasdfasdfwerqwerasdvxcvasdfaewalj jk wark qoiuwer";
                    const int g2 = 2092142;

                    const bool a3 = false;
                    const int b3 = 1054;
                    const float c3 = 13993.2f;
                    const int d3 = 201151;
                    const bool e3 = false;
                    const string f3 = "asdf asdasfwerqwreadsahhewwqrqwreqref lkjwreoiuwalj jk wark qoiuwer";
                    const int g3 = 2342;

                    using (var w = creator.GetWriter())
                    {
                        w.WriteStartNode("NodeA");
                        w.Write("a", a1);
                        w.Write("B", b1);
                        w.Write("c", c1);
                        w.Write("D", d1);
                        w.Write("E", e1);
                        w.Write("f", f1);
                        w.Write("g", g1);
                        w.WriteEndNode("NodeA");

                        w.WriteStartNode("NodeB");
                        w.Write("D", d2);
                        w.Write("E", e2);
                        w.Write("F", f2);
                        w.Write("G", g2);
                        w.Write("a", a2);
                        w.Write("b", b2);
                        w.Write("c", c2);
                        w.WriteEndNode("NodeB");

                        w.WriteStartNode("NodeC");
                        w.Write("d", d3);
                        w.Write("e", e3);
                        w.Write("f", f3);
                        w.Write("g", g3);
                        w.Write("A", a3);
                        w.Write("B", b3);
                        w.Write("c", c3);
                        w.WriteEndNode("NodeC");
                    }

                    var r = creator.GetReader();
                    {
                        var nodeB = r.ReadNode("NodeB");
                        var nodeC = r.ReadNode("NodeC");
                        var nodeA = r.ReadNode("NodeA");

                        Assert.AreEqual(f2, nodeB.ReadString("f"));
                        Assert.AreEqual(c2, nodeB.ReadFloat("c"));
                        Assert.AreEqual(b2, nodeB.ReadInt("b"));
                        Assert.AreEqual(g2, nodeB.ReadInt("g"));
                        Assert.AreEqual(d2, nodeB.ReadInt("d"));
                        Assert.AreEqual(e2, nodeB.ReadBool("e"));
                        Assert.AreEqual(f2, nodeB.ReadString("f"));
                        Assert.AreEqual(d2, nodeB.ReadInt("d"));
                        Assert.AreEqual(e2, nodeB.ReadBool("e"));
                        Assert.AreEqual(f2, nodeB.ReadString("f"));

                        Assert.AreEqual(f1, nodeA.ReadString("f"));
                        Assert.AreEqual(c1, nodeA.ReadFloat("c"));
                        Assert.AreEqual(b1, nodeA.ReadInt("b"));
                        Assert.AreEqual(g1, nodeA.ReadInt("g"));
                        Assert.AreEqual(d1, nodeA.ReadInt("d"));
                        Assert.AreEqual(e1, nodeA.ReadBool("e"));
                        Assert.AreEqual(f1, nodeA.ReadString("f"));
                        Assert.AreEqual(d1, nodeA.ReadInt("d"));
                        Assert.AreEqual(e1, nodeA.ReadBool("e"));
                        Assert.AreEqual(f1, nodeA.ReadString("f"));

                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                        Assert.AreEqual(c3, nodeC.ReadFloat("c"));
                        Assert.AreEqual(b3, nodeC.ReadInt("b"));
                        Assert.AreEqual(g3, nodeC.ReadInt("g"));
                        Assert.AreEqual(d3, nodeC.ReadInt("d"));
                        Assert.AreEqual(e3, nodeC.ReadBool("e"));
                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                        Assert.AreEqual(d3, nodeC.ReadInt("d"));
                        Assert.AreEqual(e3, nodeC.ReadBool("e"));
                        Assert.AreEqual(f3, nodeC.ReadString("f"));
                    }
                }
            }
        }

        [Test]
        public void TestNodes()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNodes)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (var w = creator.GetWriter())
                    {
                        w.WriteMany("v1", v1, w.Write);
                        w.WriteMany("v2", v2, w.Write);
                        w.WriteMany("v3", v3, w.Write);
                        w.WriteMany("v4", v4, w.Write);
                        w.WriteMany("v5", v5, w.Write);
                        w.WriteMany("v6", v6, w.Write);
                    }

                    var r = creator.GetReader();
                    {
                        var r1 = r.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname)));
                        var r2 = r.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname)));
                        var r3 = r.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname)));
                        var r4 = r.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname)));
                        var r5 = r.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname)));
                        var r6 = r.ReadMany("v6", ((preader, pname) => preader.ReadString(pname)));

                        AssertArraysEqual(v1, r1);
                        AssertArraysEqual(v2, r2);
                        AssertArraysEqual(v3, r3);
                        AssertArraysEqual(v4, r4);
                        AssertArraysEqual(v5, r5);
                        AssertArraysEqual(v6, r6);
                    }
                }
            }
        }

        [Test]
        public void TestNodesBoolsOnly()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var tmp = createCreator())
                {
                    if (!tmp.SupportsNodes)
                        continue;
                }

                for (var i = 1; i < 40; i++)
                {
                    using (var creator = createCreator())
                    {
                        var v1 = Range(0, i, 1, x => x % 3 == 0);

                        using (var w = creator.GetWriter())
                        {
                            w.WriteMany("v1", v1, w.Write);
                        }

                        var r = creator.GetReader();
                        {
                            var r1 = r.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname)));

                            AssertArraysEqual(v1, r1);
                        }
                    }
                }
            }
        }

        [Test]
        public void TestNodesDeepLinear()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNodes)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (var w = creator.GetWriter())
                    {
                        w.WriteStartNode("a1");
                        {
                            w.WriteMany("v1", v1, w.Write);
                            w.WriteMany("v2", v2, w.Write);
                            w.WriteMany("v3", v3, w.Write);
                            w.WriteMany("v4", v4, w.Write);
                            w.WriteMany("v5", v5, w.Write);
                            w.WriteMany("v6", v6, w.Write);

                            w.WriteStartNode("b1");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);

                                w.WriteStartNode("c1");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);
                                }
                                w.WriteEndNode("c1");

                                w.WriteStartNode("c2");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);

                                    w.WriteStartNode("d1");
                                    {
                                        w.WriteMany("v1", v1, w.Write);
                                        w.WriteMany("v2", v2, w.Write);
                                        w.WriteMany("v3", v3, w.Write);
                                        w.WriteMany("v4", v4, w.Write);
                                        w.WriteMany("v5", v5, w.Write);
                                        w.WriteMany("v6", v6, w.Write);
                                    }
                                    w.WriteEndNode("d1");
                                }
                                w.WriteEndNode("c2");
                            }
                            w.WriteEndNode("b1");

                            w.WriteStartNode("b2");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);

                                w.WriteStartNode("c1");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);
                                }
                                w.WriteEndNode("c1");
                            }
                            w.WriteEndNode("b2");
                        }
                        w.WriteEndNode("a1");

                        w.WriteStartNode("a2");
                        {
                            w.WriteMany("v1", v1, w.Write);
                            w.WriteMany("v2", v2, w.Write);
                            w.WriteMany("v3", v3, w.Write);
                            w.WriteMany("v4", v4, w.Write);
                            w.WriteMany("v5", v5, w.Write);
                            w.WriteMany("v6", v6, w.Write);

                            w.WriteStartNode("b1");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);
                            }
                            w.WriteEndNode("b1");

                            w.WriteStartNode("b2");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);
                            }
                            w.WriteEndNode("b2");
                        }
                        w.WriteEndNode("a2");
                    }

                    var r = creator.GetReader();
                    {
                        var a1 = r.ReadNode("a1");
                        var c = a1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b1 = a1.ReadNode("b1");
                        c = a1b1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b1c1 = a1b1.ReadNode("c1");
                        c = a1b1c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b1c2 = a1b1.ReadNode("c2");
                        c = a1b1c2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b1c2d1 = a1b1c2.ReadNode("d1");
                        c = a1b1c2d1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b2 = a1.ReadNode("b2");
                        c = a1b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a1b2c1 = a1b2.ReadNode("c1");
                        c = a1b2c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a2 = r.ReadNode("a2");
                        c = a2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a2b1 = a2.ReadNode("b1");
                        c = a2b1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        var a2b2 = a2.ReadNode("b2");
                        c = a2b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));
                    }
                }
            }
        }

        [Test]
        public void TestNodesDeepRandom()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    if (!creator.SupportsNodes || !creator.SupportsNameLookup)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (var w = creator.GetWriter())
                    {
                        w.WriteStartNode("a1");
                        {
                            w.WriteMany("v1", v1, w.Write);
                            w.WriteMany("v2", v2, w.Write);
                            w.WriteMany("v3", v3, w.Write);
                            w.WriteMany("v4", v4, w.Write);
                            w.WriteMany("v5", v5, w.Write);
                            w.WriteMany("v6", v6, w.Write);

                            w.WriteStartNode("b1");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);

                                w.WriteStartNode("c1");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);
                                }
                                w.WriteEndNode("c1");

                                w.WriteStartNode("c2");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);

                                    w.WriteStartNode("d1");
                                    {
                                        w.WriteMany("v1", v1, w.Write);
                                        w.WriteMany("v2", v2, w.Write);
                                        w.WriteMany("v3", v3, w.Write);
                                        w.WriteMany("v4", v4, w.Write);
                                        w.WriteMany("v5", v5, w.Write);
                                        w.WriteMany("v6", v6, w.Write);
                                    }
                                    w.WriteEndNode("d1");
                                }
                                w.WriteEndNode("c2");
                            }
                            w.WriteEndNode("b1");

                            w.WriteStartNode("b2");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);

                                w.WriteStartNode("c1");
                                {
                                    w.WriteMany("v1", v1, w.Write);
                                    w.WriteMany("v2", v2, w.Write);
                                    w.WriteMany("v3", v3, w.Write);
                                    w.WriteMany("v4", v4, w.Write);
                                    w.WriteMany("v5", v5, w.Write);
                                    w.WriteMany("v6", v6, w.Write);
                                }
                                w.WriteEndNode("c1");
                            }
                            w.WriteEndNode("b2");
                        }
                        w.WriteEndNode("a1");

                        w.WriteStartNode("a2");
                        {
                            w.WriteMany("v1", v1, w.Write);
                            w.WriteMany("v2", v2, w.Write);
                            w.WriteMany("v3", v3, w.Write);
                            w.WriteMany("v4", v4, w.Write);
                            w.WriteMany("v5", v5, w.Write);
                            w.WriteMany("v6", v6, w.Write);

                            w.WriteStartNode("b1");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);
                            }
                            w.WriteEndNode("b1");

                            w.WriteStartNode("b2");
                            {
                                w.WriteMany("v1", v1, w.Write);
                                w.WriteMany("v2", v2, w.Write);
                                w.WriteMany("v3", v3, w.Write);
                                w.WriteMany("v4", v4, w.Write);
                                w.WriteMany("v5", v5, w.Write);
                                w.WriteMany("v6", v6, w.Write);
                            }
                            w.WriteEndNode("b2");
                        }
                        w.WriteEndNode("a2");
                    }

                    var r = creator.GetReader();
                    {
                        var a1 = r.ReadNode("a1");
                        var a1b1 = a1.ReadNode("b1");
                        var a1b1c1 = a1b1.ReadNode("c1");
                        var a1b1c2 = a1b1.ReadNode("c2");
                        var a1b1c2d1 = a1b1c2.ReadNode("d1");
                        var a1b2 = a1.ReadNode("b2");
                        var a1b2c1 = a1b2.ReadNode("c1");

                        var a2 = r.ReadNode("a2");
                        var a2b1 = a2.ReadNode("b1");
                        var a2b2 = a2.ReadNode("b2");

                        var c = a1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b2c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b1c2d1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b1c2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a1b1c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a2b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a2b1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        c = a2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));
                    }
                }
            }
        }

        [Test]
        public void TestSBytes()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (sbyte)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadSByte(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestShorts()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (short)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadShort(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestStrings()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadString(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestUInts()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (uint)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadUInt(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestULongs()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (ulong)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadULong(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestUShorts()
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (ushort)x);

                    using (var w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    var r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadUShort(pname)));
                    }
                }
            }
        }

        #endregion

        enum TestEnum
        {
            A = -100,
            B,
            Cee = 0,
            Dee,
            Eeie,
            Effffuh,
            G,
            Ayche = 100
        }
    }
}