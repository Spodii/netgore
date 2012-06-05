using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class ObjectStatePersisterTests
    {
        static readonly SafeRandom r = new SafeRandom();

        static string CreateRandomString()
        {
            var length = r.Next(2, 10);
            var ret = new char[length];

            for (var i = 0; i < ret.Length; i++)
            {
                int j;
                if (i == 0)
                    j = r.Next(1, 3); // Don't let the first character be numeric
                else
                    j = r.Next(0, 3);

                int min;
                int max;

                // ReSharper disable RedundantCast
                switch (j)
                {
                    case 0:
                        min = (int)'0';
                        max = (int)'9';
                        break;

                    case 1:
                        min = (int)'a';
                        max = (int)'z';
                        break;

                    default:
                        min = (int)'A';
                        max = (int)'Z';
                        break;
                }
                // ReSharper restore RedundantCast

                ret[i] = (char)r.Next(min, max + 1);
            }

            return new string(ret);
        }

        static InterfaceTester GetRandomInterfaceTester()
        {
            return new InterfaceTester
            { A = CreateRandomString(), B = r.Next(int.MinValue, int.MaxValue), C = (r.Next(-1000, 1000)) };
        }

        #region Unit tests

        [Test]
        public void AddNewItemTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            InterfaceTester retT3;

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t3", t3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                    retT3 = t3;
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void CaseInsensitiveLowerTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            var retT3 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("T2", t2);
                    settingsWriter.Add("T3", t3);
                    settingsWriter.Add("T1", t1);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void CaseInsensitiveUpperTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            var retT3 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                    settingsWriter.Add("t1", t1);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("T3", retT3);
                    settingsReader.Add("T1", retT1);
                    settingsReader.Add("T2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void DuplicateKeyTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    Assert.Throws<ArgumentException>(() => settingsWriter.Add("t1", t2));
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void MissingItemTest()
        {
            new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            var retT3 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void ReadWriteTonsOfCrapTest()
        {
            const int iterations = 10000;

            var items = new List<InterfaceTester>(iterations);

            for (var i = 0; i < iterations; i++)
            {
                items.Add(GetRandomInterfaceTester());
            }

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        settingsWriter.Add("item" + Parser.Invariant.ToString(i), items[i]);
                    }
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        var newItem = new InterfaceTester();
                        settingsReader.Add("item" + Parser.Invariant.ToString(i), newItem);
                        Assert.IsTrue(items[i].HaveSameValues(newItem), "Index: {0}", i);
                    }
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void VariableLoadOrderTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            var retT3 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void VariableSaveOrderTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };
            var t3 = new InterfaceTester { A = "wb", B = 24, C = 1234.3f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();
            var retT3 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t2", t2);
                    settingsWriter.Add("t3", t3);
                    settingsWriter.Add("t1", t1);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t3", retT3);
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
            Assert.IsTrue(t3.HaveSameValues(retT3));
        }

        [Test]
        public void WriteReadTest()
        {
            var t1 = new InterfaceTester { A = "Hello", B = 10, C = 5.135f };
            var t2 = new InterfaceTester { A = "Goodbye", B = 44, C = 1897.01f };

            var retT1 = new InterfaceTester();
            var retT2 = new InterfaceTester();

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsWriter.Add("t1", t1);
                    settingsWriter.Add("t2", t2);
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    settingsReader.Add("t1", retT1);
                    settingsReader.Add("t2", retT2);
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Assert.IsTrue(t1.HaveSameValues(retT1));
            Assert.IsTrue(t2.HaveSameValues(retT2));
        }

        [Test]
        public void WriteTonsOfCrapReadOnlySomeTest()
        {
            const int iterations = 10000;

            var items = new List<InterfaceTester>(iterations);

            for (var i = 0; i < iterations; i++)
            {
                items.Add(GetRandomInterfaceTester());
            }

            var filePath = Path.GetTempFileName();

            try
            {
                using (var settingsWriter = new ObjectStatePersister("TestSettings", filePath))
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        settingsWriter.Add("item" + Parser.Invariant.ToString(i), items[i]);
                    }
                }

                using (var settingsReader = new ObjectStatePersister("TestSettings", filePath))
                {
                    for (var i = items.Count - 1; i > 0; i -= 22)
                    {
                        var newItem = new InterfaceTester();
                        settingsReader.Add("item" + Parser.Invariant.ToString(i), newItem);
                        Assert.IsTrue(items[i].HaveSameValues(newItem), "Index: {0}", i);
                    }
                }
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        #endregion

        class InterfaceTester : IPersistable
        {
            [SyncValue]
            public string A { get; set; }

            [SyncValue]
            public int B { get; set; }

            [SyncValue]
            public float C { get; set; }

            public bool HaveSameValues(InterfaceTester other)
            {
                return HaveSameValues(this, other);
            }

            static bool HaveSameValues(InterfaceTester l, InterfaceTester r)
            {
                return (l.A == r.A) && (l.B == r.B) && (l.C == r.C);
            }

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