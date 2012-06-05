using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class PersistableTests
    {
        #region Unit tests

        [Test]
        public void ReadWriteFromHelperUsingPropertyTest()
        {
            var bs = new BitStream(512);
            var a = new ClassB { A = "asdf", B = 512 };
            var b = new ClassB();

            a.WriteState(bs);

            bs.PositionBits = 0;

            b.ReadState(bs);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.B, b.B);
        }

        [Test]
        public void ReadWriteFromInterfaceTest()
        {
            var bs = new BitStream(512);
            var a = new ClassA { A = "asdf", B = 512 };
            var b = new ClassA();

            a.WriteState(bs);

            bs.PositionBits = 0;

            b.ReadState(bs);

            Assert.AreEqual(a.A, b.A);
            Assert.AreEqual(a.B, b.B);
        }

        #endregion

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class ClassA : IPersistable
        {
            public string A { get; set; }

            public int B { get; set; }

            #region IPersistable Members

            /// <summary>
            /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
            /// same order as they were written.
            /// </summary>
            /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
            public void ReadState(IValueReader reader)
            {
                A = reader.ReadString("A");
                B = reader.ReadInt("B");
            }

            /// <summary>
            /// Writes the state of the object to an <see cref="IValueWriter"/>.
            /// </summary>
            /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
            public void WriteState(IValueWriter writer)
            {
                writer.Write("A", A);
                writer.Write("B", B);
            }

            #endregion
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public class ClassB : IPersistable
        {
            [SyncValue]
            public string A { get; set; }

            [SyncValue]
            public int B { get; set; }

            #region IPersistable Members

            /// <summary>
            /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
            /// same order as they were written.
            /// </summary>
            /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
            public void ReadState(IValueReader reader)
            {
                PersistableHelper.Read(this, reader);
            }

            /// <summary>
            /// Writes the state of the object to an <see cref="IValueWriter"/>.
            /// </summary>
            /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
            public void WriteState(IValueWriter writer)
            {
                PersistableHelper.Write(this, writer);
            }

            #endregion
        }
    }
}