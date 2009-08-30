using System;
using System.Collections.Generic;
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

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public virtual void Dispose()
            {
            }
        }

        /// <summary>
        /// BinaryValueReader/Writer, using memory.
        /// </summary>
        class MemoryBinaryValueReaderWriterCreator : ReaderWriterCreatorBase
        {
            readonly BitStream _stream = new BitStream(BitStreamMode.Write, _bufferSize);

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
                () => new MemoryBinaryValueReaderWriterCreator()
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
                writeHandler(w, Parser.Invariant.ToString(i), values[i]);
            }
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
                actual[i] = readHandler(r, Parser.Invariant.ToString(i));
            }

            for (int i = 0; i < expected.Length; i++)
            {
                const string errmsg = "Writer Type: `{0}`  Value Type: `{1}`  Iteration: `{2}`";
                Assert.AreEqual(expected[i], actual[i], errmsg, r.GetType(), typeof(T), i);
            }
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

        const int _bufferSize = 1024 * 1024 * 8;

        [Test]
        public void TestSuite01()
        {
            foreach (var createCreator in _createCreators)
            {
                var creator = createCreator();

                var v1 = Range(0, 100, 1, x => (int)x);

                IValueWriter w = creator.GetWriter();

                WriteTestValues(w, v1, ((pwriter, pname, pvalue) => pwriter.Write(pname, pvalue)));

                IValueReader r = creator.GetReader();

                ReadTestValues(r, v1, ((preader, pname) => preader.ReadInt(pname)));
            }
        }
    }
}