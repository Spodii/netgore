using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetGore.Globalization;
using NUnit.Framework;

namespace NetGore.IO.Tests
{
    [TestFixture]
    public class IValueReaderWriterTests
    {
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
            /// <filterpriority>2</filterpriority>
            public virtual void Dispose()
            {
            }

            #endregion
        }

        /// <summary>
        /// Gets the path for a temp file.
        /// </summary>
        /// <returns>The path for a temp file.</returns>
        static string GetTempFile()
        {
            return Path.GetTempFileName();
        }

        /// <summary>
        /// Releases a file used with GetTempFile().
        /// </summary>
        /// <param name="filePath">Path to the file to release.</param>
        static void ReleaseFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

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

                base.Dispose();
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

                base.Dispose();
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
        /// Handler for getting a ReaderWriterCreatorBase instance. 
        /// </summary>
        /// <returns>The ReaderWriterCreatorBase instance.</returns>
        delegate ReaderWriterCreatorBase CreateCreatorHandler();

        /// <summary>
        /// Handler for writing a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="w">IValueWriter to write to.</param>
        /// <param name="name">Name to use for writing.</param>
        /// <param name="value">Value to write.</param>
        delegate void WriteTestValuesHandler<T>(IValueWriter w, string name, T value);

        /// <summary>
        /// Handler for reading a value.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="r">IValueReader to read from.</param>
        /// <param name="name">Name to use for reading.</param>
        /// <returns>The read value.</returns>
        delegate T ReadTestValuesHandler<T>(IValueReader r, string name);

        /// <summary>
        /// Handler for creating a value from a double.
        /// </summary>
        /// <typeparam name="T">The Type of value to create.</typeparam>
        /// <param name="value">The value to give the new type. Round is fine as long as it is consistent.</param>
        /// <returns>The new value as type <typeparamref name="T"/>.</returns>
        delegate T CreateValueTypeHandler<T>(double value);

        /// <summary>
        /// The CreateCreatorHandlers that will be used to create the ReaderWriterCreatorBase instances.
        /// </summary>
        static readonly IEnumerable<CreateCreatorHandler> _createCreators;

        /// <summary>
        /// Initializes the <see cref="IValueReaderWriterTests"/> class.
        /// </summary>
        static IValueReaderWriterTests()
        {
            // Create the delegates for creating the ReaderWriterCreatorBases
            _createCreators = new CreateCreatorHandler[]
            {
                () => new MemoryBinaryValueReaderWriterCreator(), () => new FileBinaryValueReaderWriterCreator(),
                //() => new XmlValueReaderWriterCreator()
            };
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
        /// Gets the key for a value.
        /// </summary>
        /// <param name="i">The index of the value.</param>
        /// <returns>The key for a value with index of <paramref name="i"/>.</returns>
        static string GetValueKey(int i)
        {
            return "V" + Parser.Invariant.ToString(i);
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

        const int _bufferSize = 1024 * 1024;

        static readonly object[] _emptyObjArray = new object[0];

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

        [Test]
        public void TestBools()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestBytes()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestFloats()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestInts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestNodesBoolsOnly()
        {

            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

                var v1 = Range(0, 7, 1, x => true);

                using (IValueWriter w = creator.GetWriter())
                {
                    w.WriteStartNode(null);
                    w.Write(null, (uint)v1.Length);
                    for (int i = 0; i < v1.Length; i++)
                        w.Write(null, v1[i]);
                    w.WriteEndNode(null);
                    //w.WriteMany("v1", v1, w.Write);
                }

                IValueReader r = creator.GetReader();
                {
                    r.ReadInt(null);
                    int len = (int)r.ReadUInt(null);
                    var r1 = new bool[len];
                    for (int i = 0; i < len; i++)
                        r1[i] = r.ReadBool(null);

                    //var r1 = r.ReadMany("v1", ((preader, pname) => preader.ReadBool(pname)));

                    AssertArraysEqual(v1, r1);
                }
            }
        }

        [Test]
        public void TestNodes()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

                var v1 = Range(0, 100, 1, x => true);
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

        [Test]
        public void TestSBytes()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestShorts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestStrings()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestUInts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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

        [Test]
        public void TestUShorts()
        {
            foreach (CreateCreatorHandler createCreator in _createCreators)
            {
                ReaderWriterCreatorBase creator = createCreator();

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
}