using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NetGore;
using NetGore;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class IValueReaderWriterTests
    {
        const int _bufferSize = 1024 * 1024;

        /// <summary>
        /// The CreateCreatorHandlers that will be used to create the ReaderWriterCreatorBase instances.
        /// </summary>
        static readonly IEnumerable<CreateCreatorHandler> _createCreators;

        static readonly List<string> _createdTempFiles = new List<string>();

        static readonly object[] _emptyObjArray = new object[0];

        /// <summary>
        /// Initializes the <see cref="IValueReaderWriterTests"/> class.
        /// </summary>
        static IValueReaderWriterTests()
        {
            // Create the delegates for creating the ReaderWriterCreatorBases
            _createCreators = new CreateCreatorHandler[]
            {
                () => new MemoryBinaryValueReaderWriterCreator(), () => new FileBinaryValueReaderWriterCreator(),
                () => new XmlValueReaderWriterCreator(), () => new BitStreamReaderWriterCreator(),
                () => new BitStreamByteArrayReaderWriterCreator()
            };
        }

        static void AssertArraysEqual<T>(T[] expected, T[] actual)
        {
            AssertArraysEqual(expected, actual, string.Empty, _emptyObjArray);
        }

        static void AssertArraysEqual<T>(T[] expected, T[] actual, string msg, params object[] objs)
        {
            string customMsg = string.Empty;
            if (!string.IsNullOrEmpty(msg))
            {
                if (objs != null && objs.Length > 0)
                    customMsg = string.Format(msg, objs);
                else
                    customMsg = msg;
            }

            Assert.AreEqual(expected.Length, actual.Length, "Lengths not equal. Type: `{0}`. Message: {1}", typeof(T), customMsg);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "Type: `{0}`  Index: `{1}`  Message: {2}", typeof(T), i, customMsg);
            }
        }

        /// <summary>
        /// Gets the path for a temp file.
        /// </summary>
        /// <returns>The path for a temp file.</returns>
        static string GetTempFile()
        {
            if (_createdTempFiles.Count > 3)
            {
                // Do a garbage collection to see if there is crap still out there, but waiting to be destructed
                GC.Collect();
                if (_createdTempFiles.Count > 3)
                    throw new Exception("Too many temp files are out. Make sure they are being released!");
                else
                    Debug.Fail("Too many objects are using the destructor to clear the temp files. Use IDisposable, damnit!");
            }

            string ret = Path.GetTempFileName();
            _createdTempFiles.Add(ret);
            return ret;
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
            StringBuilder sb = new StringBuilder();
            foreach (string s in src)
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

            double current = start;
            for (int i = 0; i < count; i++)
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
        static void ReadTestValues<T>(IValueReader r, T[] expected, ReadTestValuesHandler<T> readHandler)
        {
            var actual = new T[expected.Length];

            for (int i = 0; i < expected.Length; i++)
            {
                actual[i] = readHandler(r, GetValueKey(i));
            }

            const string errmsg = "Writer Type: `{0}`";
            AssertArraysEqual(expected, actual, errmsg, r.GetType());
        }

        /// <summary>
        /// Releases a file used with GetTempFile().
        /// </summary>
        /// <param name="filePath">Path to the file to release.</param>
        static void ReleaseFile(string filePath)
        {
            _createdTempFiles.Remove(filePath);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        [Test]
        public void TestBools()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => x % 3 == 0);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadBool(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestBytes()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (byte)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadByte(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestDoubles()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
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

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    using (IValueWriter w = creator.GetWriter())
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumName(GetValueKey(i), values[i]);
                        }
                    }

                    IValueReader r = creator.GetReader();
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

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    using (IValueWriter w = creator.GetWriter())
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumName(GetValueKey(i), values[i]);
                        }
                    }

                    IValueReader r = creator.GetReader();
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            Assert.AreEqual(values[i], r.ReadEnumName<TestEnum>(GetValueKey(i)));
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
            TestEnumHelper enumHelper = new TestEnumHelper();

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    if (!creator.SupportsNameLookup)
                        continue;

                    using (IValueWriter w = creator.GetWriter())
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumValue(enumHelper, GetValueKey(i), values[i]);
                        }
                    }

                    IValueReader r = creator.GetReader();
                    {
                        Assert.AreEqual(values[3], r.ReadEnumValue(enumHelper, GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumValue(enumHelper, GetValueKey(5)));
                        Assert.AreEqual(values[0], r.ReadEnumValue(enumHelper, GetValueKey(0)));
                        Assert.AreEqual(values[1], r.ReadEnumValue(enumHelper, GetValueKey(1)));
                        Assert.AreEqual(values[3], r.ReadEnumValue(enumHelper, GetValueKey(3)));
                        Assert.AreEqual(values[5], r.ReadEnumValue(enumHelper, GetValueKey(5)));
                        Assert.AreEqual(values[4], r.ReadEnumValue(enumHelper, GetValueKey(4)));
                        Assert.AreEqual(values[4], r.ReadEnumValue(enumHelper, GetValueKey(4)));
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
            TestEnumHelper enumHelper = new TestEnumHelper();

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    using (IValueWriter w = creator.GetWriter())
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            w.WriteEnumValue(enumHelper, GetValueKey(i), values[i]);
                        }
                    }

                    IValueReader r = creator.GetReader();
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            Assert.AreEqual(values[i], r.ReadEnumValue(enumHelper, GetValueKey(i)));
                        }
                    }
                }
            }
        }

        [Test]
        public void TestFloats()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (float)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
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
            string allStrings = Implode(illegalStrs);

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    using (IValueWriter w = creator.GetWriter())
                    {
                        for (int i = 0; i < illegalStrs.Length; i++)
                        {
                            w.Write(GetValueKey(i), illegalStrs[i]);
                        }

                        w.Write("All", allStrings);
                    }

                    IValueReader r = creator.GetReader();

                    for (int i = 0; i < illegalStrs.Length; i++)
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (int)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadInt(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestLongs()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (long)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadLong(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestNameLookup()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
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

                    using (IValueWriter w = creator.GetWriter())
                    {
                        w.Write("a", a);
                        w.Write("b", b);
                        w.Write("c", c);
                        w.Write("d", d);
                        w.Write("e", e);
                        w.Write("f", f);
                        w.Write("g", g);
                    }

                    IValueReader r = creator.GetReader();
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
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

                    using (IValueWriter w = creator.GetWriter())
                    {
                        w.Write("A", a);
                        w.Write("B", b);
                        w.Write("c", c);
                        w.Write("D", d);
                        w.Write("e", e);
                        w.Write("F", f);
                        w.Write("G", g);
                    }

                    IValueReader r = creator.GetReader();
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
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

                    using (IValueWriter w = creator.GetWriter())
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

                    IValueReader r = creator.GetReader();
                    {
                        IValueReader nodeB = r.ReadNode("NodeB");
                        IValueReader nodeC = r.ReadNode("NodeC");
                        IValueReader nodeA = r.ReadNode("NodeA");

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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
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

                    using (IValueWriter w = creator.GetWriter())
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

                    IValueReader r = creator.GetReader();
                    {
                        IValueReader nodeB = r.ReadNode("NodeB");
                        IValueReader nodeC = r.ReadNode("NodeC");
                        IValueReader nodeA = r.ReadNode("NodeA");

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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    if (!creator.SupportsNodes)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (IValueWriter w = creator.GetWriter())
                    {
                        w.WriteMany("v1", v1, w.Write);
                        w.WriteMany("v2", v2, w.Write);
                        w.WriteMany("v3", v3, w.Write);
                        w.WriteMany("v4", v4, w.Write);
                        w.WriteMany("v5", v5, w.Write);
                        w.WriteMany("v6", v6, w.Write);
                    }

                    IValueReader r = creator.GetReader();
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase tmp = createCreator())
                {
                    if (!tmp.SupportsNodes)
                        continue;
                }

                for (int i = 1; i < 100; i++)
                {
                    using (ReaderWriterCreatorBase creator = createCreator())
                    {
                        var v1 = Range(0, i, 1, x => x % 3 == 0);

                        using (IValueWriter w = creator.GetWriter())
                        {
                            w.WriteMany("v1", v1, w.Write);
                        }

                        IValueReader r = creator.GetReader();
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    if (!creator.SupportsNodes)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (IValueWriter w = creator.GetWriter())
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

                    IValueReader r = creator.GetReader();
                    {
                        IValueReader a1 = r.ReadNode("a1");
                        IValueReader c = a1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b1 = a1.ReadNode("b1");
                        c = a1b1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b1c1 = a1b1.ReadNode("c1");
                        c = a1b1c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b1c2 = a1b1.ReadNode("c2");
                        c = a1b1c2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b1c2d1 = a1b1c2.ReadNode("d1");
                        c = a1b1c2d1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b2 = a1.ReadNode("b2");
                        c = a1b2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a1b2c1 = a1b2.ReadNode("c1");
                        c = a1b2c1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a2 = r.ReadNode("a2");
                        c = a2;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a2b1 = a2.ReadNode("b1");
                        c = a2b1;
                        AssertArraysEqual(v1, c.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname))));
                        AssertArraysEqual(v2, c.ReadMany("v2", ((preader, pname) => preader.ReadInt(pname))));
                        AssertArraysEqual(v3, c.ReadMany("v3", ((preader, pname) => preader.ReadFloat(pname))));
                        AssertArraysEqual(v4, c.ReadMany("v4", ((preader, pname) => preader.ReadByte(pname))));
                        AssertArraysEqual(v5, c.ReadMany("v5", ((preader, pname) => preader.ReadUShort(pname))));
                        AssertArraysEqual(v6, c.ReadMany("v6", ((preader, pname) => preader.ReadString(pname))));

                        IValueReader a2b2 = a2.ReadNode("b2");
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    if (!creator.SupportsNodes || !creator.SupportsNameLookup)
                        continue;

                    var v1 = Range(0, 100, 1, x => x % 3 == 0);
                    var v2 = Range(0, 100, 1, x => (int)x);
                    var v3 = Range(0, 100, 1, x => (float)x);
                    var v4 = Range(0, 100, 1, x => (byte)x);
                    var v5 = Range(0, 100, 1, x => (ushort)x);
                    var v6 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (IValueWriter w = creator.GetWriter())
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

                    IValueReader r = creator.GetReader();
                    {
                        IValueReader a1 = r.ReadNode("a1");
                        IValueReader a1b1 = a1.ReadNode("b1");
                        IValueReader a1b1c1 = a1b1.ReadNode("c1");
                        IValueReader a1b1c2 = a1b1.ReadNode("c2");
                        IValueReader a1b1c2d1 = a1b1c2.ReadNode("d1");
                        IValueReader a1b2 = a1.ReadNode("b2");
                        IValueReader a1b2c1 = a1b2.ReadNode("c1");

                        IValueReader a2 = r.ReadNode("a2");
                        IValueReader a2b1 = a2.ReadNode("b1");
                        IValueReader a2b2 = a2.ReadNode("b2");

                        IValueReader c = a1;
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
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (sbyte)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadSByte(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestShorts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (short)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadShort(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestStrings()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => Parser.Invariant.ToString(x));

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadString(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestUInts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (uint)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadUInt(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestULongs()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (ulong)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadULong(pname)));
                    }
                }
            }
        }

        [Test]
        public void TestUShorts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                using (ReaderWriterCreatorBase creator = createCreator())
                {
                    var v1 = Range(0, 100, 1, x => (ushort)x);

                    using (IValueWriter w = creator.GetWriter())
                    {
                        WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));
                    }

                    IValueReader r = creator.GetReader();
                    {
                        ReadTestValues(r, v1, ((preader, pname) => preader.ReadUShort(pname)));
                    }
                }
            }
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
        static void WriteTestValues<T>(IValueWriter w, T[] values, WriteTestValuesHandler<T> writeHandler)
        {
            for (int i = 0; i < values.Length; i++)
            {
                writeHandler(w, GetValueKey(i), values[i]);
            }
        }

        /// <summary>
        /// BitStream using the ByteArray to transfer data from the reader to writer
        /// </summary>
        class BitStreamByteArrayReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _writeStream = new BitStream(BitStreamMode.Write, _bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                var buffer = _writeStream.GetBuffer();
                return new BitStream(buffer);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return _writeStream;
            }
        }

        /// <summary>
        /// BitStream
        /// </summary>
        class BitStreamReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _stream = new BitStream(BitStreamMode.Write, _bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                _stream.Mode = BitStreamMode.Read;
                return _stream;
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return _stream;
            }
        }

        /// <summary>
        /// Handler for getting a ReaderWriterCreatorBase instance. 
        /// </summary>
        /// <returns>The ReaderWriterCreatorBase instance.</returns>
        delegate ReaderWriterCreatorBase CreateCreatorHandler();

        /// <summary>
        /// Handler for creating a value from a double.
        /// </summary>
        /// <typeparam name="T">The Type of value to create.</typeparam>
        /// <param name="value">The value to give the new type. Round is fine as long as it is consistent.</param>
        /// <returns>The new value as type <typeparamref name="T"/>.</returns>
        delegate T CreateValueTypeHandler<T>(double value);

        /// <summary>
        /// BinaryValueReader/Writer, using a file.
        /// </summary>
        class FileBinaryValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly string _filePath = GetTempFile();

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                ReleaseFile(_filePath);
                GC.SuppressFinalize(this);

                base.Dispose();
            }

            ~FileBinaryValueReaderWriterCreator()
            {
                ReleaseFile(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                return new BinaryValueReader(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return new BinaryValueWriter(_filePath);
            }
        }

        /// <summary>
        /// BinaryValueReader/Writer, using memory.
        /// </summary>
        class MemoryBinaryValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _stream = new BitStream(BitStreamMode.Write, _bufferSize);

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return false; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                _stream.Mode = BitStreamMode.Read;
                return new BinaryValueReader(_stream);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return new BinaryValueWriter(_stream);
            }
        }

        /// <summary>
        /// Base class for a IValueReader and IValueWriter creator.
        /// </summary>
        abstract class ReaderWriterCreatorBase : IDisposable
        {
            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public abstract bool SupportsNameLookup { get; }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public abstract bool SupportsNodes { get; }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public abstract IValueReader GetReader();

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public abstract IValueWriter GetWriter();

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public virtual void Dispose()
            {
            }

            #endregion
        }

        /// <summary>
        /// Handler for reading a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="r">IValueReader to read from.</param>
        /// <param name="name">Name to use for reading.</param>
        /// <returns>The read value.</returns>
        delegate T ReadTestValuesHandler<T>(IValueReader r, string name);

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

        sealed class TestEnumHelper : EnumHelper<TestEnum>
        {
            /// <summary>
            /// When overridden in the derived class, casts an int to type TestEnum.
            /// </summary>
            /// <param name="value">The int value.</param>
            /// <returns>The <paramref name="value"/> casted to type TestEnum.</returns>
            protected override TestEnum FromInt(int value)
            {
                return (TestEnum)value;
            }

            /// <summary>
            /// When overridden in the derived class, casts type TestEnum to an int.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>The <paramref name="value"/> casted to an int.</returns>
            protected override int ToInt(TestEnum value)
            {
                return (int)value;
            }
        }

        /// <summary>
        /// Handler for writing a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="name">Name to use for writing.</param>
        /// <param name="value">Value to write.</param>
        delegate void WriteTestValuesHandler<T>(IValueWriter w, string name, T value);

        /// <summary>
        /// XmlValueReader/Writer.
        /// </summary>
        class XmlValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            const string _rootNodeName = "TestRootNode";

            readonly string _filePath = GetTempFile();

            /// <summary>
            /// When overridden in the derived class, gets if name lookup is supported.
            /// </summary>
            public override bool SupportsNameLookup
            {
                get { return true; }
            }

            /// <summary>
            /// When overridden in the derived class, gets if nodes are supported.
            /// </summary>
            public override bool SupportsNodes
            {
                get { return true; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public override void Dispose()
            {
                ReleaseFile(_filePath);
                GC.SuppressFinalize(this);

                base.Dispose();
            }

            ~XmlValueReaderWriterCreator()
            {
                ReleaseFile(_filePath);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueReader instance used to read the values
            /// written by the IValueWriter created with GetWriter().
            /// </summary>
            /// <returns>The IValueWriter instance.</returns>
            public override IValueReader GetReader()
            {
                return new XmlValueReader(_filePath, _rootNodeName);
            }

            /// <summary>
            /// When overridden in the derived class, gets the IValueWriter instance. This method is always called first.
            /// </summary>
            /// <returns>The IValueReader instance.</returns>
            public override IValueWriter GetWriter()
            {
                return new XmlValueWriter(_filePath, _rootNodeName);
            }
        }
    }
}