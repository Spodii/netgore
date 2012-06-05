using System;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;
using SFML.Graphics;

namespace NetGore.Tests.NetGore
{
    [TestFixture(
        Description = "Makes sure the VariableValue I/O extensions work properly with all IValueReaders and IValueWriters.")]
    public class VariableValueIOTests
    {
        const int _randomVariableTestIterations = 25;

        /// <summary>
        /// Gets the key for a value.
        /// </summary>
        /// <param name="i">The index of the value.</param>
        /// <returns>The key for a value with index of <paramref name="i"/>.</returns>
        static string GetValueKey(int i)
        {
            return "V" + Parser.Invariant.ToString(i);
        }

        static void ReadTest<T>(IVariableValue<T> value, Action<IValueWriter, string, IVariableValue<T>> write,
                                Func<IValueReader, string, IVariableValue<T>> read)
        {
            ReadWriteTest(value, write, read, true);
        }

        static void ReadWriteTest<T>(IVariableValue<T> value, Action<IValueWriter, string, IVariableValue<T>> write,
                                     Func<IValueReader, string, IVariableValue<T>> read, bool testRead)
        {
            foreach (var createCreator in IValueReaderWriterTestHelper.CreateCreators)
            {
                using (var creator = createCreator())
                {
                    using (var w = creator.GetWriter())
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            write(w, GetValueKey(i), value);
                        }
                    }

                    if (testRead)
                    {
                        var r = creator.GetReader();
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                var readValue = read(r, GetValueKey(i));
                                Assert.AreEqual(value.Min, readValue.Min);
                                Assert.AreEqual(value.Max, readValue.Max);
                            }
                        }
                    }
                }
            }
        }

        static void WriteTest<T>(IVariableValue<T> value, Action<IValueWriter, string, IVariableValue<T>> write,
                                 Func<IValueReader, string, IVariableValue<T>> read)
        {
            ReadWriteTest(value, write, read, false);
        }

        #region Unit tests

        [Test]
        public void RandomVariableByteTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const byte min = byte.MinValue;
                const byte max = byte.MaxValue;

                var a = (byte)RandomHelper.NextInt(min, max);
                var b = (byte)RandomHelper.NextInt(min, max);

                var v = new VariableByte(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableByte)z), (x, y) => x.ReadVariableByte(y));
            }
        }

        [Test]
        public void RandomVariableColorTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const byte min = byte.MinValue;
                const byte max = byte.MaxValue;

                var ar = (byte)RandomHelper.NextInt(min, max);
                var ag = (byte)RandomHelper.NextInt(min, max);
                var ab = (byte)RandomHelper.NextInt(min, max);
                var aa = (byte)RandomHelper.NextInt(min, max);
                var br = (byte)RandomHelper.NextInt(min, max);
                var bg = (byte)RandomHelper.NextInt(min, max);
                var bb = (byte)RandomHelper.NextInt(min, max);
                var ba = (byte)RandomHelper.NextInt(min, max);

                var v = new VariableColor(new Color(ar, ag, ab, aa), new Color(br, bg, bb, ba));

                WriteTest(v, (x, y, z) => x.Write(y, (VariableColor)z), (x, y) => x.ReadVariableColor(y));
            }
        }

        [Test]
        public void RandomVariableFloatTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const float min = float.MinValue;
                const float max = float.MaxValue;

                var a = (byte)RandomHelper.NextFloat(min, max);
                var b = (byte)RandomHelper.NextFloat(min, max);

                var v = new VariableFloat(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableFloat)z), (x, y) => x.ReadVariableFloat(y));
            }
        }

        [Test]
        public void RandomVariableIntTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const int min = int.MinValue;
                const int max = int.MaxValue;

                var a = RandomHelper.NextInt(min, max);
                var b = RandomHelper.NextInt(min, max);

                var v = new VariableInt(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableInt)z), (x, y) => x.ReadVariableInt(y));
            }
        }

        [Test]
        public void RandomVariableSByteTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const sbyte min = sbyte.MinValue;
                const sbyte max = sbyte.MaxValue;

                var a = (sbyte)RandomHelper.NextInt(min, max);
                var b = (sbyte)RandomHelper.NextInt(min, max);

                var v = new VariableSByte(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableSByte)z), (x, y) => x.ReadVariableSByte(y));
            }
        }

        [Test]
        public void RandomVariableShortTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const short min = short.MinValue;
                const short max = short.MaxValue;

                var a = (short)RandomHelper.NextInt(min, max);
                var b = (short)RandomHelper.NextInt(min, max);

                var v = new VariableShort(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableShort)z), (x, y) => x.ReadVariableShort(y));
            }
        }

        [Test]
        public void RandomVariableUShortTest()
        {
            for (var i = 0; i < _randomVariableTestIterations; i++)
            {
                const ushort min = ushort.MinValue;
                const ushort max = ushort.MaxValue;

                var a = (ushort)RandomHelper.NextInt(min, max);
                var b = (ushort)RandomHelper.NextInt(min, max);

                var v = new VariableUShort(a, b);

                WriteTest(v, (x, y, z) => x.Write(y, (VariableUShort)z), (x, y) => x.ReadVariableUShort(y));
            }
        }

        [Test]
        public void ReadVariableByteTest()
        {
            ReadTest(new VariableByte(50, 23), (x, y, z) => x.Write(y, (VariableByte)z), (x, y) => x.ReadVariableByte(y));
        }

        [Test]
        public void ReadVariableColorTest()
        {
            ReadTest(new VariableColor(), (x, y, z) => x.Write(y, (VariableColor)z), (x, y) => x.ReadVariableColor(y));
        }

        [Test]
        public void ReadVariableFloatTest()
        {
            ReadTest(new VariableFloat(50, 23), (x, y, z) => x.Write(y, (VariableFloat)z), (x, y) => x.ReadVariableFloat(y));
        }

        [Test]
        public void ReadVariableIntTest()
        {
            ReadTest(new VariableInt(50, 23), (x, y, z) => x.Write(y, (VariableInt)z), (x, y) => x.ReadVariableInt(y));
        }

        [Test]
        public void ReadVariableSByteTest()
        {
            ReadTest(new VariableSByte(50, 23), (x, y, z) => x.Write(y, (VariableSByte)z), (x, y) => x.ReadVariableSByte(y));
        }

        [Test]
        public void ReadVariableShortTest()
        {
            ReadTest(new VariableShort(50, 23), (x, y, z) => x.Write(y, (VariableShort)z), (x, y) => x.ReadVariableShort(y));
        }

        [Test]
        public void ReadVariableUShortTest()
        {
            ReadTest(new VariableUShort(50, 23), (x, y, z) => x.Write(y, (VariableUShort)z), (x, y) => x.ReadVariableUShort(y));
        }

        [Test]
        public void WriteVariableByteTest()
        {
            WriteTest(new VariableByte(50, 23), (x, y, z) => x.Write(y, (VariableByte)z), (x, y) => x.ReadVariableByte(y));
        }

        [Test]
        public void WriteVariableColorTest()
        {
            WriteTest(new VariableColor(), (x, y, z) => x.Write(y, (VariableColor)z), (x, y) => x.ReadVariableColor(y));
        }

        [Test]
        public void WriteVariableFloatTest()
        {
            WriteTest(new VariableFloat(50, 23), (x, y, z) => x.Write(y, (VariableFloat)z), (x, y) => x.ReadVariableFloat(y));
        }

        [Test]
        public void WriteVariableIntTest()
        {
            WriteTest(new VariableInt(50, 23), (x, y, z) => x.Write(y, (VariableInt)z), (x, y) => x.ReadVariableInt(y));
        }

        [Test]
        public void WriteVariableSByteTest()
        {
            WriteTest(new VariableSByte(50, 23), (x, y, z) => x.Write(y, (VariableSByte)z), (x, y) => x.ReadVariableSByte(y));
        }

        [Test]
        public void WriteVariableShortTest()
        {
            WriteTest(new VariableShort(50, 23), (x, y, z) => x.Write(y, (VariableShort)z), (x, y) => x.ReadVariableShort(y));
        }

        [Test]
        public void WriteVariableUShortTest()
        {
            WriteTest(new VariableUShort(50, 23), (x, y, z) => x.Write(y, (VariableUShort)z), (x, y) => x.ReadVariableUShort(y));
        }

        #endregion
    }
}